﻿using System;
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
        /// <returns>The BO.BusLine</returns>
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
            return from doBusLineStation
                   in dl.GetAllBusLineStations()
                   select BusLineStationDoBoAdapter(doBusLineStation);
        }


        public BusLineStation GetBusLineStation(int busLineID, int busStopCode)
        {
            DO.BusLineStation busStationDO;
            try
            {
                busStationDO = dl.GetBusLineStation(busLineID, busStopCode);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("bus station does not exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                throw new BO.ExceptionBL_Inactive("bus station is inactive", ex);
            }
            return BusLineStationDoBoAdapter(busStationDO);
        }

        public void AddBusLineStation(BusLineStation newStation, TimeSpan prevGapTimeUpdate, double prevGapKmUpdate)
        {
            int lineID = newStation.BusLineID;
            try
            {
                if (newStation.PrevStation == 0 && newStation.NextStation != 0) // The station added to the head of the route
                {
                    DO.BusLineStation headStation = dl.GetBusLineStation(lineID, newStation.NextStation);
                    dl.UpdateBusLineStation(lineID, newStation.NextStation, x => x.PrevStation = newStation.BusStopKey);

                    // Adding the consecutive stations entity if needed
                    if (newStation.DistanceToNext != 0 && newStation.TimeToNext != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                        newCons.BusStopKeyA = newStation.BusStopKey;
                        newCons.BusStopKeyB = newStation.NextStation;
                        newCons.Distance = newStation.DistanceToNext;
                        newCons.TravelTime = newStation.TimeToNext;
                        dl.AddConsecutiveStations(newCons);
                    }
                    newStation.LineStationIndex = 0;
                    dl.UpdateBusLine(lineID, x => x.FirstBusStopKey = newStation.BusStopKey);
                }
                else if (newStation.PrevStation != 0 && newStation.NextStation == 0)  // The station added to the end of the route
                {
                    DO.BusLineStation endStation = dl.GetBusLineStation(lineID, newStation.PrevStation);
                    dl.UpdateBusLineStation(lineID, newStation.PrevStation, x => x.NextStation = newStation.BusStopKey);

                    // Adding the consecutive stations entity if needed
                    if (prevGapKmUpdate != 0 && prevGapTimeUpdate != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                        newCons.BusStopKeyA = newStation.PrevStation;
                        newCons.BusStopKeyB = newStation.BusStopKey;
                        newCons.Distance = prevGapKmUpdate;
                        newCons.TravelTime = prevGapTimeUpdate;
                        dl.AddConsecutiveStations(newCons);
                    }
                    newStation.LineStationIndex = endStation.LineStationIndex + 1;
                    dl.UpdateBusLine(lineID, x => x.LastBusStopKey = newStation.BusStopKey);
                }

                else if (newStation.PrevStation != 0 && newStation.NextStation != 0)  // The station added to the middle of the route
                {
                    DO.BusLineStation prevStation = dl.GetBusLineStation(lineID, newStation.PrevStation);
                    DO.BusLineStation nextStation = dl.GetBusLineStation(lineID, newStation.NextStation);
                    dl.UpdateBusLineStation(lineID, newStation.PrevStation, x => x.NextStation = newStation.BusStopKey);
                    dl.UpdateBusLineStation(lineID, newStation.NextStation, x => x.PrevStation = newStation.BusStopKey);

                    // Delete consecutive stations entity if they aren't in use anymore (the new station is between the two previous consecutives!)
                    if (!IsConsecutiveInUse(prevStation.BusStopKey, nextStation.BusStopKey))
                    {
                        dl.DeleteConsecutiveStations(prevStation.BusStopKey, nextStation.BusStopKey);
                    }

                    // Adding the consecutive stations entities if needed
                    if (newStation.DistanceToNext != 0 && newStation.TimeToNext != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                        newCons.BusStopKeyA = newStation.BusStopKey;
                        newCons.BusStopKeyB = newStation.NextStation;
                        newCons.Distance = newStation.DistanceToNext;
                        newCons.TravelTime = newStation.TimeToNext;
                        dl.AddConsecutiveStations(newCons);
                    }
                    if (prevGapKmUpdate != 0 && prevGapTimeUpdate != TimeSpan.FromMinutes(0))
                    {
                        DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();
                        newCons.BusStopKeyA = newStation.PrevStation;
                        newCons.BusStopKeyB = newStation.BusStopKey;
                        newCons.Distance = prevGapKmUpdate;
                        newCons.TravelTime = prevGapTimeUpdate;
                        dl.AddConsecutiveStations(newCons);
                    }
                    newStation.LineStationIndex = nextStation.LineStationIndex;
                }

                // Indices update:
                int indexAdded = newStation.LineStationIndex;

                // The query gets all the bus line stations which indices are bigger than ther one needed to be deleted
                var query = (from station
                            in dl.GetAllBusLineStations()
                             where station.BusLineID == lineID && station.LineStationIndex >= indexAdded
                             select station).ToList();
                // And then updates the indices:
                foreach (DO.BusLineStation station in query)
                {
                    dl.UpdateBusLineStation(lineID, station.BusStopKey, x => x.LineStationIndex++);
                }

                // Adding the new bus line station:
                dl.AddBusLineStation(BusLineStationBoDoAdapter(newStation));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("Bus stop code already exist", ex);
            }
        }

        public void DeleteBusLineStation(int busLineID, int busStopCode, TimeSpan gapTimeUpdate, double gapKmUpdate)
        {
            BusLine line = GetBusLine(busLineID);
            if (line.LineStations.Count() < 3)
                throw new ExceptionBL_LessThanThreeStations("there are only two stations in the line,Unable to delete station");

            DO.BusLineStation currentStation = dl.GetBusLineStation(busLineID, busStopCode);
            if (currentStation.PrevStation != 0 && currentStation.NextStation != 0) // It means the lateral stations are in the middle
            {
                dl.UpdateBusLineStation(busLineID, currentStation.PrevStation, station => station.NextStation = currentStation.NextStation);
                dl.UpdateBusLineStation(busLineID, currentStation.NextStation, station => station.PrevStation = currentStation.PrevStation);
            }
            else if (currentStation.PrevStation == 0 && currentStation.NextStation != 0) // The station to be deleted is the first one
                dl.UpdateBusLineStation(busLineID, currentStation.NextStation, station => station.PrevStation = 0);
            else if (currentStation.PrevStation != 0 && currentStation.NextStation == 0) // The station to be deleted is the last one
                dl.UpdateBusLineStation(busLineID, currentStation.PrevStation, station => station.NextStation = 0);


            // Indices update:
            int indexDeleted = currentStation.LineStationIndex;
            // The query gets all the bus line stations which indices are bigger than ther one needed to be deleted
            var query = (from station
                        in dl.GetAllBusLineStations()
                         where station.BusLineID == busLineID && station.LineStationIndex > indexDeleted
                         select station).ToList();
            // And then updates the indices:
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
                dl.AddConsecutiveStations(newCons);
            }
            else if (currentStation.PrevStation != 0 && currentStation.NextStation != 0 && gapKmUpdate == 0)// It means consecutive exist or inactive
            {
                dl.UpdateConsecutiveStations(currentStation.PrevStation, currentStation.NextStation, con => con.ObjectActive = true);
            }

            dl.DeleteBusLineStation(busLineID, busStopCode);
            if (currentStation.PrevStation == 0 && currentStation.NextStation != 0)
                dl.UpdateBusLine(busLineID, x => x.FirstBusStopKey = currentStation.NextStation);
            else if (currentStation.PrevStation != 0 && currentStation.NextStation == 0)
                dl.UpdateBusLine(busLineID, x => x.LastBusStopKey = currentStation.PrevStation);

            // Deleting the consecutive stations (which existed before the bus line station deletion) if they aren't in use (after deleting the bus line station above).
            // A deletion of consecutive stations, is enabling the admin to UPDATE THE CONSECUTIVE later, if he wishs the add the bus line station again.
            // Therefore, if the admins wishs to update a consecutive stations object, he must delete all the line stations which creating the consecutive stations, before adding it with the updated time and distance
            // (The logic is simple - if the station is in use by other line, it is problematic to update the distance and time 'without a warning'. The stations should be removed and added again from all the lines and then added again as updated.)
            if (currentStation.PrevStation != 0 && currentStation.NextStation != 0)
            {
                if (!IsConsecutiveInUse(currentStation.PrevStation, currentStation.BusStopKey))
                    dl.DeleteConsecutiveStations(currentStation.PrevStation, currentStation.BusStopKey);
                if (!IsConsecutiveInUse(currentStation.BusStopKey, currentStation.NextStation))
                    dl.DeleteConsecutiveStations(currentStation.BusStopKey, currentStation.NextStation);
            }

            // In case the station to delete is the first or the last one, will delete if they aren't in use
            else if (currentStation.PrevStation == 0 && currentStation.NextStation != 0
                && !IsConsecutiveInUse(currentStation.BusStopKey, currentStation.NextStation))
                dl.DeleteConsecutiveStations(currentStation.BusStopKey, currentStation.NextStation);
            else if (currentStation.PrevStation != 0 && currentStation.NextStation == 0
                && !IsConsecutiveInUse(currentStation.PrevStation, currentStation.BusStopKey))
                dl.DeleteConsecutiveStations(currentStation.PrevStation, currentStation.BusStopKey);
        }

        #endregion

        #region Bus: Adaptors and CRUD implementations

        BO.Bus busDoBoAdapter(DO.Bus busDO)
        {
            BO.Bus busBO = new BO.Bus();
            //int id = busDO.License;
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }

        DO.Bus busBoDoAdapter(BO.Bus busBO)
        {
            DO.Bus busDO = new DO.Bus();
            //int id = busDO.License;
            busBO.CopyPropertiesTo(busDO);
            return busDO;
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return from doBus in dl.GetAllBuses() select busDoBoAdapter(doBus);
        }

        public Bus GetBus(int license)
        {
            DO.Bus busDO;
            try
            {
                busDO = dl.GetBus(license);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("License does not exist", ex);
            }
            return busDoBoAdapter(busDO);
        }

        public void AddBus(BO.Bus busBO)
        {
            try
            {
                if (busBO.MileageAtLastTreat > busBO.Mileage)
                {
                    throw new BO.ExceptionBL_MileageValuesConflict("Conflict between mileage values");
                }
                BusStatusUpdate(busBO);
                DO.Bus newBus = busBoDoAdapter(busBO);
                (DalFactory.GetDL()).AddBus(newBus);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("License already exist", ex);
            }
        }

        public void UpdateBus(BO.Bus busBO) //busUpdate
        {
            try
            {
                BusStatusUpdate(busBO);
                dl.UpdateBus(busBoDoAdapter(busBO));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("License does not exist or bus inactive", ex);
            }

        }

        public void UpdateBus(int licenseNumber, Action<BO.Bus> update)  // method that knows to update specific fields in Person
        {
            try
            {
                DO.Bus busUpdateDO = dl.GetBus(licenseNumber);
                update(busDoBoAdapter(busUpdateDO));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("License does not exist Or bus inactive", ex);
            }
        }

        public void DeleteBus(int license)
        {
            try
            {
                dl.DeleteBus(license);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("the bus license doesn't exist or the bus is inactive!", ex);
            }
        }

        public void RefuelBus(BO.Bus bus)
        {
            if (bus.Fuel == 1200)
            {
                throw new BO.ExceptionBL_NoNeedToRefuel(bus.License);
            }
            else
            {
                bus.Fuel = 1200;
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
            if (busBo.Mileage - busBo.MileageAtLastTreat < 20000 && busBo.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) > 0 && busBo.Fuel > 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.READY_FOR_TRAVEL;
            else if (busBo.Mileage - busBo.MileageAtLastTreat >= 20000 || busBo.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.DANGEROUS;
            else if (busBo.Fuel == 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.NEEDS_REFUEL;
        }

        #endregion

        #region BusStop: Adaptors and CRUD implementations
        BO.BusStop BusStopDoBoAdapter(DO.BusStop busStopDO)
        {
            BO.BusStop busStopBO = new BO.BusStop();
            int code = busStopDO.BusStopKey;
            busStopDO.CopyPropertiesTo(busStopBO);
            busStopBO.LinesStopHere = from boBusLine
                                      in GetAllBusLines()
                                      where boBusLine.LineStations.Any(line => line.BusStopKey == busStopBO.BusStopKey)
                                      orderby boBusLine.BusLineNumber
                                      select BusLinetoBusLineAtStopConvertor(boBusLine, code);
            return busStopBO;
        }

        DO.BusStop BusStopBoDoAdapter(BO.BusStop busStopBO)
        {
            DO.BusStop busStopDO = new DO.BusStop();
            //int id = busDO.License;
            busStopBO.CopyPropertiesTo(busStopDO);
            return busStopDO;
        }

        public IEnumerable<BusStop> GetAllBusStops()
        {
            return from doBusStop in dl.GetAllBusStops() orderby doBusStop.BusStopKey select BusStopDoBoAdapter(doBusStop);
        }


        public BusStop GetBusStop(int busStopKeyDO)
        {
            DO.BusStop busStopDO;
            try
            {
                busStopDO = dl.GetBusStop(busStopKeyDO);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("bus stop key does not exist", ex);
            }
            return BusStopDoBoAdapter(busStopDO);
        }

        public void UpdateBusStop(BO.BusStop busStopBO) //busUpdate
        {
            try
            {
                DO.BusStop busStopBeforeUpdate = dl.GetBusStop(busStopBO.BusStopKey);
                
                // In case the admin tries to update the location, while the bus stop currently serves bus lines:
                if (busStopBO.BusStopAddress != busStopBeforeUpdate.BusStopAddress || busStopBO.Latitude != busStopBeforeUpdate.Latitude || busStopBO.Longitude != busStopBeforeUpdate.Longitude)
                {
                    if (busStopBO.LinesStopHere.Count() > 0)
                        throw new BO.ExceptionBL_LinesStopHere("The bus stop serves bus lines, and the location cannot be changed.");
                }

                // In case the coordinates are impossible:
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
                if (busStopBO.Latitude > 33.3 || busStopBO.Latitude < 31 || busStopBO.Longitude < 34.3 || busStopBO.Longitude > 35.5)
                {
                    throw new BO.ExceptionBL_Incorrect_coordinates("The longitude or the latitude are not matching the range.");
                }
                dl.AddBusStop(BusStopBoDoAdapter(busStopBO));
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex)
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("Bus stop code already exist", ex);
            }
        }

        public void DeleteBusStop(int busStopCode)
        {
            try
            {
                if (BusStopDoBoAdapter(dl.GetBusStop(busStopCode)).LinesStopHere.Count() > 0)
                    throw new BO.ExceptionBL_LinesStopHere("The bus stop serves bus lines, and cannot be deleted.");
                dl.DeleteBusStop(busStopCode);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("bus stop key does not exist Or bus inactive", ex);
            }
        }

        #endregion

        #region BusLineAtBusStop: Convertor for bus line -> bus line at bus stop

        BO.BusLineAtBusStop BusLinetoBusLineAtStopConvertor(BO.BusLine busLine, int busStopCode)
        {
            BO.BusLineAtBusStop busLineAtBusStop = new BO.BusLineAtBusStop();
            busLine.CopyPropertiesTo(busLineAtBusStop);
            busLineAtBusStop.LastBusStopName = dl.GetBusStop(busLineAtBusStop.LastBusStopKey).BusStopName;
            busLineAtBusStop.TravelTimeToBusStop = StationTravelTimeCalculation(busLine.BusLineID, busStopCode); // The travel calculated by function
            return busLineAtBusStop;
        }
        #endregion

        #region LineDeparture: Add and Delete methods

        public void AddLineDeparture(TimeSpan departureTime, int busLineID)
        {
            try
            {
                DO.LineDeparture newLineDeparture = new DO.LineDeparture();
                newLineDeparture.BusLineID = busLineID;
                newLineDeparture.DepartureTime = departureTime;
                dl.AddLineDeparture(newLineDeparture);
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex)
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("The line departure time already exist", ex);
            }
        }

        public void DeleteLineDeparture(TimeSpan departureTime, int busLineID)
        {
            try
            {
                DO.LineDeparture dep = dl.GetLineDepartureByTimeAndLine(departureTime, busLineID);
                dl.DeleteLineDeparture(dep.DepartureID);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("the line departure doesn't exist", ex);
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                throw new BO.ExceptionBL_Inactive("The line departure is already inactive", ex);
            }
        }
        #endregion

        #region Consecutive Stations: Boolean functions


        public bool IsConsecutiveExist(int busStopKeyA, int busStopKeyB)
        {
            DO.ConsecutiveStations newConStations = new DO.ConsecutiveStations();
            try
            {
                newConStations = dl.GetConsecutiveStations(busStopKeyA, busStopKeyB);
                return true;// Returns true, because it exist (the dl.GetConsecutive didn't throw anything)
            }
            catch (DO.ExceptionDAL_Inactive)
            {
                return false; // If the consecutive exist but is inactive, the admin will be able to update the distance and time travel
            }
            catch (DO.ExceptionDAL_KeyNotFound)
            {
                return false; // Returns true, because the consecutive doesn't exist
            }
        }


        public bool IsConsecutiveInUse(int busStopKeyA, int busStopKeyB)
        {
            return (from station
                         in dl.GetAllBusLineStations()
                    where station.BusStopKey == busStopKeyA && station.NextStation == busStopKeyB
                    select station).Any(); // Returns true if any of the stations are consecutive
        }

        #endregion

        #region User: Adaptors and CRUD implementations

        BO.User userDoBoAdapter(DO.User userDO)
        {
            BO.User userBO = new BO.User();
            //int code = userDO.UserName;
            userDO.CopyPropertiesTo(userBO);
            return userBO;
        }

        DO.User userBoDoAdapter(BO.User userBO)
        {
            DO.User userDO = new DO.User();
            //int id = busDO.License;
            userBO.CopyPropertiesTo(userDO);
            return userDO;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return from doUser in dl.GetAllUsers() select userDoBoAdapter(doUser);
        }

        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            return from user in dl.GetAllUsers()
                   where predicate(userDoBoAdapter(user))
                   select userDoBoAdapter(user);
        }

        public User GetUser(string userName)
        {
            DO.User userDO;
            try
            {
                userDO = dl.GetUser(userName);
                return userDoBoAdapter(userDO);
            }
            catch (DO.ExceptionDAL_UserKeyNotFound ex)
            {
                throw new BO.ExceptionBL_UserKeyNotFound("user dose not exist", ex);
            }
        }

        public void AddUser(User userBO)
        {
            try
            {
                DO.User newUser = userBoDoAdapter(userBO);
                dl.AddUser(newUser);
            }
            catch (DO.ExceptionDAL_UserAlreadyExist ex)
            {
                throw new BO.ExceptionBL_UserAlreadyExist("user already exist", ex);
            }
        }

        public void UpdateUser(User userBO)
        {
            try
            {
                dl.UpdateUser(userBoDoAdapter(userBO));
            }
            catch (DO.ExceptionDAL_UserKeyNotFound ex)
            {
                throw new BO.ExceptionBL_UserKeyNotFound("user does not exist Or inactive", ex);
            }
        }

        public void UpdateUser(string userName, Action<User> update) // method that knows to updt specific fields in Person
        {
            User userUpdate = GetUser(userName);
            update(userUpdate);
        }

        public void DeleteUser(string userName)
        {
            try
            {
                dl.DeleteUser(userName);
            }
            catch (DO.ExceptionDAL_UserKeyNotFound ex)
            {
                throw new BO.ExceptionBL_UserKeyNotFound("user does not exist Or inactive", ex);
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