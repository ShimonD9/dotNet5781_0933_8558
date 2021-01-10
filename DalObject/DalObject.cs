﻿using DalApi;
using DO;
using DS;
using System;
using System.Collections.Generic;
using System.Linq;

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
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(license, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"license key not found: {bus.License}");
        }

        public void AddBus(Bus bus)
        {
            Bus existBus = DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License);
            if (existBus != null && existBus.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.License, "Duplicate bus license");
            else if (existBus != null && existBus.ObjectActive == false)
            {
                existBus.ObjectActive = true;
                existBus = bus.Clone();
            }
            else
                bus.ObjectActive = true;
            DataSource.ListBuses.Insert(0, bus.Clone());
        }

        public void UpdateBus(Bus bus) //busUpdate
        {
            int index = DataSource.ListBuses.FindIndex(bus1 => bus1.License == bus.License);
            if (DataSource.ListBuses[index] != null && DataSource.ListBuses[index].ObjectActive)
                DataSource.ListBuses[index] = bus.Clone();
            else if (DataSource.ListBuses[index] != null && !DataSource.ListBuses[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.License, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"license key not found: {bus.License}");
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
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.License, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"license key not found: {bus.License}");
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
        public BusAtTravel GetBusAtTravel(int busLineID)
        {
            BusAtTravel bus = DataSource.ListBusAtTravels.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"Wrong license: {bus.License}");
        }
        public void AddBusAtTravel(BusAtTravel bus)
        {
            BusAtTravel existBus = DataSource.ListBusAtTravels.FirstOrDefault(b => b.BusLineID == bus.BusLineID);

            if (existBus != null && existBus.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.BusLineID, "The bus line is already at travel");
            else if (existBus != null && existBus.ObjectActive == false)
            {
                existBus.ObjectActive = true;
                existBus = bus.Clone();
            }
            else
                bus.ObjectActive = true;
            DataSource.ListBusAtTravels.Add(bus.Clone());
        }
        public void UpdateBusAtTravel(BusAtTravel bus)
        {
            int index = DataSource.ListBusAtTravels.FindIndex(bus1 => bus1.BusLineID == bus.BusLineID);
            if (DataSource.ListBusAtTravels[index] != null && DataSource.ListBusAtTravels[index].ObjectActive)
                DataSource.ListBusAtTravels[index] = bus.Clone();
            else if (DataSource.ListBusAtTravels[index] != null && !DataSource.ListBusAtTravels[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.BusLineID, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.BusLineID, $"license key not found: {bus.BusLineID}");
        }
        public void UpdateBusAtTravel(int BusLineID, Action<BusAtTravel> update) // method that knows to updt specific fields in Person
        {
            BusAtTravel busUpdate = GetBusAtTravel(BusLineID);
            update(busUpdate);
        }
        public void DeleteBusAtTravel(int busLineID)
        {
            BusAtTravel bus = DataSource.ListBusAtTravels.Find(b => b.BusLineID == busLineID);

            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.BusLineID, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.BusLineID, $"license key not found: {bus.BusLineID}");
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
        public BusLineStation GetBusLineStation(int busLineID, int busStopCode)
        {
            BusLineStation bus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busStopCode && b.BusLineID == busLineID);

            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopCode, $"the bus station is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopCode, $"station key not found: {busStopCode}");
        }
        public void AddBusLineStation(BusLineStation busLineStation)
        {
            BusLineStation existBusLineStation = DataSource.ListBusLineStations.FirstOrDefault(b => b.BusStopKey == busLineStation.BusStopKey
            && b.BusLineID == busLineStation.BusLineID);
            if (existBusLineStation != null && existBusLineStation.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busLineStation.BusStopKey, "Duplicate bus station key");
            else if (existBusLineStation != null && existBusLineStation.ObjectActive == false)
            {
                existBusLineStation.ObjectActive = true;
                existBusLineStation = busLineStation.Clone();
            }
            else
            {
                busLineStation.ObjectActive = true;
                DataSource.ListBusLineStations.Add(busLineStation.Clone());
            }
        }

        public void UpdateBusLineStation(BusLineStation busLineStation)
        {
            int index = DataSource.ListBusLineStations.FindIndex(bus1 => bus1.BusLineID == busLineStation.BusLineID && bus1.BusStopKey == busLineStation.BusStopKey);         
            if (DataSource.ListBusLineStations[index] != null && DataSource.ListBusLineStations[index].ObjectActive)
            {
                DataSource.ListBusLineStations.Remove(DataSource.ListBusLineStations[index]);
                DataSource.ListBusLineStations.Add(busLineStation.Clone());
            }
            else if (DataSource.ListBusLineStations[index] != null && !DataSource.ListBusLineStations[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineStation.BusStopKey, $"the bus line station is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineStation.BusStopKey, $"station key not found: {busLineStation.BusStopKey}");
        }

        public void UpdateBusLineStation(int busLineID, int busStopCode, Action<BusLineStation> update) // method that knows to updt specific fields in Person
        {
            BusLineStation busUpdate = GetBusLineStation(busLineID, busStopCode);
            update(busUpdate);
            UpdateBusLineStation(busUpdate);
        }

        public void DeleteBusLineStation(int busLineID, int busStopCode)
        {
            BusLineStation bus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busStopCode && b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopCode, $"the bus line station is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopCode, $"station key not found: {busStopCode}");
        }
        public void DeleteBusLineStationsByID(int busLineID)
        {
            foreach (BusLineStation item in DataSource.ListBusLineStations)
            {
                if (item.BusLineID == busLineID)
                    item.ObjectActive = false;
            }
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

        public BusLine GetBusLine(int busLineID)
        {
            BusLine bus = DataSource.ListBusLines.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"the bus line is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"bus line number not found: {busLineID}");
        }

        public int AddBusLine(BusLine busLine)
        {
            int idToReturn;
            BusLine exsistBusLine = DataSource.ListBusLines.FirstOrDefault(b => b.BusLineNumber == busLine.BusLineNumber);
            if (exsistBusLine != null && exsistBusLine.ObjectActive == true && exsistBusLine.FirstBusStopKey == busLine.FirstBusStopKey
                && exsistBusLine.LastBusStopKey == busLine.LastBusStopKey)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busLine.BusLineNumber, "Duplicate bus line number");

            else if (exsistBusLine != null && exsistBusLine.ObjectActive == false)
            {
                exsistBusLine.ObjectActive = true;
                idToReturn = exsistBusLine.BusLineID;
                exsistBusLine = busLine.Clone();
            }
            else
            {
                busLine.BusLineID = Config.RunningNumBusLine;
                busLine.ObjectActive = true;
                idToReturn = busLine.BusLineID;
                DataSource.ListBusLines.Add(busLine.Clone());
            }
            return idToReturn;
        }

        public void UpdateBusLine(BusLine busLine)
        {
            int index = DataSource.ListBusLines.FindIndex(bus1 => bus1.BusLineID == busLine.BusLineID);
            if (DataSource.ListBusLines[index] != null && DataSource.ListBusLines[index].ObjectActive)
                DataSource.ListBusLines[index] = busLine.Clone();
            else if (DataSource.ListBusLines[index] != null && !DataSource.ListBusLines[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLine.BusLineID, $"the bus line is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLine.BusLineID, $"bus line number not found: {busLine.BusLineID}");
        }
        public void UpdateBusLine(int busLineNumber, Action<BusLine> update) // method that knows to updt specific fields in Person
        {
            BusLine busUpdate = GetBusLine(busLineNumber);
            update(busUpdate);
            UpdateBusLine(busUpdate);
        }
        public void DeleteBusLine(int busLineID)
        {
            BusLine bus = DataSource.ListBusLines.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"the bus line is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"bus line number not found: {busLineID}");
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
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopKey, $"the bus stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopKey, $"bus stop key not found: {busStopKey}");
        }
        public void AddBusStop(BusStop busStop)
        {
            BusStop existStop = DataSource.ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey);
            if (existStop != null && existStop.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busStop.BusStopKey, "Duplicate bus stop key");
            else if (existStop != null && existStop.ObjectActive == false)
            {
                existStop.ObjectActive = true;
                existStop = busStop.Clone();
            }
            else
            {
                busStop.ObjectActive = true;
                DataSource.ListBusStops.Insert(0, busStop.Clone());
            }
        }
        public void UpdateBusStop(BusStop busStop)
        {
            // If the old bus stop code didn't change, or it changed but it's new:
            int index = DataSource.ListBusStops.FindIndex(b => b.BusStopKey == busStop.BusStopKey);
            if (DataSource.ListBusStops[index] != null && DataSource.ListBusStops[index].ObjectActive)
                DataSource.ListBusStops[index] = busStop.Clone();
            else if (DataSource.ListBusStops[index] != null && !DataSource.ListBusStops[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStop.BusStopKey, $"the bus Stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStop.BusStopKey, $"bus stop key not found");
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
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopKey, $"the bus stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopKey, $"bus stop key not found: {busStopKey}");
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

        public ConsecutiveStations GetConsecutiveStations(int busStopCodeA, int busStopCodeB)
        {
            ConsecutiveStations conStations = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == busStopCodeA && b.BusStopKeyB == busStopCodeB);
            if (conStations != null && conStations.ObjectActive)
                return conStations.Clone();
            else if (conStations != null && !conStations.ObjectActive)
                throw new DO.ExceptionDAL_Inactive("the consecutive stations is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("the consecutive stations not found");
        }

        public void AddConsecutiveStations(ConsecutiveStations newConsecutiveStations)
        {
            ConsecutiveStations existConsecutiveStations = DataSource.ListConsecutiveStations.FirstOrDefault(b => b.BusStopKeyA == newConsecutiveStations.BusStopKeyA && b.BusStopKeyB == newConsecutiveStations.BusStopKeyB);
            if (existConsecutiveStations != null && existConsecutiveStations.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist("Duplicate consecutive stations object");
            else if (existConsecutiveStations != null && existConsecutiveStations.ObjectActive == false)
            {
                existConsecutiveStations.ObjectActive = true;
            }
            else
            {
                newConsecutiveStations.ObjectActive = true;
                DataSource.ListConsecutiveStations.Add(newConsecutiveStations.Clone());
            }
        }

        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            int index = DataSource.ListConsecutiveStations.FindIndex(consecutive => consecutive.BusStopKeyA == consecutiveStations.BusStopKeyA);
            if (DataSource.ListConsecutiveStations[index] != null && DataSource.ListConsecutiveStations[index].ObjectActive)
                DataSource.ListConsecutiveStations[index] = consecutiveStations.Clone();
            else if (DataSource.ListConsecutiveStations[index] != null && !DataSource.ListConsecutiveStations[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive("the consecutive stations is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("the consecutive stations not found");
        }

        public void UpdateConsecutiveStations(int busStopCodeA, int busStopCodeB, Action<ConsecutiveStations> update) // method that knows to updt specific fields in Person
        {
            ConsecutiveStations busConsecutiveUpdate = GetConsecutiveStations(busStopCodeA, busStopCodeB);
            update(busConsecutiveUpdate);
            UpdateConsecutiveStations(busConsecutiveUpdate);
        }

        public void DeleteConsecutiveStations(int busStopCodeA, int busStopCodeB)
        {
            ConsecutiveStations busConsecutive = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == busStopCodeA && b.BusStopKeyB == busStopCodeB);
            if (busConsecutive != null && busConsecutive.ObjectActive)
                busConsecutive.ObjectActive = false;
            else if (busConsecutive != null && !busConsecutive.ObjectActive)
                throw new DO.ExceptionDAL_Inactive("the consecutive stations is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("the consecutive stations not found");
        }
        #endregion  //

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
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"the line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"line departure key not found: {busLineID}");
        }

        public void AddLineDeparture(LineDeparture lineDeparture)
        {
            LineDeparture existLineDeparture = DataSource.ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID && b.DepartureTime == lineDeparture.DepartureTime);
            if (existLineDeparture != null && existLineDeparture.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(lineDeparture.BusLineID, "Duplicate line departure time");
            else if (existLineDeparture != null && existLineDeparture.ObjectActive == false)
            {
                existLineDeparture.ObjectActive = true;
            }
            else
            {
                lineDeparture.DepartureID = Config.RunningNumLineDeparture;
                lineDeparture.ObjectActive = true;
                DataSource.ListLineDepartures.Add(lineDeparture.Clone());
            }
        }

        public void UpdateLineDeparture(LineDeparture lineDeparture)
        {
            int index = DataSource.ListLineDepartures.FindIndex(lineDep => lineDep.BusLineID == lineDeparture.BusLineID);
            if (DataSource.ListLineDepartures[index] != null && DataSource.ListLineDepartures[index].ObjectActive)
                DataSource.ListLineDepartures[index] = lineDeparture.Clone();
            else if (DataSource.ListLineDepartures[index] != null && !DataSource.ListLineDepartures[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(lineDeparture.BusLineID, $"the line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(lineDeparture.BusLineID, $"line departure key not found: {lineDeparture.BusLineID}");
        }
        public void UpdateLineDeparture(int busLineID, Action<LineDeparture> update) // method that knows to updt specific fields in Person
        {
            LineDeparture busUpdate = GetLineDeparture(busLineID);
            update(busUpdate);
        }
        public void DeleteLineDeparture(TimeSpan departureTime, int busLineID)
        {
            LineDeparture bus = DataSource.ListLineDepartures.Find(b => b.BusLineID == busLineID && b.DepartureTime == departureTime);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.DepartureID, $"the line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.DepartureID, $"line departure key not found: {bus.DepartureID}");
        }
        public void DeleteLineDepartureByID(int busLineID)
        {
            foreach (LineDeparture item in DataSource.ListLineDepartures)
            {
                if (item.BusLineID == busLineID)
                    item.ObjectActive = false;
            }
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
            else if (user != null && !user.ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(userName, $"the user is  inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(userName, $"user name not found: {userName}");
        }

        public void AddUser(User user)
        {
            User existUser = DataSource.ListUsers.FirstOrDefault(b => b.UserName == user.UserName);
            if (existUser != null && existUser.ObjectActive == true)
                throw new DO.ExceptionDAL_UserAlreadyExist(user.UserName, "Duplicate user name");
            else if (existUser != null && existUser.ObjectActive == false)
            {
                existUser.ObjectActive = true;
                existUser = user.Clone();
            }
            else
                existUser.ObjectActive = true;
            DataSource.ListUsers.Add(user.Clone());
        }

        public void UpdateUser(User user)
        {
            int index = DataSource.ListUsers.FindIndex(user1 => user1.UserName == user.UserName);
            if (DataSource.ListUsers[index] != null && DataSource.ListUsers[index].ObjectActive)
                DataSource.ListUsers[index] = user.Clone();
            else if (DataSource.ListUsers[index] != null && !DataSource.ListUsers[index].ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(user.UserName, $"the user is inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(user.UserName, $"user name not found: {user.UserName}");
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
            else if (user != null && !user.ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(userName, $"the user is inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(userName, $"user name not found: {userName}");
        }
        #endregion

    }
}





