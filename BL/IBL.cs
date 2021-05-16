using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLAPI
{
    /// <summary>
    /// Interface of the BL-logic layer
    /// </summary>
    public interface IBL
    {
        #region Station 
        /// <summary>
        /// adds new instance of BO.station
        /// </summary>
        /// <param name="s"></param>
        void AddStation(BO.Station s);
        /// <summary>
        /// updates properties of the BO.station instance
        /// </summary>
        /// <param name="s"></param>
        void UpdateStation(BO.Station s);
        /// <summary>
        /// deletes the BO.station instance
        /// </summary>
        /// <param name="stationKey"></param>
        void DeleteStation(int stationKey);
        /// <summary>
        /// deletes all instances of the BO.stations 
        /// </summary>
        void DeleteAllStation();
        /// <summary>
        /// returns a collection of all the BO.stations
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.Station> GetAllStations();
        /// <summary>
        /// returns a collection of all the BO.stations by the presicate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> p);
        /// <summary>
        /// returns a BO.station by the key with adjacents stations
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        BO.Station GetStationWithAdjacents(int stationKey);
        /// <summary>
        /// returns a BO.station by the key
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        BO.Station GetStation(int stationKey);

        #endregion

        #region AdjacentStation
        /// <summary>
        /// deletes Adjacent station by the BO.Adjacent Station and code
        /// </summary>
        /// <param name="adj"></param>
        /// <param name="code"></param>
        void DeleteAdjStation(BO.AdjacentStation adj, int code);
        BO.AdjacentStation GetAdjacentStations(int code1, int code2);
        void AddAdjacentStations(BO.AdjacentStation adj, int code2);
        void UpdateAdjacentStations(BO.AdjacentStation adj, int code2);
        #endregion

        #region Bus 
        /// <summary>
        /// returns the BO.bus instance by the license number
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <returns></returns>
        BO.Bus GetBus(string licensNumber);
        /// <summary>
        /// returns all the BO.busses instances
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.Bus> GetAllBus();
        /// <summary>
        /// returns all the BO.busses instances by the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<BO.Bus> GetBusesBy(Predicate<BO.Bus> predicate);
        /// <summary>
        /// adds a new BO.bus instance
        /// </summary>
        /// <param name="bus"></param>
        void AddBus(BO.Bus bus);
        /// <summary>
        /// updates an exist BO.bus instance
        /// </summary>
        /// <param name="bus"></param>
        void UpdateBusDetails(BO.Bus bus);
        /// <summary>
        /// deletes a BO.bus instance by the license number
        /// </summary>
        /// <param name="licensNumber"></param>
        void DeleteBus(string licensNumber);
        /// <summary>
        /// delets all the BO.busses instances
        /// </summary>
        void DeleteAllBuses();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="km"></param>
        /// <returns></returns>
        string Risky(BO.Bus bus, int km);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        void Treatment(BO.Bus bus);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        void Refuel(BO.Bus bus);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="kmDrive"></param>
        void Drive(BO.Bus bus, int kmDrive);
        #endregion

        #region User
        /// <summary>
        /// adds a new BO.user instance
        /// </summary>
        /// <param name="user"></param>
        void AddUser(BO.User user);
        /// <summary>
        /// updates an exist BO.user instance
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(BO.User user);
        /// <summary>
        /// deletes an exist BO.user instance by the user name
        /// </summary>
        /// <param name="userName"></param>
        void DeleteUser(string userName);
        /// <summary>
        /// returns an exist BO.user instance by the user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        BO.User GetUser(string userName);
        #endregion

        #region BusOnTrip
        /// <summary>
        /// adds a new BO.bus on trip instance
        /// </summary>
        /// <param name="busOn"></param>
        void AddBusOnTrip(BO.BusOnTrip busOn);
        /// <summary>
        /// updates an exist BO.bus on trip instance
        /// </summary>
        /// <param name="busOn"></param>
        void UpdateBusOnTrip(BO.BusOnTrip busOn);
        /// <summary>
        /// deletes an exist BO.bus on trip instance by the ID
        /// </summary>
        /// <param name="id"></param>
        void DeleteBusOnTrip(int id);
        /// <summary>
        /// returns the BO.Bus On Trip user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BO.BusOnTrip GetUser(int id);
        /// <summary>
        /// returns all the BO.busses on trip instances
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.BusOnTrip> GetAllBusOnTrip();
        /// <summary>
        /// returns all the BO.busses on trip instances by a license number
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <returns></returns>
        IEnumerable<BO.BusOnTrip> GetAllBusTrips(string licensNumber);
        /// <summary>
        ///  returns all the BO.busses on trip instances by the linenumber
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IEnumerable<BO.BusOnTrip> GetAllLineTrips(int line);
        /// <summary>
        /// returns a the BO.bus on trip instance ID
        /// </summary>
        /// <returns></returns>
        int GetBusOnId();
        /// <summary>
        /// copy from BO.bus 1 to BO.bus 2
        /// </summary>
        #endregion

        #region Line
        /// <summary>
        /// adds new instance of BO.bus line
        /// </summary>
        /// <param name="line"></param>
        void AddLine(BO.Line line);
        /// <summary>
        /// updates properties of the BO.bus line instance
        /// </summary>
        /// <param name="line"></param>
        void UpdateLine(BO.Line line);
        /// <summary>
        /// updates properties of the BO.bus line instance by the ID and anction
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="update"></param>
        void UpdateLine(int lineId, Action<BO.Line> update);
        /// <summary>
        ///  deletes the BO.bus line instance
        /// </summary>
        /// <param name="lineCode"></param>
        void DeleteLine(int lineCode);
        /// <summary>
        /// deletes all the BO.busses line instances
        /// </summary>
        void DeleteAllLines();
        /// <summary>
        /// returns the BO.bus line instance
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        BO.Line GetLine(int lineId);
        /// <summary>
        /// returns all the BO.bus line instance
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.Line> GetAllLines();
        /// <summary>
        /// returns all the BO.bus line instance by the predicate
        /// </summary>
        /// <param name="pred"></param>
        /// <returns></returns>
        IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> pred);

        void UpdateLineId();
        int GetLineId();
        #endregion

        #region LineStation

        /// <summary>
        /// deletes the BO.line station instance by the predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteLineStationsBy(Predicate<BO.LineStation> predicate);
        /// <summary>
        /// gets the ID of a BO.line station instance
        /// </summary>
        /// <returns></returns>
        int GetLineStationId();
        /// <summary>
        /// updates the ID of a BO.line station instance
        /// </summary>
        void UpdateLineStationId();
        /// <summary>
        ///  adds new instance of BO.line station
        /// </summary>
        /// <param name="s"></param>
        void AddLineStation(BO.LineStation s);
        /// <summary>
        /// returns all the BO.line stations instances
        /// </summary>
        /// <param name="LineId"></param>
        /// <returns></returns>
        IEnumerable<BO.LineStation> GetAllLineStations(int LineId);
        /// <summary>
        /// returns the BO.line that pass in the station
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        IEnumerable<BO.Line> GetLinePassInStation(int stationKey);
        /// <summary>
        /// returns the BO.line stations instances with no condition
        /// </summary>
        /// <returns></returns>
        IEnumerable<BO.LineStation> GetAllLineStationsNoCondition();
        /// <summary>
        /// returns the travel time between 2 BO.stations
        /// </summary>
        /// <param name="StatCode1"></param>
        /// <param name="intStatCode2"></param>
        /// <param name="LineId"></param>
        /// <returns></returns>
        TimeSpan GetTimeBetweenStations(int StatCode1, int intStatCode2, int LineId);
        Double GetDistanceBetweenStations(int StatCode1, int StatCode2, int LineId);
        #endregion

    }
}