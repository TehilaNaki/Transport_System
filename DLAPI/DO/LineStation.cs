using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    ///  Represents a Station in the Bus line
    /// </summary>
    public class LineStation 
    {
        public int ID { get; set; }
        public int StationCode { get; set; }
        public int IndexInLine { get; set; }
        public int  LineId { get; set; }
     
    }
}
