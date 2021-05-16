using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

/// <summary>
/// A simulation class using BackgroundWorker for the stops of the bus in station
/// </summary>
namespace PLGUI
{
    public class SimulationOfTrip
    {
        public PO.CurrentStation MycurrentStation { get; set; }
        public BackgroundWorker worker { get; set; }
        public SimulationOfTrip(PO.CurrentStation current)
        {
            MycurrentStation = current;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Complate;
            worker.WorkerSupportsCancellation = true;
        }

        public void Complate(object sender, RunWorkerCompletedEventArgs e)
        {
            PO.CurrentStation.LineTimings.Remove(MycurrentStation);
        }
        public void Starting()
        {
            MycurrentStation.UpStation = MycurrentStation.UplineStations.ToList()[0].Station.Code;
            worker.RunWorkerAsync();
        }
        public void StopSimulation()
        {
                worker.CancelAsync();
        }
        public void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            int counter = 0;
            while (counter < MycurrentStation.UplineStations.Count()-1)
            {
                if (worker.CancellationPending == true)
                    worker.CancelAsync();

                var i = MycurrentStation.UplineStations.ToList()[counter];
                Thread.Sleep((int)(i.DrivingTime.TotalSeconds)*80);
                counter++;
                worker.ReportProgress(counter);
               
            }
            if (worker.CancellationPending == true)
                worker.CancelAsync();
        }
        public void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PO.CurrentStation.LineTimings.Remove(MycurrentStation);
            MycurrentStation.UpStation = (MycurrentStation.UplineStations.ToList()[e.ProgressPercentage]).Station.Code;
            PO.CurrentStation.LineTimings.Add(MycurrentStation);
        }
    }
}
