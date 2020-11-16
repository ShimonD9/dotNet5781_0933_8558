/*
 Exercise 2 - Mendi Ben Ezra (311140933), Shimon Dyskin (310468558)
 Description: The program manages a collection of bus lines, with bus stations,
offering to add, delete, search and print.
 ===Note: According to the lecturer we decided, that two round-trip lines would not pass through stations with the same code (even the first and the last ones, as it is in reality.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    public class BusLineStation
    {
        /// <summary>
        /// BusLineStation ctor  
        /// </summary>
        /// <param name="busStop"></param>
        /// <param name="dist"></param>
        /// <param name="minutes"></param>
        public BusLineStation(BusStop busStop, double dist, double minutes)
        {
            busStopKey = busStop.BusStopKey;
            busStopAddress = busStop.BusStopAddress;
            longitude = busStop.Longitude;
            latitude = busStop.Latitude;
            DistanceFromPreviousStation = dist;
            TimeTravelFromPreviousStation = TimeSpan.FromMinutes(minutes);
        }


        // NOTE: As told by Dan Zilberstein - the Bus Line Station should contain the same fields as in the bus stop (without inheritance)
        /// <summary>
        ///  Bus station key field and property
        /// </summary>
        private int busStopKey;

        public int BusStopKey { get { return busStopKey; } }

        /// <summary>
        /// Station address field and property
        /// </summary>
        private string busStopAddress;
        public string StationAddress { get { return busStopAddress; } }


        /// <summary>
        /// Latitude and longitude fields and properties:
        /// </summary>

        private double latitude;

        public double Latitude  { get { return latitude; } }

        private double longitude;

        public double Longitude { get { return longitude; } }
        /// <summary>
        /// bus distance from last station property
        /// </summary>
        private double distanceFromPreviousStation;

        public double DistanceFromPreviousStation
        {
            get { return distanceFromPreviousStation; }
            set { if (distanceFromPreviousStation < 0)          //in case of illegal input
                    throw new ArgumentException("Illegal input of distence.");
                distanceFromPreviousStation = Math.Round(value, 1); }
        }
        /// <summary>
        /// the time travel of the bus from last station property
        /// </summary>
        private TimeSpan timeTravelFromPreviousStation;
       
        public TimeSpan TimeTravelFromPreviousStation
        {
            get { return timeTravelFromPreviousStation; }
            set { 
                if (value.Minutes < 0)                  //in case of illegal input
                    throw new ArgumentException("Illegal input of minutes.");
                timeTravelFromPreviousStation = value - TimeSpan.FromMilliseconds(value.Milliseconds); // Removes the milliseconds
            }
        }

        /// <summary>
        /// Overrides the ToString function, returning info on the bus line station
        /// </summary>
        /// <returns></returns>
        public override string ToString()           //pritting details of a station
        {
            return string.Format("Bus station key = {0, -20} Address = {1, -20} Time from last station = {2, -20} Km from last station = {3}", BusStopKey, StationAddress, TimeTravelFromPreviousStation, DistanceFromPreviousStation);
        }
    }
}
