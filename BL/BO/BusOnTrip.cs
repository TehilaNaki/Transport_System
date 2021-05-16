using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
    
{
    /// <summary>
    /// Represents a bus while trip
    /// </summary>
    public class BusOnTrip
    {
        public int ID { get; set; }
        public string LicensNumber { get; set; }
        public int LineId { get; set; }
        public int LineNumber { get; set; }
        public TimeSpan ActualTakeOff { get; set; }
        public int PrevStation { get; set; }
        public String DriverId { get; set; }
    }
}
