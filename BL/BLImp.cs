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
        IDal dl = DalFactory.GetDL();




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
            catch (DO.ExceptionDALBadLicsens ex)
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
            catch (DO.ExceptionDALBadLicsens ex)
            {
                throw new BO.ExceptionBLBadLicense("License already exist", ex);
            }
        }

        public void UpdateBus(BO.Bus busBO) //busUpdate
        {
            try
            {
                dl.UpdateBus(busBoDoAdapter(busBO));
            }
            catch (DO.ExceptionDALBadLicsens ex)
            {
                throw new BO.ExceptionBLBadLicense("Licesns does not exist Or bus inactive", ex);
            }

        }
        public void UpdateBus(int licenseNumber, Action<BO.Bus> update)  // method that knows to update specific fields in Person
        {
            try
            {
                DO.Bus busUpdateDO = dl.GetBus(licenseNumber);
                update(busDoBoAdapter(busUpdateDO));
            }
            catch (DO.ExceptionDALBadLicsens ex)
            {
                throw new BO.ExceptionBLBadLicense("Licesns does not exist Or bus inactive", ex);
            }
        }
        public void DeleteBus(int license)
        {
            try
            {
                dl.DeleteBus(license);
            }
            catch (DO.ExceptionDALBadLicsens ex)
            {
                throw new BO.ExceptionBLBadLicense("Licesns does not exist Or bus inactive", ex);
            }
        }
        #endregion


        #region BusStop

        BO.BusStop busStopDoBoAdapter(DO.BusStop busStopDO)
        {
            BO.BusStop busStopBO = new BO.BusStop();
            int code = busStopDO.BusStopKey;
            busStopDO.CopyPropertiesTo(busStopBO);
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
            catch (DO.ExceptionDALBadLicsens ex)
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
            catch (DO.ExceptionDALBadLicsens ex)
            {
                throw new BO.ExceptionBLBadLicense("bus stop key not exist Or bus inactive", ex);
            }

        }

        public void AddBusStop(BO.BusStop busStopBO)
        {
            try
            {
                DO.BusStop newBus = busStopBoDoAdapter(busStopBO);
                (DalFactory.GetDL()).AddBusStop(newBus);
            }
            catch (DO.ExceptionDALBadLicsens ex)
            {
                throw new BO.ExceptionBLBadLicense("License already exist", ex);
            }
        }

        public void DeleteBusStop(int license)
        {
            try
            {
                dl.DeleteBusStop(license);
            }
            catch (DO.ExceptionDALBadLicsens ex)
            {
                throw new BO.ExceptionBLBadLicense("bus stop key does not exist Or bus inactive", ex);
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