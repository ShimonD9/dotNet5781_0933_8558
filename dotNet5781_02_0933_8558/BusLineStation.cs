using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    public class BusLineStation : BusStation
    {
        private double distance;

        public double Distance
        {
            get { return distance; }
            set { if (distance < 0)
                    throw new ArgumentException("Illegal input of distence.");
                distance = value; }
        }
        private float timeTravelFromLastStaiton;

        public float TimeTravelFromLastStaiton
        {
            get { return timeTravelFromLastStaiton; }
            set {
                if (timeTravelFromLastStaiton < 0)
                    throw new ArgumentException("Illegal input of time travel.");
                timeTravelFromLastStaiton = value; }
        }

    }
}
