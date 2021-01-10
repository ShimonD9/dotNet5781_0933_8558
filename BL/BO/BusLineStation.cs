using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusLineStation
    {
        public int BusLineID { get; set; }  // Entity Key A (not line number)
        public int BusStopKey { get; set; }  /// Entity Key B
        public string BusStopName { get; set; }
        public int LineStationIndex { get; set; }
        public int PrevStation { get; set; }
        public int NextStation { get; set; }
        public double DistanceToNext { get; set; }
        public TimeSpan TimeToNext { get; set; }

        public bool ObjectActive { get; set; }

        public override string ToString()           // Printing details of a station
        {
            return string.Format("{0} - {1}", BusStopKey, BusStopName);

        }
    }
}
