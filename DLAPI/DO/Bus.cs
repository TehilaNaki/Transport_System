using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace DO
   

{
    /// <summary>
    ///  Represents a Bus
    /// </summary>

    public class Bus
    {  
        public string LicensNumber { set; get; }
        public DateTime DateOfBeg { set; get; }
        public int KmSum  { get; set; }
        public float KmOfTreatment  { get; set; }
        public int Fuel { get; set; }
        public DateTime DateOfTreatment { get; set; }
        public bool Wifi { get; set; }
        public bool ComfortSeat { get; set; }
        public bool Charger { get; set; }
        public bool Available { get; set; } = true; 
    }
}
