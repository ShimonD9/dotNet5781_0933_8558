using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    public static class DataSource
    {
        public static List<Bus> ListBuses;
        public static List<BusAtTravel> ListBusAtTravels;
        public static List<BusLine> ListBusLines;
        public static List<BusLineStation> ListBusLineStations;
        public static List<BusStop> ListBusStops;
        public static List<ConsecutiveStations> ListConsecutiveStations;
        public static List<LineDeparture> ListLineDepartures;
        public static List<User> ListUsers;

        static DataSource()
        {
            InitAllLists();
        }

        static void InitAllLists()
        {
            ListBuses = new List<Bus>
            {
                new Bus
                {
              License="1234567",
                LicenseDate= 10.12.2016,
                },

                new Bus
                {
                    Name = "Yossi",
                    ID = 23,
                    Street = "Moshe Dayan",
                    HouseNumber = 145,
                    City = "Jerusalem",
                    PersonalStatus = PersonalStatus.SINGLE,
                    BirthDate = DateTime.Parse("13.10.95")
                }
            };

            ListBusAtTravels = new List<BusAtTravel>
            {
                new BusAtTravel
                {

                }
            };

            ListBusLines = new List<BusLine>
            {
                new BusLine
                {

                }
            };

            ListBusLineStations = new List<BusLineStation>
            {
                new BusLineStation
                {

                }
            };
            ListBusStops = new List<BusStop>
            {
                new BusStop
                {

                }

            }
            ListConsecutiveStations = new List<ConsecutiveStations>
            {
                new ConsecutiveStations
                {

                }
            }
             ListLineDepartures = new List<LineDeparture>
             {
                 new LineDeparture
                 {

                 }
             }
              ListUsers = new List<User>
              {
                  new User
                  {

                  }
              }
        }
    }
}