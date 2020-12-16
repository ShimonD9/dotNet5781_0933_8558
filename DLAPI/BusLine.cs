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
    }
}
