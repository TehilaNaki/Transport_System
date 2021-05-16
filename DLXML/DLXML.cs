using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using DO;
using DALAPI;

namespace DL
{
    public sealed class DLXML : IDL
    {
        //Using a design pattern of singelton
        #region singelton
        static readonly DLXML instance = new DLXML();
        /// <summary>
        /// static constructor to ensure instance init is done just before first use
        /// </summary>
        static DLXML() { }
        /// <summary>
        ///  constructor. default => private
        /// </summary>
        DLXML() { }
        /// <summary>
        /// the public Instance property for use. returns the instance
        /// </summary>
        public static DLXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files Path
        /// <summary>
        /// users XElement
        /// </summary>
        string UseresPath = @"UseresXml.xml";
        /// <summary>
        /// busses XMLSerializer
        /// </summary>
        string BusesPath = @"BusesXml.xml";
        /// <summary>
        /// stations XMLSerializer
        /// </summary>
        string StationsPath = @"StationsXml.xml";
        /// <summary>
        /// Adjacent Station XMLSerializer
        /// </summary>
        string AdjacentStationPath = @"AdjacentStationXml.xml";
        /// <summary>
        /// line station XMLSerializer
        /// </summary>
        string LineStationsPath = @"LineStationsXml.xml";
        /// <summary>
        /// bus on trp XMLSerializer
        /// </summary>
        string busOnTripPath = @"BusOnTripXml.xml";
        /// <summary>
        /// ID XMLSerializer
        /// </summary>
        string IDisPath = @"IDPath.xml";
        /// <summary>
        /// bus lines XMLSerializer
        /// </summary>
        string LinesPath = @"LinesPath.xml";
        #endregion

