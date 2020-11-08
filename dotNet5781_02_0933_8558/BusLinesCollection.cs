using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace dotNet5781_02_0933_8558
{
    //NOTE:
    /*
     התייחסנו לכך שנדיר שישנם 2 קווי אוטובוס באותו העיר
     */
    class BusLinesCollection : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            IEnumerator counterBusLine = new BusLinesCollectionIEnumartor(busLineCollections);
            return counterBusLine;
        }

        List<BusLine> busLineCollections = new List<BusLine> { };
        void AddBusLine(BusLine BusLine)
        {
            busLineCollections.Add(BusLine);
        }

        void DeleteBusLine(BusLine BusLine)
        {       
            foreach (var item in busLineCollections)
            {
                GetEnumerator();
            }
        }
        bool searchBusLine(int keyBusLine)
        {
            int count = 0;
            foreach (BusLine bus in busLineCollections)
            {
                if (bus.BusLineNmber == keyBusLine)
                    count++;
            }
            if (count < 3)
                return true;
            return false;
        }
                
    }
}
