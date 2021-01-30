using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BLApi;
using System.Threading;
using BO;


namespace BL
{
    class BLImp : IBL //internal
    {
        IDal dl = DalFactory.GetDL(); // Asks for the data layer singelton

        #region BusLine: Adaptors and CRUD implementations

        /// <summary>
        /// Adaption from DO to BO of the bus line entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The BO.BusLine</returns>
        BO.BusLine BusLineDoBoAdapter(DO.BusLine busLineDO)
        {
            BO.BusLine busLineBO = new BO.BusLine();
            busLineDO.CopyPropertiesTo(busLineBO); // Using the deep copy method

            try
            {
                // Creation of the line stations (route) iEnumarable
                busLineBO.LineStations = from boLineStation
                                         in GetAllBusLineStations()
                                         where boLineStation.BusLineID == busLineBO.BusLineID
                                         orderby boLineStation.LineStationIndex // Orders by the index
                                         select boLineStation;

                // Creation of the schedule iEnumarable
                busLineBO.Schedule = from doLineDeparture
                                     in dl.GetAllLineDeparture()
                                     where doLineDeparture.BusLineID == busLineBO.BusLineID
                                     orderby doLineDeparture.DepartureTime // Orders by the timeSpan
                                     select doLineDeparture.DepartureTime;
                return busLineBO;
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("The requested doesn't exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                throw new BO.ExceptionBL_Inactive("The requested doesn't active", ex);
            }
        }

        /// <summary>
        /// Adaption from BO to DO of the bus line entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The DO.BusLine</returns>
        DO.BusLine BusLineBoDoAdapter(BO.BusLine busLineBO)
        {
            DO.BusLine busLineDO = new DO.BusLine();
            busLineBO.CopyPropertiesTo(busLineDO); // Using the deep copy method
            return busLineDO;
        }

        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from doBusLine in dl.GetAllBusLines()
                   orderby doBusLine.BusLineID  // Orders by the bus line ID
                   select BusLineDoBoAdapter(doBusLine); // Adapts from DO to BO
        }

        public BusLine GetBusLine(int busLineID)
        {
            return BusLineDoBoAdapter(dl.GetBusLine(busLineID)); // Gets a bus line from bl and adapts it
        }

        public void AddBusLine(BusLine busLine, BusLineStation busLineStationA, BusLineStation busLineStationB)
        {
            int idToReturn; // id for adding the line stations
            DO.BusLine newBus = BusLineBoDoAdapter(busLine); // Adapts the bus line from BO to DO

            try
            {
                idToReturn = dl.AddBusLine(newBus); // Add the bus line and gets an id

                // Adding the consecutive stations entity if needed:

                if (!IsConsecutiveExist(busLine.FirstBusStopKey, busLine.LastBusStopKey))
                {
                    DO.ConsecutiveStations newConStations = new DO.ConsecutiveStations();
                    newConStations.BusStopKeyA = busLine.FirstBusStopKey;
                    newConStations.BusStopKeyB = busLine.LastBusStopKey;
                    newConStations.Distance = busLineStationA.DistanceToNext;
                    newConStations.TravelTime = busLineStationA.TimeToNext;
                    dl.AddConsecutiveStations(newConStations); // Adds the consecutive stations
                }

                // Adding the bus line stations:
                busLineStationA.BusLineID = idToReturn;
                busLineStationB.BusLineID = idToReturn;
                dl.AddBusLineStation(BusLineStationBoDoAdapter(busLineStationA));
                dl.AddBusLineStation(BusLineStationBoDoAdapter(busLineStationB));
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex) // In case the bus line already exist
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("Key or bus stop already exist", ex);
            }
        }

        public void UpdateBusLine(BusLine busLineBO)
        {
            // It is only possible to update the number of the line, so there is only need to call the dl
            try
            {
                dl.UpdateBusLine(BusLineBoDoAdapter(busLineBO));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("The bus line doesn't exist", ex);
            }
        }

