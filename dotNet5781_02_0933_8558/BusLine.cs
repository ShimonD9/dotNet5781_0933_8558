/*
 Exercise 2 - Mendi Ben Ezra (311140933), Shimon Dyskin (310468558)
 Description: The program manages a collection of bus lines, with bus stations,
offering to add, delete, search and print.
 ===Note: According to the lecturer we decided, that two round-trip lines would not pass through stations with the same code (even the first and the last ones, as it is in reality.
*/
using System;
using System.Collections.Generic;

namespace dotNet5781_02_0933_8558
{

    public class BusLine : IComparable
    {
        /// <summary>
        /// Empty bus line constructor
        /// </summary>
        BusLine() { }

        /// <summary>
        /// Bus Line Constructor
        /// </summary>
        /// <param name="busLineKey"></param>
        /// <param name="firstStation"></param>
        /// <param name="secondStation"></param>
        /// <param name="areaKey"></param>
        public BusLine(int busLineKey, BusLineStation firstStation, BusLineStation secondStation, int areaKey)
        {
            FirstStation = firstStation;
            LastStation = secondStation;
            BusLineNumber = busLineKey;
            BusArea = (AREA)areaKey;
            busStationList = new List<BusLineStation> { FirstStation, LastStation };

        }

        // List of bus line stations, with get property:
        private List<BusLineStation> busStationList;

        public List<BusLineStation> BusStationList
        {
            get { return busStationList; }
        }

        /// <summary>
        /// // Bus line number, with get & set property:
        /// </summary>
        private int busLineNumber;
        public int BusLineNumber
        {
            get { return busLineNumber; }
            set
            {
                if (busLineNumber < 0)
                    throw new ArgumentException("Illegal bus line number.");
                busLineNumber = value;
            }
        }

        // Bus area of AREA Enum, with get & set property:
        private AREA busArea;
        public AREA BusArea
        {
            get { return busArea; }
            set { busArea = value; }
        }

        // First station, with get & set property:
        private BusLineStation firstStation;
        public BusLineStation FirstStation
        {
            get { return firstStation; }
            set
            {
                firstStation = value;
            }
        }

        // Last station, with get & set property:
        private BusLineStation lastStation;
        public BusLineStation LastStation
        {
            get { return lastStation; }
            set
            {
                lastStation = value;
            }
        }

        /// <summary>
        /// Add bus station to the list
        /// </summary>
        /// <param name="newStation"></param>
        /// <param name="prevKey"></param>
        /// <param name="distanceToNextStation"></param>
        /// <param name="timeToNextStation"></param>
        public void addBusStation(BusLineStation newStation, int prevKey, double distanceToNextStation, double timeToNextStation)
        {
            if (prevKey == 0)                       //in case of first station in the bus
            {
                FirstStation = newStation;          //update the first station of the bus
                busStationList[0].DistanceFromPreviousStation = distanceToNextStation;      //input details of distance
                busStationList[0].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(timeToNextStation);      //input details of time travel (using time span class)
                busStationList.Insert(0, newStation);       //insert the station to the bus
            }
            else if (prevKey > 0)                    //in case of insert station in the midddle
            {
                BusLineStation tempStation = findAndReturnStation(prevKey);     //create a temp station to insert
                int index = busStationList.IndexOf(tempStation);                //find the index for the new station
                busStationList[index + 1].DistanceFromPreviousStation = distanceToNextStation;      //update the previous station distance
                busStationList[index + 1].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(timeToNextStation);      //update the previous station time travel
                busStationList.Insert(index + 1, newStation);           //insert the station to the bus
            }
        }

        /// <summary>
        /// Add bus station to the end of the list
        /// </summary>
        /// <param name="newStation"></param>
        public void addBusStationToEnd(BusLineStation newStation)       //in case of insert station to the end
        {
            LastStation = newStation;                                   //update the new last station
            busStationList.Add(newStation);                             //add the station to the end
        }

