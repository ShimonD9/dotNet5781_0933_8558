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
        IEnumerable<BusLineStation> GetAllBusLineStations();
        BusLineStation GetBusLineStation(int busLineID, int busStopCode);
        void AddBusLineStation(BusLineStation station, TimeSpan prevGapTimeUpdate, double prevGapKmUpdate);
        void DeleteBusLineStation(int busLineID, int busStopCode, TimeSpan gapTimeUpdate, double gapKmUpdate);
        #endregion

        #region BusStop: Get, Add, Update, Delete methods
        IEnumerable<BusStop> GetAllBusStops();
        BusStop GetBusStop(int busStopCode);
        void AddBusStop(BusStop busStop);
        void UpdateBusStop(BusStop busStop);
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
        void AddLineDeparture(TimeSpan departureTime, int busLineID);
        void DeleteLineDeparture(TimeSpan departureTime, int busLineID);
        #endregion

        #region Bus: Get, Add, Update, Delete, Refuel and Treat methods
        IEnumerable<Bus> GetAllBuses();
        Bus GetBus(int license);
        void AddBus(Bus bus);
        void UpdateBus(Bus bus);
        void UpdateBus(int license, Action<Bus> update); // method that knows to updt specific fields in Person
        void DeleteBus(int license);
        void RefuelBus(BO.Bus bus);
        void TreatBus(BO.Bus bus);
        #endregion

        #region User: Get, Add, Update, Delete methods
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        User GetUser(string userName);
        void AddUser(User user);
        void UpdateUser(User user);
        void UpdateUser(string license, Action<User> update); // method that knows to updt specific fields in Person
        void DeleteUser(string license);
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
        #endregion



    }
}