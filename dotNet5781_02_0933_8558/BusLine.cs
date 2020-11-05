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
        private int firstStation;
        private string area;

        public string Area
        {
            get { return area; }
            set { area = value; }
        }

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
        
        void addBusStation(int keyStation, int existingStationKey, double lati, double longi,
            string address, double dis, int timeTravel,double distanceToNextStation)
        {
            
            if (busStationList[0] == null || existingStationKey == 0)
            {
                BusLineStation firstStation = new BusLineStation(dis, timeTravel, lati, longi, keyStation, address);
                busStationList.Add(firstStation);
                return;//לעדכן את התחנה הבאה במרחק החדש
            }
            else if (busStationList[busStationList.Count - 1].BusStationKey == existingStationKey)
            {
                //write code //in case the new station to insert is in the end
            }
            else
            {
                BusLineStation middleStation = new BusLineStation(dis, timeTravel, lati, longi, keyStation, address);
                BusLineStation findIndexStation = findStation(existingStationKey);
                int index = busStationList.IndexOf(findIndexStation);
                busStationList.Insert(index, middleStation);
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
        double distanceStation(int first, int seconde)
        {
            BusLineStation firstStation = findStation(first);
            BusLineStation secondeStation = findStation(seconde);
            return Math.Abs(firstStation.DistanceFromLastStation - secondeStation.DistanceFromLastStation);
        }
        BusLine track(BusLineStation first, BusLineStation seconde)
        {

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
