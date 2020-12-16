using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BusAtTravel
    {
        public int BusIdentifier { get; set; } // Entity Identifier (Key) - Automatic running number
        public string License { get; set; }  // Entity Key A
        public int BusLineNumberIdentifier { get; set; }  // Entity Key B
        public TimeSpan FormalDepartureTime { get; set; }  // Entity Key C
        public TimeSpan ActualDepartureTime { get; set; }
        public int PrevBusLineStationNumber { get; set; }
        public TimeSpan PrevStationArrivalTime { get; set; }
        public TimeSpan NextStationArrivalTime { get; set; }
        public int BusDriverID { get; set; }

    }
}
