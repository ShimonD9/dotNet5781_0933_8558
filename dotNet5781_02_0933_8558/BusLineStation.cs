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
            BusStop = busStop;
            DistanceFromPreviousStation = dist;
            TimeTravelFromPreviousStation = TimeSpan.FromMinutes(minutes);
        }

        private BusStop busStop;

        public BusStop BusStop
        {
            get { return busStop; }
            set
            {
                busStop = value;
            }
        }

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
                timeTravelFromPreviousStation = TimeSpan.FromMinutes(value.Minutes); }
        }

        /// <summary>
        /// Overrides the ToString function, returning info on the bus line station
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Bus Station Code:\n" +
                                  "BusStationKey = {0}, Latitude = {1}, Longitude = {2}, Time from last station = {3}, Km from last station = {4}", BusStop.BusStopKey, BusStop.Latitude, BusStop.Longitude, TimeTravelFromPreviousStation, DistanceFromPreviousStation);
        }
    }
}
