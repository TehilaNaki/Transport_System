using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BO;
using BLAPI;
using PLGUI;


/// <summary>
/// Bus class for the GUI
/// </summary>
namespace PO
{
    public class Bus : BO.Bus, INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SimulationOfBus simulation { get; set; }
        public Bus() { }

        //copy&convert constractor
        public Bus(BO.Bus b)
        {

            LicensNumber = b.LicensNumber;
            KmSum = b.KmSum;
            DateOfBeg = b.DateOfBeg;
            Charger = b.Charger;
            ComfortSeat = b.ComfortSeat;
            Wifi = b.Wifi;
            Fuel = b.Fuel;
            upFuel = b.Fuel;
            DateOfTreatment = b.DateOfTreatment;
            KmOfTreatment = b.KmOfTreatment;
            Upstatus = b.State;
        }
        
        /// <summary>
        /// function that update the status of the bus
        /// </summary>
        public bool Status(int km)
        {
            if (Fuel < km)//chek if there is enough feul for the drive
            {
                if (Fuel <= 5)
                {
                    Upstatus = BO.Status.NeedRefuel;
                }
                return false;
            }
            if (KmOfTreatment + km >= 20000)
            {
                Upstatus = BO.Status.NeedTreatment;
                return false;
            }
            if (DateOfTreatment <= DateTime.Now.AddYears(-1))
            {
                Upstatus = BO.Status.NeedTreatment;
                return false;
            }
            else Upstatus = BO.Status.Ready;
            return true;
        }

        //update precent of progress that the bus in
        private int progressPrecent = 0;
        public int ProgressPrecent
        {
            get { return progressPrecent; }
            set
            {
                progressPrecent = value;
                OnPropertyChanged(nameof(progressPrecent));
            }

        }
        //update status of bus
        private BO.Status upstatus;
        public BO.Status Upstatus
        {
            get => upstatus;
            set
            {
                upstatus = value;
                OnPropertyChanged(nameof(upstatus));
            }
        }

        //update fuel
        private int upFuel;
        public int UpFuel
        {
            get => upFuel;
            set
            {
                upFuel = value;
                OnPropertyChanged(nameof(upFuel));
            }
        }

    }
}

