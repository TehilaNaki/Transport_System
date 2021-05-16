using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    /// <summary>
    /// enums for the BL-logic layer
    /// </summary>
    public enum Line_Area { Eilat, Jerusalem, Tel_Aviv, Ramat_Gan, Haifa, Herzelia, Netanya }
    public enum Status { Ready, NeedRefuel, NeedTreatment, DurringDrive, Refueling, InTreatment }
    public enum Permission { Managment, Passenger }

    /// <summary>
    ///  a help classes for the GUI
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

    public struct Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public override string ToString()
        {
            return $"{Latitude:n3}°N, {Longitude:n3}°E";
        }

    }
}
