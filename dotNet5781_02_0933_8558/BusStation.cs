using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    public class BusStation
    {
        public static Random lineLocation = new Random(DateTime.Now.Millisecond);
       public BusStation(double lati,double longi,int stationKey, string address)
        {
            Latitude = lati;
            Longitude = longi;
            BusStationKey = stationKey;
            StationAddress = address;
        }
        public BusStation() {; }
        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set { latitude = 31 + lineLocation.NextDouble() * 2.3; }
        }

        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set { longitude = 34.3 + lineLocation.NextDouble() * 1.2 ; }
        }

        private int busStationKey;

        public int BusStationKey
        {
            get { return busStationKey; }
            set
            {
                if (value < 0 || value > 1000000)
                    throw new ArgumentException("Worng input for station code.");
                else
                    busStationKey = value;
            }
        }

        
        string stationAddress;
        public string StationAddress
        {
            get { return stationAddress; }
            set
            {
                stationAddress = value;
            }
        }
        public override string ToString()
        {
            return string.Format("Bus Station Code:\n" +
                                  "BusStationKey = {0},Latitude = {1}, Longitude = {2}", BusStationKey, Latitude, Longitude);
        }
    }
}
