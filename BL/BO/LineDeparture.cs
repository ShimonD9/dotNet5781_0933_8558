using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineDeparture
    {
        public int DepartureID { get; set; }  // Entity Key A
        public int BusLineID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; } // Only if Frequency > 0
        public int Frequency { get; set; } // If 0 - A single travel. If 6 - it means, every 6 minutes
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Identifier = {0}, Start Time= {1},End Time = {2},Frequency = {3}", BusLineID, StartTime, EndTime, Frequency);
        }
    }
}
