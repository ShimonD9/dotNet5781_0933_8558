using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    class Config
    {
        private static int runningNumBusAtTravel = 0;
        public static int RunningNumBusAtTravel { get => ++runningNumBusAtTravel; }

        private static int runningNumBusLine = 0;
        public static int RunningNumBusLine { get => ++runningNumBusLine; }

        private static int runningNumLineDeparture = 0;
        public static int RunningNumLineDeparture { get => ++runningNumLineDeparture; }

    }
}
