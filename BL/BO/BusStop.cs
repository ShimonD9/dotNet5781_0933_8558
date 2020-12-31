using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusStop
    {
        public int BusStopKey { get; set; }   // Entity Identifier (Key)
        public string BusStopName { get; set; }
        public string BusStopAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Sunshade { get; set; } // גגון
        public bool DigitalPanel { get; set; } // פאנל דיגיטלי
        public IEnumerable<BusLine> LinesStopHere { get; set; }
        public bool ObjectActive { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Stop Key= {0}, Bus Stop Name= {1}, Bus Stop Address = {2}, Latitude = {3}, Longitude = {4}, Sunshade = {5}, Digital Panel = {6}", BusStopKey, BusStopName, BusStopAddress, Latitude, Longitude, Sunshade, DigitalPanel);
        }

    }
}
