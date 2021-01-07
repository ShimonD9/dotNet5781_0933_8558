using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusLineAtBusStop
    {
        public int BusLineID { get; set; }  // Entity Identifier (Key) - Automatic running number
        public int BusLineNumber { get; set; }
        public int LastBusStopKey { get; set; }
        public string LastBusStopName { get; set; }


        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Line Number= {1}, Last Bus Stop Name = {2}", BusLineNumber, LastBusStopName);
        }
    }
}