        /// <summary>
        /// Deletes bus station based on it's key
        /// </summary>
        /// <param name="keyStation"></param>
        public void deleteBusStation(int keyStation)        //delete station
        {

            foreach (BusLineStation station in busStationList)      //loop for finding the key of the station to delete
            {
                if (station.BusStopKey == keyStation)       //in case the key was found 
                {
                    busStationList.Remove(station);         //remove the station
                    return;                                 //out of the loop
                }
            }
            throw new KeyNotFoundException("the station was not found");        //in case the key was not found
        }


        /// <summary>
        /// Finds and returns bus line station based on it's key
        /// </summary>
        /// <param name="keyStation"></param>
        public BusLineStation findAndReturnStation(int key)         //found the station by key and return the object
        {
            foreach (BusLineStation station in busStationList)      //looop for finding the key
            {
                if (station.BusStopKey == key)                      //in case the key was found
                    return station;                                 //return the station
            }
            return null;                                            //in case of the station was not found
        }

        /// <summary>
        /// Search if bus line station exist based on it's key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool stationExist(int key)                       //checks if station exist in the busline
        {
            foreach (BusLineStation station in busStationList)  //loop for checking if the station exist
            {
                if (station.BusStopKey == key)
                    return true;                                //in case of existing station
            }
            return false;                                        //in case the station was not found
        }

        /// <summary>
        /// Calculates the distance between two bus stations, based on their keys
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        /// 
        public double distanceStation(int keyA, int keyB)           //check distance between two stations
        {
            BusLineStation firstStation = findAndReturnStation(keyA);     //create the first station with keyA
            BusLineStation lastStation = findAndReturnStation(keyB);      //create the first station with keyB
            int indexA = busStationList.IndexOf(firstStation);      //find the index of first station
            int indexB = busStationList.IndexOf(lastStation);       //find the index of second station
            if (indexA == -1)           //in case first station was not found
            {
                throw new KeyNotFoundException("the first station was not found");
            }
            else if (indexB == -1)      //in case second station was not found
            {
                throw new KeyNotFoundException("the last station was not found");

            }
            else if (indexA > indexB)       //in case of oppiset input of station
            {
                throw new KeyNotFoundException("the order of the stations is inccorect");
            }
            double total = 0;           //create total distance
            bool flag = false;          //flag for known when the first station of the distance was found
            foreach (BusLineStation station in busStationList)          //loop for finding the station, and sum the total distance 
            {

                if (flag == false && station == firstStation)       //first station found
                    flag = true;

                if (flag == true && station != lastStation)         //as long we didnet get to the second station 
                    total += station.DistanceFromPreviousStation;   //sum the disctance

                else if (flag == true && station == lastStation)    //in case we got the second station
                {
                    total += station.DistanceFromPreviousStation;
                    break;                                          //out of the loop
                }
            }
            return total;                                   //return the sum
        }
        /// <summary>
        /// find and return the index of a specific station
        /// </summary>
        /// <param name="busStationKey"></param>
        /// <returns></returns>
        public int findIndexStation(int busStationKey)
        {
            for (int i = 0; i < busStationList.Count; i++)                           //loop for find the station
                if (busStationList[i].BusStopKey == busStationKey)      
                    return i;                                                       //return index
            throw new ArgumentException("the station number was not found\n");   //in case index was not found
        }

