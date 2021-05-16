using System;
using System.Collections.Generic;
using DO;



namespace DALAPI
{
    /// <summary>
    /// Database interface for the logic layer
    /// </summary>
    public interface IDL
    {
        #region Station  

        /// <summary>
        /// adds new instance of DO.station
        /// </summary>
        /// <param name="s"></param>
        void AddStation(DO.Station s);
        /// <summary>
        /// updates properties of the DO.station instance
        /// </summary>
        /// <param name="s"></param>
        void UpdateStation(DO.Station s);
        /// <summary>
        /// updates specific properties of the DO.station instance by the action
        /// </summary>
        /// <param name="code"></param>
        /// <param name="update"></param>
        void UpdateStation(int code, Action<DO.Station> update);
        /// <summary>
        /// deletes the DO.station instance
        /// </summary>
        /// <param name="stationKey"></param>
        void DeleteStation(int stationKey);
        /// <summary>
        /// deletes all instances of the DO.stations 
        /// </summary>
        void DeleteAllStation();
        /// <summary>
        /// returns a collection of all the DO.stations
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Station> GetAllStations();
        /// <summary>
        /// returns a collection of all the DO.stations by the presicate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> p);
        /// <summary>
        /// returns a DO.station by the key
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        DO.Station GetStation(int stationKey);
        #endregion

        #region AdjacentStations  
        /// <summary>
        /// adds new instance of DO.Adjacent Stations
        /// </summary>
        /// <param name="s"></param>
        void AddAdjacentStations(DO.AdjacentStations s);
        /// <summary>
        /// adds an exist instance of DO.Adjacent Stations
        /// </summary>
        /// <param name="s"></param>
        void UpdateAdjacentStations(DO.AdjacentStations s);
        /// <summary>
        /// deletes a pair of DO.Adjacent Stations
        /// </summary>
        /// <param name="code1"></param>
        /// <param name="code2"></param>
        void DeleteAdjacentStations(int code1, int code2);
        /// <summary>
        /// deletes all the DO.adjacent stations of a particular station
        /// </summary>
        /// <param name="code"></param>
        void DeleteAllAdjOfStation(int code);
        /// <summary>
        /// deletes all the DO.adjacent stations
        /// </summary>
        void DeleteAllAdjacentStations();
        /// <summary>
        /// gets all the DO.adjacent stations
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        /// <summary>
        /// gets all the DO.adjacent stations by a predicate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> p);
        /// <summary>
        ///  gets a pair of DO.Adjacent Stations
        /// </summary>
        /// <param name="code1"></param>
        /// <param name="code2"></param>
        /// <returns></returns>
        DO.AdjacentStations GetAdjacentStations(int code1, int code2);
        #endregion

        #region LineStation 

        /// <summary>
        /// gets the ID of a DO.line station instance
        /// </summary>
        /// <returns></returns>
        int GetLineStationId();
        /// <summary>
        /// updates the id of a DO.line station instance
        /// </summary>
        void UpdateLineStationId();
        /// <summary>
        /// adds new instance of DO.line station
        /// </summary>
        /// <param name="s"></param>
        void AddLineStation(DO.LineStation s);
        /// <summary>
        /// updates the DO.line station instance by the station
        /// </summary>
        /// <param name="s"></param>
        void UpdateLineStationByStation(DO.LineStation s);
        /// <summary>
        /// updates the DO.line station instance by the index
        /// </summary>
        /// <param name="s"></param>
        void UpdateLineStationByIndex(DO.LineStation s);
        /// <summary>
        /// deletes the DO.line station instance by the station key
        /// </summary>
        /// <param name="stationKey"></param>
        void DeleteLineStationByStation(int stationKey);
        /// <summary>
        /// deletes the DO.line station instance by the predicate
        /// </summary>
        /// <param name="p"></param>
        void DeleteLineStationBy(Predicate<DO.LineStation> p);
        /// <summary>
        /// deletes all the DO.line stations instances
        /// </summary>
        /// <param name="LineId"></param>
        void DeleteAllLineStations(int LineId);
        /// <summary>
        /// returns all the DO.line stations instances
        /// </summary>
        /// <param name="LineId"></param>
        /// <returns></returns>
        IEnumerable<DO.LineStation> GetAllLineStations(int LineId);
        /// <summary>
        /// returns the DO.line station instance by the predicate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        IEnumerable<DO.LineStation> GetLineStationsBy(Predicate<DO.LineStation> p);
        /// <summary>
        /// returns the DO.line station instance by the station key and predicate
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        IEnumerable<LineStation> GetLineStationsBy(int LineId, Predicate<DO.LineStation> p);
        /// <summary>
        /// returns the DO.line station instance by the line ID and the station key
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        DO.LineStation GetLineStationByStation(int LineId, int stationKey);
        /// <summary>
        /// returns the DO.line station instance by the line ID and sline code
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="codeLine"></param>
        /// <returns></returns>
        DO.LineStation GetLineStationByIndex(int LineId, int codeLine);
        /// <summary>
        /// returns the DO.line stations instances with no condition
        /// </summary>
        /// <returns></returns>
        IEnumerable<LineStation> GetAllLineStationsNoCondition();

