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
        public int MinutesLeftUntilArrival { get; set; }
        public string LastBusStopName { get; set; }
    }
}
