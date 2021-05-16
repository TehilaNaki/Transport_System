using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Time that line start a trip
    /// </summary>
    public class LineTiming
    {
        public int LineID { get; set; }
        public int LineNum { get; set; }
        public string EndStationName { get; set; }
        public TimeSpan TripStartTime { get; set; }
    }
}
