using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;


namespace BLApi
{
    public interface IBL
    {
        #region Bus
        IEnumerable<Bus> GetAllBuses();
        IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate);
        Bus GetBus(int license);
        void AddBus(Bus bus);
        void UpdateBus(Bus bus);
        void UpdateBus(int license, Action<Bus> update); // method that knows to updt specific fields in Person
        void DeleteBus(int license);
        #endregion

        #region BusStop
        IEnumerable<BusStop> GetAllBusStops();
        //IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate);
        //BusStop GetBusStop(int license);
        //void AddBusStop(BusStop busStop);
        //void UpdateBusStop(BusStop busStop);
        //void UpdateBusStop(int license, Action<BusStop> update); // method that knows to updt specific fields in Person
        //void DeleteBusStop(int license);
        #endregion

    }
}