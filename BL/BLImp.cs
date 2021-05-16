using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using System.Threading;
using BLAPI;
using DALAPI;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace BL
{
    /// <summary>
    /// Implement the interface of BL layer using the interface of the DL layer
    /// </summary>
    class BLImp : IBL 
    {
        IDL dl = DLFactory.GetDL();

        #region Station 

        private BO.Station StationFromDoToBo(DO.Station station)
        {
            BO.Station BoStation = (BO.Station)station.CopyPropertiesToNew(typeof(BO.Station));
            BoStation.Location = new BO.Location(station.Latitude, station.Longitude);
            return BoStation;
        }
        private BO.Station StationFromDoToBoAdjacents(DO.Station station1)
        {
            BO.Station result = StationFromDoToBo(station1);
            IEnumerable<DO.AdjacentStations> DoAdjacents = dl.GetAllAdjacentStationsBy(ad => ad.StationCode1 == station1.Code || ad.StationCode2 == station1.Code);
            result.AdjacentStations = from item in DoAdjacents select AdjFromDoToBo(item, item.StationCode1 == station1.Code ? item.StationCode2 : item.StationCode1);//דואגים שתמיד התחנה שלנו תיהיה ראשונה בעוקבות ;//המרה של כל תחנה עוקבת 
            return result;

        }
        private DO.Station StationFromBoToDo(BO.Station station)
        {
            DO.Station DoStation = (DO.Station)station.CopyPropertiesToNew(typeof(DO.Station));
            DoStation.Latitude = station.Location.Latitude;
            DoStation.Longitude = station.Location.Longitude;
            return DoStation;
        }
        public void AddStation(BO.Station s)
        {
            //from BO to DO
            DO.Station DOstation = StationFromBoToDo(s);
            try
            {
                dl.AddStation(DOstation);
                foreach (BO.AdjacentStation item in s.AdjacentStations)
                {
                    dl.AddAdjacentStations(AdjFromBoToDo(item, s.Code));
                }
            }
            catch (DO.BadStationKeyException e)
            {
                throw new BO.BadStationKeyException(s.Code, e.Message);
            }

        }
        public void UpdateStation(BO.Station s)
        {
            //from BO to DO
            DO.Station DOstation = StationFromBoToDo(s);
            try
            {
     
                dl.UpdateStation(DOstation);
                foreach (var item in s.AdjacentStations)
                    dl.AddAdjacentStations(AdjFromBoToDo(item, s.Code));

            }
            catch (DO.BadStationKeyException e)
            {
                throw new BO.BadStationKeyException(s.Code, e.Message);
            }
        }
        public void DeleteStation(int stationKey)
        {
            try
            {
                dl.DeleteAllAdjOfStation(stationKey);
                dl.DeleteStation(stationKey);

            }
            catch (DO.BadStationKeyException e)
            {
                throw new BO.BadStationKeyException(stationKey, e.Message);
            }
        }
        public void DeleteAllStation()
        {
            dl.DeleteAllAdjacentStations();
            dl.DeleteAllStation();
        }
        public IEnumerable<BO.Station> GetAllStations()
        {
            return (from station in dl.GetAllStations()
                    select StationFromDoToBo(station));
        }
        public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> p)
        {
            IEnumerable<BO.Station> stations = GetAllStations();
            //עבור כל תחנה שמקיימת את הפרדיקט הוא מחזיר אותה  
            return from station in stations
                   where p(station)
                   select station;
        }
        public BO.Station GetStation(int stationKey)
        {
            try
            {
                DO.Station DoStation = dl.GetStation(stationKey);
                return StationFromDoToBo(DoStation);
            }
            catch (DO.BadStationKeyException e)
            {
                throw new BO.BadStationKeyException(stationKey, e.Message);
            }

        }
        public BO.Station GetStationWithAdjacents(int stationKey)
        {
            try
            {
                DO.Station DoStation = dl.GetStation(stationKey);
                return StationFromDoToBoAdjacents(DoStation);
            }
            catch (DO.BadStationKeyException e)
            {
                throw new BO.BadStationKeyException(stationKey, e.Message);
            }
        }
        #endregion

        #region AdjacentStation

        private BO.AdjacentStation AdjFromDoToBo(DO.AdjacentStations adjS, int code)
        {
            BO.AdjacentStation result = (BO.AdjacentStation)adjS.CopyPropertiesToNew(typeof(BO.AdjacentStation));
            result.AdjStation = GetStation(code);
            return result;
        }
        private DO.AdjacentStations AdjFromBoToDo(BO.AdjacentStation adj, int code)
        {
            DO.AdjacentStations result = (DO.AdjacentStations)adj.CopyPropertiesToNew(typeof(DO.AdjacentStations));
            result.StationCode1 = code;
            result.StationCode2 = adj.AdjStation.Code;
            return result;
        }
        public void AddAdjacentStations(BO.AdjacentStation adj, int code2)
		{
			try
			{
            dl.AddAdjacentStations(AdjFromBoToDo(adj, code2));
			}
			catch (DO.BadAdjacentStationsException)
			{
                throw new BO.BadAdjacentStationsException(adj);
			}
		}
        public void UpdateAdjacentStations(BO.AdjacentStation adj, int code2)
        {
            try
            {
                dl.UpdateAdjacentStations(AdjFromBoToDo(adj, code2));
            }
            catch (DO.BadAdjacentStationsException)
            {
                throw new BO.BadAdjacentStationsException(adj);
            }
        }

        public void DeleteAdjStation(BO.AdjacentStation adj, int code)
        {
            try
            {
                dl.DeleteAdjacentStations(code, adj.AdjStation.Code);
            }
            catch (DO.BadAdjacentStationsException e)
            {
                throw new BO.BadAdjacentStationsException(adj, e.Message);
            }
        }
        public BO.AdjacentStation GetAdjacentStations(int code1, int code2)
        {
			try
			{
               return AdjFromDoToBo(dl.GetAdjacentStations(code1, code2),code1);
			}
			catch (DO.BadAdjacentStationsException)
			{
                throw new BO.BadAdjacentStationsException(null);
			}
              
        }
        #endregion

        #region Bus


        
       
        BO.Bus BusDoBoAdapter(DO.Bus busDO)
        {
            BO.Bus busBO = new BO.Bus();
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }

        DO.Bus BusBoDoAdapter(BO.Bus busBO)
        {
            DO.Bus busDO = new DO.Bus();
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }
        public void DeleteBus(string licensNumber)
        {
            try
            {
                dl.DeleteBus(licensNumber);
            }
            catch (DO.BadBusLicenseNumberException ex)
            {
                throw new BO.BadLicensNumberException(ex.Message, ex);
            }
        }
        public IEnumerable<BO.Bus> GetAllBus()
        {
            return from item in dl.GetAllBuses()
                   select BusDoBoAdapter(item);
        }
        public BO.Bus GetBus(string licensNumber)
        {
            DO.Bus busDO = null;
            try
            {
                busDO = dl.GetBus(licensNumber);
            }
            catch (DO.BadBusLicenseNumberException ex)
            {
                throw new BO.BadLicensNumberException(ex.Message, ex);
            }
            return BusDoBoAdapter(busDO);
        }
        public IEnumerable<BO.Bus> GetBusesBy(Predicate<BO.Bus> predicate)
        {
            IEnumerable<BO.Bus> buses = GetAllBus();
            return from bus in buses
                   where predicate(bus)
                   select bus;
        }
        public void UpdateBusDetails(BO.Bus bus)
        {
            try
            {
                dl.UpdateBus(BusBoDoAdapter(bus));

            }

            catch (DO.BadBusLicenseNumberException ex)
            {
                throw new BO.BadLicensNumberException(ex.Message, ex);
            }
        }
        public void AddBus(BO.Bus bus)
        {
            try
            {
                dl.AddBus(BusBoDoAdapter(bus));
            }

            catch (DO.BadBusLicenseNumberException ex)
            {
                throw new BO.BadLicensNumberException(ex.Message, ex);
            }
            catch { }

        }
        public void DeleteAllBuses()
        {
            dl.DeleteAllBuses();
        }
        public string Risky(BO.Bus bus, int km)//check if its ok to drive
        {
            if (bus.Fuel < km)//chek if there is enough feul for the drive
            {
                if (bus.Fuel <= 5)
                {
                    bus.State = Status.NeedRefuel;
                }
                return "There is not enough fuel for the trip ";
            }
            if (bus.KmOfTreatment + km >= 20000)
            {//check if it ok to drive
                bus.State = Status.NeedTreatment;
                return "Time for treatment";
            }
            if (bus.DateOfTreatment <= DateTime.Now.AddYears(-1))//Checks if more than a year has passed since the last treatment
            {
                bus.State = Status.NeedTreatment;
                //throw new Exception("Time for treatment, more then year from the last");
            }
            else bus.State = Status.Ready;
            return "OK";
        }
        public void Treatment(BO.Bus bus)//Update the current date, and the trip on which the treatment was performed.
        {
            bus.DateOfTreatment = DateTime.Now;
            bus.KmOfTreatment = 0;
        }
        public void Refuel(BO.Bus bus)  //Fuel update
        {
            bus.Fuel = 1200;
        }
        public void Drive(BO.Bus bus, int kmDrive)//take the bus for drive 
        {
            if (Risky(bus, kmDrive) == "OK")
            {
                //Fuel and km update   
                bus.KmSum += kmDrive;
                bus.Fuel -= kmDrive;
                bus.KmOfTreatment += kmDrive;

            }
            else throw new ArgumentException("Can not drive!");
        }
        #endregion

        #region User
        DO.User UserBoDoAdapter(BO.User userBO)
        {
            DO.User userDO = new DO.User();
            userBO.CopyPropertiesTo(userDO);
            return userDO;
        }
        BO.User UserDoBoAdapter(DO.User userDO)
        {
            BO.User userBO = new BO.User();
            userDO.CopyPropertiesTo(userBO);
            return userBO;
        }
        public void AddUser(BO.User user)
        {
            try
            {
                dl.AddUser(UserBoDoAdapter(user));
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BO.BadUserNameException(ex.Message, ex);
            }
            catch { }
        }
        public void UpdateUser(BO.User user)
        {
            try
            {
                dl.UpdateUser(UserBoDoAdapter(user));
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BO.BadUserNameException(ex.Message, ex);
            }
        }
        public void DeleteUser(string userName)
        {

            try
            {
                dl.DeleteUser(userName);
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BO.BadUserNameException(ex.Message, ex);
            }

        }
        public BO.User GetUser(string userName)
        {
            DO.User userDO = null;
            try
            {
                userDO = dl.GetUser(userName);
            }
            catch (DO.BadUserNameException ex)
            {
                throw new BO.BadUserNameException(ex.Message, ex);
            }
            return UserDoBoAdapter(userDO);
        }


        #endregion

        #region BusOnTrip
        DO.BusOnTrip BusOnTripBoDoAdapter(BO.BusOnTrip busOnBO)
        {
            DO.BusOnTrip busOnDO = new DO.BusOnTrip();
            busOnBO.CopyPropertiesTo(busOnDO);
            busOnDO.ActualTake = busOnBO.ActualTakeOff.ToString();
            return busOnDO;
        }
        BO.BusOnTrip BusOnTripDoBoAdapter(DO.BusOnTrip busOnDO)
        {
            BO.BusOnTrip busOnBO = new BO.BusOnTrip();
            busOnDO.CopyPropertiesTo(busOnBO);
            busOnBO.ActualTakeOff = TimeSpan.Parse(busOnDO.ActualTake);
            return busOnBO;
        }
        public void AddBusOnTrip(BusOnTrip busOn)
        {
            try
            {
                dl.AddBusOnTrip(BusOnTripBoDoAdapter(busOn));
                dl.UpdateBusOnId();
            }
            catch (DO.BadIdBUTException ex)
            {
                throw new BO.BadIDBusONeException(ex.Message, ex);
            }
        }


        public void UpdateBusOnTrip(BusOnTrip busOn)
        {
            try
            {
                dl.UpdateBusOnTrip(BusOnTripBoDoAdapter(busOn));
            }
            catch (DO.BadIdBUTException ex)
            {
                throw new BO.BadIDBusONeException(ex.Message, ex);
            }
        }

        public void DeleteBusOnTrip(int id)
        {
            try
            {
                dl.DeleteBusOnTrip(id);
            }
            catch (DO.BadIdBUTException ex)
            {
                throw new BO.BadIDBusONeException(ex.Message, ex);
            }
        }

        public BusOnTrip GetUser(int id)
        {
            try
            {
                return BusOnTripDoBoAdapter(dl.GetBusOnTrip(id));
            }
            catch (DO.BadIdBUTException ex)
            {
                throw new BO.BadIDBusONeException(ex.Message, ex);
            }
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTrip()
        {

            return from item in dl.GetAllBusOnTrip()
                   select BusOnTripDoBoAdapter(item);
        }

        public IEnumerable<BusOnTrip> GetAllBusTrips(string licensNumber)
        {
            return from item in dl.GetAllBusTrips(licensNumber)
                   select BusOnTripDoBoAdapter(item);
        }

        public IEnumerable<BusOnTrip> GetAllLineTrips(int line)
        {
            return from item in dl.GetAllLineTrips(line)
                   select BusOnTripDoBoAdapter(item);
        }

        public int GetBusOnId()
        {
            return dl.GetBusOnId();
        }





        #endregion

        #region Line
        public int GetLineId()
        {
            return dl.GetLineId();
        }
        public void UpdateLineId()
        {
            dl.UpdateLineId();
        }
        private BO.Line LineDoBoAdapter(DO.Line doLine)
        {
			var busLine = (BO.Line)doLine.CopyPropertiesToNew(typeof(BO.Line));

			List<BO.LineStation> route = new List<BO.LineStation>();
			for (int i = 0; i < doLine.RouteLength; i++)
			{
				var doLineStation = dl.GetLineStationByIndex(busLine.ID, i);
                if (doLineStation == null) { }
				else if (i == 0)
				{
					route.Add(LineStationDoToBo(doLineStation));
				}
				else
				{
					route.Add(LineStationDoToBo(doLineStation, route[i - 1]));
				}
			}
			busLine.StationsRoute = route;
			return busLine;
		}
        private IEnumerable<BO.LineStation> GetRouteStationsFromDOLine(DO.Line doLine)
        {
            return from item1 in dl.GetAllLineStations(doLine.ID)
                   orderby item1.IndexInLine
                   select LineStationDoToBo(item1);
        }
        private DO.Line LineBoDoAdapter(BO.Line boLine)
        {
            DO.Line doLine = new DO.Line();
            boLine.CopyPropertiesTo(doLine);
            return doLine;
        }
        public void AddLine(BO.Line line)
        {
            DO.Line doLine = LineBoDoAdapter(line);
            try
            {
                dl.AddLine(doLine);
            }
            catch (DO.BadLineCodeException exp)
            {
                throw new BO.BadLineCodeException(line.ID, line.LineCode, exp.Message);
            }
            catch(DO.BadLineStationKeyException ex)
            {
                throw new BO.BadStationKeyException(ex.Id);
            }
        }
        public void UpdateLine(BO.Line line)
        {
            if(line.StationsRoute!=null&& line.StationsRoute.Count()>0)
            {
                int place = line.StationsRoute.Count() - 1;
                line.FirstStation = line.StationsRoute.ToList()[0].Station.Code;
                line.LastStation = line.StationsRoute.ToList()[place].Station.Code;
            }
            else
            {
                line.FirstStation = 0;
                line.LastStation = 0;
            }

            DO.Line doLine = LineBoDoAdapter(line);
            try
            {
                dl.UpdateLine(doLine);
            }
            catch (DO.BadLineCodeException exp)
            {
                throw new BO.BadLineCodeException(line.ID, line.LineCode, exp.Message);
            }
        }
        public void UpdateLine(int lineCode, Action<BO.Line> update)
        {
            throw new BO.BadLineCodeException(lineCode);
        }
        public void DeleteLine(int lineID)
        {
            try
            {
                dl.DeleteLine(lineID);
                dl.DeleteLineStationBy(st => st.LineId == lineID);
            }
            catch (DO.BadLineCodeException)
            {
                throw new BO.BadLineCodeException(lineID);
            }
        }
        public void DeleteAllLines() { dl.DeleteAllLines(); }
        public BO.Line GetLine(int lineId)
        {
            try
            {
                DO.Line doLine = dl.GetLine(lineId);
                return LineDoBoAdapter(doLine);
            }
            catch (DO.BadLineCodeException)
            {
                throw new BO.BadLineCodeException(lineId);
            }
        }
        public IEnumerable<BO.Line> GetAllLines()
        {
            return (from Line in dl.GetAllLines() select LineDoBoAdapter(Line));
        }
        public IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> pred)
        {
            IEnumerable<BO.Line> lines = GetAllLines();
            return from line in lines
                   where pred(line)
                   select line;
        }

        #endregion

        #region LineStation
        public void AddLineStation(BO.LineStation s)
        {
            try
            {
                dl.AddLineStation(LineStationBoDoAdapter(s));
            }

            catch (DO.BadLineStationKeyException ex)
            {

            }

        }
        public IEnumerable<BO.LineStation> GetAllLineStations(int LineId)
        {
            return from item in dl.GetAllLineStations(LineId)
                   select LineStationDoToBo(item);
        }
        public TimeSpan GetTimeBetweenStations(int StatCode1, int StatCode2, int LineId)
        {
            TimeSpan sum = TimeSpan.Zero;
            if (StatCode1==StatCode2)
             sum = new TimeSpan(0,0,15);
            BO.Line l = GetLine(LineId);
            int place = 0;
            if (l.StationsRoute.Count() > 1)
            {
                while (l.StationsRoute.ToList()[place].Station.Code != StatCode1) { place++; }
                while (l.StationsRoute.ToList()[place].Station.Code != StatCode2 && place < l.StationsRoute.Count() - 1) { sum += l.StationsRoute.ToList()[place].DrivingTime; place++; }
                if (l.StationsRoute.ToList()[place].Station.Code != StatCode2)
                    return TimeSpan.Zero;
            }
            return sum;
        }
        public Double GetDistanceBetweenStations(int StatCode1, int StatCode2, int LineId)
        {
            double sum = 0;
            BO.Line l = GetLine(LineId);
            int place = 0;
            if (l.StationsRoute.Count() <= 1)
                return 0;
            while (l.StationsRoute.ToList()[place].Station.Code != StatCode1) { place++; }
            while (l.StationsRoute.ToList()[place].Station.Code != StatCode2 && place < l.StationsRoute.Count() - 1) { sum += l.StationsRoute.ToList()[place].Distance; place++; }
            return sum;
        }
        public int GetLineStationId()
        {
            return dl.GetLineStationId();
        }
        public void UpdateLineStationId()
		{
            dl.UpdateLineStationId();
		}
        public DO.LineStation LineStationBoDoAdapter(BO.LineStation LineStationBO)
        {
            DO.LineStation LineStationDO = new DO.LineStation();
            LineStationBO.CopyPropertiesTo(LineStationDO);
            LineStationDO.StationCode = LineStationBO.Station.Code;
            return LineStationDO;
        }
        public IEnumerable<LineStation> GetAllLineStationsNoCondition()
        {
            return from item in dl.GetAllLineStationsNoCondition()
                   select LineStationDoToBo(item);
        }
        public IEnumerable<BO.Line> GetLinePassInStation(int stationKey)
        {
            List<BO.Line> l = new List<Line>();
            foreach (var item in GetAllLineStationsNoCondition())
            {
                if(item.Station.Code==stationKey)
                {
                    l.Add(GetLine(item.LineId));
                }
            }
            return l;
        }

        private BO.LineStation LineStationDoToBo(DO.LineStation doLineStation, BO.LineStation prevStation = null)
        {
            if (prevStation != null)
            {
                try
                {
                    var doAdjStations = dl.GetAdjacentStations(prevStation.Station.Code, doLineStation.StationCode);
                    prevStation.Distance = doAdjStations.Distance;
                    prevStation.DrivingTime = doAdjStations.AverageTime;
                }
                catch (DO.BadLineStationKeyException) { }
            }

            return new BO.LineStation
            {
                Station = GetStation(doLineStation.StationCode),
                DrivingTime = TimeSpan.Zero,
                Distance = 0,
                ID = doLineStation.ID,
                LineId = doLineStation.LineId,
                IndexInLine = doLineStation.IndexInLine
            };
        }
        public void DeleteLineStationsBy(Predicate<BO.LineStation> predicate)
        {
            var allLS = from ls in GetAllLineStationsNoCondition() 
                        where predicate(ls) 
                        select ls;
            foreach (var toDelete in allLS.ToArray())
            {
                dl.DeleteLineStationByStation(toDelete.Station.Code);

                var doLine = dl.GetLine(toDelete.LineId);
                for (int i = toDelete.IndexInLine + 1; i < doLine.RouteLength; i++)
                {
                    var doLs = dl.GetLineStationByIndex(toDelete.LineId, i);
                    doLs.IndexInLine -= 1;
                    dl.UpdateLineStationByIndex(doLs);
                }
                doLine.RouteLength -= 1;
                dl.UpdateLine(doLine);
            }
        }

        #endregion

    }
}