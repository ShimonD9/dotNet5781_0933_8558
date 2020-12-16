using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class LineDeparture
    {
        public int BusIdentifier { get; set; }  // Entity Key A
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; } // Only if Frequency > 0
        public int Frequency { get; set; } // If 0 - A single travel
        
    }
}
