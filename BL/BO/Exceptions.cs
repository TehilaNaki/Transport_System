using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    /// <summary>
    /// Exceptions of the BL-logic layer
    /// </summary>
    
    #region StationException
    [Serializable]
    public class BadStationKeyException : Exception
    {
        public int Code;
        public BadStationKeyException(int code) : base() => Code = code;
        public BadStationKeyException(int code, string message) :
            base(message) => Code = code;
        public BadStationKeyException(int code, string message, Exception innerException) :
              base(message, innerException) => Code = code;
        public override string ToString() => base.ToString() + $", bad Station code: {Code}";
    }
    [Serializable]
    public class BadAdjacentStationsException : Exception
    {
        public AdjacentStation Adjstation;

        public BadAdjacentStationsException(AdjacentStation Adj) : base() => Adjstation = Adj;
      
        public BadAdjacentStationsException(AdjacentStation Adj, string message) : base(message) => Adjstation = Adj;


        public BadAdjacentStationsException(AdjacentStation Adj, string message, Exception innerException) : base(message, innerException) => Adjstation = Adj;

        public override string ToString() => base.ToString() + ", bad AdjacentStations";
    }
    [Serializable]
    #endregion

    #region BusException
    public class BadLicensNumberException : Exception
    {
        public string Licens;
        public BadLicensNumberException(string message, Exception innerException) :
            base(message, innerException) => Licens = ((DO.BadBusLicenseNumberException)innerException).Code;
        public override string ToString() => base.ToString() + $", bad Licens Number : {Licens}";
    }
    [Serializable]
    public class BadIDBusONeException : Exception
    {
        public int ID;
        public BadIDBusONeException(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.BadIdBUTException)innerException).ID;
        public override string ToString() => base.ToString() + $", bad User name : {ID}";
    }
    #endregion

    #region UserException
    [Serializable]
    public class BadUserNameException : Exception
    {
        public string UserName;
        public BadUserNameException(string message, Exception innerException) :
            base(message, innerException) => UserName = ((DO.BadUserNameException)innerException).Code;
        public override string ToString() => base.ToString() + $", bad User name : {UserName}";
    }
    #endregion

    #region LineException
    [Serializable]
    public class BadLineCodeException : Exception
    {
        public int lineID;
        public int? Code;
        public BadLineCodeException(int lineId, int? lineCode=null) : base()
        {
            lineID = lineId;
            Code = lineCode;
        }
        public BadLineCodeException(int lineId, int? lineCode, string message) : base(message)
        {
            lineID = lineId;
            Code = lineCode;
        }
        public BadLineCodeException(int lineId, int? lineCode, string message, Exception innerException) : base(message, innerException)
        {
            lineID = lineId;
            Code = lineCode;
        }
        public override string ToString() => base.ToString() + $",bad Line Code Number: {Code}";
    }

    #endregion
}
