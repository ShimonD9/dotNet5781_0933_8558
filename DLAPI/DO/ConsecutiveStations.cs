using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class ConsecutiveStations
    {
        public int BusStopKeyA { get; set; }   // Entity Key A
        public int BusStopKeyB { get; set; }   // Entity Key B
        public double Distance { get; set; }
        public TimeSpan TravelTime { get; set; }
        public bool ObjectActive { get; set; }
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
