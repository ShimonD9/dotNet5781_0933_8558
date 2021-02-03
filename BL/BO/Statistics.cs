using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Statistics
    {
        public int TotalOfCons { get; set; }
        public int TotalOfBusStops { get; set; }
        public int TotalOfBuses { get; set; }
        public int TotalOfBusLines { get; set; }

        /// <summary>
        /// Formats a string which represents the Statistics object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Total of consecutive stations: {0}", TotalOfCons);
        }
    }
}
