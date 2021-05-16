using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
    
{
    /// <summary>
    /// Represents Adjacent stations
    /// </summary>
    public class AdjacentStations 
    {
        public int StationCode1 { get; set; }
        public int StationCode2 { get; set; }
        public double Distance { get; set; }
        public TimeSpan AverageTime { get; set; }
        public bool Available { get; set; } = true;
      
    }
}
