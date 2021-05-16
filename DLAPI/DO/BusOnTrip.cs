using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Represents a Trip of Bus with Line
    /// </summary>
    public class BusOnTrip
    {
       public int ID { get; set; }
        public string LicensNumber { get; set; }
        public int LineId { get; set; }
        public int LineNumber { get; set; }
        public string ActualTake { get; set; }
        public int PrevStation { get; set; }
        public String DriverId { get; set; }
        public bool Available { get; set; } = true;
    }
}
