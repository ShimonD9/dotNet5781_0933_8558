using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Enums;

namespace BO
{
    public class BusLine
    {
        public int BusLineIdentifier { get; set; }  // Entity Identifier (Key) - Automatic running number
        public int BusLineNumber { get; set; }
        public AREA Area { get; set; }
        public int FirstBusStopKey { get; set; }
        public int LastBusStopKey { get; set; }
        public bool ObjectActive { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Line Running Number = {0}, Bus Line Number= {1}, Area = {2}, First Bus Stop Key = {3}, Last Bus Stop Key = {4}", BusLineIdentifier, BusLineNumber, Area, FirstBusStopKey, LastBusStopKey);
        }
    }
}
