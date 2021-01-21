using DalApi;
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


        #region Bus  // IDL implemetation for Class Bus objects (crud)

        /// <summary>
        /// Return all buses using linq
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Bus> GetAllBuses()
        {

            return from bus in DataSource.ListBuses
                   where bus.ObjectActive == true
                   select bus.Clone();

        }

        /// <summary>
        /// Return all buses with condition using predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            return from bus in DataSource.ListBuses
                   where predicate(bus)
                   select bus.Clone();
        }

        /// <summary>
        /// Return bus by is license
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public Bus GetBus(int license)
        {
            Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            if (bus != null && bus.ObjectActive)                        //in case the bus exist
                return bus.Clone();
            else if (bus != null && !bus.ObjectActive)                  //in case the bus exist but inactive
                throw new DO.ExceptionDAL_Inactive(license, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"license key not found: {bus.License}");
        }

        /// <summary>
        /// add bus to the list
        /// </summary>
        /// <param name="bus"></param>
        public void AddBus(Bus bus)
        {
            Bus existBus = DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License);
            if (existBus != null && existBus.ObjectActive == true)                  //in case the bus alredy exist
                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.License, "The bus alredy exist");
            else if (existBus != null && existBus.ObjectActive == false)            //in case the bus exist but inactive
            {
                existBus.ObjectActive = true;
                existBus = bus.Clone();
            }
            else                                                        //in case the bus is not exist
            {
                bus.ObjectActive = true;
                DataSource.ListBuses.Insert(0, bus.Clone());
            }
        }

        /// <summary>
        /// Update exist bus
        /// </summary>
        /// <param name="bus"></param>
        /// 
        public void UpdateBus(Bus bus)
        {
            int index = DataSource.ListBuses.FindIndex(bus1 => bus1.License == bus.License);                 //find the place of the bus in the list (by index)
            if (DataSource.ListBuses[index] != null && DataSource.ListBuses[index].ObjectActive)             //in case the bus exist  
                DataSource.ListBuses[index] = bus.Clone();                                                   //update the bus
            else if (DataSource.ListBuses[index] != null && !DataSource.ListBuses[index].ObjectActive)      //in case the bus exsit but inactive
                throw new DO.ExceptionDAL_Inactive(bus.License, $"The bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"License key not found: {bus.License}");    //in case the bus not exist
        }

        /// <summary>
        /// Method that knows to update specific fields in Bus
        /// </summary>
        /// <param name="licenseNumber"></param>
        /// <param name="update"></param>
        public void UpdateBus(int licenseNumber, Action<Bus> update)  
        {
            Bus busUpdate = GetBus(licenseNumber);
            update(busUpdate);
        }

        /// <summary>
        /// Unactive specifid Bus
        /// </summary>
        /// <param name="license"></param>
        public void DeleteBus(int license)      
        {
            Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            if (bus != null && bus.ObjectActive)                                //in case the bus found 
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.License, $"The bus is alredy inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"License key not found: {bus.License}");
        }
        #endregion 

        #region BusAtTravel // IDL implemetation for Class BusAtTravel objects (crud) - currently not in use (will exist for future adaption)
        /// <summary>
        /// Return all bus at travel
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusAtTravel> GetAllBusesAtTravel()  
        {
            return from busAtTravel in DataSource.ListBusAtTravels
                   where busAtTravel.ObjectActive == true
                   select busAtTravel.Clone();
        }
        /// <summary>
        /// Returns all bus at travel specifide using predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BusAtTravel> GetAllBusesAtTravelBy(Predicate<BusAtTravel> predicate)
        {
            return from busAtTravel in DataSource.ListBusAtTravels
                   where predicate(busAtTravel)
                   select busAtTravel.Clone();
        }
        /// <summary>
        /// Get bus at travel by is license
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public BusAtTravel GetBusAtTravel(int license)
        {
            BusAtTravel bus = DataSource.ListBusAtTravels.Find(b => b.License == license);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(license, $"The bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"The bus was not found: {bus.License}");
        }
        /// <summary>
        /// Add bus at travel to the list
        /// </summary>
        /// <param name="busAtTravel"></param>
        public void AddBusAtTravel(BusAtTravel busAtTravel)
        {
            BusAtTravel existBus = DataSource.ListBusAtTravels.FirstOrDefault(b => b.BusLineID == busAtTravel.BusLineID);

            if (existBus != null && existBus.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busAtTravel.BusLineID, "The bus line is already at travel");
            else if (existBus != null && existBus.ObjectActive == false)
            {
                existBus.ObjectActive = true;
                existBus = busAtTravel.Clone();
            }
            else
            {
                busAtTravel.BusLineID = Config.RunningNumBusAtTravel;               //update the running number of the bus 
                busAtTravel.ObjectActive = true;
                DataSource.ListBusAtTravels.Add(busAtTravel.Clone());
            }
        }
        /// <summary>
        /// Update an existing bus at travel
        /// </summary>
        /// <param name="busAtTravel"></param>
        public void UpdateBusAtTravel(BusAtTravel busAtTravel)
        {
            int index = DataSource.ListBusAtTravels.FindIndex(bus1 => bus1.License == busAtTravel.License);
            if (DataSource.ListBusAtTravels[index] != null && DataSource.ListBusAtTravels[index].ObjectActive)
                DataSource.ListBusAtTravels[index] = busAtTravel.Clone();
            else if (DataSource.ListBusAtTravels[index] != null && !DataSource.ListBusAtTravels[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busAtTravel.BusLineID, $"The bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busAtTravel.BusLineID, $"License key not found: {busAtTravel.BusLineID}");
        }
        /// <summary>
        /// Method that knows to update specific fields in BusAtTravel
        /// </summary>
        /// <param name="license"></param>
        /// <param name="update"></param>
        public void UpdateBusAtTravel(int license, Action<BusAtTravel> update) 
        {
            BusAtTravel busUpdate = GetBusAtTravel(license);
            update(busUpdate);
            UpdateBusAtTravel(busUpdate);
        }
        /// <summary>
        /// Delete bus at travel by make it unactive
        /// </summary>
        /// <param name="license"></param>
        public void DeleteBusAtTravel(int license)
        {
            BusAtTravel bus = DataSource.ListBusAtTravels.Find(b => b.License == license);

            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.License, $"the bus is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"license key not found: {bus.License}");
        }
        #endregion

        #region BusLineStation // IDL implemetation for Class BusLineStation objects (crud)
        /// <summary>
        /// Returns all buses line stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusLineStation> GetAllBusLineStations()
        {
            return from busLineStation in DataSource.ListBusLineStations
                   where busLineStation.ObjectActive == true
                   select busLineStation.Clone();
        }
        /// <summary>
        /// Return all buses lines stations sprcifide using predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BusLineStation> GetAllBusLineStationsBy(Predicate<BusLineStation> predicate)
        {
            return from busLineStation in DataSource.ListBusLineStations
                   where predicate(busLineStation)
                   select busLineStation.Clone();
        }
        /// <summary>
        /// Return bus line station using is special ID and is bos stop key
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="busStopCode"></param>
        /// <returns></returns>
        public BusLineStation GetBusLineStation(int busLineID, int busStopCode)
        {
            BusLineStation bus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busStopCode && b.BusLineID == busLineID 
            && b.ObjectActive == true); // Assuming only active stations will be asked for 

            if (bus != null)
                return bus.Clone();
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopCode, $"Station key not found: {busStopCode}");
        }
        /// <summary>
        /// Add bus line station to the list
        /// </summary>
        /// <param name="busLineStation"></param>
        public void AddBusLineStation(BusLineStation busLineStation)
        {
            BusLineStation existBusLineStation = DataSource.ListBusLineStations.FirstOrDefault(b => b.BusStopKey == busLineStation.BusStopKey //in case we found the specific bus line station (by is ID and bus stop key)
            && b.BusLineID == busLineStation.BusLineID && b.PrevStation == busLineStation.PrevStation && b.NextStation == busLineStation.NextStation); // and checking by the prev and next (for avoiding collision with unactive bus line station)
            if (existBusLineStation != null && existBusLineStation.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busLineStation.BusStopKey, "The bus line station already exist");
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
        /// <summary>
        /// Update bus line station 
        /// </summary>
        /// <param name="busLineStation"></param>
        public void UpdateBusLineStation(BusLineStation busLineStation) // Assuming it will be asked to update only an active bus line station
        {
            //get the index of the specific bus line stationn in the list (using is speicel ID and is Bus Stop Key
            int index = DataSource.ListBusLineStations.FindIndex(bus1 => bus1.BusLineID == busLineStation.BusLineID && bus1.BusStopKey == busLineStation.BusStopKey && bus1.ObjectActive == true);
            if (DataSource.ListBusLineStations[index] != null)
            {
                DataSource.ListBusLineStations.Remove(DataSource.ListBusLineStations[index]);       //remove the current bus line station
                DataSource.ListBusLineStations.Add(busLineStation.Clone());                         //and add the update line station insted
            }
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineStation.BusStopKey, $"Station key not found: {busLineStation.BusStopKey}");
        }
        /// <summary>
        /// // method that knows to update specific fields in BusLineStation using is ID and Bus Stop Key
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="busStopCode"></param>
        /// <param name="update"></param>
        public void UpdateBusLineStation(int busLineID, int busStopCode, Action<BusLineStation> update) 
        {
            BusLineStation busUpdate = GetBusLineStation(busLineID, busStopCode);
            update(busUpdate);
            UpdateBusLineStation(busUpdate);
        }
        /// <summary>
        /// Delete a spesific busLineStation by make it unactive
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="busStopCode"></param>
        public void DeleteBusLineStation(int busLineID, int busStopCode)
        {
            BusLineStation bus = DataSource.ListBusLineStations.Find(b => b.BusStopKey == busStopCode && b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopCode, $"The bus line station is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopCode, $"Station key not found: {busStopCode}");
        }
        #endregion

        #region BusLine  // IDL implemetation for Class BusLine objects (crud)

        /// <summary>
        /// Return all bus lines
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from busLine in DataSource.ListBusLines
                   where busLine.ObjectActive == true
                   select busLine.Clone();
        }


        /// <summary>
        /// Return all specific bus line by predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            return from busLine in DataSource.ListBusLines
                   where predicate(busLine)
                   select busLine.Clone();
        }


        /// <summary>
        /// Get bus line by is ID (runing number)
        /// </summary>
        /// <param name="busLineID"></param>
        /// <returns></returns>
        public BusLine GetBusLine(int busLineID)
        {
            BusLine bus = DataSource.ListBusLines.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                return bus.Clone();
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"The bus line is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"Bus line number not found: {busLineID}");
        }


        /// <summary>
        /// Add bus line to the list
        /// </summary>
        /// <param name="busLine"></param>
        /// <returns></returns>
        public int AddBusLine(BusLine busLine)
        {
            int idToReturn;             //return the id for updating that in the BL
            BusLine existBusLine = DataSource.ListBusLines.FirstOrDefault(b => b.BusLineNumber == busLine.BusLineNumber && b.FirstBusStopKey == busLine.FirstBusStopKey
                && b.LastBusStopKey == busLine.LastBusStopKey);
            if (existBusLine != null && existBusLine.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busLine.BusLineNumber, "The bus line alredy exist");

            else if (existBusLine != null && existBusLine.ObjectActive == false)
            {
                existBusLine.ObjectActive = true;
                idToReturn = existBusLine.BusLineID;
                existBusLine = busLine.Clone();
            }
            else
            {
                busLine.BusLineID = Config.RunningNumBusLine;       //update is id (runing number)
                busLine.ObjectActive = true;
                idToReturn = busLine.BusLineID;
                DataSource.ListBusLines.Add(busLine.Clone());
            }
            return idToReturn;
        }
        /// <summary>
        /// Updaet bus line
        /// </summary>
        /// <param name="busLine"></param>
        public void UpdateBusLine(BusLine busLine)
        {
            int index = DataSource.ListBusLines.FindIndex(bus1 => bus1.BusLineID == busLine.BusLineID);   //find the index of the bus line by is ID
            if (DataSource.ListBusLines[index] != null && DataSource.ListBusLines[index].ObjectActive)
            {
                DataSource.ListBusLines.Remove(DataSource.ListBusLines[index]);                     //remove the existing one
                DataSource.ListBusLines.Add(busLine.Clone());                                       //and add to is place the update one
            }

            else if (DataSource.ListBusLines[index] != null && !DataSource.ListBusLines[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLine.BusLineID, $"The bus line is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLine.BusLineID, $"Bus line number not found: {busLine.BusLineID}");
        }
        /// <summary>
        /// Method that knows to update specific fields in BusLine by is "Bus line number"
        /// </summary>
        /// <param name="busLineNumber"></param>
        /// <param name="update"></param>
        public void UpdateBusLine(int busLineNumber, Action<BusLine> update) 
        {
            BusLine busUpdate = GetBusLine(busLineNumber);
            update(busUpdate);
            UpdateBusLine(busUpdate);
        }
        /// <summary>
        /// Delete bus line by make it unactive
        /// </summary>
        /// <param name="busLineID"></param>
        public void DeleteBusLine(int busLineID)
        {
            BusLine bus = DataSource.ListBusLines.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                bus.ObjectActive = false;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"The bus line is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"Bus line number not found: {busLineID}");
        }
        #endregion

        #region BusStop  // IDL implemetation for Class BusStop objects (crud)

        /// <summary>
        /// Returns all bus stops
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BusStop> GetAllBusStops()
        {
            return from busStopLine in DataSource.ListBusStops
                   where busStopLine.ObjectActive == true
                   select busStopLine.Clone();
        }


        /// <summary>
        /// Return all bus stops specifid using predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate)
        {
            return from busStop in DataSource.ListBusStops
                   where predicate(busStop)
                   select busStop.Clone();
        }


        /// <summary>
        /// Get bus stop by is Key
        /// </summary>
        /// <param name="busStopKey"></param>
        /// <returns></returns>
        public BusStop GetBusStop(int busStopKey)
        {
            BusStop busStop = DataSource.ListBusStops.Find(b => b.BusStopKey == busStopKey);
            if (busStop != null && busStop.ObjectActive)
                return busStop.Clone();
            else if (busStop != null && !busStop.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopKey, $"The bus stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopKey, $"Bus stop key not found: {busStopKey}");
        }


        /// <summary>
        /// Adding new bus stop to the list
        /// </summary>
        /// <param name="busStop"></param>
        public void AddBusStop(BusStop busStop)
        {
            BusStop existStop = DataSource.ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey);
            if (existStop != null && existStop.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busStop.BusStopKey, "Bus stop alredy exist");
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


        /// <summary>
        /// Update bus stop
        /// </summary>
        /// <param name="busStop"></param>
        public void UpdateBusStop(BusStop busStop)
        {
           //find the index of the specific bus stop to update
            int index = DataSource.ListBusStops.FindIndex(b => b.BusStopKey == busStop.BusStopKey);
            if (DataSource.ListBusStops[index] != null && DataSource.ListBusStops[index].ObjectActive)
                DataSource.ListBusStops[index] = busStop.Clone();                       //update the bus stop
            else if (DataSource.ListBusStops[index] != null && !DataSource.ListBusStops[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStop.BusStopKey, $"The bus stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStop.BusStopKey, $"Bus stop key not found");
        }


        /// <summary>
        /// Method that knows to update specific fields in BusStop using is key
        /// </summary>
        /// <param name="busStopKey"></param>
        /// <param name="update"></param>
        public void UpdateBusStop(int busStopKey, Action<BusStop> update) 
        {
            BusStop busStopUpdate = GetBusStop(busStopKey);
            update(busStopUpdate);
        }


        /// <summary>
        /// Delete bus stop by make it unactive
        /// </summary>
        /// <param name="busStopKey"></param>
        public void DeleteBusStop(int busStopKey)
        {
            BusStop existBusStop = DataSource.ListBusStops.Find(b => b.BusStopKey == busStopKey);
            if (existBusStop != null && existBusStop.ObjectActive)
                existBusStop.ObjectActive = false;
            else if (existBusStop != null && !existBusStop.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopKey, $"the bus stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopKey, $"bus stop key not found: {busStopKey}");
        }
        #endregion

        #region ConsecutiveStations // IDL implemetation for Class ConsecutiveStations objects (crud)
        /// <summary>
        /// Return all consecutive station
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            return from busConsecutiveStations in DataSource.ListConsecutiveStations
                   where busConsecutiveStations.ObjectActive == true
                   select busConsecutiveStations.Clone();
        }
        /// <summary>
        /// Return all Consecutive Stations specifide using predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)
        {
            return from busConsecutiveStations in DataSource.ListConsecutiveStations
                   where predicate(busConsecutiveStations)
                   select busConsecutiveStations.Clone();
        }
        /// <summary>
        /// Get Consecutive Stations by is tow stations key
        /// </summary>
        /// <param name="busStopCodeA"></param>
        /// <param name="busStopCodeB"></param>
        /// <returns></returns>
        public ConsecutiveStations GetConsecutiveStations(int busStopCodeA, int busStopCodeB)
        {
            ConsecutiveStations conStations = DataSource.ListConsecutiveStations.Find(b => b.BusStopKeyA == busStopCodeA && b.BusStopKeyB == busStopCodeB);
            if (conStations != null && conStations.ObjectActive)
                return conStations.Clone();
            else if (conStations != null && !conStations.ObjectActive)
                throw new DO.ExceptionDAL_Inactive("The consecutive stations is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("The consecutive stations not found");
        }
        /// <summary>
        /// Add Consecutive Stations to the list
        /// </summary>
        /// <param name="newConsecutiveStations"></param>
        public void AddConsecutiveStations(ConsecutiveStations newConsecutiveStations)
        {
            ConsecutiveStations existConsecutiveStations = DataSource.ListConsecutiveStations.FirstOrDefault(b => b.BusStopKeyA == newConsecutiveStations.BusStopKeyA && b.BusStopKeyB == newConsecutiveStations.BusStopKeyB);
            if (existConsecutiveStations != null && existConsecutiveStations.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist("The consecutive stations alredy exist");
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
        /// <summary>
        /// Update Consecutive Stations
        /// </summary>
        /// <param name="consecutiveStations"></param>
        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            //find the index of thr Consecutive Stations using is two stations keys
            int index = DataSource.ListConsecutiveStations.FindIndex(consecutive => consecutive.BusStopKeyA == consecutiveStations.BusStopKeyA && consecutive.BusStopKeyB == consecutiveStations.BusStopKeyB);
            if (DataSource.ListConsecutiveStations[index] != null && DataSource.ListConsecutiveStations[index].ObjectActive)
                DataSource.ListConsecutiveStations[index] = consecutiveStations.Clone();
            else if (DataSource.ListConsecutiveStations[index] != null && !DataSource.ListConsecutiveStations[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive("The consecutive stations is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("The consecutive stations not found");
        }
        /// <summary>
        /// Method that knows to update specific fields in ConsecutiveStations using its two stops keys
        /// </summary>
        /// <param name="busStopCodeA"></param>
        /// <param name="busStopCodeB"></param>
        /// <param name="update"></param>
        public void UpdateConsecutiveStations(int busStopCodeA, int busStopCodeB, Action<ConsecutiveStations> update) 
        {
            ConsecutiveStations busConsecutiveUpdate = GetConsecutiveStations(busStopCodeA, busStopCodeB);
            update(busConsecutiveUpdate);
            UpdateConsecutiveStations(busConsecutiveUpdate);
        }
        /// <summary>
        /// Delete Consecutive Stations by make it unactive
        /// </summary>
        /// <param name="busStopCodeA"></param>
        /// <param name="busStopCodeB"></param>
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

        #region LineDeparture // IDL implemetation for Class LineDeparture objects (crud)
        /// <summary>
        /// Return all Line Departures
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LineDeparture> GetAllLineDeparture()
        {
            return from busLineDeparture in DataSource.ListLineDepartures
                   where busLineDeparture.ObjectActive == true
                   select busLineDeparture.Clone();
        }
        /// <summary>
        /// Return all specifide Line Departure using predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<LineDeparture> GetAllLineDepartureBy(Predicate<LineDeparture> predicate)
        {
            return from busLineDeparture in DataSource.ListLineDepartures
                   where predicate(busLineDeparture)
                   select busLineDeparture.Clone();
        }
        /// <summary>
        /// Get Line Departure using is ID (runing number)
        /// </summary>
        /// <param name="busLineID"></param>
        /// <returns></returns>
        public LineDeparture GetLineDeparture(int busLineID)
        {
            LineDeparture busLineDeparture = DataSource.ListLineDepartures.Find(b => b.BusLineID == busLineID);
            if (busLineDeparture != null && busLineDeparture.ObjectActive)
                return busLineDeparture.Clone();
            else if (busLineDeparture != null && !busLineDeparture.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"The line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"Line departure key not found: {busLineID}");
        }
        /// <summary>
        /// Get Line Departure by is ID (runing number) and is departure time
        /// this function help when we want to delete specific Line Departure
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="busLineID"></param>
        /// <returns></returns>
        public LineDeparture GetLineDepartureByTimeAndLine(TimeSpan departureTime, int busLineID)
        {
            LineDeparture busLineDeparture = DataSource.ListLineDepartures.Find(b => b.BusLineID == busLineID && b.DepartureTime == departureTime);
            if (busLineDeparture != null && busLineDeparture.ObjectActive)
                return busLineDeparture.Clone();
            else if (busLineDeparture != null && !busLineDeparture.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"The line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"Line departure key not found: {busLineID}");
        }
        /// <summary>
        /// Add Line Departure to the list
        /// </summary>
        /// <param name="lineDeparture"></param>
        public void AddLineDeparture(LineDeparture lineDeparture)
        {
            LineDeparture existLineDeparture = DataSource.ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID && b.DepartureTime == lineDeparture.DepartureTime);
            if (existLineDeparture != null && existLineDeparture.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(lineDeparture.BusLineID, "The line departure is alredy exist");
            else if (existLineDeparture != null && existLineDeparture.ObjectActive == false)
            {
                existLineDeparture.ObjectActive = true;
            }
            else
            {
                lineDeparture.DepartureID = Config.RunningNumLineDeparture;    //update the Id of the line departure (runing number)
                lineDeparture.ObjectActive = true;
                DataSource.ListLineDepartures.Add(lineDeparture.Clone());
            }
        }
        /// <summary>
        /// Update Line Departure
        /// </summary>
        /// <param name="lineDeparture"></param>
        public void UpdateLineDeparture(LineDeparture lineDeparture)
        {
            //find the index of the line departure to update
            int index = DataSource.ListLineDepartures.FindIndex(lineDep => lineDep.BusLineID == lineDeparture.BusLineID);
            if (DataSource.ListLineDepartures[index] != null && DataSource.ListLineDepartures[index].ObjectActive)
                DataSource.ListLineDepartures[index] = lineDeparture.Clone();
            else if (DataSource.ListLineDepartures[index] != null && !DataSource.ListLineDepartures[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(lineDeparture.BusLineID, $"The line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(lineDeparture.BusLineID, $"Line departure key not found: {lineDeparture.BusLineID}");
        }
        /// <summary>
        /// Method that knows to update specific fields in Line Departure usin gis ID (runing number)
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="update"></param>
        public void UpdateLineDeparture(int busLineID, Action<LineDeparture> update) 
        {
            LineDeparture busUpdate = GetLineDeparture(busLineID);
            update(busUpdate);
        }
        /// <summary>
        /// Delete Line Departure by make it unactive
        /// </summary>
        /// <param name="departureID"></param>
        public void DeleteLineDeparture(int departureID)
        {
            LineDeparture dep = DataSource.ListLineDepartures.Find(b => b.DepartureID == departureID);
            if (dep != null && dep.ObjectActive)
                dep.ObjectActive = false;
            else if (dep != null && !dep.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(dep.DepartureID, $"The line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(dep.DepartureID, $"Line departure key not found: {dep.DepartureID}");
        }

        #endregion

        #region User // IDL implemetation for Class User objects (crud)
        /// <summary>
        /// Return all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return from user in DataSource.ListUsers
                   where user.ObjectActive == true
                   select user.Clone();
        }
        /// <summary>
        /// Return all specific users usinf predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            return from user in DataSource.ListUsers
                   where predicate(user)
                   select user.Clone();
        }
        /// <summary>
        /// Get user by is user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            User user = DataSource.ListUsers.Find(b => b.UserName == userName);
            if (user != null && user.ObjectActive)
                return user.Clone();
            else if (user != null && !user.ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(userName, $"The user is  inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(userName, $"User name not found: {userName}");
        }
        /// <summary>
        /// Add user to the system
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(User user)
        {
            User existUser = DataSource.ListUsers.FirstOrDefault(b => b.UserName == user.UserName);
            if (existUser != null && existUser.ObjectActive == true)
                throw new DO.ExceptionDAL_UserAlreadyExist(user.UserName, "User name is alredy exist");
            else if (existUser != null && existUser.ObjectActive == false)
            {
                existUser.ObjectActive = true;
                existUser = user.Clone();
            }
            else
            {
                user.ObjectActive = true;
                DataSource.ListUsers.Add(user.Clone());
            }
        }
        /// <summary>
        /// Update user informations
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            //find the index of the user to update
            int index = DataSource.ListUsers.FindIndex(user1 => user1.UserName == user.UserName);
            if (DataSource.ListUsers[index] != null && DataSource.ListUsers[index].ObjectActive)
                DataSource.ListUsers[index] = user.Clone();
            else if (DataSource.ListUsers[index] != null && !DataSource.ListUsers[index].ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(user.UserName, $"The user is inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(user.UserName, $"User name not found: {user.UserName}");
        }
        /// <summary>
        /// Method that knows to update specific fields in user by is user name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="update"></param>
        public void UpdateUser(string userName, Action<User> update) 
        {
            User userUpdate = GetUser(userName);
            update(userUpdate);
        }
        /// <summary>
        /// Delte user from the system by make him inactive
        /// </summary>
        /// <param name="userName"></param>
        public void DeleteUser(string userName)
        {
            User user = DataSource.ListUsers.Find(b => b.UserName == userName);
            if (user != null && user.ObjectActive)
                user.ObjectActive = false;
            else if (user != null && !user.ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(userName, $"The user is inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(userName, $"User name not found: {userName}");
        }
        #endregion

    }
}





