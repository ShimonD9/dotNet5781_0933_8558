using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTiming
    {
        public int BusLineID { get; set; }  // Entity Identifier (Key) - Automatic running number
        public int BusLineNumber { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int MinutesToArrival { get; set; }
        public String ShowMinutesOrArrow { get; set; }
        public string LastBusStopName { get; set; }

        /// <summary>
        /// Formats a string which represents the LineTiming object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("User Name = {0}, BusLineNumber= {1},ArrivalTime = {2}", BusLineID, BusLineNumber, ArrivalTime);
        }
    }
}