        /// <summary>
        /// Calculates the time travel difference between two stations, based on their keys
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public TimeSpan timeTravel(int keyA, int keyB)
        {
            BusLineStation firstStation = findAndReturnStation(keyA);     //create the first station with keyA
            BusLineStation lastStation = findAndReturnStation(keyB);      //create the first station with keyB
            int indexA = busStationList.IndexOf(firstStation);            //find the index of first station
            int indexB = busStationList.IndexOf(lastStation);             //find the index of second station  

            if (indexA == -1)                           //in case second station was not found
            {
                throw new KeyNotFoundException("the first station was not found");
            }
            else if (indexB == -1)                      //in case second station was not found
            {
                throw new KeyNotFoundException("the last station was not found");
            }
            else if (indexA > indexB)                   //in case of oppiset input of station
            {
                throw new ArgumentException("the order of the stations is inccorect");
            }

            TimeSpan total = new TimeSpan();    //create total time
            bool flag = false;               //flag for known when the first station of the time was found

            foreach (BusLineStation station in busStationList)              //loop for finding the station, and sum the total time
            {
                if (flag == false && station == firstStation)                //first station found
                    flag = true;
                else if (flag == true && station != lastStation)                //as long we didnet get to the second station 
                    total = total.Add(station.TimeTravelFromPreviousStation);       //sum the time
                else if (flag == true && station == lastStation)
                {
                    total = total.Add(station.TimeTravelFromPreviousStation);       //in case we got the second station
                    break;                                                          //out of the loop
                }
            }

            return total;                                                           //retunr sum
        }

        /// <summary>
        /// Returns a new bus line, which contains the track between two given bus station keys
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        public BusLine Track(int keyA, int keyB)
        {
            BusLineStation first = findAndReturnStation(keyA);      //create the first station with keyA
            BusLineStation last = findAndReturnStation(keyB);       //create the first station with keyB
            int indexA = busStationList.IndexOf(first);             //find the index of first station
            int indexB = busStationList.IndexOf(last);              //find the index of second station 

            if (indexA == -1)               //in case first station was not found
            {
                throw new KeyNotFoundException("the first station was not found");
            }
            else if (indexB == -1)          //in case second station was not found
            {
                throw new KeyNotFoundException("the last station was not found");
            }
            else if (indexA > indexB)        //in case of oppiset input of station
            {
                throw new KeyNotFoundException("the order of the stations is inccorect");
            }

            int index = 0;
            bool flag = false;          //flag for known when the first station of the track was found
            BusLine trackList = new BusLine(this.busLineNumber, first, last, (int)this.BusArea);       //create the first station of the new bus track
            foreach (BusLineStation station in busStationList)
            {
                if (flag == false && station == first) // Means we find the first bus station of the track
                {
                    flag = true;
                }
                else if (flag == true && station != last) // Adding the bus stations between
                {
                    trackList.busStationList.Insert(++index, station);
                }
                else if (flag == true && station == last) // Means we find the last bus station of the track and can stop the loop
                {
                    return trackList;
                }
            }
            return null;                    //in case nothing found
        }

        /// <summary>
        /// Calculates the total time of ride for a bus line
        /// using "time travel" function
        /// </summary>
        /// <returns></returns>
        public TimeSpan TotalTimeTravel()       
        {
            TimeSpan totalTime = timeTravel(this.firstStation.BusStopKey, this.lastStation.BusStopKey);
            return totalTime;
        }

        /// <summary>
        /// Calculates the total distance of ride for a bus line
        /// </summary>
        /// <returns></returns>
        public double TotalDistance()
        {
            double totalDistance = new double();
            foreach (BusLineStation station in busStationList)      //loop for sum the distance
            {
                totalDistance += station.DistanceFromPreviousStation;
            }
            return totalDistance;
        }

        /// <summary>
        ///  Compare-to functions compares between the total time travel of two bus lines
        /// </summary>
        /// <param name="keyBus"></param>
        /// <returns></returns>
        public int CompareTo(object keyBus)
        {
           
            BusLine compareBusLine = (BusLine)keyBus;
            return TotalTimeTravel().CompareTo(compareBusLine.TotalTimeTravel());
        }

        /// <summary>
        /// Overrides the ToString function, returning info on the bus line
        /// </summary>
        /// <returns></returns>
        public override string ToString()        //pritting details of a bus
        {
            string stations = null;
            foreach (BusLineStation item in busStationList)         //loop for printing the all stations
            {
                stations += item.BusStopKey + ", ";
            }
            stations = stations.Remove(stations.Length - 2);
            return string.Format("Bus line = {0, -7} Area line = {1, -11} Stops at next stations: {2, -14}",
                                  BusLineNumber, busArea, stations);
        }

    }
}