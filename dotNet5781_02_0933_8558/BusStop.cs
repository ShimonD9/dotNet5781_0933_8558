﻿/*
 Exercise 2 - Mendi Ben Ezra (311140933), Shimon Dyskin (310468558)
 Description: The program manages a collection of bus lines, with bus stations,
offering to add, delete, search and print.
 ===Note: According to the lecturer we decided, that two round-trip lines would not pass through stations with the same code (even the first and the last ones, as it is in reality.)
*/
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
            latitude = Math.Round(31 + lineLocation.NextDouble() * 2.3, 6); //random numbers between [31,33.3] for Latitud
            longitude = Math.Round(34.3 + lineLocation.NextDouble() * 1.2, 6); //random numbers between [34.3,35.5] for Longitude
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
                if (value < 0 || value > 1000000)       // The bus station key is 6 digits only
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
            set { latitude = Math.Round(31 + lineLocation.NextDouble() * 2.3, 6); }
        }

        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set { longitude = Math.Round(34.3 + lineLocation.NextDouble() * 1.2, 6) ; }
        }

        /// <summary>
        /// Overrides the ToString function, returning info on the bus station
        /// </summary>
        /// <returns></returns>
        public override string ToString()           //printting details of the stations
        {
            return string.Format("Bus Station Code: {0}, Address: {1}, Latitude = {2}°N , Longitude = {3}°E", BusStopKey, BusStopAddress, Latitude, Longitude);
        }
    }
}
