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

            // Schedule initializing:
            DO.LineDeparture lineDeparture = dl.GetLineDeparture(busLineBO.BusLineID);
            TimeSpan addedTS = lineDeparture.StartTime;
            TimeSpan lastTS = lineDeparture.EndTime;
            TimeSpan toAdd = new TimeSpan(0, lineDeparture.Frequency, 0);
            List<TimeSpan> schedule = new List<TimeSpan> { addedTS };

            while (addedTS.CompareTo(lastTS) < 0)
            {
                addedTS = addedTS.Add(toAdd);
                schedule.Add(addedTS);
            }
            schedule.Add(lastTS);
            busLineBO.Schedule = from ts in schedule select ts; // MyList.Select(item => new Item(<cons params>).ToList() - הצעת מתן
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
        public BusLine GetBusLine(int license)
        {
            throw new NotImplementedException();
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
                dl.AddConsecutiveStations(newConStations);
                newLineDeparture.BusLineID = idToReturn;
                newLineDeparture.StartTime = startTime;
                newLineDeparture.EndTime = endTime;
                newLineDeparture.Frequency = frequency;
                dl.AddLineDeparture(newLineDeparture);
            }

            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("Line already exist", ex);
            }
            return idToReturn;
        }
        public void UpdateBusLine(BusLine busLine)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusLine(int license, Action<BusLine> update)
        {
            throw new NotImplementedException();
        }
        public void DeleteBusLine(int license)
        {
            throw new NotImplementedException();
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
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("License does not exist", ex);
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
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("License already exist", ex);
            }
        }

        public void UpdateBus(BO.Bus busBO) //busUpdate
        {
            try
            {
                BusStatusUpdate(busBO);
                dl.UpdateBus(busBoDoAdapter(busBO));
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("License does not exist or bus inactive", ex);
            }

        }
        public void UpdateBus(int licenseNumber, Action<BO.Bus> update)  // method that knows to update specific fields in Person
        {
            try
            {
                DO.Bus busUpdateDO = dl.GetBus(licenseNumber);
                update(busDoBoAdapter(busUpdateDO));
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("License does not exist Or bus inactive", ex);
            }
        }
        public void DeleteBus(int license)
        {
            try
            {
                dl.DeleteBus(license);
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("he bus license doesn't exist or the bus is inactive!", ex);
            }
        }
        #endregion

        #region BusStop

        BO.BusStop busStopDoBoAdapter(DO.BusStop busStopDO)
        {
            BO.BusStop busStopBO = new BO.BusStop();
            int code = busStopDO.BusStopKey;
            busStopDO.CopyPropertiesTo(busStopBO);
            busStopBO.LinesStopHere = from boBusLine
                                      in GetAllBusLines()
                                      where boBusLine.LineStations.Any(line => line.BusStopKey == busStopBO.BusStopKey)
                                      orderby boBusLine.BusLineNumber
                                      select boBusLine;
            return busStopBO;
        }

        DO.BusStop busStopBoDoAdapter(BO.BusStop busStopBO)
        {
            DO.BusStop busStopDO = new DO.BusStop();
            //int id = busDO.License;
            busStopBO.CopyPropertiesTo(busStopDO);
            return busStopDO;
        }
        public IEnumerable<BusStop> GetAllBusStops()
        {
            return from doBusStop in dl.GetAllBusStops() select busStopDoBoAdapter(doBusStop);
        }

        public BusStop GetBusStop(int bosStopKeyDO)
        {
            DO.BusStop bosStopDO;
            try
            {
                bosStopDO = dl.GetBusStop(bosStopKeyDO);
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("bus stop key does not exist", ex);
            }
            return busStopDoBoAdapter(bosStopDO);
        }

        public void UpdateBusStop(BO.BusStop busStopBO) //busUpdate
        {
            try
            {
                dl.UpdateBusStop(busStopBoDoAdapter(busStopBO));
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("bus stop key not exist Or bus inactive", ex);
            }

        }

        public void UpdateBusStop(int license, Action<BusStop> update)
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
                DO.BusStop newStop = busStopBoDoAdapter(busStopBO);
                (DalFactory.GetDL()).AddBusStop(newStop);
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("Bus stop code already exist", ex);
            }
        }

        public void DeleteBusStop(int BusStopCode)
        {
            try
            {
                if (busStopDoBoAdapter(dl.GetBusStop(BusStopCode)).LinesStopHere.Count() > 0)
                    throw new BO.ExceptionBLLinesStopHere("The bus stop serves bus lines, and cannot be deleted.");
                dl.DeleteBusStop(BusStopCode);
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("bus stop key does not exist Or bus inactive", ex);
            }
        }

        #endregion

        //#region LineDeparture
        //BO.LineDeparture LineDepartureDoBoAdapter(DO.LineDeparture LineDepartureDO)
        //{
        //    BO.LineDeparture LineDepartureBO = new BO.LineDeparture();
        //    LineDepartureDO.CopyPropertiesTo(LineDepartureBO);
        //    return LineDepartureBO;
        //}

        //DO.LineDeparture LineDepartureBoDoAdapter(BO.LineDeparture lineDepartureBO)
        //{
        //    DO.LineDeparture lineDepartureDO = new DO.LineDeparture();
        //    lineDepartureBO.CopyPropertiesTo(lineDepartureDO);
        //    return lineDepartureDO;
        //}
        //public IEnumerable<LineDeparture> GetAllLineDepartures()
        //{
        //    return from doLineDeparture in dl.GetAllLineDeparture() select LineDepartureDoBoAdapter(doLineDeparture);
        //}

        //public LineDeparture GetLineDeparture(int busLineID)
        //{
        //    DO.LineDeparture lineDepartureDO;
        //    try
        //    {
        //        lineDepartureDO = dl.GetLineDeparture(busLineID);
        //    }
        //    catch (DO.ExceptionDALBadIdUser ex) // לתקן לחריגה בהתאם!
        //    {
        //        throw new BO.ExceptionBLBadUserId("user dose not exist", ex);
        //    }
        //    return LineDepartureDoBoAdapter(lineDepartureDO);

        //}

        //public void AddLineDeparture(LineDeparture lineDeparture)
        //{
        //    try
        //    {
        //        DO.LineDeparture newlineDeparture = LineDepartureBoDoAdapter(lineDeparture);
        //        (DalFactory.GetDL()).AddLineDeparture(newlineDeparture);
        //    }
        //    catch (DO.ExceptionDALBadLicense ex)
        //    {
        //        throw new BO.ExceptionBLBadLicense("Bus stop code already exist", ex);
        //    }
        //}
        //#endregion

        #region BusLineStation

        BO.BusLineStation BusLineStationDoBoAdapter(DO.BusLineStation busLineStationDO)
        {
            BO.BusLineStation busLineStationBO = new BO.BusLineStation();
            busLineStationDO.CopyPropertiesTo(busLineStationBO);
            // לוודא שצורת השאילתא תקינה
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

        public BusLineStation GetBusLineStation(int license)
        {
            throw new NotImplementedException();
        }
        public void AddBusLineStation(BusLineStation busLineStationBo)
        {
            try
            {
                DO.BusLineStation newStop = BusLineStationBoDoAdapter(busLineStationBo);
                (DalFactory.GetDL()).AddBusLineStation(newStop);
            }
            catch (DO.ExceptionDALBadLicense ex)
            {
                throw new BO.ExceptionBLBadLicense("Bus stop code already exist", ex);
            }
        }
        public void UpdateBusLineStation(BusLineStation busLineStation)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusLineStation(int license, Action<BusLineStation> update)
        {
            throw new NotImplementedException();
        }
        public void DeleteBusLineStation(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Consecutive Stations
        public void CheckIfConsecutiveExistOrInactive(int busStopKeyA, int busStopKeyB)
        {
            DO.ConsecutiveStations newConStations = new DO.ConsecutiveStations();
            try
            {
                newConStations = dl.GetConsecutiveStations(busStopKeyA, busStopKeyB);
            }
            catch(DO.ExceptionDALInactive ex)
            {
                throw new BO.ExceptionBLInactive("Consecutive stations object exist but isn't active", ex);
            }
            catch(DO.ExceptionDALunexist ex)
            {
                throw new BO.ExceptionBLunexist("Consecutive stations object doesn't exist", ex);
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
            catch (DO.ExceptionDALBadIdUser ex)
            {
                throw new BO.ExceptionBLBadUserId("user dose not exist", ex);
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
            catch (DO.ExceptionDALBadIdUser ex)
            {
                throw new BO.ExceptionBLBadUserId("user already exist", ex);
            }
        }

        public void UpdateUser(User userBO)
        {
            try
            {
                dl.UpdateUser(userBoDoAdapter(userBO));
            }
            catch (DO.ExceptionDALBadIdUser ex)
            {
                throw new BO.ExceptionBLBadUserId("user does not exist Or inactive", ex);
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
            catch (DO.ExceptionDALBadIdUser ex)
            {
                throw new BO.ExceptionBLBadUserId("user does not exist Or inactive", ex);
            }
        }
        #endregion
    }
}