using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DalApi;
using DO;
using DS;

namespace DL
{
    sealed class DalObject : IDal    //internal
    {
        #region singelton
        static readonly DalObject instance = new DalObject();
        static DalObject() { } // static ctor to ensure instance init is done just before first usage
        DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
        #endregion

        //Implement IDL methods, CRUD
        #region Bus
        public IEnumerable<Bus> GetAllBuses()
        {

            return from bus in DataSource.ListBuses
                   where bus.ObjectActive == true
                   select bus.Clone();

        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            return from bus in DataSource.ListBuses
                   where predicate(bus)
                   select bus.Clone();
        }

        public Bus GetBus(int license)
        {
            Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(license, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(bus.License, $"bad id: {bus.License}");
        }

        public void AddBus(Bus bus)
        {
            if (DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License) != null &&
                DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License).ObjectActive == true)
                throw new DO.ExceptionDALBadLicsens(bus.License, "Duplicate bus ID");
            else if (DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License) != null &&
                DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License).ObjectActive == false)
            {
                Bus addBus = DataSource.ListBuses.Find(b => b.License == bus.License);
                addBus.ObjectActive = true;
                addBus = bus.Clone();
            }
            else
                DataSource.ListBuses.Insert(0, bus.Clone());
        }

        public void UpdateBus(Bus bus) //busUpdate
        {
            Bus busUpdate = DataSource.ListBuses.Find(b => b.License == bus.License);
            if (busUpdate != null && busUpdate.ObjectActive)
                busUpdate = bus.Clone();
            else if (!busUpdate.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(bus.License, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(bus.License, $"bad id: {bus.License}");
        }

        public void UpdateBus(int licenseNumber, Action<Bus> update)  // method that knows to update specific fields in Person
        {
            Bus busUpdate = GetBus(licenseNumber);
            update(busUpdate);
        }

        public void DeleteBus(int license)
        {
            Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(bus.License, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(bus.License, $"bad id: {bus.License}");
        }
        #endregion 

        #region BusAtTravel
        public IEnumerable<BusAtTravel> GetAllBusesAtTravel()
        {
            return from busAtTravel in DataSource.ListBusAtTravels
                   where busAtTravel.ObjectActive == true
                   select busAtTravel.Clone();
        }
        public IEnumerable<BusAtTravel> GetAllBusesAtTravelBy(Predicate<BusAtTravel> predicate)
        {
            return from busAtTravel in DataSource.ListBusAtTravels
                   where predicate(busAtTravel)
                   select busAtTravel.Clone();
        }
        public BusAtTravel GetBusAtTravel(int license)
        {
            BusAtTravel bus = DataSource.ListBusAtTravels.Find(b => b.License == license);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(license, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(bus.License, $"bad id: {bus.License}");
        }
        public void AddBusAtTravel(BusAtTravel bus)
        {
            if (DataSource.ListBusAtTravels.FirstOrDefault(b => b.License == bus.License) != null &&
               DataSource.ListBusAtTravels.FirstOrDefault(b => b.License == bus.License).ObjectActive == true)
                throw new DO.ExceptionDALBadLicsens(bus.License, "Duplicate bus ID");
            else if (DataSource.ListBusAtTravels.FirstOrDefault(b => b.License == bus.License) != null &&
                DataSource.ListBusAtTravels.FirstOrDefault(b => b.License == bus.License).ObjectActive == false)
            {
                BusAtTravel addBus = DataSource.ListBusAtTravels.Find(b => b.License == bus.License);
                addBus.ObjectActive = true;
                addBus = bus.Clone();
            }
            else
                DataSource.ListBusAtTravels.Add(bus.Clone());
        }
        public void UpdateBusAtTravel(BusAtTravel bus)
        {
            BusAtTravel busUpdate = DataSource.ListBusAtTravels.Find(b => b.License == bus.License);
            if (busUpdate != null && busUpdate.ObjectActive)
                busUpdate = bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(bus.License, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(bus.License, $"bad id: {bus.License}");
        }
        public void UpdateBusAtTravel(int licenseNumber, Action<BusAtTravel> update) // method that knows to updt specific fields in Person
        {
            BusAtTravel busUpdate = GetBusAtTravel(licenseNumber);
            update(busUpdate);
        }
        public void DeleteBusAtTravel(int license)
        {
            BusAtTravel bus = DataSource.ListBusAtTravels.Find(b => b.License == license);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(bus.License, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(bus.License, $"bad id: {bus.License}");
        }
        #endregion

        #region BusLineStation
        public IEnumerable<BusLineStation> GetAllBusLineStations()
        {
            return from busLineStation in DataSource.ListBusLineStations
                   where busLineStation.ObjectActive == true
                   select busLineStation.Clone();
        }
        public IEnumerable<BusLineStation> GetAllBusLineStationsBy(Predicate<BusLineStation> predicate)
        {
            return from busLineStation in DataSource.ListBusLineStations
                   where predicate(busLineStation)
                   select busLineStation.Clone();
        }
        public BusLineStation GetBusLineStation(int busStopKey)
        {
            BusLineStation bus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busStopKey);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busStopKey, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busStopKey, $"bad id: {busStopKey}");
        }
        public void AddBusLineStation(BusLineStation busLineStation)
        {
            BusLineStation newBusLineStation = DataSource.ListBusLineStations.FirstOrDefault(b => b.BusStopKey == busLineStation.BusStopKey);
            if (newBusLineStation != null && newBusLineStation.ObjectActive == true)
                throw new DO.ExceptionDALBadLicsens(busLineStation.BusStopKey, "Duplicate bus ID");
            else if (newBusLineStation.ObjectActive == false)
            {
                BusLineStation addBus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busLineStation.BusStopKey);
                addBus.ObjectActive = true;
                addBus = busLineStation.Clone();
            }
            else
                DataSource.ListBusLineStations.Add(busLineStation.Clone());
        }
        public void UpdateBusLineStation(BusLineStation busLineStation)
        {
            BusLineStation busUpdate = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busLineStation.BusStopKey);
            if (busUpdate != null && busUpdate.ObjectActive)
                busUpdate = busLineStation.Clone();
            else if (!busUpdate.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busUpdate.BusStopKey, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busUpdate.BusStopKey, $"bad id: {busUpdate.BusStopKey}");
        }
        public void UpdateBusLineStation(int busStopKey, Action<BusLineStation> update) // method that knows to updt specific fields in Person
        {
            BusLineStation busUpdate = GetBusLineStation(busStopKey);
            update(busUpdate);
        }
        public void DeleteBusLineStation(int busStopKey)
        {
            BusLineStation bus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busStopKey);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busStopKey, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busStopKey, $"bad id: {busStopKey}");
        }
        #endregion

        #region BusLine
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in DataSource.ListBusLines
                   where busLine.ObjectActive == true
                   select busLine.Clone();
        }
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            return from busLine in DataSource.ListBusLines
                   where predicate(busLine)
                   select busLine.Clone();
        }

        public BusLine GetBusLine(int busLineNumber)
        {
            BusLine bus = DataSource.ListBusLines.Find(b => b.BusLineNumber == busLineNumber);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busLineNumber, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busLineNumber, $"bad id: {busLineNumber}");
        }

        public void AddBusLine(BusLine busLine)
        {
            BusLine newBusLine = DataSource.ListBusLines.FirstOrDefault(b => b.BusLineNumber == busLine.BusLineNumber);
            //int idToReturn;
            // Need to correct this condition:
            if (newBusLine != null && newBusLine.ObjectActive == true && newBusLine.FirstBusStopKey == busLine.FirstBusStopKey
                && newBusLine.LastBusStopKey == busLine.LastBusStopKey)
                throw new DO.ExceptionDALBadLicsens(busLine.BusLineNumber, "Duplicate bus ID");

            else if (DataSource.ListBusLines.FirstOrDefault(b => b.BusLineNumber == busLine.BusLineNumber) != null &&
                DataSource.ListBusLines.FirstOrDefault(b => b.BusLineNumber == busLine.BusLineNumber).ObjectActive == false)
            {
                BusLine addBus = DataSource.ListBusLines.Find(b => b.BusLineNumber == busLine.BusLineNumber);
                addBus.ObjectActive = true;
                //idToReturn = addBus.BusLineID;
                addBus = busLine.Clone();
            }
            else
            {
                busLine.BusLineID = Config.RunningNumBusLine;
                //idToReturn = busLine.BusLineID;
                DataSource.ListBusLines.Add(busLine.Clone());
            }
            //return idToReturn;
        }

        public void UpdateBusLine(BusLine busLine)
        {
            BusLine busUpdate = DataSource.ListBusLines.Find(b => b.BusLineNumber == busLine.BusLineNumber);
            if (busUpdate != null && busUpdate.ObjectActive)
                busUpdate = busLine.Clone();
            else if (!busUpdate.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busLine.BusLineNumber, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busLine.BusLineNumber, $"bad id: {busLine.BusLineNumber}");
        }
        public void UpdateBusLine(int busLineNumber, Action<BusLine> update) // method that knows to updt specific fields in Person
        {
            BusLine busUpdate = GetBusLine(busLineNumber);
            update(busUpdate);
        }
        public void DeleteBusLine(int busLineNumber)
        {
            BusLine bus = DataSource.ListBusLines.Find(b => b.BusLineNumber == busLineNumber);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busLineNumber, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busLineNumber, $"bad id: {busLineNumber}");
        }
        #endregion

        #region BusStop
        public IEnumerable<BusStop> GetAllBusStops()
        {
            return from busStopLine in DataSource.ListBusStops
                   where busStopLine.ObjectActive == true
                   select busStopLine.Clone();
        }
        public IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate)
        {
            return from busStop in DataSource.ListBusStops
                   where predicate(busStop)
                   select busStop.Clone();
        }
        public BusStop GetBusStop(int busStopKey)
        {
            BusStop bus = DataSource.ListBusStops.Find(b => b.BusStopKey == busStopKey);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busStopKey, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busStopKey, $"bad id: {busStopKey}");
        }
        public void AddBusStop(BusStop busStop)
        {
            if (DataSource.ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey) != null &&
              DataSource.ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey).ObjectActive == true)
                throw new DO.ExceptionDALBadLicsens(busStop.BusStopKey, "Duplicate bus ID");
            else if (DataSource.ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey) != null &&
                DataSource.ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey).ObjectActive == false)
            {
                BusStop addBus = DataSource.ListBusStops.Find(b => b.BusStopKey == busStop.BusStopKey);
                addBus.ObjectActive = true;
                addBus = busStop.Clone();
            }
            else
                DataSource.ListBusStops.Insert(0, busStop.Clone());
        }
        public void UpdateBusStop(BusStop busStop)
        {
            BusStop busUpdate = DataSource.ListBusStops.Find(b => b.BusStopKey == busStop.BusStopKey);
            if (busUpdate != null && busUpdate.ObjectActive)
                busUpdate = busStop.Clone();
            else if (!busUpdate.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busUpdate.BusStopKey, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busUpdate.BusStopKey, $"bad id: {busUpdate.BusStopKey}");
        }
        public void UpdateBusStop(int busStopKey, Action<BusStop> update) // method that knows to updt specific fields in Person
        {
            BusStop busUpdate = GetBusStop(busStopKey);
            update(busUpdate);
        }

        public void DeleteBusStop(int busStopKey)
        {
            BusStop bus = DataSource.ListBusStops.Find(b => b.BusStopKey == busStopKey);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busStopKey, $"the bus stop is inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busStopKey, $"bad id: {busStopKey}");
        }
        #endregion

        #region ConsecutiveStations
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            return from busConsecutiveStations in DataSource.ListConsecutiveStations
                   where busConsecutiveStations.ObjectActive == true
                   select busConsecutiveStations.Clone();
        }
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)
        {
            return from busConsecutiveStations in DataSource.ListConsecutiveStations
                   where predicate(busConsecutiveStations)
                   select busConsecutiveStations.Clone();
        }
        public ConsecutiveStations GetConsecutiveStations(int busConsecutive)
        {
            ConsecutiveStations bus = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == busConsecutive);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busConsecutive, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busConsecutive, $"bad id: {busConsecutive}");
        }

        public void AddConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            ConsecutiveStations newConsecutiveStations = DataSource.ListConsecutiveStations.FirstOrDefault(b => b.BusStopKeyA == consecutiveStations.BusStopKeyA);
            if (newConsecutiveStations != null && newConsecutiveStations.ObjectActive == true)
                throw new DO.ExceptionDALBadLicsens(consecutiveStations.BusStopKeyA, "Duplicate bus ID");
            else if (DataSource.ListConsecutiveStations.FirstOrDefault(b => b.BusStopKeyA == consecutiveStations.BusStopKeyA) != null &&
                DataSource.ListConsecutiveStations.FirstOrDefault(b => b.BusStopKeyA == consecutiveStations.BusStopKeyA).ObjectActive == false)
            {
                ConsecutiveStations addConsecutiveStations = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == consecutiveStations.BusStopKeyA);
                addConsecutiveStations.ObjectActive = true;
                addConsecutiveStations = consecutiveStations.Clone();
            }
            else
                DataSource.ListConsecutiveStations.Add(consecutiveStations.Clone());
        }
        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            ConsecutiveStations consecutiveUpdate = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == consecutiveStations.BusStopKeyA);
            if (consecutiveUpdate != null && consecutiveUpdate.ObjectActive)
                consecutiveUpdate = consecutiveStations.Clone();
            else if (!consecutiveUpdate.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(consecutiveStations.BusStopKeyA, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(consecutiveStations.BusStopKeyA, $"bad id: {consecutiveStations.BusStopKeyA}");
        }

        public void UpdateConsecutiveStations(int busConsecutive, Action<ConsecutiveStations> update) // method that knows to updt specific fields in Person
        {
            ConsecutiveStations busConsecutiveUpdate = GetConsecutiveStations(busConsecutive);
            update(busConsecutiveUpdate);
        }

        public void DeleteConsecutiveStations(int busConsecutiveKey)
        {
            ConsecutiveStations busConsecutive = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == busConsecutiveKey);
            if (busConsecutive != null && busConsecutive.ObjectActive)
                busConsecutive.ObjectActive = false;
            else if (!busConsecutive.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busConsecutiveKey, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busConsecutiveKey, $"bad id: {busConsecutiveKey}");
        }
        #endregion  //בדיקה לעומק איך לבצע מימוש!!

        #region LineDeparture
        public IEnumerable<LineDeparture> GetAllLineDeparture()
        {
            return from busLineDeparture in DataSource.ListLineDepartures
                   where busLineDeparture.ObjectActive == true
                   select busLineDeparture.Clone();
        }
        public IEnumerable<LineDeparture> GetAllLineDepartureBy(Predicate<LineDeparture> predicate)
        {
            return from busLineDeparture in DataSource.ListLineDepartures
                   where predicate(busLineDeparture)
                   select busLineDeparture.Clone();
        }

        public LineDeparture GetLineDeparture(int busLineID)
        {
            LineDeparture bus = DataSource.ListLineDepartures.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busLineID, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busLineID, $"bad id: {busLineID}");
        }
        public void AddLineDeparture(LineDeparture lineDeparture)
        {
            if (DataSource.ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID) != null &&
              DataSource.ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID).ObjectActive == true)
                throw new DO.ExceptionDALBadLicsens(lineDeparture.BusLineID, "Duplicate bus ID");
            else if (DataSource.ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID) != null &&
                DataSource.ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID).ObjectActive == false)
            {
                LineDeparture addBus = DataSource.ListLineDepartures.Find(b => b.BusLineID == lineDeparture.BusLineID);
                addBus.ObjectActive = true;
                addBus = lineDeparture.Clone();
            }
            else
                DataSource.ListLineDepartures.Add(lineDeparture.Clone());
        }
        public void UpdateLineDeparture(LineDeparture lineDeparture)
        {
            LineDeparture busUpdate = DataSource.ListLineDepartures.Find(b => b.BusLineID == lineDeparture.BusLineID);
            if (busUpdate != null && busUpdate.ObjectActive)
                busUpdate = lineDeparture.Clone();
            else if (!busUpdate.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busUpdate.BusLineID, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busUpdate.BusLineID, $"bad id: {busUpdate.BusLineID}");
        }
        public void UpdateLineDeparture(int busLineID, Action<LineDeparture> update) // method that knows to updt specific fields in Person
        {
            LineDeparture busUpdate = GetLineDeparture(busLineID);
            update(busUpdate);
        }
        public void DeleteLineDeparture(int busLineID)
        {
            LineDeparture bus = DataSource.ListLineDepartures.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (!bus.ObjectActive)
                throw new DO.ExceptionDALInactiveBus(busLineID, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadLicsens(busLineID, $"bad id: {busLineID}");
        }
        #endregion

        #region User
        public IEnumerable<User> GetAllUsers()
        {
            return from user in DataSource.ListUsers
                   where user.ObjectActive == true
                   select user.Clone();
        }

        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            return from user in DataSource.ListUsers
                   where predicate(user)
                   select user.Clone();
        }

        public User GetUser(string userName)
        {
            User user = DataSource.ListUsers.Find(b => b.UserName == userName);
            if (user != null && user.ObjectActive)
                return user.Clone();
            else if (!user.ObjectActive)
                throw new DO.ExceptionDALInactiveUser(userName, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALInactiveUser(userName, $"bad id: {userName}");
        }

        public void AddUser(User user)
        {
            if (DataSource.ListUsers.FirstOrDefault(b => b.UserName == user.UserName) != null &&
              DataSource.ListUsers.FirstOrDefault(b => b.UserName == user.UserName).ObjectActive == true)
                throw new DO.ExceptionDALBadIdUser(user.UserName, "Duplicate bus ID");
            else if (DataSource.ListUsers.FirstOrDefault(b => b.UserName == user.UserName) != null &&
                DataSource.ListUsers.FirstOrDefault(b => b.UserName == user.UserName).ObjectActive == false)
            {
                User addUser = DataSource.ListUsers.Find(b => b.UserName == user.UserName);
                addUser.ObjectActive = true;
                addUser = user.Clone();
            }
            else
                DataSource.ListUsers.Add(user.Clone());
        }

        public void UpdateUser(User user)
        {
            User userUpdate = DataSource.ListUsers.Find(b => b.UserName == user.UserName);
            if (userUpdate != null && userUpdate.ObjectActive)
                userUpdate = user.Clone();
            else if (!userUpdate.ObjectActive)
                throw new DO.ExceptionDALInactiveUser(user.UserName, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadIdUser(user.UserName, $"bad id: {user.UserName}");
        }

        public void UpdateUser(string userName, Action<User> update) // method that knows to updt specific fields in Person
        {
            User userUpdate = GetUser(userName);
            update(userUpdate);
        }

        public void DeleteUser(string userName)
        {
            User user = DataSource.ListUsers.Find(b => b.UserName == userName);
            if (user != null && user.ObjectActive)
                user.ObjectActive = false;
            else if (!user.ObjectActive)
                throw new DO.ExceptionDALInactiveUser(userName, $"the bus is  inactive");
            else
                throw new DO.ExceptionDALBadIdUser(userName, $"bad id: {userName}");
        }
        #endregion

    }
}





