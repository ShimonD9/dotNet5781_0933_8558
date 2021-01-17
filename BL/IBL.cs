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
        BusLineStation GetBusLineStation(int busLineID, int busStopCode);
        void AddBusLineStation(BusLineStation station, TimeSpan prevGapTimeUpdate, double prevGapKmUpdate);
        void UpdateBusLineStation(BusLineStation busLineStation);
        void UpdateBusLineStation(int license, Action<BusLineStation> update); // method that knows to updt specific fields in Person
        void DeleteBusLineStation(int busLineID, int busStopCode, TimeSpan gapTimeUpdate, double gapKmUpdate);
        #endregion

        #region BusLine
        IEnumerable<BusLine> GetAllBusLines();
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);
        BusLine GetBusLine(int busLineID);
        void AddBusLine(BusLine busLine, BusLineStation busLineStationA, BusLineStation busLineStationB);
        void UpdateBusLine(BusLine busLine);
        void UpdateBusLine(int busLineID, Action<BusLine> update); // method that knows to updt specific fields in Person
        void DeleteBusLine(int busLineIDlicense);
        #endregion

        #region Consecutive Stations
        /// <summary>
        /// Checks if two given bus stop keys represented as consecutive stations at the data source
        /// </summary>
        /// <param name="busStopKeyA"></param>
        /// <param name="busStopKeyB"></param>
        /// <returns>True if the consecutive stations exist</returns>
        bool IsConsecutiveExist(int busStopKeyA, int busStopKeyB);

        /// <summary>
        /// Checks if two given bus stops (assumed as a consecutive stations) are in use as active bus line stations
        /// </summary>
        /// <param name="busStopKeyA"></param>
        /// <param name="busStopKeyB"></param>
        /// <returns></returns>
        bool IsConsecutiveInUse(int busStopKeyA, int busStopKeyB);
        #endregion

        #region LineDeparture
        void AddLineDeparture(TimeSpan departureTime, int busLineID);
        void DeleteLineDeparture(TimeSpan departureTime, int busLineID);
       #endregion

        #region User
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        User GetUser(string userName);
        void AddUser(User user);
        void UpdateUser(User user);
        void UpdateUser(string license, Action<User> update); // method that knows to updt specific fields in Person
        void DeleteUser(string license);
        #endregion

        #region Time simulator and Line Timing

        IEnumerable<LineTiming> GetLineTimingsPerStation(BusStop currBusStation, TimeSpan tsCurrentTime);
        TimeSpan FindLastDepartureTime(int busLineID, TimeSpan tsCurrentTime);
        TimeSpan StationTravelTimeCalculation(int busLineID, int busStopCode);
        #endregion

    }
}