        #region User
        /// <summary>
        /// returns user by the user name from the file
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DO.User GetUser(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
           
            DO.User user = users.Find(u => u.UserName==userName && u.Available == true);
            if (user != null)
                return user; //no need to Clone()
            else
                throw new DO.BadUserNameException(userName, $"bad User Name: {userName}");

        }
        /// <summary>
        /// returns all users from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.User> GetAllUseres()
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
            return users.AsEnumerable();
        }
        /// <summary>
        /// returns all users by predicate from the file
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<DO.User> GetAllUseresBy(Predicate<DO.User> predicate)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
            return from u1 in users
                   where predicate(u1) && u1.Available == true
                   select u1;
        }
        /// <summary>
        /// adds new user to the file
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(DO.User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);
            DO.User user1 = (from u in users
                              where u.UserName == user.UserName
                              select u).FirstOrDefault();

            if (user1 != null)
                throw new DO.BadUserNameException(user.UserName, "Duplicate user name");
            users.Add(user);
            XMLTools.SaveListToXMLSerializer(users,UseresPath);

        }
        /// <summary>
        /// deletes user from the file
        /// </summary>
        /// <param name="userName"></param>
        public void DeleteUser(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);

            User user = (from u in users
                             where u.UserName == userName
                             select u).FirstOrDefault();

            if (user != null)
            {
                user.Available = false;
                users.Add(user);
                XMLTools.SaveListToXMLSerializer(users, UseresPath);
            }
            else
                throw new DO.BadUserNameException(userName, $"bad user name: {userName}");
        }
        /// <summary>
        /// updates user in the file
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(DO.User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(UseresPath);

            User user1 = (from u in users
                              where u.UserName == user.UserName
                              select u).FirstOrDefault();

            if (user1 != null)
            {
                users.Remove(user1);
                users.Add(user);
                XMLTools.SaveListToXMLSerializer(users, UseresPath);
            }
            else
                throw new DO.BadUserNameException(user.UserName, $"bad user name: {user.UserName}");
        }


        #endregion

        #region Bus 

        /// <summary>
        /// returns bus by the string license numbber from the file
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <returns></returns>
        public DO.Bus GetBus(string licensNumber)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);

            DO.Bus bus = ListBuses.Find(b => b.LicensNumber == licensNumber && b.Available == true);
            if (bus != null)
                return bus; //no need to Clone()
            else
                throw new DO.BadBusLicenseNumberException(licensNumber, $"bad Licens number: {licensNumber}");
        }
        /// <summary>
        /// adds new bus to the file
        /// </summary>
        /// <param name="bus"></param>
        public void AddBus(DO.Bus bus)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);

            if (ListBuses.FirstOrDefault(b => b.LicensNumber == bus.LicensNumber) != null)
                throw new DO.BadBusLicenseNumberException(bus.LicensNumber, "Duplicate Licens number");

            ListBuses.Add(bus); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListBuses, BusesPath);

        }
        /// <summary>
        /// returns all busses from the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DO.Bus> GetAllBuses()
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);

            return from bus in ListBuses
                   where bus.Available == true
                   select bus; //no need to Clone()
        }
        /// <summary>
        /// updates bus in the file
        /// </summary>
        /// <param name="bus"></param>
        public void UpdateBus(DO.Bus bus)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);

            DO.Bus bus1 = ListBuses.Find(b => b.LicensNumber == bus.LicensNumber);
            if (bus1 != null)
            {
                ListBuses.Remove(bus1);
                ListBuses.Add(bus); //no need to Clone()
            }
            else
                throw new DO.BadBusLicenseNumberException(bus.LicensNumber, $"bad licens number: {bus.LicensNumber}");

            XMLTools.SaveListToXMLSerializer(ListBuses, BusesPath);
        }
        /// <summary>
        /// deletes bus from file
        /// </summary>
        /// <param name="LicensNumber"></param>
        public void DeleteBus(string LicensNumber)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);

            DO.Bus bus = ListBuses.Find(b => b.LicensNumber == LicensNumber);

            if (bus != null)
            {
                ListBuses.Remove(bus);
                bus.Available = false;
                ListBuses.Add(bus);
            }
            else
                throw new DO.BadBusLicenseNumberException(LicensNumber, $"bad licens number: {LicensNumber}");

            XMLTools.SaveListToXMLSerializer(ListBuses, BusesPath);
        }
        /// <summary>
        /// returns all busses by the predicate from the file
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);

            IEnumerable<DO.Bus> buses = (from b in ListBuses
                                         where predicate(b)
                                         select b).ToList();
            if (buses != null)
                return buses; //no need to Clone()
            else
                return null;
        }
        /// <summary>
        /// updates bus in the file by the license number and action
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <param name="update"></param>
        public void UpdateBus(string licensNumber, Action<Bus> update)
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);
            DO.Bus bus1 = ListBuses.Find(b => b.LicensNumber == licensNumber);
            if (bus1 != null)
            {
                update(bus1);
                ListBuses.Remove(bus1);
                ListBuses.Add(bus1); //no need to Clone()
            }
            else
                throw new DO.BadBusLicenseNumberException(licensNumber, $"bad licens number: {licensNumber}");

            XMLTools.SaveListToXMLSerializer(ListBuses, BusesPath);
        }
        /// <summary>
        /// deletes all the busses in the file
        /// </summary>
        public void DeleteAllBuses()
        {
            List<Bus> ListBuses = XMLTools.LoadListFromXMLSerializer<Bus>(BusesPath);
            foreach (Bus b in ListBuses)
            {
                b.Available = false;
            }
            XMLTools.SaveListToXMLSerializer(ListBuses, BusesPath);
        }

        #endregion

        #region Stations 
        /// <summary>
        /// adds station to the file
        /// </summary>

        public void AddStation(Station s)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            var StationAdd = (from item in ListStation
                                   where item.Code == s.Code
                                   select item).FirstOrDefault();

            if (StationAdd != null && StationAdd.Available)
                throw new DO.BadStationKeyException(s.Code, "The station exists");

            if (StationAdd!=null &&!StationAdd.Available)
            {
                DeleteStation(s.Code);
            }
            ListStation.Add(s);

            XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
        }
        /// <summary>
        /// updates station in the file
        /// </summary>
        public void UpdateStation(Station s)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.Code == s.Code && item.Available
                              select item).FirstOrDefault();
            if (StationAdd != null)
            {
                ListStation.Remove(StationAdd);
                ListStation.Add(s);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            else throw new DO.BadStationKeyException(s.Code, $"The station: {s.Code} doesn't exist");
        }
        /// <summary>
        /// updates station by the code and action in the file 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="update"></param>
        public void UpdateStation(int code, Action<Station> update)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);

            var StationAdd = (from item in ListStation
                              where item.Code == code && item.Available
                              select item).FirstOrDefault();

            if (StationAdd != null)
            {    
                update(StationAdd);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
            throw new DO.BadStationKeyException(code, $"The station doesn't exist");
        }
        /// <summary>
        /// deletes station by the key number from the file
        /// </summary>
        /// <param name="stationKey"></param>
        public void DeleteStation(int stationKey)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            var StationDelete = (from item in ListStation
                              where item.Code == stationKey && item.Available
                              select item).FirstOrDefault();
            if (StationDelete != null)
            {
                ListStation.Remove(StationDelete);
                StationDelete.Available = false;
                ListStation.Add(StationDelete);
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
          else   throw new DO.BadStationKeyException(stationKey, $"The station: {stationKey} doesn't exist");
        }
        /// <summary>
        /// deletes all stations in file
        /// </summary>
        public void DeleteAllStation()
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            if (ListStation != null)
            {
                foreach (var item in ListStation)
                {
                    ListStation.Remove(item);
                    item.Available = false;    
                    ListStation.Add(item);
                }
                XMLTools.SaveListToXMLSerializer(ListStation, StationsPath);
            }
        }
        /// <summary>
        /// returns all stations in files
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetAllStations()
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from item in ListStation where item.Available
                   select item;
        }
        /// <summary>
        /// returns all stations by the predicate from file
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> p)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            return from item in ListStation
                where p(item)&&item.Available
                   select item;

        }
        /// <summary>
        /// returns station by the key number
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        public Station GetStation(int stationKey)
        {
            List<Station> ListStation = XMLTools.LoadListFromXMLSerializer<Station>(StationsPath);
            var s = (from item in ListStation
                     where item.Code == stationKey && item.Available
                          select item).FirstOrDefault();
            if (s == null)
                throw new DO.BadStationKeyException(stationKey);
            return new Station
            {
                Code = stationKey,
                Name = s.Name,
                Region = s.Region,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Address = s.Address,
                StationRoof = s.StationRoof,
                DigitalPanel = s.DigitalPanel,
                AccessForDisabled = s.AccessForDisabled,
                Available = s.Available
            };
        }
        #endregion

        #region AdjacentStations 
        /// <summary>
        /// adds new Adjacent Stationsto the file
        /// </summary>
    
        public void AddAdjacentStations(AdjacentStations s)
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);

            XElement AdjStationAdd = (from item in AdjStationsRootElem.Elements()
                                      where (int.Parse(item.Element("Code1").Value) == s.StationCode1 && int.Parse(item.Element("Code2").Value) == s.StationCode2) ||
                                            (int.Parse(item.Element("Code2").Value) == s.StationCode1 && int.Parse(item.Element("Code1").Value) == s.StationCode2)
                                      select item).FirstOrDefault();

            if (AdjStationAdd != null && !Convert.ToBoolean(AdjStationAdd.Element("Available").Value))//false
            {
                DeleteAdjacentStations(s.StationCode1, s.StationCode2);
            }
            if (AdjStationAdd != null && Convert.ToBoolean(AdjStationAdd.Element("Available").Value)) { }
            else
            {

                XElement AdjstationElement = new XElement("AdjacentStation",
                                                  new XElement("Code1", s.StationCode1),
                                                  new XElement("Code2", s.StationCode2),
                                                  new XElement("Distance", s.Distance),
                                                  new XElement("AverageTime", s.AverageTime.ToString()),
                                                  new XElement("Available", s.Available));

                AdjStationsRootElem.Add(AdjstationElement);

                XMLTools.SaveListToXMLElement(AdjStationsRootElem, AdjacentStationPath);
            }


        }
        /// <summary>
        /// updates Adjacen tStations in the file
        /// </summary>
        /// <param name="s"></param>
        public void UpdateAdjacentStations(AdjacentStations s)
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);

            XElement sNew = (from item in AdjStationsRootElem.Elements()
                             where (int.Parse(item.Element("Code1").Value) == s.StationCode1 && int.Parse(item.Element("Code2").Value) == s.StationCode2 && Convert.ToBoolean(item.Element("Available").Value) || (int.Parse(item.Element("Code1").Value) == s.StationCode2 && int.Parse(item.Element("Code2").Value) == s.StationCode1 && Convert.ToBoolean(item.Element("Available").Value)))
                             select item).FirstOrDefault();
            if (sNew != null)
            {
                sNew.Element("Code1").Value = s.StationCode1.ToString();
                sNew.Element("Code2").Value = s.StationCode2.ToString();
                sNew.Element("Distance").Value = s.Distance.ToString();
                sNew.Element("AverageTime").Value = s.AverageTime.ToString();
                sNew.Element("Available").Value = s.Available.ToString();

                XMLTools.SaveListToXMLElement(AdjStationsRootElem, AdjacentStationPath);
            }
            throw new DO.BadStationKeyException(s.StationCode1, "The adjacentstation doesn't exist");
        }
        /// <summary>
        /// deletes a pair of Adjacent Stations by the 2 codes from the file
        /// </summary>
        /// <param name="code1"></param>
        /// <param name="code2"></param>
        public void DeleteAdjacentStations(int code1, int code2)
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);
            XElement AdjStationDelete = (from item in AdjStationsRootElem.Elements()
                                         where (int.Parse(item.Element("Code1").Value) == code1 && int.Parse(item.Element("Code2").Value) == code2) || (int.Parse(item.Element("Code1").Value) == code2 && int.Parse(item.Element("Code2").Value) == code1)
                                         select item).FirstOrDefault();
            if (AdjStationDelete != null)
            {
                AdjStationDelete.Element("Available").Value = false.ToString();
                AdjStationsRootElem.Add(AdjStationDelete);
                XMLTools.SaveListToXMLElement(AdjStationsRootElem, AdjacentStationPath);
            }
            throw new DO.BadAdjacentStationsException(code1, code2);
        }
        /// <summary>
        /// deletes by the code all the Adjacen tStations from file
        /// </summary>
        /// <param name="code"></param>
        public void DeleteAllAdjOfStation(int code)
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);
            if (AdjStationsRootElem != null)
            {
                foreach (XElement item in AdjStationsRootElem.Elements())
                {
                    if (item.Element("Code1").Value == code.ToString() || item.Element("Code2").Value == code.ToString())
                    {
                        item.Element("Available").Value = false.ToString();
                        AdjStationsRootElem.Add(item);
                    }
                }
                XMLTools.SaveListToXMLElement(AdjStationsRootElem, AdjacentStationPath);
            }
        }
        /// <summary>
        /// deletes all Adjacent Stations in file
        /// </summary>
        public void DeleteAllAdjacentStations()
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);
            if (AdjStationsRootElem != null)
            {
                foreach (XElement item in AdjStationsRootElem.Elements())
                {
                    item.Element("Available").Value = false.ToString();
                    AdjStationsRootElem.Add(item);
                }
                XMLTools.SaveListToXMLElement(AdjStationsRootElem, AdjacentStationPath);
            }
        }
        /// <summary>
        /// returns all Adjacent Stations from file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdjacentStations> GetAllAdjacentStations()
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);
            return from item in AdjStationsRootElem.Elements()
                   select GetAdjacentStations(int.Parse(item.Element("Code1").Value), int.Parse(item.Element("Code2").Value));
        }
        /// <summary>
        /// returns by a predicate all Adjacent Stations from file
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<AdjacentStations> GetAllAdjacentStationsBy(Predicate<AdjacentStations> p)
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);
            return from item in AdjStationsRootElem.Elements()
                   let s = GetAdjacentStations(int.Parse(item.Element("Code1").Value), int.Parse(item.Element("Code2").Value))
                   where p(s)
                   select s;
        }
        /// <summary>
        /// returns by 2 codes the Adjacent Stations from file
        /// </summary>
        /// <param name="code1"></param>
        /// <param name="code2"></param>
        /// <returns></returns>
        public AdjacentStations GetAdjacentStations(int code1, int code2)
        {
            XElement AdjStationsRootElem = XMLTools.LoadListFromXMLElement(AdjacentStationPath);
            XElement s = (from item in AdjStationsRootElem.Elements()
                          where ((int.Parse(item.Element("Code1").Value) == code1 && int.Parse(item.Element("Code2").Value) == code2) || (int.Parse(item.Element("Code2").Value) == code1 && int.Parse(item.Element("Code1").Value) == code2)) && Convert.ToBoolean(item.Element("Available").Value)
                          select item).FirstOrDefault();
            if (s == null)
                throw new DO.BadAdjacentStationsException(code1, code2);
            return new AdjacentStations
            {
                StationCode1 = code1,
                StationCode2 = code2,
                Distance = float.Parse(s.Element("Distance").Value),
                AverageTime = TimeSpan.Parse(s.Element("AverageTime").Value),
                Available =bool.Parse(s.Element("Available").Value)
            };

        }
        #endregion

        #region LineStation
        /// <summary>
        /// returns line station ID from file
        /// </summary>
        /// <returns></returns>
        public int GetLineStationId()
        {
            List<IDis> ListID = XMLTools.LoadListFromXMLSerializer<IDis>(IDisPath);
            int id = ListID.Find(i => i.Name == "LineStation").Id;
            return id;
        }
        /// <summary>
        /// updates the line station in file
        /// </summary>
        public void UpdateLineStationId()
        {
            List<IDis> ListID = XMLTools.LoadListFromXMLSerializer<IDis>(IDisPath);
            IDis i = ListID.Find(x => x.Name == "LineStation");
            ListID.Remove(i);
            i.Id++;
            ListID.Add(i); //no need to Clone()
            XMLTools.SaveListToXMLSerializer(ListID, IDisPath);
        }
        /// <summary>
        /// adds new line station to file
        /// </summary>
        /// <param name="s"></param>
        public void AddLineStation(LineStation s)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            //the station exist
            GetStation(s.StationCode);

            // the line exist
            GetLine(s.LineId);

            //the line station does not exist
            try
            {
                GetLineStationByStation(s.ID, s.StationCode);
                throw new LineStationAlreadyExistException(s.ID, s.StationCode, "The station exists in this line");
            }
            catch (DO.BadLineStationKeyException) { ListLineStation.Add(s); XMLTools.SaveListToXMLSerializer(ListLineStation, LineStationsPath); }//לא מצא תחנת קו
        }
        /// <summary>
        /// updates one line station by other one ito file
        /// </summary>
        /// <param name="s"></param>
        public void UpdateLineStationByStation(LineStation s)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            DO.LineStation target = GetLineStationByStation(s.ID, s.StationCode);
            ListLineStation.Remove(target);
            ListLineStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListLineStation, LineStationsPath);
        }
        /// <summary>
        /// updates line station by index in file
        /// </summary>
        /// <param name="s"></param>
        public void UpdateLineStationByIndex(LineStation s)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            DO.LineStation target = GetLineStationByIndex(s.ID, s.IndexInLine);
            ListLineStation.Remove(target);
            ListLineStation.Add(s);
            XMLTools.SaveListToXMLSerializer(ListLineStation, LineStationsPath);
        }
        /// <summary>
        /// deletes one line station by other one from file 
        /// </summary>
        /// <param name="stationKey"></param>
        public void DeleteLineStationByStation(int stationKey)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            IEnumerable<DO.LineStation> target = GetLineStationsBy(st => st.StationCode == stationKey);
            foreach (var item in target)
                ListLineStation.Remove(item);
            XMLTools.SaveListToXMLSerializer(ListLineStation, LineStationsPath);
        }
        /// <summary>
        /// deletesb by predicate a line station from file
        /// </summary>
        /// <param name="p"></param>
        public void DeleteLineStationBy(Predicate<LineStation> p)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            ListLineStation.RemoveAll(t => p(t));
            XMLTools.SaveListToXMLSerializer(ListLineStation, LineStationsPath);
        }
        /// <summary>
        /// deletes all line stations in file
        /// </summary>
        /// <param name="LineId"></param>
        public void DeleteAllLineStations(int LineId)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            ListLineStation.RemoveAll(st => st.ID == LineId);
            XMLTools.SaveListToXMLSerializer(ListLineStation, LineStationsPath);
        }
        /// <summary>
        /// returns all line stations by ID
        /// </summary>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public IEnumerable<LineStation> GetAllLineStations(int LineId)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            return from item in ListLineStation where item.ID == LineId select item;
        }
        /// <summary>
        /// returns by predicate a line station from file
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<LineStation> GetLineStationsBy(Predicate<LineStation> p)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            return from item in ListLineStation where p(item) select item;

        }
        /// <summary>
        /// returns by ID and predicate a line station from file
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public IEnumerable<LineStation> GetLineStationsBy(int LineId, Predicate<LineStation> p)
        {
            List<LineStation> ListLineStation = XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
            return (
                from item in ListLineStation 
                where item!=null && p(item) && item.LineId == LineId 
                select item
                );
        }
        /// <summary>
        /// returns by ID and key number a line station from file
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        public LineStation GetLineStationByStation(int LineId, int stationKey)
        {
            IEnumerable<LineStation> station = GetLineStationsBy(LineId, st => st.StationCode == stationKey);
            if (station.Count() == 0)
                throw new BadLineStationKeyException(LineId, stationKey);
            return station.First();
        }
        /// <summary>
        /// returns by ID and index a line station from file
        /// </summary>
        /// <param name="LineId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public LineStation GetLineStationByIndex(int LineId, int index)
        {
            IEnumerable<LineStation> station = GetLineStationsBy(LineId, st => st.IndexInLine == index);
          //  if (station.Count() == 0)
                //throw new BadLineStationKeyException(LineId);
            return station.FirstOrDefault();       
            
        }
        /// <summary>
        /// returns all line stations with no condition from file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LineStation> GetAllLineStationsNoCondition()
        {
            return (IEnumerable<LineStation>)XMLTools.LoadListFromXMLSerializer<LineStation>(LineStationsPath);
  
        }
        #endregion

        #region Bus on trip 
        /// <summary>
        /// adds new bus on trip tp the file
        /// </summary>
        /// <param name="busOn"></param>
        public void AddBusOnTrip(BusOnTrip busOn)
        {
            List<BusOnTrip> ListBuson = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(busOnTripPath);

            if (ListBuson.FirstOrDefault(b => b.ID == busOn.ID) != null)
                throw new DO.BadIdBUTException(busOn.ID, "Duplicate ID number");

            ListBuson.Add(busOn); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListBuson, busOnTripPath);
        }
        /// <summary>
        /// updates the bus on trip in file
        /// </summary>
        /// <param name="busOn"></param>
        public void UpdateBusOnTrip(BusOnTrip busOn)
        {
            List<BusOnTrip> ListBusOn = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(busOnTripPath);

            DO.BusOnTrip busOn1 = ListBusOn.Find(b => b.ID == busOn.ID);
            if (busOn1 != null)
            {
                ListBusOn.Remove(busOn1);
                ListBusOn.Add(busOn); //no need to Clone()
            }
            else
                throw new DO.BadIdBUTException(busOn.ID, $"bad ID number: {busOn.ID}");

            XMLTools.SaveListToXMLSerializer(ListBusOn, busOnTripPath);
        }
        /// <summary>
        /// deletes bus on trip from the file
        /// </summary>
        /// <param name="boId"></param>
        public void DeleteBusOnTrip(int boId)
        {
            List<BusOnTrip> ListBusOn = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(busOnTripPath);

            DO.BusOnTrip busOn1 = ListBusOn.Find(b => b.ID == boId);
            if (busOn1 != null)
            {
                ListBusOn.Remove(busOn1);
                busOn1.Available = false;
                ListBusOn.Add(busOn1);
            }
            else
                throw new DO.BadIdBUTException(boId, $"bad ID number: {boId}");

            XMLTools.SaveListToXMLSerializer(ListBusOn, busOnTripPath);
        }
        /// <summary>
        /// returns the bus on trip from file
        /// </summary>
        /// <param name="boId"></param>
        /// <returns></returns>
        public BusOnTrip GetBusOnTrip(int boId)
        {
            List<BusOnTrip> ListBusOn = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(busOnTripPath);

            DO.BusOnTrip busOn = ListBusOn.Find(b => b.ID == boId);
            if (busOn != null)
                return busOn; //no need to Clone()
            else
                throw new DO.BadIdBUTException(boId, $"bad ID number: {boId}");
        }
        /// <summary>
        /// returns all the busses on trip in file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusOnTrip> GetAllBusOnTrip()
        {
            List<BusOnTrip> ListBusOn = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(busOnTripPath);


            IEnumerable<DO.BusOnTrip> busesOn = (from b in ListBusOn
                                                 where b.Available == true
                                                 select b).ToList();
            if (busesOn != null)
                return busesOn; //no need to Clone()
            else
                return null;
        }
        /// <summary>
        /// returns by the license number all the busses on trip in file
        /// </summary>
        /// <param name="licensNumber"></param>
        /// <returns></returns>
        public IEnumerable<BusOnTrip> GetAllBusTrips(string licensNumber)
        {
            List<BusOnTrip> ListBusOn = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(busOnTripPath);


            IEnumerable<DO.BusOnTrip> busesOn = (from b in ListBusOn
                                                 where b.Available == true && b.LicensNumber == licensNumber
                                                 select b).ToList();
            if (busesOn != null)
                return busesOn; //no need to Clone()
            else
                return null;
        }
        /// <summary>
        /// returns by the line number all the lines on trip in file 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public IEnumerable<BusOnTrip> GetAllLineTrips(int line)
        {
            List<BusOnTrip> ListBusOn = XMLTools.LoadListFromXMLSerializer<BusOnTrip>(busOnTripPath);


            IEnumerable<DO.BusOnTrip> busesOn = (from b in ListBusOn
                                                 where b.Available == true && b.LineId == line
                                                 select b).ToList();
            if (busesOn != null)
                return busesOn; //no need to Clone()
            else
                return null;
        }
        /// <summary>
        /// retruns the bus on trip ID from file
        /// </summary>
        /// <returns></returns>
        public int GetBusOnId()
        {
            List<IDis> ListID = XMLTools.LoadListFromXMLSerializer<IDis>(IDisPath);
            int id = ListID.Find(i => i.Name == "BusOnTrip").Id;
            return id;
        }
        /// <summary>
        /// updates bus on trip ID in file
        /// </summary>
        public void UpdateBusOnId()
        {
            List<DO.IDis> ListID = XMLTools.LoadListFromXMLSerializer<IDis>(IDisPath);
            DO.IDis i = ListID.Find(x => x.Name == "BusOnTrip");
            ListID.Remove(i);
            i.Id++;
            ListID.Add(i); //no need to Clone()
            XMLTools.SaveListToXMLSerializer(ListID, IDisPath);
        }
        #endregion

        #region Line
        /// <summary>
        /// returns line ID from file
        /// </summary>
        /// <returns></returns>
        public int GetLineId()
        {
            List<IDis> ListID = XMLTools.LoadListFromXMLSerializer<IDis>(IDisPath);
            int id = ListID.Find(i => i.Name == "Line").Id;
            return id;
        }
        /// <summary>
        /// updates line ID from in file
        /// </summary>
        public void UpdateLineId()
        {
            List<DO.IDis> ListID = XMLTools.LoadListFromXMLSerializer<IDis>(IDisPath);
            DO.IDis i = ListID.Find(x => x.Name == "Line");
            ListID.Remove(i);
            i.Id++;
            ListID.Add(i); //no need to Clone()
            XMLTools.SaveListToXMLSerializer(ListID, IDisPath);
        }
        /// <summary>
        /// returns all lines in file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Line> GetAllLines()
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            return (from item in Lines where item.Available select item).AsEnumerable();
        }
        /// <summary>
        /// returns by the predicate line from file
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Line> GetLinesBy(Predicate<Line> predicate)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            return from Line in Lines
                   where predicate(Line)
                   select Line;
        }
        /// <summary>
        /// returns by the ID the line from file
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Line GetLine(int ID)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);

            Line line = (
                from l in Lines 
                where l.ID == ID && 
                l.Available select l
                ).FirstOrDefault();
            if (line == null) throw new BadLineCodeException(ID, $"no bus line with ID {ID}");
            return line;
        }
        /// <summary>
        /// adds new line to file
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(Line line)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            if (Lines.Any(s => s.ID == line.ID && s.Available))
                throw new BadLineCodeException(line.ID, $"bus line with ID {line.ID} already exists");

            Line lineDeleted = (from l in Lines where line.ID == l.ID & !l.Available select l).FirstOrDefault();
            if (lineDeleted != null)
                Lines.Remove(lineDeleted);
            Lines.Add(line);
            XMLTools.SaveListToXMLSerializer(Lines, LinesPath);

        }
        /// <summary>
        /// updates line in file
        /// </summary>
        /// <param name="line"></param>
        public void UpdateLine(Line line)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            Line exists = Lines.Find(l => l.ID == line.ID && l.Available);
            if (exists == null) throw new BadLineCodeException(line.ID, $"no bus line with ID {line.ID}");
            Lines.Remove(exists);
            Lines.Add(line);

            XMLTools.SaveListToXMLSerializer(Lines, LinesPath);
        }
        /// <summary>
        /// deletes by the ID line from file 
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteLine(int ID)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            Line exists = Lines.Find(l => l.ID == ID && l.Available);
            if (exists == null) throw new BadLineCodeException(ID, $"no bus line with ID {ID}");

            int index = Lines.IndexOf(exists);
            Lines.Remove(exists);
            exists.Available = false;
            Lines.Insert(index, exists);

            XMLTools.SaveListToXMLSerializer(Lines, LinesPath);
        }
        /// <summary>
        /// deletes all lines in file
        /// </summary>
        public void DeleteAllLines()
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);
            for(int i=0; i<Lines.Count()-1;i++)
            {
               Lines[i].Available = false;
                int index = Lines.IndexOf(Lines[i]);
                Lines.Remove(Lines[i]);
                Lines[i].Available = false;
                Lines.Insert(index, Lines[i]);
            }
            XMLTools.SaveListToXMLSerializer(Lines, LinesPath);
        }

        /// <summary>
        /// deletes all lines by the predicate in file
        /// </summary>
        /// <param name="predicate"></param>
        public void DeleteLinesBy(Predicate<Line> predicate)
        {
            List<Line> Lines = XMLTools.LoadListFromXMLSerializer<Line>(LinesPath);

            var updatedStations = from line in Lines
                                  where !predicate(line)
                                  select line;

            XMLTools.SaveListToXMLSerializer(updatedStations.ToList(), LinesPath);
        }
        #endregion

    }
}
