using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BusLineStation
    {
        public int BusLineRunningNumber { get; set; }
        public int BusStopKey { get; set; }
        public int BusStopNumberAtLine { get; set; }

        public override string ToString()           // Printing details of a station
        {
            return string.Format("Bus station key = {0, -20} ", BusStopKey);
        }
    }
}