        public void DeleteBusLine(int busLineID)
        {
            try
            {
                // Deleting all the line departure objects:
                foreach (DO.LineDeparture item in dl.GetAllLineDepartureBy(x => x.BusLineID == busLineID && x.ObjectActive == true))
                {
                    dl.DeleteLineDeparture(item.DepartureID);
                }

                // Deleting all the stations objects:
                foreach (DO.BusLineStation item in dl.GetAllBusLineStationsBy(x => x.BusLineID == busLineID && x.ObjectActive == true))
                {
                    // Deleting the station itself:
                    dl.DeleteBusLineStation(item.BusLineID, item.BusStopKey);


                    // Deleting the consecutive stations (which existed before the bus line station deletion) if they aren't in use (after deleting the bus line station above)
                    if (item.NextStation != 0 && !IsConsecutiveInUse(item.BusStopKey, item.NextStation))
                        dl.DeleteConsecutiveStations(item.BusStopKey, item.NextStation);
                }

                // Finally, delete the bus line itself
                dl.DeleteBusLine(busLineID);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the bus line doesn't exist
            {
                throw new BO.ExceptionBL_KeyNotFound("The bus line not exist", ex);
            }
        }


        #endregion

        #region BusLineStation: Adaptors and CRUD implementations

        /// <summary>
        /// Adaption from DO to BO of the bus line station entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The BO.BusLineStation</returns>
        BO.BusLineStation BusLineStationDoBoAdapter(DO.BusLineStation busLineStationDO)
        {
            try
            {
                BO.BusLineStation busLineStationBO = new BO.BusLineStation();
                busLineStationDO.CopyPropertiesTo(busLineStationBO); // Copies from do to bo

                // Copies the bus stop name by get bus stop.
                busLineStationBO.BusStopName = dl.GetBusStop(busLineStationBO.BusStopKey).BusStopName;


                if (busLineStationBO.NextStation != 0) // If the next station code isn't 0, it means there must be a consecutive station
                {
                    DO.ConsecutiveStations cons = dl.GetConsecutiveStations(busLineStationBO.BusStopKey, busLineStationBO.NextStation);

                    busLineStationBO.DistanceToNext = cons.Distance;

                    busLineStationBO.TimeToNext = cons.TravelTime;
                }
                else // In case the current station is the last one in the route
                {
                    busLineStationBO.DistanceToNext = 0;

                    busLineStationBO.TimeToNext = TimeSpan.FromMinutes(0);
                }

                return busLineStationBO;
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("The consecutive/bus stop doesn't exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                throw new BO.ExceptionBL_Inactive("The consecutive/bus stop doesn't active", ex);
            }
        }

        /// <summary>
        /// Adaption from BO to DO of the bus line station entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The BO.BusLineStation</returns>
        DO.BusLineStation BusLineStationBoDoAdapter(BO.BusLineStation busLineStationBO)
        {
            DO.BusLineStation busLineStationDO = new DO.BusLineStation();
            busLineStationBO.CopyPropertiesTo(busLineStationDO);
            return busLineStationDO;
        }

        public IEnumerable<BusLineStation> GetAllBusLineStations()
        {
            // A linq query to get and adapt all the bus line stations from dl:
            return from doBusLineStation
                   in dl.GetAllBusLineStations()
                   select BusLineStationDoBoAdapter(doBusLineStation);
        }

        public BusLineStation GetBusLineStation(int busLineID, int busStopCode)
        {
            try
            {
                DO.BusLineStation busStationDO = dl.GetBusLineStation(busLineID, busStopCode); // Gets the line station from dl
                return BusLineStationDoBoAdapter(busStationDO); // Returns the adapted station
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the bus line station doesn't exist
            {
                throw new BO.ExceptionBL_KeyNotFound("bus station does not exist", ex);
            }
        }

        public void AddBusLineStation(BusLineStation newStation, TimeSpan prevGapTimeUpdate, double prevGapKmUpdate)
        {
            int lineID = newStation.BusLineID; // Holds the line id 
            try
            {
                // The station added to the head of the route:
                if (newStation.PrevStation == 0 && newStation.NextStation != 0) // Indicates the new station is added to the head
                {
                    // Updates the prevStation property of the head station by action update:
                    dl.UpdateBusLineStation(lineID, newStation.NextStation, x => x.PrevStation = newStation.BusStopKey); 

                    // Adding the consecutive stations entity if needed (if it's different from zero, it means the admin has been requested to fill the gap)
                    if (newStation.DistanceToNext != 0 && newStation.TimeToNext != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations(); 
                        newCons.BusStopKeyA = newStation.BusStopKey;
                        newCons.BusStopKeyB = newStation.NextStation;
                        newCons.Distance = newStation.DistanceToNext;
                        newCons.TravelTime = newStation.TimeToNext;
                        dl.AddConsecutiveStations(newCons); // Adds the consecutive stations
                    }
                    newStation.LineStationIndex = 0; // The index of the new station is zero
                    dl.UpdateBusLine(lineID, x => x.FirstBusStopKey = newStation.BusStopKey); // The first bus stop key of the line is being updated via action
                }

                // The station added to the end of the route:
                else if (newStation.PrevStation != 0 && newStation.NextStation == 0) // Indicates the new station is added to the end
                {
                    DO.BusLineStation endStation = dl.GetBusLineStation(lineID, newStation.PrevStation); // Gets the last station
                    dl.UpdateBusLineStation(lineID, newStation.PrevStation, x => x.NextStation = newStation.BusStopKey); // Updates the previous last station via action

                    // Adding the consecutive stations entity if needed (if it's different from zero, it means the admin has been requested to fill the gap)
                    if (prevGapKmUpdate != 0 && prevGapTimeUpdate != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                        newCons.BusStopKeyA = newStation.PrevStation;
                        newCons.BusStopKeyB = newStation.BusStopKey;
                        newCons.Distance = prevGapKmUpdate;
                        newCons.TravelTime = prevGapTimeUpdate;
                        dl.AddConsecutiveStations(newCons); // Adds the consecutive stations
                    }
                    newStation.LineStationIndex = endStation.LineStationIndex + 1; // Updates new station index 
                    dl.UpdateBusLine(lineID, x => x.LastBusStopKey = newStation.BusStopKey); // Updates the bus line about the last bus stop via action
                }

                // The station added to the middle of the route:
                else if (newStation.PrevStation != 0 && newStation.NextStation != 0) 
                {
                    // Gets the stations on the both sides of the new added station:
                    DO.BusLineStation prevStation = dl.GetBusLineStation(lineID, newStation.PrevStation);
                    DO.BusLineStation nextStation = dl.GetBusLineStation(lineID, newStation.NextStation);

                    // Updates their prev and next accordingly, via action:
                    dl.UpdateBusLineStation(lineID, newStation.PrevStation, x => x.NextStation = newStation.BusStopKey);
                    dl.UpdateBusLineStation(lineID, newStation.NextStation, x => x.PrevStation = newStation.BusStopKey);

                    // Delete consecutive stations entity if they aren't in use anymore (the new station is between the two previous consecutives!)
                    if (!IsConsecutiveInUse(prevStation.BusStopKey, nextStation.BusStopKey))
                    {
                        // The deletion will give the admin the opportunity to update the consecutive stations if being created again:
                        dl.DeleteConsecutiveStations(prevStation.BusStopKey, nextStation.BusStopKey);
                    }

                    // Adding the consecutive stations entities if needed (from the new one to the next):
                    if (newStation.DistanceToNext != 0 && newStation.TimeToNext != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                        newCons.BusStopKeyA = newStation.BusStopKey;
                        newCons.BusStopKeyB = newStation.NextStation;
                        newCons.Distance = newStation.DistanceToNext;
                        newCons.TravelTime = newStation.TimeToNext;
                        dl.AddConsecutiveStations(newCons);
                    }

                    // Adding the consecutive stations entities if needed (from the previous to the new one):
                    if (prevGapKmUpdate != 0 && prevGapTimeUpdate != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                        newCons.BusStopKeyA = newStation.PrevStation;
                        newCons.BusStopKeyB = newStation.BusStopKey;
                        newCons.Distance = prevGapKmUpdate;
                        newCons.TravelTime = prevGapTimeUpdate;
                        dl.AddConsecutiveStations(newCons);
                    }

                    // Updating the index of the new added station:
                    newStation.LineStationIndex = nextStation.LineStationIndex;
                }


                /// Indices update ///

                int indexAdded = newStation.LineStationIndex;

                // The query gets all the bus line stations which indices are bigger than ther one needed to be added:
                var collection = (from station
                            in dl.GetAllBusLineStations()
                             where station.BusLineID == lineID && station.LineStationIndex >= indexAdded 
                             select station).ToList();

                // Updates the bus line stations collection with action of updating the index UP by 1
                foreach (DO.BusLineStation station in collection)
                {
                    dl.UpdateBusLineStation(lineID, station.BusStopKey, x => x.LineStationIndex++);
                }

                // Finnaly, adding the new bus line station:
                dl.AddBusLineStation(BusLineStationBoDoAdapter(newStation));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the bus line station already exist
            {
                throw new BO.ExceptionBL_KeyNotFound("Bus stop code already exist", ex);
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex)
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("The station already exist", ex);
            }
        }

        public void DeleteBusLineStation(int busLineID, int busStopCode, TimeSpan gapTimeUpdate, double gapKmUpdate)
        {
            try
            {
                BusLine line = GetBusLine(busLineID); // Checks if the station can be deleted (a line must hold at least two stations)
                if (line.LineStations.Count() < 3) // Throws an exception if not
                    throw new ExceptionBL_LessThanThreeStations("There are only two stations in the line. Unable to delete station");

                DO.BusLineStation currentStation = dl.GetBusLineStation(busLineID, busStopCode); // Gets the station to be deleted

                if (currentStation.PrevStation != 0 && currentStation.NextStation != 0) // It means the lateral stations are in the middle
                {
                    // Updates the next station field of the previous to the deleted one, via action:
                    dl.UpdateBusLineStation(busLineID, currentStation.PrevStation, station => station.NextStation = currentStation.NextStation);

                    // Updates the previous station field of the next to the deleted one, via action:
                    dl.UpdateBusLineStation(busLineID, currentStation.NextStation, station => station.PrevStation = currentStation.PrevStation);
                }

                // The station to be deleted is the first one, updates the previous station field of the next to the deleted one, via action:
                else if (currentStation.PrevStation == 0 && currentStation.NextStation != 0) 
                {
                    dl.UpdateBusLineStation(busLineID, currentStation.NextStation, station => station.PrevStation = 0);
                    dl.UpdateBusLine(busLineID, x => x.FirstBusStopKey = currentStation.NextStation); // Updates the bus line itself (first bus stop field)
                }

                // The station to be deleted is the last one, updates the next station field of the previous to the deleted one, via action:
                else if (currentStation.PrevStation != 0 && currentStation.NextStation == 0) 
                { 
                    dl.UpdateBusLineStation(busLineID, currentStation.PrevStation, station => station.NextStation = 0);
                    dl.UpdateBusLine(busLineID, x => x.LastBusStopKey = currentStation.PrevStation);  // Updates the bus line itself (last bus stop field)
                }


                /// Indices update ///

                int indexDeleted = currentStation.LineStationIndex; // The index of the deleted one

                // The query gets all the bus line stations which indices are bigger than ther one needed to be deleted
                var query = (from station
                            in dl.GetAllBusLineStations()
                             where station.BusLineID == busLineID && station.LineStationIndex > indexDeleted
                             select station).ToList();

                // Updates the bus line stations collection with action of updating the index DOWN by 1
                foreach (DO.BusLineStation station in query)
                {
                    dl.UpdateBusLineStation(busLineID, station.BusStopKey, x => x.LineStationIndex--);
                }

                // Consecutive stations addition/deletion:

                if (currentStation.PrevStation != 0 && currentStation.NextStation != 0 && gapKmUpdate != 0) // It means there is a need to fill the consecutive stations info gap (there are no consecutive stations to this case)
                {
                    DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                    newCons.BusStopKeyA = currentStation.PrevStation;
                    newCons.BusStopKeyB = currentStation.NextStation;
                    newCons.Distance = gapKmUpdate;
                    newCons.TravelTime = gapTimeUpdate;
                    dl.AddConsecutiveStations(newCons); // Adds the new consecutive stations entity
                }

                // Finally, deletes the bus line station:
                dl.DeleteBusLineStation(busLineID, busStopCode);

                // Deleting the consecutive stations (which existed before the bus line station deletion) if they aren't in use (after deleting the bus line station above).
                // A deletion of consecutive stations, is enabling the admin to UPDATE THE CONSECUTIVE later, if he wishs the add the bus line station again.
                // Therefore, if the admins wishs to update a consecutive stations object, he must delete all the line stations which creating the consecutive stations, before adding it with the updated time and distance
                // (The logic is simple - if the station is in use by other line, it is problematic to update the distance and time 'without a warning'. The stations should be removed and added again from all the lines and then added again as updated.)
                
                // If the station to deleted is in the middle
                if (currentStation.PrevStation != 0 && currentStation.NextStation != 0)
                {
                    // First gap check and delete if not in use:
                    if (!IsConsecutiveInUse(currentStation.PrevStation, currentStation.BusStopKey))
                        dl.DeleteConsecutiveStations(currentStation.PrevStation, currentStation.BusStopKey);

                    // Second gap check and delete if not in use:
                    if (!IsConsecutiveInUse(currentStation.BusStopKey, currentStation.NextStation))
                        dl.DeleteConsecutiveStations(currentStation.BusStopKey, currentStation.NextStation);
                }

                // In case the station to delete is the first, will delete the "consecutive" if it not in use
                else if (currentStation.PrevStation == 0 && currentStation.NextStation != 0
                    && !IsConsecutiveInUse(currentStation.BusStopKey, currentStation.NextStation))
                    dl.DeleteConsecutiveStations(currentStation.BusStopKey, currentStation.NextStation);

                // In case the station to delete is the last, will delete the "consecutive" if it  not in use
                else if (currentStation.PrevStation != 0 && currentStation.NextStation == 0
                    && !IsConsecutiveInUse(currentStation.PrevStation, currentStation.BusStopKey))
                    dl.DeleteConsecutiveStations(currentStation.PrevStation, currentStation.BusStopKey);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the bus station to delete doesn't exist
            {
                throw new BO.ExceptionBL_KeyNotFound("bus station does not exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case the bus station to delete was already deleted
            {
                throw new BO.ExceptionBL_Inactive("bus station is already inactive", ex);
            }
        }

        #endregion

        #region Bus: Adaptors and CRUD implementations

        /// <summary>
        /// Adaption from DO to BO of the bus entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The BO.Bus</returns>
        BO.Bus busDoBoAdapter(DO.Bus busDO)
        {
            BO.Bus busBO = new BO.Bus();
            busDO.CopyPropertiesTo(busBO); // Deep copy from DO to BO
            return busBO;
        }

        /// <summary>
        /// Adaption from BO to DO of the bus entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The dO.Bus</returns>
        DO.Bus busBoDoAdapter(BO.Bus busBO)
        {
            DO.Bus busDO = new DO.Bus();
            busBO.CopyPropertiesTo(busDO);  // Deep copy from BO to DO
            return busDO;
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from doBus in dl.GetAllBuses() select busDoBoAdapter(doBus); // A linq query to get all the adapted buses
        }

        public Bus GetBus(int license)
        {
            try
            {
                DO.Bus busDO = dl.GetBus(license); // Gets the do bus
                return busDoBoAdapter(busDO); // Adapts and returns the bo bus
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case doesn't exist
            {
                throw new BO.ExceptionBL_KeyNotFound("License does not exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case doesn't active
            {
                throw new BO.ExceptionBL_Inactive("Bus is inactive", ex);
            }
        }

        public void AddBus(BO.Bus busBO)
        {
            try
            {
                if (busBO.MileageAtLastTreat > busBO.Mileage) // In case there is a logical conflict in the mileage values
                {
                    throw new BO.ExceptionBL_MileageValuesConflict("Conflict between mileage values");
                }
                BusStatusUpdate(busBO); // Updates the status of the bus
                dl.AddBus(busBoDoAdapter(busBO)); // Adapts to DO and calls dl to add
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex) // In case the bus already exist
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("License already exist", ex);
            }
        }

        public void UpdateBus(BO.Bus busBO) //busUpdate
        {
            try
            {
                BusStatusUpdate(busBO); // Updates the bus status
                dl.UpdateBus(busBoDoAdapter(busBO)); // Updates the bus by dl 
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case doesn't exist
            {
                throw new BO.ExceptionBL_KeyNotFound("License does not exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case doesn't active
            {
                throw new BO.ExceptionBL_Inactive("Bus is inactive", ex);
            }
        }

        public void DeleteBus(int license)
        {
            try
            {
                dl.DeleteBus(license); // Calls the dl to delete the bus
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case doesn't exist
            {
                throw new BO.ExceptionBL_KeyNotFound("License does not exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case doesn't active
            {
                throw new BO.ExceptionBL_Inactive("Bus is inactive", ex);
            }
        }

        public void RefuelBus(BO.Bus bus)
        {
            if (bus.Fuel == 1200) // In case the gas tank is already full, an exception is being thrown
            {
                throw new BO.ExceptionBL_NoNeedToRefuel(bus.License);
            }
            else
            {
                bus.Fuel = 1200; // Updates the gas tank to 1200
                UpdateBus(bus); // Calls the bl.update function
            }
        }

        public void TreatBus(BO.Bus bus)
        {
            if (bus.Mileage - bus.MileageAtLastTreat < 20000 && bus.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) > 0) // In case the bus doesn't need a treatment (according to mileage and date of last treatment)
            {
                throw new BO.ExceptionBL_NoNeedToTreat(bus.License);
            }
            else
            {
                // Updates the mileage at last treatment and treatment date fields:
                bus.MileageAtLastTreat = bus.Mileage;
                bus.LastTreatmentDate = DateTime.Now;

                // At any treat a refuel is also being made:
                bus.Fuel = 1200;

                UpdateBus(bus); // Calls the bl.update function
            }
        }

        /// <summary>
        /// Updates the bus status, needed for add and update functions
        /// </summary>
        /// <param name="busBo"></param>
        public void BusStatusUpdate(BO.Bus busBo)
        {
            // In case the bus is ready for travel (checked by the mileage, last treatment date (not a year later!) and the gas tank condition
            if (busBo.Mileage - busBo.MileageAtLastTreat < 20000 && busBo.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) > 0 && busBo.Fuel > 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.READY_FOR_TRAVEL;
            // In case the bus is dangerous:
            else if (busBo.Mileage - busBo.MileageAtLastTreat >= 20000 || busBo.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.DANGEROUS;
            // In case there is no fuel
            else if (busBo.Fuel == 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.NEEDS_REFUEL;
        }

        #endregion

        #region BusStop: Adaptors and CRUD implementations

        /// <summary>
        /// Adaption from DO to BO of the bus stop entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The BO.BusLine</returns>
        BO.BusStop BusStopDoBoAdapter(DO.BusStop busStopDO)
        {
            BO.BusStop busStopBO = new BO.BusStop();
            busStopDO.CopyPropertiesTo(busStopBO); // Deep copy from DO to BO entity

            // Filling the linesStopHere iEnumarable BO property: 
            // The query gets all the bo bus lines, checks inside the lineStations for any that stops at the current bus stop
            // It orders the collection by the bus line number, and select the converted bus line to BusLineAtStop BO entity
            busStopBO.LinesStopHere = from boBusLine
                                      in GetAllBusLines()
                                      where boBusLine.LineStations.Any(line => line.BusStopKey == busStopBO.BusStopKey)
                                      orderby boBusLine.BusLineNumber
                                      select BusLinetoBusLineAtStopConvertor(boBusLine, busStopDO.BusStopKey);
            return busStopBO;
        }

        /// <summary>
        /// Adaption from BO to DO of the bus stop entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The DO.BusLine</returns>
        DO.BusStop BusStopBoDoAdapter(BO.BusStop busStopBO)
        {
            DO.BusStop busStopDO = new DO.BusStop();
            busStopBO.CopyPropertiesTo(busStopDO); // Deep copy from BO to DO entity
            return busStopDO;
        }

        public IEnumerable<BusStop> GetAllBusStops()
        {
            return from doBusStop in dl.GetAllBusStops() orderby doBusStop.BusStopKey select BusStopDoBoAdapter(doBusStop); // A linq query to get all the bo adaptions of the bus stop, ordered by its key
        }

        public BusStop GetBusStop(int busStopKeyDO)
        {
            try
            {
                DO.BusStop busStopDO;
                busStopDO = dl.GetBusStop(busStopKeyDO); // Asks the bus stop from the dl
                return BusStopDoBoAdapter(busStopDO); // Returns the bo adaption of the bus stop
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the admin tries to get an unexisting bus stop
            {
                throw new BO.ExceptionBL_KeyNotFound("The bus stop code doesn't exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case the admin tries to get an unactive bus stop
            {
                throw new BO.ExceptionBL_Inactive("The bus stop is inactive", ex);
            }
        }

        public void UpdateBusStop(BO.BusStop busStopBO)
        {
            try
            {
                DO.BusStop busStopBeforeUpdate = dl.GetBusStop(busStopBO.BusStopKey); // Gets the do.busStop before the update

                // In case the admin tries to update the location, while the bus stop currently serves bus lines, it will throw an exception:
                if (busStopBO.BusStopAddress != busStopBeforeUpdate.BusStopAddress || busStopBO.Latitude != busStopBeforeUpdate.Latitude || busStopBO.Longitude != busStopBeforeUpdate.Longitude)
                {
                    if (busStopBO.LinesStopHere.Count() > 0)
                        throw new BO.ExceptionBL_LinesStopHere("The bus stop serves bus lines, and the location cannot be changed.");
                }

                // In case the coordinates are impossible, it throws an exception:
                if (busStopBO.Latitude > 33.3 || busStopBO.Latitude < 31 || busStopBO.Longitude < 34.3 || busStopBO.Longitude > 35.5)
                {
                    throw new BO.ExceptionBL_Incorrect_coordinates("The longitude or the latitude are not matching the range.");
                }

                // Finnaly, updates the bus stop by dl:
                dl.UpdateBusStop(BusStopBoDoAdapter(busStopBO));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the admin tries to update an unexisting bus stop
            {
                throw new BO.ExceptionBL_KeyNotFound("The bus stop code doesn't exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case the admin tries to update an unactive bus stop
            {
                throw new BO.ExceptionBL_Inactive("The bus stop is inactive", ex);
            }
        }

        public void AddBusStop(BO.BusStop busStopBO)
        {
            try
            {
                // Checks if the coordinates are impossible, and throws an exception:
                if (busStopBO.Latitude > 33.3 || busStopBO.Latitude < 31 || busStopBO.Longitude < 34.3 || busStopBO.Longitude > 35.5)
                {
                    throw new BO.ExceptionBL_Incorrect_coordinates("The longitude or the latitude are not matching the range.");
                }
                dl.AddBusStop(BusStopBoDoAdapter(busStopBO)); // Adds the do adaption of the bus stop
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex) // In case the bus stop already exist
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("Bus stop code already exist", ex);
            }
        }

        public void DeleteBusStop(int busStopCode)
        {
            try
            {
                if (BusStopDoBoAdapter(dl.GetBusStop(busStopCode)).LinesStopHere.Count() > 0) // Checks if the bus stop serves any line, therfore cannot be deleted and throws exception
                    throw new BO.ExceptionBL_LinesStopHere("The bus stop serves bus lines, and cannot be deleted.");
                dl.DeleteBusStop(busStopCode); // Else, it calls the dl to delete the bus stop
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the admin tries to delete an unexisting bus stop
            {
                throw new BO.ExceptionBL_KeyNotFound("The bus stop code doesn't exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case the admin tries to delete an unactive bus stop
            {
                throw new BO.ExceptionBL_Inactive("The bus stop is inactive", ex);
            }
        }

        /// <summary>
        /// Creats a new object of bus line at bus stop, containg info about the last bus stop name for the line and the travel time for the line until the bus stop.
        /// </summary>
        /// <param name="busLine"></param>
        /// <param name="busStopCode"></param>
        /// <returns>BO.BusLineAtBusStop</returns>
        BO.BusLineAtBusStop BusLinetoBusLineAtStopConvertor(BO.BusLine busLine, int busStopCode)
        {
            BO.BusLineAtBusStop busLineAtBusStop = new BO.BusLineAtBusStop(); // Creats the new object of bus line at bus stop
            busLine.CopyPropertiesTo(busLineAtBusStop); // Deep copy from the given bus line matching properties
            busLineAtBusStop.LastBusStopName = dl.GetBusStop(busLineAtBusStop.LastBusStopKey).BusStopName; // Updates the last bus stop name
            busLineAtBusStop.TravelTimeToBusStop = StationTravelTimeCalculation(busLine.BusLineID, busStopCode); // The travel calculated by function
            return busLineAtBusStop;
        }

        #endregion

        #region LineDeparture: Add and Delete methods

        public void AddLineDeparture(TimeSpan departureTime, int busLineID)
        {
            try
            {
                DO.LineDeparture newLineDeparture = new DO.LineDeparture(); // Creats the line departure to be added and initialize it
                newLineDeparture.BusLineID = busLineID;
                newLineDeparture.DepartureTime = departureTime;
                dl.AddLineDeparture(newLineDeparture); // Calls the dl to add the line departure
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex) // In case there is already such a departure time
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("The line departure time already exist", ex);
            }
        }

        public void DeleteLineDeparture(TimeSpan departureTime, int busLineID)
        {
            try
            {
                DO.LineDeparture dep = dl.GetLineDepartureByTimeAndLine(departureTime, busLineID); // Gets the departure time to be deleted
                dl.DeleteLineDeparture(dep.DepartureID); // Calls the dl method of deleting the departure time
            }
            catch (DO.ExceptionDAL_KeyNotFound ex) // In case the departure time doesn't exist
            {
                throw new BO.ExceptionBL_KeyNotFound("the line departure time doesn't exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex) // In case the departure time is inactive
            {
                throw new BO.ExceptionBL_Inactive("The line departure time is already inactive", ex);
            }
        }
        #endregion

        #region Consecutive Stations: Boolean functions

        public bool IsConsecutiveExist(int busStopKeyA, int busStopKeyB)
        {
            DO.ConsecutiveStations getConStations = new DO.ConsecutiveStations(); // The consecutive to be asked for
            try
            {
                getConStations = dl.GetConsecutiveStations(busStopKeyA, busStopKeyB);
                return true;// Returns true, because it exists (the dl.GetConsecutive didn't throw anything)
            }
            catch (DO.ExceptionDAL_Inactive)
            {
                return false; // If the consecutive exist but is inactive, the admin will be able to update the distance and time travel
            }
            catch (DO.ExceptionDAL_KeyNotFound)
            {
                return false; // Returns false, because the consecutive doesn't exist
            }
        }

        public bool IsConsecutiveInUse(int busStopKeyA, int busStopKeyB)
        {
            return (from station
                         in dl.GetAllBusLineStations() // Checks at all the bus line stations
                    where station.BusStopKey == busStopKeyA && station.NextStation == busStopKeyB // If the current and next are matching
                    select station).Any(); // Returns true if any of the stations are consecutive
        }

        #endregion

        #region User: Adaptors and CRUD implementations

        /// <summary>
        /// Adaption from DO to BO of the user entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The BO.BusLine</returns>
        BO.User userDoBoAdapter(DO.User userDO)
        {
            BO.User userBO = new BO.User();
            userDO.CopyPropertiesTo(userBO); // Using the deep copy method
            return userBO;
        }

        /// <summary>
        /// Adaption from BO to DO of the user entity
        /// </summary>
        /// <param name="busLineDO"></param>
        /// <returns>The DO.BusLine</returns>
        DO.User userBoDoAdapter(BO.User userBO)
        {
            DO.User userDO = new DO.User();
            userBO.CopyPropertiesTo(userDO); // Using the deep copy method
            return userDO;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return from doUser 
                   in dl.GetAllUsers() 
                   select userDoBoAdapter(doUser); // Returns all the users by dl, adapting it to BO
        }

        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            return from user in dl.GetAllUsers()
                   where predicate(userDoBoAdapter(user)) // Returning all the users in DO which meet the predicate
                   select userDoBoAdapter(user);
        }

        public User GetUser(string userName)
        {
            DO.User userDO;
            try
            {
                userDO = dl.GetUser(userName);
                return userDoBoAdapter(userDO); // Gets the user and returns its adaption
            }
            catch (DO.ExceptionDAL_UserKeyNotFound ex) // In case the user's name doesn't exist
            {
                throw new BO.ExceptionBL_UserKeyNotFound("user dose not exist", ex);
            }
        }

        public void AddUser(User userBO)
        {
            try
            {
                DO.User newUser = userBoDoAdapter(userBO); // Adapts and adds the user trough dl
                dl.AddUser(newUser);
            }
            catch (DO.ExceptionDAL_UserAlreadyExist ex) // In case the user already exist
            {
                throw new BO.ExceptionBL_UserAlreadyExist("user already exist", ex);
            }
        }
        #endregion

        #region Other methods (Especially for the clock simulator)

        public IEnumerable<LineTiming> GetLineTimingsPerStation(BusStop currBusStop, TimeSpan tsCurrentTime)
        {
            // Explanation of the linq query:
            // 1. from - It checks for every line in the LinesStopHere collection (created for BO.BusStop)
            // 2. let -Finds the last departure time for a line according to the current given time (using a function)
            // 3. let - Calculates the time left to the arrival (each BO.BusLineAtBusStop object holds the total time of travel to the bus stop)
            // 4. Where - Filters by the time left within the hour until 5 minutes after the expected arrival
            // 5. Select new - creatrs the new lineTiming object to be added to the new collection

            try
            {
                IEnumerable<LineTiming> stationTimings = from lineAtStop in GetBusStop(currBusStop.BusStopKey).LinesStopHere
                                                         let depTime = FindLastDepartureTime(lineAtStop.BusLineID, tsCurrentTime)
                                                         let timeLeft = depTime.Add(lineAtStop.TravelTimeToBusStop).Subtract(tsCurrentTime)
                                                         where timeLeft.Hours == 0 && timeLeft.Minutes > -5 // It means the bus is late or passed maximum by 5 minutes, but only buses in a margin of one hour will be showed
                                                         select new LineTiming
                                                         {
                                                             BusLineNumber = lineAtStop.BusLineNumber,
                                                             LastBusStopName = lineAtStop.LastBusStopName,
                                                             DepartureTime = depTime,
                                                             ArrivalTime = depTime.Add(lineAtStop.TravelTimeToBusStop), // Adds the total travel time to the departure timr
                                                             MinutesToArrival = timeLeft.Minutes,
                                                             ShowMinutesOrArrow = timeLeft.CompareTo(TimeSpan.Zero) > 0 ? timeLeft.Minutes.ToString() : "↓", // Checks the time left, and decides if to show minutes or arrow
                                                         };

                return stationTimings.OrderBy(x => x.MinutesToArrival); // Orders the objects by the minutes left to the arrival (the closest will appear first) and returns the collection
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new ExceptionBL_KeyNotFound("The bus stop code doesn't exist.", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                throw new ExceptionBL_Inactive("The bus stop is inactive", ex);
            }
            catch
            {
                throw new ExceptionBL_UnexpectedProblem("Unexpected problem occured");
            }
        }

        public TimeSpan FindLastDepartureTime(int busLineID, TimeSpan tsCurrentTime)
        {
            try
            {
                // Linq query for the departure times of the desired bus line
                var collection = (from lineDeparture in dl.GetAllLineDeparture()
                                  where lineDeparture.BusLineID == busLineID
                                  select lineDeparture);


                if (collection.Count() == 0)            // It means the line has no departure times (yet, or by manager accident)
                    return TimeSpan.FromMinutes(-1000); // A very far number so it will be exculded in the Linq Query of GetLineTimingsPerStation

                else
                {
                    var collB = collection.OrderBy(x => tsCurrentTime.Subtract(x.DepartureTime)); // Orders the departure times by the closest to the current time 

                    DO.LineDeparture lastDep = new DO.LineDeparture();
                    lastDep = collB.FirstOrDefault(x => tsCurrentTime.Subtract(x.DepartureTime).CompareTo(TimeSpan.Zero) > 0); // Takes the first one (the closest departure time)
                    if (lastDep == null)                    // If there is no such departure time
                        return TimeSpan.FromMinutes(-1000); // As explained above
                    else
                        return lastDep.DepartureTime;       // Returns the departure time
                }
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new ExceptionBL_KeyNotFound("The bus line doesn't exist.", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                throw new ExceptionBL_Inactive("The bus line is inactive", ex);
            }
            catch
            {
                throw new ExceptionBL_UnexpectedProblem("Unexpected problem occured");
            }
        }

        public TimeSpan StationTravelTimeCalculation(int busLineID, int busStopCode)
        {
            try
            {
                if (!GetBusLine(busLineID).LineStations.Any(x => x.BusStopKey == busStopCode)) // in case the bus stop doesn't exist in the bus line route
                {
                    throw new BO.ExceptionBL_BusStopNotExistInTheRoute("The bus stop code doesn't sevre the given bus line");
                }

                // Creates a collection of all the bus stops in the routes until the current bus stop:
                var collection = GetBusLine(busLineID).LineStations.TakeWhile(x => x.BusStopKey != busStopCode);

                if (collection == null) // It means that the bus stop is the first one, so it returns zero timeSpan
                    return TimeSpan.Zero;
                else
                {
                    // The calculation of the travel time proccess (using aggregate)
                    TimeSpan calc = collection.Aggregate
                        (TimeSpan.Zero,
                        (sumSoFar, nextMyObject) => sumSoFar + nextMyObject.TimeToNext);
                    return calc; // Returns the calculated
                }
            } // In case the bus line doesn't exist (or inactive):
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new ExceptionBL_KeyNotFound("The bus line wasn't found", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                throw new ExceptionBL_Inactive("The bus line doesn't active (deleted)", ex);
            }
            catch
            {
                throw new ExceptionBL_UnexpectedProblem("Unexpected problem occured");
            }
        }

        public bool isTimeSpanInvalid(TimeSpan timeUpdate)
        {
            return (timeUpdate.Days > 0 || timeUpdate.Hours > 23 || timeUpdate.Minutes > 59 || timeUpdate.Seconds > 59);
        }

        #endregion
    }
}