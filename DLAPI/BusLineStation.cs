using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusLineStation
    {
        public int BusLineRunningNumber { get; set; }  // Entity Key A
        public int BusStopKey { get; set; }  /// Entity Key B
        public int BusStopNumberAtLine { get; set; }

        public override string ToString()           // Printing details of a station
        {
            return string.Format("Bus station key = {0, -20} ", BusStopKey);
        }
    }
}
