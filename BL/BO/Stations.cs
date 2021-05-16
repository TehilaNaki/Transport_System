using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
	/// <summary>
	/// Represents a physical station
	/// </summary>
	public class Station
    {
		public int Code { get; set; }
		public string Name { get; set; }
		public string Region { get; set; }
		public Location Location { get; set; }
		public string Address { get; set; }     
		public bool StationRoof { get; set; }
		public bool DigitalPanel { get; set; }
		public bool AccessForDisabled { get; set; }
		public IEnumerable<BO.AdjacentStation> AdjacentStations { get; set; }
		public override string ToString()
		{
			return string.Format("{0} - {1}", Name, Code);
		}
	}
}
