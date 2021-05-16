using System;
using System.Linq.Expressions;

namespace DO
{
	/// <summary>
	/// Represents a physical station
	/// </summary>
	public class Station 
	{
		public int Code { get; set; }
		public string Name { get; set; }
		public string Region { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public string Address { get;  set; }
		public bool StationRoof { get; set; }
		public bool DigitalPanel { get; set; }
		public bool AccessForDisabled { get; set; }
		public bool Available { get; set; } = true;
       		
	}
}





