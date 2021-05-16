using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Exception of the data base layer
/// </summary>
namespace DO
{
    [Serializable]
    #region StationException
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

    public class BadLineStationKeyException : Exception
    {
        public int Id;
        public int? Code;
        public BadLineStationKeyException(int id,int? code=null) : base()
        {
            Code = code;
            Id = id;
        }
        public BadLineStationKeyException(int id,int code, string message) : base(message)
        {
            Code = code;
            Id = id;
        }
           
        public BadLineStationKeyException(int id,int code, string message, Exception innerException) :base(message, innerException)
        {
            Code = code;
            Id = id;
        }
        public override string ToString() => base.ToString() + $", The bus is already passing through the station: {Id}";
    }
    public class LineStationAlreadyExistException : Exception
    {
        public int Id;
        public int? Code;
        public LineStationAlreadyExistException(int id, int code) : base()
        {
            Code = code;
            Id = id;
        }
        public LineStationAlreadyExistException(int id, int code, string message) : base(message)
        {
            Code = code;
            Id = id;
        }

        public LineStationAlreadyExistException(int id, int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
            Id = id;
        }
        public override string ToString() => base.ToString() + $", The bus is already passing through the station: {Id}";
    }
    public class BadAdjacentStationsException : Exception
    {
        public int Code1;
        public int Code2;
        public BadAdjacentStationsException(int code1,int code2):base()
        {
            Code1 = code1;
            Code2 = code2;
        }
        public BadAdjacentStationsException(int code1,int code2, string message) : base(message)
        {
            Code1 = code1;
            Code2 = code2;
        }

        public BadAdjacentStationsException(int code1,int code2, string message, Exception innerException) : base(message, innerException)
        {
            Code1 = code1;
            Code2 = code2;
        }
        public override string ToString() => base.ToString() + ", bad AdjacentStations";
    }
    #endregion

    #region BusException
    public class BadBusLicenseNumberException : Exception
    {
        public string Code;
        public BadBusLicenseNumberException(string licensNumber) : base() => Code = licensNumber;
        public BadBusLicenseNumberException(string licensNumber, string message) :
            base(message) => Code = licensNumber;
        public BadBusLicenseNumberException(string licensNumber, string message, Exception innerException) :
              base(message, innerException) => Code = licensNumber;
        public override string ToString() => base.ToString() + $", bad Licens Number: {Code}";
    }
    #endregion

    #region UserException
    public class BadUserNameException:Exception
    {
        public string Code;
        public BadUserNameException(string userName) : base() => Code = userName;
        public BadUserNameException(string userName, string message): base(message) => Code = userName;
        public BadUserNameException(string userName, string message, Exception innerException) :
              base(message, innerException) => Code = userName;
        public override string ToString() => base.ToString() + $", bad User name: {Code}";
        

    }
    #endregion

    #region BusOnTripException
    public class BadIdBUTException : Exception
    {
        public int ID;
        public BadIdBUTException(int id) : base() => ID = id;
        public BadIdBUTException(int id, string message) :
            base(message) => ID = id;
        public BadIdBUTException(int id, string message, Exception innerException) :
              base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id of bus on trip: {ID}";
    }
    #endregion

    #region LineException

    public class BadLineCodeException : Exception
    {
        public int? Code;
        public int Id;
        private int lineCode;
        private string v;

        public BadLineCodeException(int id, int? lineCode=null) : base()
        {
            Code = lineCode;
            Id = id;
        }

        public BadLineCodeException(int lineCode, string v)
        {
            this.lineCode = lineCode;
            this.v = v;
        }

        public BadLineCodeException(int id,int? lineCode, string message) : base(message)
        {
            Code = lineCode;
            Id = id;
        }

        public BadLineCodeException(int id, int? lineCode, string message, Exception innerException) : base(message, innerException)
        {
            Code = lineCode;
            Id = id;
        }

        public override string ToString() => base.ToString() + $",bad Line Code Number: {Code}";
    }

    #endregion

    #region XMLException
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }
    #endregion
}