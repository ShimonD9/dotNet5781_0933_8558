using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusAtTravel
    {
        public int BusAtTravelID { get; set; } // Entity Identifier (Key) - Automatic running number
        public int License { get; set; }  // Entity Key A
        public int BusLineID { get; set; }  // Entity Key B
        public TimeSpan FormalDepartureTime { get; set; }  // Entity Key C
        public TimeSpan ActualDepartureTime { get; set; }
        public int PrevBusLineStationNumber { get; set; }
        public TimeSpan PrevStationArrivalTime { get; set; }
        public TimeSpan NextStationArrivalTime { get; set; }
        public bool ObjectActive { get; set; }
        public int BusDriverID { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Identifier= {0},License= {1},Bus Line Number Identifier = {2},Formal Departure Time = {3}, Actual Departure Time = {4}, Prev Bus Line Station Number = {5}, Prev Station Arrival Time = {6}, Next Station Arrival Time = {7}, Bus DriverID = {8}",
                BusAtTravelID, License, BusLineID, FormalDepartureTime, ActualDepartureTime, PrevBusLineStationNumber, PrevStationArrivalTime, NextStationArrivalTime, BusDriverID);
        }

    }
}