        #endregion

        #region Bus
        /// <summary>
        ///  returns all the DO.busses instances
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Bus> GetAllBuses();
        /// <summary>
        ///  returns all the DO.busses instances by the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate);
        /// <summary>
        ///  returns the DO.bus instance by the license number
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <returns></returns>
        DO.Bus GetBus(string licensNumber);
        /// <summary>
        ///  adds a new DO.bus instance
        /// </summary>
        /// <param name="bus"></param>
        void AddBus(DO.Bus bus);
        /// <summary>
        ///  updates an exist DO.bus instance
        /// </summary>
        /// <param name="bus"></param>
        void UpdateBus(DO.Bus bus);
        /// <summary>
        /// updates specific fields in a DO.bus
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="update"></param>
        void UpdateBus(string licensNumber, Action<DO.Bus> update);
        /// <summary>
        ///  delets a DO.bus instance by the license number
        /// </summary>
        /// <param name="licensNumber"></param>
        void DeleteBus(string licensNumber);
        /// <summary>
        ///  delets all the DO.busses instances
        /// </summary>
        void DeleteAllBuses();
        //  bool Risky(DO.Bus bus, int km);
        #endregion

        #region User
        /// <summary>
        ///  adds a new DO.user instance
        /// </summary>
        /// <param name="user"></param>
        void AddUser(DO.User user);
        /// <summary>
        /// updates an exist DO.user instance
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(DO.User user);
        /// <summary>
        /// deletes an exist DO.user instance by the user name
        /// </summary>
        /// <param name="userName"></param>
        void DeleteUser(string userName);
        /// <summary>
        ///  returns an exist DO.user instance by the user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DO.User GetUser(string userName);
        #endregion

        #region BusOnTrip
        /// <summary>
        ///  adds a new DO.bus on trip instance
        /// </summary>
        /// <param name="busOn"></param>
        void AddBusOnTrip(DO.BusOnTrip busOn);
        /// <summary>
        /// updates an exist DO.bus on trip instance
        /// </summary>
        /// <param name="busOn"></param>
        void UpdateBusOnTrip(DO.BusOnTrip busOn);
        /// <summary>
        ///  deletes an exist DO.bus on trip instance by the ID
        /// </summary>
        /// <param name="boId"></param>
        void DeleteBusOnTrip(int boId);
        /// <summary>
        ///  returns an exist DO.bus on trip instance by the ID 
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        DO.BusOnTrip GetBusOnTrip(int boId);
        /// <summary>
        ///  returns all the DO.busses on trip instances
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.BusOnTrip> GetAllBusOnTrip();
        /// <summary>
        /// returns all the DO.busses on trip instances by the license number
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <returns></returns>
        IEnumerable<DO.BusOnTrip> GetAllBusTrips(string licensNumber);
        /// <summary>
        /// returns all the DO.busses on trip instances by the line number
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IEnumerable<DO.BusOnTrip> GetAllLineTrips(int line);
        /// <summary>
        /// returns a the DO.bus on trip instance ID
        /// </summary>
        /// <returns></returns>
        int GetBusOnId();
        void UpdateBusOnId();
        #endregion

        #region Line 

        /// <summary>
        ///  adds new instance of DO.bus line 
        /// </summary>
        /// <param name="line"></param>
        void AddLine(DO.Line line);
        /// <summary>
        ///  updates properties of the DO.bus line instance
        /// </summary>
        /// <param name="line"></param>
        void UpdateLine(DO.Line line);
        /// <summary>
        /// deletes the DO.bus line instance
        /// </summary>
        /// <param name="lineId"></param>
        void DeleteLine(int lineId);
        /// <summary>
        /// deletes all the DO.busses line instances
        /// </summary>
        void DeleteAllLines();
        /// <summary>
        /// deletes a DO.bus line instance by the predicate
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteLinesBy(Predicate<DO.Line> predicate);
        /// <summary>
        /// returns the DO.bus line instance
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        DO.Line GetLine(int lineId);
        /// <summary>
        /// returns all the DO.bus line instance
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Line> GetAllLines();

        int GetLineId();
        void UpdateLineId();

        #endregion

    }
}