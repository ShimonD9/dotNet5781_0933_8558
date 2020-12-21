using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusLineStation
    {
        public int BusLineID { get; set; }  // Entity Key A (not line number)
        public int BusStopKey { get; set; }  // Entity Key B
        public int LineStationIndex { get; set; }
        public int PrevStation { get; set; }
        public int NextStation { get; set; }
        public bool ObjectActive { get; set; }

        public override string ToString()           // Printing details of a station
        {
            return string.Format("Bus station key = {0, -20} ", BusStopKey);
        }
    }
}
