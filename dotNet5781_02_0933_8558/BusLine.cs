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
    class BusLine : IComparable
    {

        BusLine() {; }
        BusLine(int busLineKey, BusLineStation firstStationKey, BusLineStation secondStationKey, int areaKey)
        {
            FirstStation = firstStationKey;
            LastStation = secondStationKey;
            BusLineNumber = busLineKey;
            BusArea = (AREA)areaKey;
            //busStationList.Add(LastStation);
            // busStationList.Add(FirstStation);
            busStationList = new List<BusLineStation> { FirstStation, LastStation };
          
        }

        //public List<BusLineStation> busStationList;
        private List<BusLineStation> busStationList;
        public List<BusLineStation> BusStationList
        {
            get { return busStationList; }          
        }


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


        private AREA busArea;
        public AREA BusArea
        {
            get { return busArea; }
            set { busArea = value; }
        }

        private BusLineStation firstStation;
        public BusLineStation FirstStation
        {
            get { return firstStation; }
            set
            {
                firstStation = value;
            }
        }

        private BusLineStation lastStation;
        public BusLineStation LastStation
        {
            get { return lastStation; }
            set
            {
                lastStation = value;
            }
        }

        void addBusStation(int stationKey, int previousStationKey, double lati, double longi,
            string address, double distanceFromPreviousStation, double timeTravelFromPreviousStation, double distanceToNextStation, double timeTravelToNextStation)
        { 
            if (timeTravelToNextStation < 0 || distanceToNextStation < 0)
                throw new ArgumentException("Illegal input of minutes.");
            TimeSpan timeToNext = TimeSpan.FromMinutes(timeTravelFromPreviousStation);

            // If it's the first station
            if (previousStationKey == 0)
            {
                if (busStationList[0] == null)
                {
                    BusLineStation firstStation = new BusLineStation(0, 0, stationKey, address);
                    busStationList.Add(firstStation);
                }
                else if (busStationList[0] != null)
                {
                    busStationList[0].DistanceFromPreviousStation = distanceToNextStation;
                    busStationList[0].TravelTimeFromPreviousStation = timeToNext;
                    BusLineStation firstStation = new BusLineStation(0, 0, stationKey, address);
                    busStationList.Add(firstStation);
                }
            }
            // If needs to be put in the end
            else if (busStationList[busStationList.Count - 1].BusStationKey == previousStationKey)
            {
                BusLineStation lastStation = new BusLineStation(distanceFromPreviousStation, timeTravelFromPreviousStation, stationKey, address);
                busStationList.Insert(busStationList.Count - 1, lastStation);
            }
            // If needs to be put in the middle
            else
            {
                BusLineStation newStation = new BusLineStation(distanceFromPreviousStation, timeTravelFromPreviousStation, stationKey, address);
                BusLineStation previouStation = findStation(previousStationKey);
                int index = busStationList.IndexOf(previouStation);
                // Updates the travel and distance of the bus station ahead (based on a subtraction):
                busStationList[index + 1].DistanceFromPreviousStation = distanceToNextStation;
                busStationList[index + 1].TravelTimeFromPreviousStation = timeToNext;
            }
            throw new ArgumentException("the station was not found");
        }

        void deleteBusStation(int keyStation)
        {
            foreach (BusLineStation station in busStationList)
            {
                if (station.BusStationKey == keyStation)
                {
                    busStationList.Remove(station);
                    return;
                }
            }
            throw new ArgumentException("the station was not found");
        }

        BusLineStation findStation(int key)
        {
            foreach (BusLineStation station in busStationList)
            {
                if (station.BusStationKey == key)
                    return station;
            }
            throw new ArgumentException("the station was not found");
        }

        bool searchStation(int keyStation)
        {
            foreach (BusLineStation station in busStationList)
            {
                if (station.BusStationKey == keyStation)
                    return true;
            }
            return false;
        }

        double distanceStation(int keyA, int keyB)
        {
            BusLineStation firstStation = findStation(keyA);
            BusLineStation lastStation = findStation(keyB);
            int indexA = busStationList.IndexOf(firstStation);
            int indexB = busStationList.IndexOf(lastStation);
            if (indexA == -1)
            {
                throw new ArgumentException("the first station was not found");
            }
            else if (indexB == -1)
            {
                throw new ArgumentException("the last station was not found");

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

        TimeSpan timeTravel(int keyA, int keyB)
        {

            BusLineStation firstStation = findStation(keyA);
            BusLineStation lastStation = findStation(keyB);
            int indexA = busStationList.IndexOf(firstStation);
            int indexB = busStationList.IndexOf(lastStation);
            if (indexA == -1)
            {
                throw new ArgumentException("the first station was not found");
            }
            else if (indexB == -1)
            {
                throw new ArgumentException("the last station was not found");

            }
            TimeSpan total = new TimeSpan();
            bool flag = false;
            foreach (BusLineStation station in busStationList)
            {

                if (flag == false && station == firstStation)
                    flag = true;

                if (flag == true && station != lastStation)
                    total.Add(station.TravelTimeFromPreviousStation);

                else if (flag == true && station == lastStation)
                {
                    total.Add(station.TravelTimeFromPreviousStation);
                    break;
                }
            }
            return total;
        }

        BusLine track(int keyA, int keyB)
        {
            BusLineStation first = findStation(keyA);
            BusLineStation last = findStation(keyB);
            int indexA = busStationList.IndexOf(first);
            int indexB = busStationList.IndexOf(last);
            if (indexA == -1)
            {
                throw new ArgumentException("the first station was not found");
            }
            else if (indexB == -1)
            {
                throw new ArgumentException("the last station was not found");
            }
            else if (indexA < indexB)
            {
                throw new ArgumentException("the order of the stations is inccorect");
            }

            bool flag = false;
            BusLine trackList = new BusLine();
            foreach (BusLineStation station in busStationList)
            {

                if (flag == false && station == first)
                {
                    trackList.busStationList.Add(station);
                    flag = true;
                }
                if (flag == true && station != last)
                {
                    trackList.busStationList.Add(station);
                }
                else if (flag == true && station == last)
                {
                    trackList.busStationList.Add(station);
                    return trackList;
                }

            }
            throw new ArgumentException("the station was not found");
        }

        TimeSpan TotalTimeTravel()
        {
            TimeSpan totalTime = new TimeSpan();
            totalTime = timeTravel(this.firstStation.BusStationKey, this.lastStation.BusStationKey);
            return totalTime;
        }

        double TotalDistance()
        {
            double totalDistance = new double();
            foreach (BusLineStation station in busStationList)
            {
                totalDistance += station.DistanceFromPreviousStation;
            }
            return totalDistance;
        }

        public int CompareTo(object keyBus)
        {
            //TimeSpan busTimeA , busTimeB ;
            BusLine compareBusLine = (BusLine)keyBus;
            // busTimeA = this.TotalTimeTravel();
            //busTimeB = this.TotalTimeTravel();

            return TotalTimeTravel().CompareTo(compareBusLine.TotalTimeTravel());
        }

        public override string ToString()
        {
            string stations = null;
            foreach (BusLineStation item in busStationList)
            {
                stations += item.BusStationKey + '\n'; 
            }
            return string.Format("Bus line details:\n+" +
                                  "Bus line = {0},Aera line = {1}, busStationList: = {2}",
                                  BusLineNumber, busArea, stations);
        }
    }
}
