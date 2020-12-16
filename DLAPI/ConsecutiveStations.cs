using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class ConsecutiveStations
    {
        public int BusStopKeyA { get; set; }
        public int BusStopKeyB { get; set; }
        public double Distance { get; set; }
        public TimeSpan TravelTime { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Stop Key A= {0}, Bus Stop Key B= {1}, Distance = {2}, TravelTime = {3}", BusStopKeyA, BusStopKeyB, Distance, TravelTime);
        }
    }
}
