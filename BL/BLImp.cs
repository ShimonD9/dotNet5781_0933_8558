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
        IDal dl = DalFactory.GetDL();


        #region BusLine

        BO.BusLine BusLineDoBoAdapter(DO.BusLine busLineDO)
        {
            BO.BusLine busLineBO = new BO.BusLine();
            busLineDO.CopyPropertiesTo(busLineBO);
            busLineBO.LineStations = from boLineStation
                                     in GetAllBusLineStations()
                                     where boLineStation.BusLineID == busLineBO.BusLineID
                                     orderby boLineStation.LineStationIndex
                                     select boLineStation;
            busLineBO.Schedule = from doLineDeparture
                                 in dl.GetAllLineDeparture()
                                 where doLineDeparture.BusLineID == busLineBO.BusLineID
                                 orderby doLineDeparture.DepartureTime
                                 select doLineDeparture.DepartureTime;
            return busLineBO;
        }

        DO.BusLine BusLineBoDoAdapter(BO.BusLine busLineBO)
        {
            DO.BusLine busLineDO = new DO.BusLine();
            busLineBO.CopyPropertiesTo(busLineDO);
            return busLineDO;
        }

        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from doBusLine in dl.GetAllBusLines() orderby doBusLine.BusLineID select BusLineDoBoAdapter(doBusLine);
        }

        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            throw new NotImplementedException();
        }

        public BusLine GetBusLine(int busLineID)
        {
            return BusLineDoBoAdapter(dl.GetBusLine(busLineID));
        }

        public void AddBusLine(BusLine busLine, BusLineStation busLineStationA, BusLineStation busLineStationB)
        {
            int idToReturn;
            DO.BusLine newBus = BusLineBoDoAdapter(busLine);

            try
            {
                idToReturn = dl.AddBusLine(newBus);

                // Adding the consecutive stations entity if needed:

                if (!IsConsecutiveExist(busLine.FirstBusStopKey, busLine.LastBusStopKey))
                {
                    DO.ConsecutiveStations newConStations = new DO.ConsecutiveStations();
                    newConStations.BusStopKeyA = busLine.FirstBusStopKey;
                    newConStations.BusStopKeyB = busLine.LastBusStopKey;
                    newConStations.Distance = busLineStationA.DistanceToNext;
                    newConStations.TravelTime = busLineStationA.TimeToNext;
                    dl.AddConsecutiveStations(newConStations);
                }

                // Adding the bus line stations:
                busLineStationA.BusLineID = idToReturn;
                busLineStationB.BusLineID = idToReturn;
                dl.AddBusLineStation(BusLineStationBoDoAdapter(busLineStationA));
                dl.AddBusLineStation(BusLineStationBoDoAdapter(busLineStationB));
            }
            catch (DO.ExceptionDAL_KeyAlreadyExist ex)
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("Key or bus stop already exist", ex);
            }

            // EXPAND WITH ANOTHER CATCH FOR BUS STOP KEY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public void UpdateBusLine(BusLine busLineBO)
        {
            try
            {
                dl.UpdateBusLine(BusLineBoDoAdapter(busLineBO));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("The bus line doesn't exist", ex);
            }
        }

        public void UpdateBusLine(int busLineID, Action<BusLine> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLine(int busLineID)
        {
            try
            {
                
                foreach (DO.LineDeparture item in dl.GetAllLineDepartureBy(x=>x.BusLineID == busLineID))
                {
                        dl.DeleteLineDeparture(item.DepartureID);
                }

                foreach (DO.BusLineStation item in dl.GetAllBusLineStationsBy(x => x.BusLineID == busLineID))
                {

                    dl.DeleteBusLineStation(item.BusLineID, item.BusStopKey);
                }
                dl.DeleteBusLine(busLineID);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {

                throw new BO.ExceptionBL_KeyNotFound("The bus line not exist", ex);
            }
        }

        #endregion

        #region BusLineStation

        BO.BusLineStation BusLineStationDoBoAdapter(DO.BusLineStation busLineStationDO)
        {
            BO.BusLineStation busLineStationBO = new BO.BusLineStation();
            busLineStationDO.CopyPropertiesTo(busLineStationBO);

            busLineStationBO.BusStopName = (from doBusStop
                                            in dl.GetAllBusStops()
                                            where doBusStop.BusStopKey == busLineStationBO.BusStopKey
                                            select doBusStop.BusStopName).FirstOrDefault();

            busLineStationBO.DistanceToNext = (from doConStations
                                              in dl.GetAllConsecutiveStations()
                                               where doConStations.BusStopKeyA == busLineStationBO.BusStopKey &&
                                                     doConStations.BusStopKeyB == busLineStationBO.NextStation
                                               select doConStations.Distance).FirstOrDefault();
            busLineStationBO.TimeToNext = (from doConStations
                                              in dl.GetAllConsecutiveStations()
                                           where doConStations.BusStopKeyA == busLineStationBO.BusStopKey &&
                                                 doConStations.BusStopKeyB == busLineStationBO.NextStation
                                           select doConStations.TravelTime).FirstOrDefault();
            return busLineStationBO;
        }

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

        public IEnumerable<BusLineStation> GetAllBusLineStationsBy(Predicate<BusLineStation> predicate)
        {
            throw new NotImplementedException();
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
                var query = (from station
                            in dl.GetAllBusLineStations()
                             where station.BusLineID == lineID && station.LineStationIndex >= indexAdded
                             select station).ToList();
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

        public void UpdateBusLineStation(BusLineStation busLineStation)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusLineStation(int busLineID, Action<BusLineStation> update)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLineStation(int busLineID, int busStopCode, TimeSpan gapTimeUpdate, double gapKmUpdate)
        {
            BusLine line = GetBusLine(busLineID);
            if (line.LineStations.Count() < 3)
                throw new ExceptionBL_LessThanThreeStation("there are only two stations in the line,Unable to delete station");

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
            var query = (from station
                        in dl.GetAllBusLineStations()
                         where station.BusLineID == busLineID && station.LineStationIndex > indexDeleted
                         select station).ToList();
            foreach (DO.BusLineStation station in query)
            {
                dl.UpdateBusLineStation(busLineID, station.BusStopKey, x => x.LineStationIndex--);
            }

            // Consecutive stations addition/deletion:

            if (currentStation.PrevStation != 0 && currentStation.NextStation != 0 && gapKmUpdate != 0) // It means there is need to fill the consecutive stations info gap (there are no consecutive stations to this case)
            {
                DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();

                newCons.BusStopKeyA = currentStation.PrevStation;
                newCons.BusStopKeyB = currentStation.NextStation;
                newCons.Distance = gapKmUpdate;
                newCons.TravelTime = gapTimeUpdate;
                dl.AddConsecutiveStations(newCons);
            }
            else if (currentStation.PrevStation != 0 && currentStation.NextStation != 0 && gapKmUpdate == 0)//it means consecutive exist or inactive
            {
                //DO.ConsecutiveStations existCon = dl.GetConsecutiveStations(currentStation.PrevStation, currentStation.NextStation);
                //existCon.ObjectActive = true;
                dl.UpdateConsecutiveStations(currentStation.PrevStation, currentStation.NextStation, con => con.ObjectActive = true);
            }


            dl.DeleteBusLineStation(busLineID, busStopCode);
            if (currentStation.PrevStation == 0 && currentStation.NextStation != 0)
                dl.UpdateBusLine(busLineID, x => x.FirstBusStopKey = currentStation.NextStation);
            else if (currentStation.PrevStation != 0 && currentStation.NextStation == 0)
                dl.UpdateBusLine(busLineID, x => x.LastBusStopKey = currentStation.PrevStation);

            // Deleting the consecutive stations (which existed before the bus line station deletion) if they aren't in use (after deleting the bus line station above)
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

        #region Bus

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

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
            //return from bus in DataSource.ListBuses
            //       where predicate(bus)
            //       select bus.Clone();
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

        public void BusStatusUpdate(BO.Bus busBo)
        {
            if (busBo.Mileage - busBo.MileageAtLastTreat < 20000 && busBo.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) > 0 && busBo.Fuel > 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.READY_FOR_TRAVEL;
            else if (busBo.Mileage - busBo.MileageAtLastTreat >= 20000 || busBo.LastTreatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.DANGEROUS;
            else if (busBo.Fuel == 0)
                busBo.BusStatus = BO.Enums.BUS_STATUS.NEEDS_REFUEL;
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
        #endregion

        #region BusStop
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
                if (busStopBO.Latitude > 33.3 || busStopBO.Latitude < 31 || busStopBO.Longitude < 34.3 || busStopBO.Longitude > 35.5)
                {
                    throw new BO.ExceptionBL_Incorrect_coordinates("The longitude or the latitude are not matching the range.");
                }
                dl.UpdateBusStop(BusStopBoDoAdapter(busStopBO));
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("The bus stop code doesn't exist", ex);
            }

        }

        public void UpdateBusStop(int bosStopKeyDO, Action<BusStop> update)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate)
        {
            throw new NotImplementedException();
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

        public void DeleteBusStop(int BusStopCode)
        {
            try
            {
                if (BusStopDoBoAdapter(dl.GetBusStop(BusStopCode)).LinesStopHere.Count() > 0)
                    throw new BO.ExceptionBL_LinesStopHere("The bus stop serves bus lines, and cannot be deleted.");
                dl.DeleteBusStop(BusStopCode);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("bus stop key does not exist Or bus inactive", ex);
            }
        }

        #endregion

        #region BusLineAtBusStop
        BO.BusLineAtBusStop BusLinetoBusLineAtStopConvertor(BO.BusLine busLine, int busStopCode)
        {
            BO.BusLineAtBusStop busLineAtBusStop = new BO.BusLineAtBusStop();
            busLine.CopyPropertiesTo(busLineAtBusStop);
            busLineAtBusStop.LastBusStopName = dl.GetBusStop(busLineAtBusStop.LastBusStopKey).BusStopName;
            busLineAtBusStop.TravelTimeToBusStop = StationTravelTimeCalculation(busLine.BusLineID, busStopCode);
            return busLineAtBusStop;
        }
        #endregion

        #region LineDeparture

        public void AddLineDeparture(TimeSpan departureTime, int busLineID)
        {
            try
            {
                DO.LineDeparture newLineDeparture = new DO.LineDeparture();
                newLineDeparture.BusLineID = busLineID;
                newLineDeparture.DepartureTime = departureTime;
                (DalFactory.GetDL()).AddLineDeparture(newLineDeparture);
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
               DO.LineDeparture dep =  dl.GetLineDepartureByTimeAndLine(departureTime,busLineID);
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



        #region Consecutive Stations

        public bool IsConsecutiveExist(int busStopKeyA, int busStopKeyB)
        {
            DO.ConsecutiveStations newConStations = new DO.ConsecutiveStations();
            try
            {
                newConStations = dl.GetConsecutiveStations(busStopKeyA, busStopKeyB);
                return true;
            }
            catch (DO.ExceptionDAL_Inactive)
            {
                return true;
            }
            catch (DO.ExceptionDAL_KeyNotFound)
            {
                return false;
            }
        }

        public bool IsConsecutiveInUse(int busStopKeyA, int busStopKeyB)
        {
            return (from station
                         in dl.GetAllBusLineStations()
                    where station.BusStopKey == busStopKeyA && station.NextStation == busStopKeyB
                    select station).Any();
        }
        #endregion

        #region User
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


        public IEnumerable<LineTiming> GetLineTimingsPerStation(BusStop currBusStop, TimeSpan tsCurrentTime)
        {
            IEnumerable<LineTiming> stationTimings = from lineAtStop in GetBusStop(currBusStop.BusStopKey).LinesStopHere
                                                     let depTime = FindLastDepartureTime(lineAtStop.BusLineID, tsCurrentTime)
                                                     let timeLeft = depTime.Add(lineAtStop.TravelTimeToBusStop).Subtract(tsCurrentTime)
                                                     where timeLeft.CompareTo(TimeSpan.FromMinutes(-5)) > 0 // It means the bus is late or passed maximum by 5 minutes
                                                     select new LineTiming
                                                     {
                                                         BusLineNumber = lineAtStop.BusLineNumber,
                                                         LastBusStopName = lineAtStop.LastBusStopName,
                                                         DepartureTime = depTime,
                                                         ArrivalTime = depTime.Add(lineAtStop.TravelTimeToBusStop),
                                                         MinutesToArrival = timeLeft.Minutes,
                                                         ShowMinutesOrArrow = timeLeft.CompareTo(TimeSpan.Zero) > 0 ? timeLeft.Minutes.ToString() : "↓",
                                                     };


            return stationTimings.OrderBy(x => x.MinutesToArrival);
        }

        public TimeSpan FindLastDepartureTime(int busLineID, TimeSpan tsCurrentTime)
        {
            var collection = (from lineDeparture in dl.GetAllLineDeparture()
                              where lineDeparture.BusLineID == busLineID
                              select lineDeparture);
            if (collection.Count() == 0) // It means the line has no departure times (yet, or by manager accident)
                return TimeSpan.FromMinutes(-1000); // A very far number so it will be exculded in the Linq Query of GetLineTimingsPerStation
            else
            {
                var collB = collection.OrderBy(x => tsCurrentTime.Subtract(x.DepartureTime));
                var lastDep = collB.FirstOrDefault(x => tsCurrentTime.Subtract(x.DepartureTime).CompareTo(TimeSpan.Zero) > 0);
                if (lastDep == null)
                    return TimeSpan.FromMinutes(-1000);
                else
                    return lastDep.DepartureTime;
            }
        }


        public TimeSpan StationTravelTimeCalculation(int busLineID, int busStopCode)
        {
            var collection = GetBusLine(busLineID).LineStations.TakeWhile(x => x.BusStopKey != busStopCode);
            if (collection == null) // It means that the bus stop is the first one, so it returns zero timeSpan
                return TimeSpan.Zero;
            else
            {
                TimeSpan calc = collection.Aggregate
                    (TimeSpan.Zero,
                    (sumSoFar, nextMyObject) => sumSoFar + nextMyObject.TimeToNext);
                return calc;
            }
        }

        public bool isTimeSpanInvalid(TimeSpan timeUpdate)
        {
            return (timeUpdate.Hours > 23 || timeUpdate.Minutes > 59 || timeUpdate.Seconds > 59);
        }

    }
}