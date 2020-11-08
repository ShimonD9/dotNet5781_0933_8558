using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    class BusLineStation : BusStation
    {
        public BusLineStation(double dist, double minutes, int stationKey, string address)
        {
            base.Latitude = 31 + lineLocation.NextDouble() * 2.3;
            base.Longitude = 34.3 + lineLocation.NextDouble() * 1.2; 
            base.StationAddress = address;
            base.BusStationKey = stationKey;
            DistanceFromPreviousStation = dist;
            TravelTimeFromPreviousStation = TimeSpan.FromMinutes(minutes);
        }

        private double distanceFromPreviousStation;

        public double DistanceFromPreviousStation
        {
            get { return distanceFromPreviousStation; }
            set { if (distanceFromPreviousStation < 0)
                    throw new ArgumentException("Illegal input of distence.");
                distanceFromPreviousStation = value; }
        }

        private TimeSpan travelTimeFromPreviousStation;
       
        public TimeSpan TravelTimeFromPreviousStation
        {
            get { return travelTimeFromPreviousStation; }
            set { 
                if (value.Minutes < 0)
                    throw new ArgumentException("Illegal input of minutes.");
                travelTimeFromPreviousStation = TimeSpan.FromMinutes(value.Minutes); }
        }

    }
}
