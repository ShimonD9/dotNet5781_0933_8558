using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    class BusStation
    {
        public static Random lineLocation = new Random(DateTime.Now.Millisecond);

        
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

        private int stationCode;

        public int StationCode
        {
            get { return stationCode; }
            set
            {
                if (value < 0 || value > 1000000)
                    throw new ArgumentException("Worng input for station code.");
                else
                    stationCode = value;
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


        private double stationLocation;

        public double StationLocation
        {
            get { return stationLocation; }
            set { stationLocation = value; }
        }

        public override string ToString()
        {
            return string.Format("Bus Station Code:\n+" +
                                  "BusStationKey = {0},Latitude = {1}, Longitude = {2}", StationCode, Location.latitude, Location.longitude);
        }
    }
}
