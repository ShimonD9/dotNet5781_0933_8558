/*
 Exercise 2 - Mendi Ben Ezra (311140933), Shimon Dyskin (310468558)
 Description: The program manages a collection of bus lines, with bus stations,
offering to add, delete, search and print.
 ===Note: According to the lecturer we decided, that two round-trip lines would not pass through stations with the same code (even the first and the last ones, as it is in reality.)
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml.Schema;

namespace dotNet5781_02_0933_8558
{
  
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
            busLinesList.Insert(0, BusLine); //add new bus line
        }

        /// <summary>
        /// Removes a bus from a collection by it's key
        /// </summary>
        /// <param name="BusLineKey"></param>
        public void DeleteBusLine(int BusLineKey)       //delete bus line
        {
            foreach (BusLine bus in busLinesList)       //loop for finding the bus line
            {
                if (bus.BusLineNumber == BusLineKey)
                {
                    busLinesList.Remove(bus);
                    return;
                }
            }
            throw new KeyNotFoundException("The bus number was not found");       //in case the bus was not found
        }

        /// <summary>
        ///search if the are already two bus line with the same number
        /// </summary>
        /// <param name="keyBusLine"></param>
        /// <returns></returns>
       public bool SearchBusLine(int keyBusLine)
        {
            int count = 0;                              //for count the bus line
            foreach (BusLine bus in busLinesList)       //loop counting the bus line
            {
                if (bus.BusLineNumber == keyBusLine)
                    count++;                                
            }
            if (count < 2)                              //in case of alredy two bus line with the same number
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
            foreach (BusLine bus in busLinesList)           //loop for the buses
            {
                foreach (BusLineStation station in bus.BusStationsList)      //inner loop for stations of each bus
                {
                    if (station.BusStopKey == stationKey)
                    {
                        tempCollection.busLinesList.Insert(0,bus);      //in case staion found add the bus line to the collection
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)  //in case of not found station
                throw new KeyNotFoundException("There is no line buses for this station");
            return tempCollection;      //return the collection
        }


        /// <summary>
        /// Returns an index of a bus line in a collection
        /// </summary>
        /// <param name="busLineKey"></param>
        /// <returns></returns>
        public int SearchIndex(int busLineKey)
        {
            for (int i = 0; i < busLinesList.Count; i++)        //loop for find the index
                if (busLinesList[i].BusLineNumber == busLineKey)
                    return i;                       //return index
            throw new KeyNotFoundException("The bus line number was not found\n");  //in case index was not found
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
        public BusLine this[int numBusLine]   //Indexer
        {
            get
            {
                int index = SearchIndex(numBusLine);
                return busLinesList[index];
            }
            set
            {
                int index = SearchIndex(numBusLine);
                busLinesList[index] = value;
            }

        }

        /// <summary>
        /// Returns true if a station exist in the bus collection
        /// </summary>
        /// <param name="stationKey"></param>
        /// <returns></returns>
        public bool StaionExist(int stationKey)         //check for exsit station
        {           
            foreach (BusLine bus in busLinesList)       //loop for the bus
                foreach (BusLineStation station in bus.BusStationsList)          //inner loop for stations              
                    if (station.BusStopKey == stationKey)
                        return true;
            return false;           
        }
    }
}
