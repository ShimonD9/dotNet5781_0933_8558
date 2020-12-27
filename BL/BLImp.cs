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


        BO.BusStop busStopDoBoAdapter(DO.BusStop busStopDO)
        {
            BO.BusStop busStopBO = new BO.BusStop();
            int code = busStopDO.BusStopKey;
            busStopDO.CopyPropertiesTo(busStopBO);
            return busStopBO;
        }

        #region Bus
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
            throw new NotImplementedException();
            //DO.Bus busDO;
            //try
            //{
            //    busDO = dl.GetBus(license);
            //}
            //catch (DO.BadIdException ex)
            //{
            //    throw new BO.BadIdException("Person id does not exist or he is not a student", ex);
            //}
            //return studentDoBoAdapter(studentDO);
        }

        public void AddBus(BO.Bus busBO)
        {
            try
            {
                dl.AddBus(busBoDoAdapter(busBO));
            }
            catch (DO.BadIdException ex)
            {
                throw new BO.BadIdException("Licesns does not exist", ex);
            }
        }

        public void UpdateBus(BO.Bus busBO) //busUpdate
        {
            try
            {
                dl.UpdateBus(busBoDoAdapter(busBO));
            }
            catch (DO.BadIdException ex)
            {
                throw new BO.BadIdException("Licesns does not exist Or bus inactive", ex);
            }

        }
        public void UpdateBus(int licenseNumber, Action<Bus> update)  // method that knows to update specific fields in Person
        {
            throw new NotImplementedException();
            //Bus busUpdate = GetBus(licenseNumber);
            //update(busUpdate);
        }
        public void DeleteBus(int license)
        {
            throw new NotImplementedException();
            //Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            //if (bus != null && bus.ObjectActive)
            //    bus.ObjectActive = false;
            //else if (!bus.ObjectActive)
            //    throw new DO.InactiveBusException(bus.License, $"the bus is  inactive");
            //else
            //    throw new DO.BadIdException(bus.License, $"bad id: {bus.License}");
        }
        #endregion


        #region BusStop
        public IEnumerable<BusStop> GetAllBusStops()
        {
            return from doBusStop in dl.GetAllBusStops() select busStopDoBoAdapter(doBusStop);
        }
        #endregion
    }
}