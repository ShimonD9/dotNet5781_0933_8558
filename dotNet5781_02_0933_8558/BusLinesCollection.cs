using System;
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

        /// <summary>
        /// Get enumerator of the collection
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            IEnumerator counterBusLine = new BusLinesCollectionIEnumartor(busLinesList);
            return counterBusLine;
        }

        /// <summary>
        /// List of bus lines initializer
        /// </summary>
        public List<BusLine> busLinesList = new List<BusLine> { };

        /// <summary>
        ///  Inserts a new bus to the collection
        /// </summary>
        /// <param name="BusLine"></param>
        public void AddBusLine(BusLine BusLine)
        {
            busLinesList.Insert(0, BusLine);;
        }

        /// <summary>
        /// Removes a bus from a collection by it's key
        /// </summary>
        /// <param name="BusLineKey"></param>
        public void DeleteBusLine(int BusLineKey)
        {
            foreach (BusLine bus in busLinesList)
            {
                if (bus.BusLineNumber == BusLineKey)
                {
                    busLinesList.Remove(bus);
                    return;
                }
            }
            throw new KeyNotFoundException("the bus number was not found");
        }

        /// <summary>
        /// Search a bus line in the collection by it's key and returns true if exist
        /// </summary>
        /// <param name="keyBusLine"></param>
        /// <returns></returns>
       public bool searchBusLine(int keyBusLine)
        {
            int count = 0;
            foreach (BusLine bus in busLinesList)
            {
                if (bus.BusLineNumber == keyBusLine)
                    count++;
            }
            if (count < 2)
                return true;
            return false;
        }


        /// <summary>
        /// Finds and returns a collection of all bus lines which stop in a given station
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        public BusLinesCollection BusLinesContainStation(int stationKey)
        {
            bool flag = false;
            BusLinesCollection tempCollection = new BusLinesCollection();
            foreach (BusLine bus in busLinesList)
            {
                foreach (BusLineStation station in bus.BusStationList)
                {
                    if (station.BusStopKey == stationKey)
                    {
                        tempCollection.busLinesList.Insert(0,bus);
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
                throw new KeyNotFoundException("there is no line buses for this station");
            return tempCollection;
        }


        /// <summary>
        /// Returns an index of a bus line in a collection
        /// </summary>
        /// <param name="busLineKey"></param>
        /// <returns></returns>
        public int searchIndex(int busLineKey)
        {
            for (int i = 0; i < busLinesList.Count; i++)
                if (busLinesList[i].BusLineNumber == busLineKey)
                    return i;
            throw new KeyNotFoundException("the bus line number was not found\n");
        }

        /// <summary>
        /// Sorts a collection of bus lines by travel time, using Icompareable, and returns the collection
        /// </summary>
        /// <returns></returns>
        public List<BusLine> SortBusLinesList()
        {
            List<BusLine> tempList = busLinesList;
            tempList.Sort();
            return tempList;
        }

        /// <summary>
        /// Index operator
        /// </summary>
        /// <param name="numBusLine"></param>
        /// <returns></returns>
        public BusLine this[int numBusLine]
        {
            get
            {
                int index = searchIndex(numBusLine);
                return busLinesList[index];
            }
            set
            {
                int index = searchIndex(numBusLine);
                busLinesList[index] = value;
            }

        }

        /// <summary>
        /// Returns true if a station exist in the bus collection
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        public bool staionExist(int stationKey)
        {           
            foreach (BusLine bus in busLinesList)
                foreach (BusLineStation station in bus.BusStationList)                
                    if (station.BusStopKey == stationKey)
                        return true;
            return false;           
        }
    }
}
