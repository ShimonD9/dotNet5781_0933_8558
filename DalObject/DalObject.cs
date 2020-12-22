﻿using System;
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
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(license, $"bad id: {license}");
        }

        public void AddBus(Bus bus)
        {
            if (DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License) != null)
                throw new DO.BadIdException(bus.License, "Duplicate bus ID");
            DataSource.ListBuses.Add(bus.Clone());
        }
        public void UpdateBus(Bus bus)
        {
            Bus busUpdate = DataSource.ListBuses.Find(b => b.License == bus.License);
            if (busUpdate != null)
                busUpdate = bus.Clone();
            else
                throw new DO.BadIdException(bus.License, $"bad id: {bus.License}");
        }
        public void UpdateBus(int licenseNumber, Action<Bus> update)  // method that knows to update specific fields in Person
        {
            Bus busUpdate = GetBus(licenseNumber);
            update(busUpdate);
        }
        public void DeleteBus(int license)
        {
            Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            if (bus != null)
                bus.ObjectActive = false;
            else
                throw new DO.BadIdException(license, $"bad id: {license}");
        }
        #endregion 

        #region BusAtTravel
        public IEnumerable<BusAtTravel> GetAllBusesAtTravel()
        {
            return from busAtTravel in DataSource.ListBusAtTravels
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
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(license, $"bad id: {license}");
        }
        public void AddBusAtTravel(BusAtTravel bus)
        {
            if (DataSource.ListBusAtTravels.FirstOrDefault(b => b.License == bus.License) != null)
                throw new DO.BadIdException(bus.License, "Duplicate bus ID");
            DataSource.ListBusAtTravels.Add(bus.Clone());
        }
        public void UpdateBusAtTravel(BusAtTravel bus)
        {
            BusAtTravel busUpdate = DataSource.ListBusAtTravels.Find(b => b.License == bus.License);
            if (busUpdate != null)
                busUpdate = bus.Clone();
            else
                throw new DO.BadIdException(bus.License, $"bad id: {bus.License}");
        }
        public void UpdateBusAtTravel(int licenseNumber, Action<BusAtTravel> update) // method that knows to updt specific fields in Person
        {
            BusAtTravel busUpdate = GetBusAtTravel(licenseNumber);
            update(busUpdate);
        }
        public void DeleteBusAtTravel(int license)
        {
            BusAtTravel bus = DataSource.ListBusAtTravels.Find(b => b.License == license);
            if (bus != null)
                bus.ObjectActive = false;
            else
                throw new DO.BadIdException(license, $"bad id: {license}");
        }
        #endregion

        #region BusLine
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in DataSource.ListBusLines
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
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(busLineNumber, $"bad id: {busLineNumber}");
        }
        public void AddBusLine(BusLine busLine)
        {
            if (DataSource.ListBusLines.FirstOrDefault(b => b.BusLineNumber == busLine.BusLineNumber) != null)
                throw new DO.BadIdException(busLine.BusLineNumber, "Duplicate bus ID");
            DataSource.ListBusLines.Add(busLine.Clone());
        }
        public void UpdateBusLine(BusLine busLine)
        {
            BusLine busUpdate = DataSource.ListBusLines.Find(b => b.BusLineNumber == busLine.BusLineNumber);
            if (busUpdate != null)
                busUpdate = busLine.Clone();
            else
                throw new DO.BadIdException(busLine.BusLineNumber, $"bad id: {busLine.BusLineNumber}");
        }
        public void UpdateBusLine(int busLineNumber, Action<BusLine> update) // method that knows to updt specific fields in Person
        {
            BusLine busUpdate = GetBusLine(busLineNumber);
            update(busUpdate);
        }
        public void DeleteBusLine(int busLineNumber)
        {
            BusLine bus = DataSource.ListBusLines.Find(b => b.BusLineNumber == busLineNumber);
            if (bus != null)
                bus.ObjectActive = false;
            else
                throw new DO.BadIdException(busLineNumber, $"bad id: {busLineNumber}");
        }
        #endregion

        #region BusLineStation
        public IEnumerable<BusLineStation> GetAllBusLineStations()
        {
            return from busLineStation in DataSource.ListBusLineStations
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
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(busStopKey, $"bad id: {busStopKey}");
        }
        public void AddBusLineStation(BusLineStation busLineStation)
        {
            if (DataSource.ListBusLineStations.FirstOrDefault(b => b.BusLineID == busLineStation.BusLineID) != null)
                throw new DO.BadIdException(busLineStation.BusLineID, "Duplicate bus ID");
            DataSource.ListBusLineStations.Add(busLineStation.Clone());
        }
        public void UpdateBusLineStation(BusLineStation busLineStation)
        {
            BusLineStation busUpdate = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busLineStation.BusStopKey);
            if (busUpdate != null)
                busUpdate = busLineStation.Clone();
            else
                throw new DO.BadIdException(busLineStation.BusStopKey, $"bad id: {busLineStation.BusStopKey}");
        }
        public void UpdateBusLineStation(int busStopKey, Action<BusLineStation> update) // method that knows to updt specific fields in Person
        {
            BusLineStation busUpdate = GetBusLineStation(busStopKey);
            update(busUpdate);
        }
        public void DeleteBusLineStation(int busStopKey)
        {
            BusLineStation bus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busStopKey);
            if (bus != null)
                bus.ObjectActive = false;
            else
                throw new DO.BadIdException(busStopKey, $"bad id: {busStopKey}");
        }
        #endregion

        #region BusStop
        public IEnumerable<BusStop> GetAllBusStops()
        {
            return from busStopLine in DataSource.ListBusStops
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
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(busStopKey, $"bad id: {busStopKey}");
        }
        public void AddBusStop(BusStop busStop)
        {
            if (DataSource.ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey) != null)
                throw new DO.BadIdException(busStop.BusStopKey, "Duplicate bus ID");
            DataSource.ListBusStops.Add(busStop.Clone());
        }
        public void UpdateBusStop(BusStop busStop)
        {
            BusStop busUpdate = DataSource.ListBusStops.Find(b => b.BusStopKey == busStop.BusStopKey);
            if (busUpdate != null)
                busUpdate = busStop.Clone();
            else
                throw new DO.BadIdException(busStop.BusStopKey, $"bad id: {busStop.BusStopKey}");
        }
        public void UpdateBusStop(int busStopKey, Action<BusStop> update) // method that knows to updt specific fields in Person
        {
            BusStop busUpdate = GetBusStop(busStopKey);
            update(busUpdate);
        }
        public void DeleteBusStop(int busStopKey)
        {
            BusStop bus = DataSource.ListBusStops.Find(b => b.BusStopKey == busStopKey);
            if (bus != null)
                bus.ObjectActive = false;
            else
                throw new DO.BadIdException(busStopKey, $"bad id: {busStopKey}");
        }
        #endregion

        #region ConsecutiveStations
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            return from busConsecutiveStations in DataSource.ListConsecutiveStations
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
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(busConsecutive, $"bad id: {busConsecutive}");
        }
        public void AddConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            if (DataSource.ListConsecutiveStations.FirstOrDefault(b => b.BusStopKeyA == consecutiveStations.BusStopKeyA) != null)
                throw new DO.BadIdException(consecutiveStations.BusStopKeyA, "Duplicate bus ID");
            DataSource.ListConsecutiveStations.Add(consecutiveStations.Clone());
        }
        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            ConsecutiveStations consecutiveUpdate = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == consecutiveStations.BusStopKeyA);
            if (consecutiveUpdate != null)
                consecutiveUpdate = consecutiveStations.Clone();
            else
                throw new DO.BadIdException(consecutiveStations.BusStopKeyA, $"bad id: {consecutiveStations.BusStopKeyA}");
        }
        public void UpdateConsecutiveStations(int busConsecutive, Action<ConsecutiveStations> update) // method that knows to updt specific fields in Person
        {
            ConsecutiveStations busConsecutiveUpdate = GetConsecutiveStations(busConsecutive);
            update(busConsecutiveUpdate);
        }
        public void DeleteConsecutiveStations(int busConsecutiveKey)
        {
            ConsecutiveStations busConsecutive = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == busConsecutiveKey);
            if (busConsecutive != null)
                busConsecutive.ObjectActive = false;
            else
                throw new DO.BadIdException(busConsecutiveKey, $"bad id: {busConsecutiveKey}");
        }
        #endregion  //בדיקה לעומק איך לבצע מימוש!!

        #region LineDeparture
        public IEnumerable<LineDeparture> GetAllLineDeparture()
        {
            return from busLineDeparture in DataSource.ListLineDepartures
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
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(busLineID, $"bad id: {busLineID}");
        }
        public void AddLineDeparture(LineDeparture lineDeparture)
        {
            if (DataSource.ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID) != null)
                throw new DO.BadIdException(lineDeparture.BusLineID, "Duplicate bus ID");
            DataSource.ListLineDepartures.Add(lineDeparture.Clone());
        }
        public void UpdateLineDeparture(LineDeparture lineDeparture)
        {
            LineDeparture busUpdate = DataSource.ListLineDepartures.Find(b => b.BusLineID == lineDeparture.BusLineID);
            if (busUpdate != null)
                busUpdate = lineDeparture.Clone();
            else
                throw new DO.BadIdException(lineDeparture.BusLineID, $"bad id: {lineDeparture.BusLineID}");
        }
        public void UpdateLineDeparture(int busLineID, Action<LineDeparture> update) // method that knows to updt specific fields in Person
        {
            LineDeparture busUpdate = GetLineDeparture(busLineID);
            update(busUpdate);
        }
        public void DeleteLineDeparture(int busLineID)
        {
            LineDeparture bus = DataSource.ListLineDepartures.Find(b => b.BusLineID == busLineID);
            if (bus != null)
                bus.ObjectActive = false;
            else
                throw new DO.BadIdException(busLineID, $"bad id: {busLineID}");
        }
        #endregion

        #region User
        public IEnumerable<User> GetAllUsers()
        {
            return from user in DataSource.ListUsers
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
            if (user != null)
                return user.Clone();
            else
                throw new DO.BadIdUserException(userName, $"bad id: {userName}");
        }

        public void AddUser(User user)
        {
            if (DataSource.ListUsers.FirstOrDefault(b => b.UserName == user.UserName) != null)
                throw new DO.BadIdUserException(user.UserName, "Duplicate bus ID");
            DataSource.ListUsers.Add(user.Clone());
        }

        public void UpdateUser(User user)
        {
            User userUpdate = DataSource.ListUsers.Find(b => b.UserName == user.UserName);
            if (userUpdate != null)
                userUpdate = user.Clone();
            else
                throw new DO.BadIdUserException(user.UserName, $"bad id: {user.UserName}");
        }

        public void UpdateUser(string userName, Action<User> update) // method that knows to updt specific fields in Person
        {
            User userUpdate = GetUser(userName);
            update(userUpdate);
        }

        public void DeleteUser(string userName)
        {
            User user = DataSource.ListUsers.Find(b => b.UserName == userName);
            if (user != null)
                user.ObjectActive = false;
            else
                throw new DO.BadIdUserException(userName, $"bad id: {userName}");
        }
        #endregion

    }
}





