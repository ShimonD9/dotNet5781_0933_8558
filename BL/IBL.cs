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
        #region BusLine: Get, Add, Update, Delete methods

        /// <summary>
        /// Gets all the bus lines
        /// </summary>
        /// <returns>IEnumerable of all the bus lines</returns>
        IEnumerable<BusLine> GetAllBusLines();

        /// <summary>
        /// Asks for a bus line by bus line id, and returns its BO adaption
        /// </summary>
        /// <param name="busLineID"></param>
        /// <returns>BO.BusLine show</returns>
        BusLine GetBusLine(int busLineID);

        /// <summary>
        /// Add bus line
        /// </summary>
        /// <param name="busLine"></param>
        /// <param name="busLineStationA"></param>
        /// <param name="busLineStationB"></param>
        void AddBusLine(BusLine busLine, BusLineStation busLineStationA, BusLineStation busLineStationB);

        /// <summary>
        /// Updates the bus line
        /// </summary>
        /// <param name="busLineBO"></param>
        void UpdateBusLine(BusLine busLine);

        /// <summary>
        /// Deletes a bus line by its ID
        /// </summary>
        /// <param name="busLineID"></param>
        void DeleteBusLine(int busLineID);
        #endregion

        #region BusLineStation: Get, Add, Delete methods

        /// <summary>
        /// Gets all the bus line stations
        /// </summary>
        /// <returns>Returns the IEnumerable collection of bus line stations</returns>
        IEnumerable<BusLineStation> GetAllBusLineStations();

        /// <summary>
        /// Gets a bus line stations based on a bus line ID and a bus stop code
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="busStopCode"></param>
        /// <returns>Returns a BO.BusLineStation</returns>
        BusLineStation GetBusLineStation(int busLineID, int busStopCode);

        /// <summary>
        /// Adds a bus line station, and adds/deletes consecutive stations if needed based on the parameters
        /// </summary>
        /// <param name="station"></param>
        /// <param name="prevGapTimeUpdate"></param>
        /// <param name="prevGapKmUpdate"></param>
        void AddBusLineStation(BusLineStation station, TimeSpan prevGapTimeUpdate, double prevGapKmUpdate);
        
        /// <summary>
        /// Deletes a bus line stations, and adds/deletes consecutive stations based on the given parameters
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="busStopCode"></param>
        /// <param name="gapTimeUpdate"></param>
        /// <param name="gapKmUpdate"></param>
        void DeleteBusLineStation(int busLineID, int busStopCode, TimeSpan gapTimeUpdate, double gapKmUpdate);

        /// <summary>
        /// Groups a route by its line id
        /// </summary>
        /// <returns>A collection of grouping by id and bus line station</returns>
        IEnumerable<IGrouping<int, BusLineStation>> RouteByLine();

        #endregion

        #region BusStop: Get, Add, Update, Delete methods

        /// <summary>
        /// Gets all the bus stops
        /// </summary>
        /// <returns>Returns the IEnumerable collection of bus stops</returns>
        IEnumerable<BusStop> GetAllBusStops();

        /// <summary>
        /// Gets a bus stop based on the bus stop code
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="busStopCode"></param>
        /// <returns>Returns a BO.BusStop</returns>
        BusStop GetBusStop(int busStopCode);

        /// <summary>
        /// Adds a bus stop to the data
        /// </summary>
        /// <param name="busStop"></param>
        void AddBusStop(BusStop busStop);

        /// <summary>
        /// Updates the bus stop by a given BO.BusStop
        /// </summary>
        /// <param name="busStop"></param>
        void UpdateBusStop(BusStop busStop);

        /// <summary>
        /// Deletes a bus stop based on a given bus stop code
        /// </summary>
        /// <param name="busStopCode"></param>
        void DeleteBusStop(int busStopCode);

        #endregion

        #region Consecutive Stations: Boolean functions
        /// <summary>
        /// Checks if the consecutive stations exist, so there is no need to add the information
        /// </summary>
        /// <param name="busStopKeyA"></param>
        /// <param name="busStopKeyB"></param>
        /// <returns>True if exist, else - false</returns>
        bool IsConsecutiveExist(int busStopKeyA, int busStopKeyB);

        /// <summary>
        /// Checks if a pair of consecutive stations are in use
        /// </summary>
        /// <param name="busStopKeyA"></param>
        /// <param name="busStopKeyB"></param>
        /// <returns>True if in use, else - false</returns>
        bool IsConsecutiveInUse(int busStopKeyA, int busStopKeyB);

        #endregion

        #region LineDeparture: Add, Delete methods

        /// <summary>
        /// Adds a line departure to a given bus line id
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="busLineID"></param>
        void AddLineDeparture(TimeSpan departureTime, int busLineID);

        /// <summary>
        /// Deletes a line departure based on the given parameters
        /// </summary>
        /// <param name="departureTime"></param>
        /// <param name="busLineID"></param>
        void DeleteLineDeparture(TimeSpan departureTime, int busLineID);
        #endregion

        #region Bus: Get, Add, Update, Delete, Refuel and Treat methods

        /// <summary>
        /// Gets all the buses
        /// </summary>
        /// <returns>Returns the IEnumerable collection of buses</returns>
        IEnumerable<Bus> GetAllBuses();

        /// <summary>
        /// Gets a BO.Bus based on a given license
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        Bus GetBus(int license);

        /// <summary>
        /// Adds a bus to the data source
        /// </summary>
        /// <param name="bus"></param>
        void AddBus(Bus bus);

        /// <summary>
        /// Updates a bus by a given BO.Bus
        /// </summary>
        /// <param name="bus"></param>
        void UpdateBus(Bus bus);
        
        /// <summary>
        /// Deletes a bus by a given license number
        /// </summary>
        /// <param name="license"></param>
        void DeleteBus(int license);

        /// <summary>
        /// Refuels a given BO.Bus
        /// </summary>
        /// <param name="bus"></param>
        void RefuelBus(BO.Bus bus);

        /// <summary>
        /// Treats a given BO.Bus
        /// </summary>
        /// <param name="bus"></param>
        void TreatBus(BO.Bus bus);

        #endregion

        #region User: Get, Add, Update, Delete methods

        /// <summary>
        /// Gets all the users
        /// </summary>
        /// <returns>Returns the IEnumerable collection of users</returns>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Gets all the users matching the predicate
        /// </summary>
        /// <returns>Returns the IEnumerable collection of users</returns>
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);

        /// <summary>
        /// Gets user by the user name string
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>BO.User</returns>
        User GetUser(string userName);

        /// <summary>
        /// Adds a new user to the data source
        /// </summary>
        /// <param name="user"></param>
        void AddUser(User user);

        #endregion

        #region Other methods (Especially for the clock simulator)

        /// <summary>
        /// Finds and creats LineTiming objects by given the current bus stop and time
        /// </summary>
        /// <param name="currBusStop"></param>
        /// <param name="tsCurrentTime"></param>
        /// <returns>Collection of LineTimings</returns>
        IEnumerable<LineTiming> GetLineTimingsPerStation(BusStop currBusStation, TimeSpan tsCurrentTime);

        /// <summary>
        /// Finds the last departrue time of a bus line given the bus line ID and the current time
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="tsCurrentTime"></param>
        /// <returns>The time span of the last departure time</returns>
        TimeSpan FindLastDepartureTime(int busLineID, TimeSpan tsCurrentTime);

        /// <summary>
        /// Calculates the travel time from the first station of a given bus line ID to the given bus stop code
        /// </summary>
        /// <param name="busLineID"></param>
        /// <param name="busStopCode"></param>
        /// <returns>Time span of the total travel time</returns>
        TimeSpan StationTravelTimeCalculation(int busLineID, int busStopCode);

        /// <summary>
        /// A boolean function to check if a time span is valid
        /// </summary>
        /// <param name="timeUpdate"></param>
        /// <returns>True if the timeSpan is invalid, else - false</returns>
        bool isTimeSpanInvalid(TimeSpan timeUpdate);

        /// <summary>
        /// Statistics information
        /// </summary>
        /// <returns>Statistics object</returns>
        Statistics GetStats();
        
        #endregion

    }
}