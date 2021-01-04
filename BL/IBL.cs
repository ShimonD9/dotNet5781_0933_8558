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
        IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate);
        BusStop GetBusStop(int license);
        void AddBusStop(BusStop busStop);
        void UpdateBusStop(BusStop busStop);
        void UpdateBusStop(int license, Action<BusStop> update); // method that knows to updt specific fields in Person
        void DeleteBusStop(int license);
        #endregion

        #region BusLineStation
        IEnumerable<BusLineStation> GetAllBusLineStations();
        IEnumerable<BusLineStation> GetAllBusLineStationsBy(Predicate<BusLineStation> predicate);
        BusLineStation GetBusLineStation(int license);
        void AddBusLineStation(BusLineStation busLineStation);
        void UpdateBusLineStation(BusLineStation busLineStation);
        void UpdateBusLineStation(int license, Action<BusLineStation> update); // method that knows to updt specific fields in Person
        void DeleteBusLineStation(int license);
        #endregion

        #region BusLine
        IEnumerable<BusLine> GetAllBusLines();
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);
        BusLine GetBusLine(int license);
        int AddBusLine(BusLine busLine, double kmToNext, TimeSpan timeToNext, TimeSpan startTime, TimeSpan endTime, int frequency);
        void UpdateBusLine(BusLine busLine);
        void UpdateBusLine(int license, Action<BusLine> update); // method that knows to updt specific fields in Person
        void DeleteBusLine(int license);
        #endregion

        #region Consecutive Stations
        void CheckIfConsecutiveExistOrInactive(int busStopKeyA, int busStopKeyB);
        #endregion

       // #region LineDeparture
       // IEnumerable<LineDeparture> GetAllLineDepartures();
       // LineDeparture GetLineDeparture(int busLineID);
       //void AddLineDeparture(LineDeparture busLineStationBo);
       // #endregion

        #region User
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        User GetUser(string userName);
        void AddUser(User user);
        void UpdateUser(User user);
        void UpdateUser(string license, Action<User> update); // method that knows to updt specific fields in Person
        void DeleteUser(string license);
        #endregion

    }
}