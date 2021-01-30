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
        public IEnumerable<BusLineAtBusStop> LinesStopHere { get; set; }
        public bool ObjectActive { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus stop object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("{0} - {1}", BusStopKey, BusStopName);
        }

    }
}
