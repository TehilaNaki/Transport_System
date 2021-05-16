using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using BL;

namespace BO
{
    /// <summary>
    ///  Represents a bus
    /// </summary>
    public class Bus
    {
        public Status State { get; set; } = Status.Ready;
        public string LicensNumber { get; set; } = "";
        public DateTime DateOfBeg { set; get; } = DateTime.Now;
        public int KmSum { get; set; } = 0;
        public float KmOfTreatment { get; set; } = 0;
        public int Fuel { get; set; } = 0;
        public DateTime DateOfTreatment { get; set; } = DateTime.Now;
        public bool Charger { get; set; } = false;
        public bool Wifi { get; set; } = false;
        public bool ComfortSeat { get; set; } = false;
        

    }
}
