using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
	/// <summary>
	/// Represents  Adjacent stations on a trip line
	/// </summary>
	public class AdjacentStation 
	{
	public Station AdjStation { get; set; }
	public double Distance { get; set; }
	public TimeSpan AverageTime { get; set; }

	}
}
