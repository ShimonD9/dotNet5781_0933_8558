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
       public BusLineStation(BusStop busStop, double dist, double minutes)
        {
            busStopKey = busStop.BusStopKey;
            busStopAddress = busStop.BusStopAddress;
            longitude = busStop.Longitude;
            latitude = busStop.Latitude;
            DistanceFromPreviousStation = dist;
            TimeTravelFromPreviousStation = TimeSpan.FromMinutes(minutes);
        }


        // As told by Dan Zilberstein - the Bus Line Station should contain the same fields as in the bus stop (without inheritance)
        /// <summary>
        ///  Bus station key field and property
        /// </summary>
        private int busStopKey;

        public int BusStopKey { get { return busStopKey; } }

        /// <summary>
        /// Station address field and property
        /// </summary>
        string busStopAddress;
        public string StationAddress { get { return busStopAddress; } }


        /// <summary>
        /// Latitude and longitude fields and properties:
        /// </summary>

        private double latitude;

        public double Latitude  { get { return latitude; } }

        private double longitude;

        public double Longitude { get { return longitude; } }

        private double distanceFromPreviousStation;

        public double DistanceFromPreviousStation
        {
            get { return distanceFromPreviousStation; }
            set { if (distanceFromPreviousStation < 0)
                    throw new ArgumentException("Illegal input of distence.");
                distanceFromPreviousStation = value; }
        }

        private TimeSpan timeTravelFromPreviousStation;
       
        public TimeSpan TimeTravelFromPreviousStation
        {
            get { return timeTravelFromPreviousStation; }
            set { 
                if (value.Minutes < 0)
                    throw new ArgumentException("Illegal input of minutes.");
                timeTravelFromPreviousStation = value; }
        }

        /// <summary>
        /// Overrides the ToString function, returning info on the bus line station
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Bus Station Code:\n" +
                                  "BusStationKey = {0}, Latitude = {1}, Longitude = {2}, Time from last station = {3}, Km from last station = {4}", BusStopKey, Latitude, Longitude, TimeTravelFromPreviousStation, DistanceFromPreviousStation);
        }
    }
}
