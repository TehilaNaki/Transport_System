using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGUI
{
    /// <summary>
    /// Help structures for the GUI
    /// </summary>
    public class Stat
    {
        public int Code { get; set; }
        string Name { get; set; }
        public Stat(int c, string n)
        {
            Code = c;
            Name = n;
        }
        public override string ToString()
        {
            return Code + ": " + Name;
        }
    }
    
    public class AREA
    {
        public BO.Line_Area Line_Area { get; set; }
        public override string ToString()
        {
            return Line_Area.ToString();
        }
    }
}
