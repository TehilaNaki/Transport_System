
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Represents a bus line-list of stations
    /// </summary>
    public class Line
    {  
        public int ID { get; set; }
        public int LineCode { get; set; }
        public Line_Area Area { get; set; }
        public int RouteLength { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public IEnumerable<LineStation> StationsRoute { get; set; }
    }
}
