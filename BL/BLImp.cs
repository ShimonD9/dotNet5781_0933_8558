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
            return from doBusLine in dl.GetAllBusLines() select BusLineDoBoAdapter(doBusLine);
        }

        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            throw new NotImplementedException();
        }

        public BusLine GetBusLine(int busLineID)
        {
            return BusLineDoBoAdapter(dl.GetBusLine(busLineID));
        }

        public int AddBusLine(BusLine busLine, double kmToNext, TimeSpan timeToNext, TimeSpan startTime, TimeSpan endTime, int frequency)
        {
            int idToReturn;
            DO.BusLine newBus = BusLineBoDoAdapter(busLine);
            DO.ConsecutiveStations newConStations = new DO.ConsecutiveStations();
            DO.LineDeparture newLineDeparture = new DO.LineDeparture();
            try
            {
                idToReturn = (DalFactory.GetDL()).AddBusLine(newBus);


                newConStations.BusStopKeyA = busLine.FirstBusStopKey;
                newConStations.BusStopKeyB = busLine.LastBusStopKey;
                newConStations.Distance = kmToNext;
                newConStations.TravelTime = timeToNext;
                if (!CheckIfConsecutiveExist(busLine.FirstBusStopKey, busLine.LastBusStopKey))
                    dl.AddConsecutiveStations(newConStations);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyAlreadyExist("Line already exist", ex);
            }
            return idToReturn;
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
                dl.DeleteLineDepartureByID(busLineID);
                dl.DeleteBusLineStationsByID(busLineID);
                dl.DeleteBusLine(busLineID);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {

                throw new BO.ExceptionBL_KeyNotFound("The bus line not exist", ex);
            }
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
                                      select BusLinetoBusLineAtStopConvertor(boBusLine);
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
            return from doBusStop in dl.GetAllBusStops() select BusStopDoBoAdapter(doBusStop);
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
                DO.BusStop newStop = BusStopBoDoAdapter(busStopBO);
                (DalFactory.GetDL()).AddBusStop(newStop);
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                throw new BO.ExceptionBL_KeyNotFound("Bus stop code already exist", ex);
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
        BO.BusLineAtBusStop BusLinetoBusLineAtStopConvertor(BO.BusLine busLine)
        {
            BO.BusLineAtBusStop busLineAtBusStop = new BO.BusLineAtBusStop();
            busLine.CopyPropertiesTo(busLineAtBusStop);
            busLineAtBusStop.LastBusStopName = dl.GetBusStop(busLineAtBusStop.LastBusStopKey).BusStopName;
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
                dl.DeleteLineDeparture(departureTime, busLineID);
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

        public void AddBusLineStation(BusLineStation busLineStationBo)
        {
            try
            {
                DO.BusLineStation newStop = BusLineStationBoDoAdapter(busLineStationBo);
                (DalFactory.GetDL()).AddBusLineStation(newStop);
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
            DO.BusLineStation currentInfo = dl.GetBusLineStation(busLineID, busStopCode);
            DO.BusLineStation prevStation = dl.GetBusLineStation(busLineID, currentInfo.PrevStation);
            DO.BusLineStation nextStation = dl.GetBusLineStation(busLineID, currentInfo.NextStation);
            prevStation.NextStation = nextStation.BusStopKey;
            nextStation.PrevStation = prevStation.BusStopKey;
            dl.UpdateBusLineStation(prevStation);
            dl.UpdateBusLineStation(nextStation);


            ///
            /// NEED TO UPDATE THE INDEX OF THE OTHER BUS LINE STATIONS AS WELL!!! (DO IT AT DL OR BL?!?!?)
            ///

            // Consecutive stations addition:
            if (gapKmUpdate != 0) // It means there is need to fill the consecutive stations info gap (there are no consecutive stations to this case)
            { 
                DO.ConsecutiveStations newCons = new DO.ConsecutiveStations();

                newCons.BusStopKeyA = currentInfo.PrevStation;
                newCons.BusStopKeyB = currentInfo.NextStation;
                newCons.Distance = gapKmUpdate;
                newCons.TravelTime = gapTimeUpdate;
                dl.AddConsecutiveStations(newCons);

            }

            dl.DeleteBusLineStation(busLineID, busStopCode);
        }
  
        #endregion

        #region Consecutive Stations

        public bool CheckIfConsecutiveExist(int busStopKeyA, int busStopKeyB)
        {
            DO.ConsecutiveStations newConStations = new DO.ConsecutiveStations();
            try
            {
                newConStations = dl.GetConsecutiveStations(busStopKeyA, busStopKeyB);
                return true;
            }
            catch (DO.ExceptionDAL_Inactive ex)
            {
                            return true;
            }
            catch (DO.ExceptionDAL_KeyNotFound ex)
            {
                return false;
            }

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
            }
            catch (DO.ExceptionDAL_UserKeyNotFound ex)
            {
                throw new BO.ExceptionBL_UserKeyNotFound("user dose not exist", ex);
            }
            return userDoBoAdapter(userDO);

        }

        public void AddUser(User userBO)
        {
            try
            {
                DO.User newUser = userBoDoAdapter(userBO);
                (DalFactory.GetDL()).AddUser(newUser);
            }
            catch (DO.ExceptionDAL_UserKeyNotFound ex)
            {
                throw new BO.ExceptionBL_UserKeyNotFound("user already exist", ex);
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
    }
}