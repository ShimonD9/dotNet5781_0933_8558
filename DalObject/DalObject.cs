using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DalApi;
using DO;
using DS;

namespace DL
{
    sealed class DalObject : IDal    //internal
    {
        #region singelton
        static readonly DalObject instance = new DalObject();
        static DalObject() { } // static ctor to ensure instance init is done just before first usage
        DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
        #endregion

        //Implement IDL methods, CRUD
        #region Bus
        public IEnumerable<Bus> GetAllBuses()
        {
            return from bus in DataSource.ListBuses
                   select bus.Clone();
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public Bus GetBus(int license)
        {
            Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadIdException(license, $"bad person id: {license}");
        }

        public void AddBus(Bus bus)
        {
            if (DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License) != null)
                throw new DO.BadIdException(bus.License, "Duplicate person ID");
            DataSource.ListBuses.Add(bus.Clone());
        }
        public void UpdateBus(Bus bus)
        {
            throw new NotImplementedException();
        }
        public void UpdateBus(int license, Action<Bus> update)  // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteBus(int license)
        {
            throw new NotImplementedException();
        }
        #endregion 


        #region BusAtTravel
        public IEnumerable<BusAtTravel> GetAllBusesAtTravel()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<BusAtTravel> GetAllBusesAtTravelBy(Predicate<BusAtTravel> predicate)
        {
            throw new NotImplementedException();
        }
        public BusAtTravel GetBusAtTravel(int license)
        {
            throw new NotImplementedException();
        }
        public void AddBusAtTravel(BusAtTravel bus)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusAtTravel(BusAtTravel bus)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusAtTravel(int license, Action<BusAtTravel> update) // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteBusAtTravel(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BusLine
        public IEnumerable<BusLine> GetAllBusLines()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            throw new NotImplementedException();
        }
        public BusLine GetBusLine(int license)
        {
            throw new NotImplementedException();
        }
        public void AddBusLine(BusLine busLine)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusLine(BusLine busLine)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusLine(int license, Action<BusLine> update) // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteBusLine(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BusLineStation
        public IEnumerable<BusLineStation> GetAllBusLineStations()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<BusLineStation> GetAllBusLineStationsBy(Predicate<BusLineStation> predicate)
        {
            throw new NotImplementedException();
        }
        public BusLineStation GetBusLineStation(int license)
        {
            throw new NotImplementedException();
        }
        public void AddBusLineStation(BusLineStation busLineStation)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusLineStation(BusLineStation busLineStation)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusLineStation(int license, Action<BusLineStation> update) // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteBusLineStation(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BusStop
        public IEnumerable<BusStop> GetAllBusStops()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate)
        {
            throw new NotImplementedException();
        }
        public BusStop GetBusStop(int license)
        {
            throw new NotImplementedException();
        }
        public void AddBusStop(BusStop busStop)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusStop(BusStop busStop)
        {
            throw new NotImplementedException();
        }
        public void UpdateBusStop(int license, Action<BusStop> update) // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteBusStop(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ConsecutiveStations
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)
        {
            throw new NotImplementedException();
        }
        public ConsecutiveStations GetConsecutiveStations(int license)
        {
            throw new NotImplementedException();
        }
        public void AddConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            throw new NotImplementedException();
        }
        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            throw new NotImplementedException();
        }
        public void UpdateConsecutiveStations(int license, Action<ConsecutiveStations> update) // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteConsecutiveStations(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region LineDeparture
        public IEnumerable<LineDeparture> GetAllLineDeparture()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<LineDeparture> GetAllLineDepartureBy(Predicate<LineDeparture> predicate)
        {
            throw new NotImplementedException();
        }
        public LineDeparture GetLineDeparture(int license)
        {
            throw new NotImplementedException();
        }
        public void AddLineDeparture(LineDeparture lineDeparture)
        {
            throw new NotImplementedException();
        }
        public void UpdateLineDeparture(LineDeparture lineDeparture)
        {
            throw new NotImplementedException();
        }
        public void UpdateLineDeparture(int license, Action<LineDeparture> update) // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteLineDeparture(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region User
        public IEnumerable<Bus> GetAllUsers()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Bus> GetAllUsersBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }
        public User GetUser(int license)
        {
            throw new NotImplementedException();
        }
        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }
        public void UpdateUser(User user)
        {

        }
        public void UpdateUser(int license, Action<User> update) // method that knows to updt specific fields in Person
        {
            throw new NotImplementedException();
        }
        public void DeleteUser(int license)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}





