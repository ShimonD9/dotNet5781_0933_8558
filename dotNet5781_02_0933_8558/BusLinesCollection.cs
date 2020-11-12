﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Schema;

namespace dotNet5781_02_0933_8558
{
    //NOTE:
    public class BusLinesCollection : IEnumerable
    {
        public BusLinesCollection() { }

        public IEnumerator GetEnumerator()
        {
            IEnumerator counterBusLine = new BusLinesCollectionIEnumartor(busLineCollectionsList);
            return counterBusLine;
        }

        public List<BusLine> busLineCollectionsList = new List<BusLine> { };

        public void AddBusLine(BusLine BusLine)
        {
            busLineCollectionsList.Insert(0, BusLine);;
        }

        public void DeleteBusLine(int BusLineKey)
        {
            foreach (BusLine bus in busLineCollectionsList)
            {
                if (bus.BusLineNumber == BusLineKey)
                {
                    busLineCollectionsList.Remove(bus);
                    return;
                }
            }
            throw new KeyNotFoundException("the bus number was not found");
        }

       public bool searchBusLine(int keyBusLine)
        {
            int count = 0;
            foreach (BusLine bus in busLineCollectionsList)
            {
                if (bus.BusLineNumber == keyBusLine)
                    count++;
            }
            if (count < 2)
                return true;
            return false;
        }

        public List<BusLine> listLinesForStation(int stationKey)
        {
            bool flag = false;
            List<BusLine> tempList = new List<BusLine> { };
            foreach (BusLine bus in busLineCollectionsList)
            {
                foreach (BusLineStation station in bus.BusStationList)
                {
                    if (station.BusStopKey == stationKey)
                    {
                        tempList.Insert(0,bus);
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
                throw new KeyNotFoundException("there is no line buses for this station");
            return tempList;
        }

        public int searchIndex(int busLineKey)
        {
            for (int i = 0; i < busLineCollectionsList.Count; i++)
                if (busLineCollectionsList[i].BusLineNumber == busLineKey)
                    return i;
            throw new KeyNotFoundException("the bus line number was not found\n");
        }

        public List<BusLine> sortBusList()
        {
            List<BusLine> tempList = busLineCollectionsList;
            tempList.Sort();
            return tempList;
        }

        public BusLine this[int numBusLine]
        {
            get
            {
                int index = searchIndex(numBusLine);
                return busLineCollectionsList[index];
            }
            set
            {
                int index = searchIndex(numBusLine);
                busLineCollectionsList[index] = value;
            }

        }

        public bool staionExist(int stationKey)
        {           
            foreach (BusLine bus in busLineCollectionsList)
                foreach (BusLineStation station in bus.BusStationList)                
                    if (station.BusStopKey == stationKey)
                        return true;
            return false;           
        }
    }
}