using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    class BusLine
    {
        public List<BusLineStation> bus;
        
        private int busLineNmber;
                            
        public int BusLineNmber
        {
            get { return busLineNmber; }
            set { if(busLineNmber < 0)
                    throw new ArgumentException("Illegal bus line number.");
                busLineNmber = value; }
        }
        private int firstStation;

        public int FirstStation
        {
            get { return firstStation; }
            set {
                if (firstStation < 0)
                    throw new ArgumentException("Illegal input of first station.");
                firstStation = value; }
        }

        private int lastStation;
        public int LastStation
        {
            get { return lastStation; }
            set
            {
                if (lastStation < 0)
                    throw new ArgumentException("Illegal input of last station.");
                lastStation = value;
            }
        }



    }
       
}
