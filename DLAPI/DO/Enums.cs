using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// enums of the data base layer
/// </summary>
namespace DO
{
        public enum Status { Ready, NeedRefuel, NeedTreatment, DurringDrive, Refueling, InTreatment }
        public enum Line_Area { Eilat, Jerusalem, Tel_Aviv, Ramat_Gan, Haifa, Herzelia, Netanya }
        public enum Permission { Managment,Passenger}
        public class IDis
        {
        public string Name { get; set; }
        public int Id { get; set; }
        }
}
