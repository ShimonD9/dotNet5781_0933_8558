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
        public BusLineStation(double dist, int minutes, double lati,
            double longi, int stationKey, string address)
        {
            base.Latitude = lati;
            base.Longitude = longi;
            base.StationAddress = address;
            base.BusStationKey = stationKey;
            Distance = dist;
            TravelTimeFromLastStation = TimeSpan.FromMinutes(minutes);
        }
        private double distance;

        public double Distance
        {
            get { return distance; }
            set { if (distance < 0)
                    throw new ArgumentException("Illegal input of distence.");
                distance = value; }
        }
           void addStation(int keyStation, double lati, double longi, string address)
        {
            BusStationKey = keyStation;
            Latitude = lati;
            Longitude = longi;
            StationAddress = address;
        }
        private TimeSpan travelTimeFromLastStation;
       
        public TimeSpan TravelTimeFromLastStation
        {
            get { return travelTimeFromLastStation; }
            set { 
                if(value.Minutes <0)
                    throw new ArgumentException("Illegal input of minutes.");
                travelTimeFromLastStation =TimeSpan.FromMinutes(value.Minutes); }
        }

    }
}
