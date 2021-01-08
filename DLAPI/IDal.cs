using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
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

        #region BusAtTravel
        IEnumerable<BusAtTravel> GetAllBusesAtTravel();
        IEnumerable<BusAtTravel> GetAllBusesAtTravelBy(Predicate<BusAtTravel> predicate);
        BusAtTravel GetBusAtTravel(int license);
        void AddBusAtTravel(BusAtTravel bus);
        void UpdateBusAtTravel(BusAtTravel bus);
        void UpdateBusAtTravel(int license, Action<BusAtTravel> update); // method that knows to updt specific fields in Person
        void DeleteBusAtTravel(int license);
        #endregion

        #region BusLine
        IEnumerable<BusLine> GetAllBusLines();
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);
        BusLine GetBusLine(int  busLineID);
        int AddBusLine(BusLine busLine);
        void UpdateBusLine(BusLine busLine);
        void UpdateBusLine(int busLineID, Action<BusLine> update); // method that knows to updt specific fields in Person
        void DeleteBusLine(int busLineID);
        #endregion

        #region BusLineStation
        IEnumerable<BusLineStation> GetAllBusLineStations();
        IEnumerable<BusLineStation> GetAllBusLineStationsBy(Predicate<BusLineStation> predicate);
        BusLineStation GetBusLineStation(int busLineID, int busStopCode);
        void AddBusLineStation(BusLineStation busLineStation);
        void UpdateBusLineStation(BusLineStation busLineStation);
        void UpdateBusLineStation(int busLineID, int busStopCode, Action<BusLineStation> update); // method that knows to updt specific fields in Person
        void DeleteBusLineStation(int busLineID, int busStopCode);
        void DeleteBusLineStationsByID(int busLineID);

        #endregion

        #region BusStop
        IEnumerable<BusStop> GetAllBusStops();
        IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate);
        BusStop GetBusStop(int busStopKey);
        void AddBusStop(BusStop busStop);
        void UpdateBusStop(BusStop busStop);
        void UpdateBusStop(int busStopKey, Action<BusStop> update); // method that knows to updt specific fields in Person
        void DeleteBusStop(int busStopKey);
        #endregion

        #region ConsecutiveStations
        IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();
        IEnumerable<ConsecutiveStations> GetAllConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate);
        ConsecutiveStations GetConsecutiveStations(int busStopCodeA, int busStopCodeB);
        void AddConsecutiveStations(ConsecutiveStations consecutiveStations);
        void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations);
        void UpdateConsecutiveStations(int busStopCodeA, int busStopCodeB, Action<ConsecutiveStations> update); // method that knows to updt specific fields in Person
        void DeleteConsecutiveStations(int busStopCodeA, int busStopCodeB);
        #endregion

        #region LineDeparture
        IEnumerable<LineDeparture> GetAllLineDeparture();
        IEnumerable<LineDeparture> GetAllLineDepartureBy(Predicate<LineDeparture> predicate);
        LineDeparture GetLineDeparture(int busLineID);
        void AddLineDeparture(LineDeparture lineDeparture);
        void UpdateLineDeparture(LineDeparture lineDeparture);
        void UpdateLineDeparture(int busLineID, Action<LineDeparture> update); // method that knows to updt specific fields in Person
        void DeleteLineDeparture(TimeSpan departureTime, int busLineID);
        void DeleteLineDepartureByID(int busLineID);
        #endregion

        #region User
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        User GetUser(string userName);
        void AddUser(User user);
        void UpdateUser(User user);
        void UpdateUser(string userName, Action<User> update); // method that knows to updt specific fields in Person
        void DeleteUser(string userName);
        #endregion
    }
}
