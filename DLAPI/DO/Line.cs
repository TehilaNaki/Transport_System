using System;
using System.Collections.Generic;
using System.Text;


namespace DO
{
    /// <summary>
    /// Represents a Line
    /// </summary>
    public class Line
    {
        
        public int ID { get; set; }
        public int LineCode { get; set; }
        public Line_Area Area { get; set; }
        public int? FirstStation { get; set; }
        public int? LastStation { get; set; }
        public bool Available { get; set; } = true;
        public int RouteLength { get; set; }       
   
    }
}