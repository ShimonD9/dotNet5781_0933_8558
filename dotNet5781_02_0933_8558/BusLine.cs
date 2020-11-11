using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.CodeDom.Compiler;
using System.Runtime.Remoting.Messaging;

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
            //busStationList.Add(LastStation);
            // busStationList.Add(FirstStation);
            busStationList = new List<BusLineStation> { FirstStation, LastStation };

        }

        // List of bus line stations, with get property:
        private List<BusLineStation> busStationList;

        public List<BusLineStation> BusStationList
        {
            get { return busStationList; }
        }

        // Bus line number, with get & set property:
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
            if (prevKey == 0)
            {
                FirstStation = newStation;
                busStationList[0].DistanceFromPreviousStation = distanceToNextStation;
                busStationList[0].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(timeToNextStation);
                busStationList.Insert(0, newStation);
                busStationList[0].DistanceFromPreviousStation = 0;
                busStationList[0].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(0);
            }
            else if (prevKey > 0)
            {
                BusLineStation tempStation = findStation(prevKey);
                int index = busStationList.IndexOf(tempStation);
                busStationList[index].DistanceFromPreviousStation = distanceToNextStation;
                busStationList[index].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(timeToNextStation);
                busStationList.Insert(index, newStation);
            }
        }

        /// <summary>
        /// Add bus station to the end of the list
        /// </summary>
        /// <param name="newStation"></param>
        public void addBusStationToTheEnd(BusLineStation newStation)
        {
            LastStation = newStation;
            busStationList.Add(newStation);
        }

        /// <summary>
        /// Deletes bus station based on it's key
        /// </summary>
        /// <param name="keyStation"></param>
        void deleteBusStation(int keyStation)
        {
            foreach (BusLineStation station in busStationList)
            {
                if (station.BusStopKey == keyStation)
                {
                    busStationList.Remove(station);
                    return;
                }
            }
            throw new KeyNotFoundException("the station was not found");
        }


        /// <summary>
        /// Finds and returns bus line station based on it's key
        /// </summary>
        /// <param name="keyStation"></param>
        BusLineStation findStation(int key)
        {
            foreach (BusLineStation station in busStationList)
            {
                if (station.BusStopKey == key)
                    return station;
            }
            throw new KeyNotFoundException("the station was not found");
        }

        /// <summary>
        /// Search if bus line station exist based on it's key
        /// </summary>
        /// <param name="keyStation"></param>
        /// <returns></returns>
        bool searchStation(int keyStation)
        {
            foreach (BusLineStation station in busStationList)
            {
                if (station.BusStopKey == keyStation)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Calculates the distance between two bus stations, based on their keys
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        double distanceStation(int keyA, int keyB)
        {
            BusLineStation firstStation = findStation(keyA);
            BusLineStation lastStation = findStation(keyB);
            int indexA = busStationList.IndexOf(firstStation);
            int indexB = busStationList.IndexOf(lastStation);
            if (indexA == -1)
            {
                throw new KeyNotFoundException("the first station was not found");
            }
            else if (indexB == -1)
            {
                throw new KeyNotFoundException("the last station was not found");

            }
            double total = 0;
            bool flag = false;
            foreach (BusLineStation station in busStationList)
            {

                if (flag == false && station == firstStation)
                    flag = true;

                if (flag == true && station != lastStation)
                    total += station.DistanceFromPreviousStation;

                else if (flag == true && station == lastStation)
                {
                    total += station.DistanceFromPreviousStation;
                    break;
                }
            }
            return total;
        }

        /// <summary>
        /// Calculates the time travel difference between two stations, based on their keys
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        TimeSpan timeTravel(int keyA, int keyB)
        {

            BusLineStation firstStation = findStation(keyA);
            BusLineStation lastStation = findStation(keyB);
            int indexA = busStationList.IndexOf(firstStation);
            int indexB = busStationList.IndexOf(lastStation);
            if (indexA == -1)
            {
                throw new KeyNotFoundException("the first station was not found");
            }
            else if (indexB == -1)
            {
                throw new KeyNotFoundException("the last station was not found");

            }
            TimeSpan total = new TimeSpan();
            bool flag = false;
            foreach (BusLineStation station in busStationList)
            {

                if (flag == false && station == firstStation)
                    flag = true;

                if (flag == true && station != lastStation)
                    total.Add(station.TimeTravelFromPreviousStation);

                else if (flag == true && station == lastStation)
                {
                    total.Add(station.TimeTravelFromPreviousStation);
                    break;
                }
            }
            return total;
        }

        /// <summary>
        /// Returns a new bus line, which contains the track between two given bus station keys
        /// </summary>
        /// <param name="keyA"></param>
        /// <param name="keyB"></param>
        /// <returns></returns>
        BusLine track(int keyA, int keyB)
        {
            BusLineStation first = findStation(keyA);
            BusLineStation last = findStation(keyB);
            int indexA = busStationList.IndexOf(first);
            int indexB = busStationList.IndexOf(last);
            if (indexA == -1)
            {
                throw new KeyNotFoundException("the first station was not found");
            }
            else if (indexB == -1)
            {
                throw new KeyNotFoundException("the last station was not found");
            }
            else if (indexA < indexB)
            {
                throw new KeyNotFoundException("the order of the stations is inccorect");
            }

            bool flag = false;
            BusLine trackList = new BusLine();
            foreach (BusLineStation station in busStationList)
            {

                if (flag == false && station == first)
                {
                    trackList.busStationList.Insert(0, station);
                    flag = true;
                }
                if (flag == true && station != last)
                {
                    trackList.busStationList.Insert(0, station);
                }
                else if (flag == true && station == last)
                {
                    trackList.busStationList.Insert(0, station);
                    return trackList;
                }

            }
            throw new ArgumentException("the station was not found");
        }

        /// <summary>
        /// Calculates the total time of ride for a bus line
        /// </summary>
        /// <returns></returns>
        TimeSpan TotalTimeTravel()
        {
            TimeSpan totalTime = new TimeSpan();
            totalTime = timeTravel(this.firstStation.BusStopKey, this.lastStation.BusStopKey);
            return totalTime;
        }

        /// <summary>
        /// Calculates the total distance of ride for a bus line
        /// </summary>
        /// <returns></returns>
        double TotalDistance()
        {
            double totalDistance = new double();
            foreach (BusLineStation station in busStationList)
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
            //TimeSpan busTimeA , busTimeB ;
            BusLine compareBusLine = (BusLine)keyBus;
            // busTimeA = this.TotalTimeTravel();
            //busTimeB = this.TotalTimeTravel();

            return TotalTimeTravel().CompareTo(compareBusLine.TotalTimeTravel());
        }

        /// <summary>
        /// Overrides the ToString function, returning info on the bus line
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string stations = null;
            foreach (BusLineStation item in busStationList)
            {
                stations += item.ToString() + " \n";
            }
            return string.Format("Bus line details:\n+" +
                                  "Bus line = {0}, Area line = {1}, busStationList: =\n{2}",
                                  BusLineNumber, busArea, stations);
        }

    }
}