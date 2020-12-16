using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DO.Enums;

namespace DO
{
    class BusLine
    {
        public int BusLineRunningNumber { get; set; }
        public int BusLineNumber { get; set; }
        public AREA Area { get; set; }
        public int FirstBusStopKey { get; set; }
        public int LastBusStopKey { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Line Running Number = {0}, Bus Line Number= {1}, Area = {2}, First Bus Stop Key = {3}, Last Bus Stop Key = {4}", BusLineRunningNumber, BusLineNumber, Area, FirstBusStopKey, LastBusStopKey);
        }
    }
}
