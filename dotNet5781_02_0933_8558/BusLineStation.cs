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
        public BusLineStation(double dist, double minutes, double lati,
            double longi, int stationKey, string address)
        {
            base.Latitude = lati;
            base.Longitude = longi;
            base.StationAddress = address;
            base.BusStationKey = stationKey;
            DistanceFromPreviousStation = dist;
            TravelTimeFromPreviousStation = TimeSpan.FromMinutes(minutes);
        }

        private double distanceFromLastStation;

        public double DistanceFromPreviousStation
        {
            get { return distanceFromLastStation; }
            set { if (distanceFromLastStation < 0)
                    throw new ArgumentException("Illegal input of distence.");
                distanceFromLastStation = value; }
        }

        private TimeSpan travelTimeFromLastStation;
       
        public TimeSpan TravelTimeFromPreviousStation
        {
            get { return travelTimeFromLastStation; }
            set { 
                if (value.Minutes < 0)
                    throw new ArgumentException("Illegal input of minutes.");
                travelTimeFromLastStation = TimeSpan.FromMinutes(value.Minutes); }
        }

    }
}
