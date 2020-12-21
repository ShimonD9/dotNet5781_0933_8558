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
        BusLine GetBusLine(int license);
        void AddBusLine(BusLine busLine);
        void UpdateBusLine(BusLine busLine);
        void UpdateBusLine(int license, Action<BusLine> update); // method that knows to updt specific fields in Person
        void DeleteBusLine(int license);
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

        #region BusStop
        IEnumerable<BusStop> GetAllBusStops();
        IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate);
        BusStop GetBusStop(int license);
        void AddBusStop(BusStop busStop);
        void UpdateBusStop(BusStop busStop);
        void UpdateBusStop(int license, Action<BusStop> update); // method that knows to updt specific fields in Person
        void DeleteBusStop(int license);
        #endregion

        #region ConsecutiveStations
        IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();
        IEnumerable<ConsecutiveStations> GetAllConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate);
        ConsecutiveStations GetConsecutiveStations(int busConsecutiveA);
        void AddConsecutiveStations(ConsecutiveStations consecutiveStations);
        void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations);
        void UpdateConsecutiveStations(int license, Action<ConsecutiveStations> update); // method that knows to updt specific fields in Person
        void DeleteConsecutiveStations(int license);
        #endregion

        #region LineDeparture
        IEnumerable<LineDeparture> GetAllLineDeparture();
        IEnumerable<LineDeparture> GetAllLineDepartureBy(Predicate<LineDeparture> predicate);
        LineDeparture GetLineDeparture(int license);
        void AddLineDeparture(LineDeparture lineDeparture);
        void UpdateLineDeparture(LineDeparture lineDeparture);
        void UpdateLineDeparture(int license, Action<LineDeparture> update); // method that knows to updt specific fields in Person
        void DeleteLineDeparture(int license);
        #endregion

        #region User
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllUsersBy(Predicate<Bus> predicate);
        User GetUser(string userName);
        void AddUser(User user);
        void UpdateUser(User user);
        void UpdateUser(string license, Action<User> update); // method that knows to updt specific fields in Person
        void DeleteUser(string license);
        #endregion
    }
}
