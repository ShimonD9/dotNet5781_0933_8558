using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BusStop
    {
        public int BusStopKey { get; set; }
        public string BusStopName { get; set; }
        public string BusStopAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Sunshade { get; set; } // גגון
        public bool DigitalPanel { get; set; } // פאנל דיגיטלי
    }
}
