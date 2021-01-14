using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DO
{
    public class BusAtTravel
    {
        public int BusAtTravelID { get; set; } // Entity Identifier (Key) - Automatic running number
        public int License { get; set; }  // Entity Key A
        public int BusLineID { get; set; }  // Entity Key B

        private TimeSpan formalDepartureTime;
        public TimeSpan FormalDepartureTime // Entity Key C
        {
            get { return formalDepartureTime; }
            set { formalDepartureTime = value; }
        }
        [XmlElement("FormalDepartureTime", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlTime
        {
            get { return XmlConvert.ToString(formalDepartureTime); }
            set { formalDepartureTime = XmlConvert.ToTimeSpan(value); }
        }

        private TimeSpan actualDepartureTime;
        public TimeSpan ActualDepartureTime 
        {
            get { return actualDepartureTime; }
            set { actualDepartureTime = value; }
        }
        [XmlElement("ActualDepartureTime", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlTime1
        {
            get { return XmlConvert.ToString(actualDepartureTime); }
            set { actualDepartureTime = XmlConvert.ToTimeSpan(value); }
        }
        public int PrevBusLineStationNumber { get; set; }

        private TimeSpan prevStationArrivalTime;
        public TimeSpan PrevStationArrivalTime
        {
            get { return prevStationArrivalTime; }
            set { prevStationArrivalTime = value; }
        }
        [XmlElement("PrevStationArrivalTime", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlTime2
        {
            get { return XmlConvert.ToString(prevStationArrivalTime); }
            set { prevStationArrivalTime = XmlConvert.ToTimeSpan(value); }
        }
        private TimeSpan nextStationArrivalTime;
        public TimeSpan NextStationArrivalTime
        {
            get { return nextStationArrivalTime; }
            set { nextStationArrivalTime = value; }
        }
        [XmlElement("NextStationArrivalTime", DataType = "duration")]
        [DefaultValue("PT10M")]
        public string XmlTime3
        {
            get { return XmlConvert.ToString(nextStationArrivalTime); }
            set { nextStationArrivalTime = XmlConvert.ToTimeSpan(value); }
        }
        public int BusDriverID { get; set; }
        public bool ObjectActive { get; set; }
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
