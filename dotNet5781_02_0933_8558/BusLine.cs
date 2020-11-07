using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.CodeDom.Compiler;

namespace dotNet5781_02_0933_8558
{
    class BusLine 
    {
        public List<BusLineStation> busStationList = new List<BusLineStation> { };
        private int busLineNumber;
        public int BusLineNmber
        {
            get { return busLineNumber; }
            set
            {
                if (busLineNumber < 0)
                    throw new ArgumentException("Illegal bus line number.");
                busLineNumber = value;
            }
        }

        private string area;

        public string Area
        {
            get { return area; }
            set { area = value; }
        }

        private int firstStation;
        public int FirstStation
        {
            get { return firstStation; }
            set
            {
                if (firstStation < 0)
                    throw new ArgumentException("Illegal input of first station.");
                firstStation = value;
            }
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

      
        void addBusStation(int stationKey, int previousStationKey, double lati, double longi,
            string address, double distanceFromPreviousStation, int timeTravelFromPreviousStation)
        {
            if (busStationList[0] == null || previousStationKey == 0)
            {
                BusLineStation firstStation = new BusLineStation(distanceFromPreviousStation, timeTravelFromPreviousStation, lati, longi, stationKey, address);
                busStationList.Add(firstStation);
                return;//לעדכן את התחנה הבאה במרחק החדש
            }
            else if (busStationList[busStationList.Count - 1].BusStationKey == previousStationKey)
            {
                BusLineStation lastStation = new BusLineStation(distanceFromPreviousStation, timeTravelFromPreviousStation, lati, longi, stationKey, address);
                busStationList.Insert(busStationList.Count - 1, lastStation);
            }
            else
            {
                BusLineStation newStation = new BusLineStation(distanceFromPreviousStation, timeTravelFromPreviousStation, lati, longi, stationKey, address);
                BusLineStation previouStation = findStation(previousStationKey);
                int index = busStationList.IndexOf(previouStation);
                busStationList[index + 1].DistanceFromLastStation -= distanceFromPreviousStation;

            }
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
                    total += station.DistanceFromLastStation;

                else if (flag == true && station == lastStation)
                {
                    total += station.DistanceFromLastStation;
                    return total;
                }
            }
            return 0;
        }

        double timeTravel(int keyA, int keyB)
        {


        }
        BusLine track(int keyA, int keyB)
        {
            BusLineStation first = findStation(keyA);
            BusLineStation last = findStation(keyB);
            int indexA = busStationList.IndexOf(first);
            int indexB = busStationList.IndexOf(last);
            if(indexA == -1)
            {
                throw new ArgumentException("the first station was not found");
            }
            else if (indexB == -1)
            {
                throw new ArgumentException("the last station was not found");
            }
            else if(indexA < indexB)
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





        public override string ToString()
        {
            foreach (BusLine in busStationList)
            {

            }
            return string.Format("Bus line details:\n+" +
                                  "Bus line = {0},Aera line = {1}, Longitude = {2},Station List{3}", BusLineNmber, Area, busStationList);
        }
    }

}
