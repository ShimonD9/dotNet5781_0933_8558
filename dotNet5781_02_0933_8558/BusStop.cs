using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    public class BusStop
    {
       // A Random initialized for creating random longitude and latitude
       public static Random lineLocation = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Bus stop default ctor
        /// </summary>
        public BusStop() {; }


        /// <summary>
        /// The bus station ctor
        /// </summary>
        /// <param name="stationKey"></param>
        /// <param name="address"></param>
        public BusStop(int stopKey, string address)
        {
            BusStopKey = stopKey;
            BusStopAddress = address;
            latitude = 31 + lineLocation.NextDouble() * 2.3;
            longitude = 34.3 + lineLocation.NextDouble() * 1.2;
        }


        /// <summary>
        ///  Bus station key field and property
        /// </summary>
        private int busStopKey;

        public int BusStopKey
        {
            get { return busStopKey; }
            set
            {
                if (value < 0 || value > 1000000) // The bus station key is 6 digits only
                    throw new ArgumentException("Worng input for station code.");
                else
                    busStopKey = value;
            }
        }

        /// <summary>
        /// Station address field and property
        /// </summary>
        string busStopAddress;
        public string BusStopAddress
        {
            get { return busStopAddress; }
            set
            {
                busStopAddress = value;
            }
        }


        /// <summary>
        /// Latitude and longitude fields and properties:
        /// </summary>

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

        /// <summary>
        /// Overrides the ToString function, returning info on the bus station
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Bus Stop Code:\n" +
                                  "BusStopKey = {0}, Latitude = {1}, Longitude = {2}", BusStopKey, Latitude, Longitude);
        }
    }
}
