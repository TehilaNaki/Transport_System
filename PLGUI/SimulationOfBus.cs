using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using BLAPI;
using PO;

/// <summary>
/// A simulation class using BackgroundWorker for the bus status
/// </summary>
namespace PLGUI
{
    public class SimulationOfBus
    {
        public PO.Bus MyBus { get; set; }
        public BackgroundWorker worker { get; set; }
        public SimulationOfBus(PO.Bus b)
        {
            MyBus = b;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Complate;
            worker.WorkerSupportsCancellation = true;
        }

        public void Complate(object sender, RunWorkerCompletedEventArgs e)
        {
            MyBus.ProgressPrecent = 0;
            if (bool.Parse(e.Result.ToString()))//if it is a refuling func-fill the fuel
                MyBus.UpFuel = 1200;
            MyBus.Status(1);
        }
        public void Driving(int km)
        {
            MyBus.Upstatus = BO.Status.DurringDrive;

            worker.RunWorkerAsync(km);
        }
        public void StopSimulation()
        {   
                worker.CancelAsync();
        }
        public void Refuling()
        {
            MyBus.Upstatus = BO.Status.Refueling;
            worker.RunWorkerAsync(true);
        }
        public void Treating()
        {
            MyBus.Upstatus = BO.Status.InTreatment;
            worker.RunWorkerAsync(30);
        }
        public void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int counter = 0;
            bool ok;
            bool reful = bool.TryParse(e.Argument.ToString(), out ok);
            if (reful && ok)
                counter = MyBus.UpFuel;
            while (counter != 1200)
            {
                if (worker.CancellationPending == true)
                    worker.CancelAsync();
                worker.ReportProgress(counter);
                counter++;
                if (reful)
                    Thread.Sleep(20);
                else Thread.Sleep((int)e.Argument/10);
            }
            if (worker.CancellationPending == true)
                worker.CancelAsync();
            e.Result = true;
        }
        public void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MyBus.ProgressPrecent = e.ProgressPercentage;
        }
    }
}

