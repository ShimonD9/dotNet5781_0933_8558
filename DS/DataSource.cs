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
                License="1324544",
                LicenseDate = DateTime.Parse("10.12.2016"),
                Mileage = 200030,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.12.2020"),
                MileageAtLastTreat = 200020,
                },

                new Bus
                {
                License="93029403",
                LicenseDate = DateTime.Parse("10.11.2019"),
                Mileage = 301010,
                Fuel = 0,
                BusStatus = Enums.BUS_STATUS.NEEDS_REFUEL,
                LastTreatmentDate = DateTime.Parse("10.12.2020"),
                MileageAtLastTreat = 300020,
                }
            };

            ListBusAtTravels = new List<BusAtTravel>
            {
                new BusAtTravel
                {
                    BusIdentifier = 0,
                    License = "93029403",
                    BusLineNumberIdentifier = 101,
                    FormalDepartureTime = TimeSpan.Parse("10:20:00"),
                    ActualDepartureTime = TimeSpan.Parse("10:22:00"),
                    PrevBusLineStationNumber = 66033,
                    PrevStationArrivalTime = TimeSpan.Parse("10:15:00"),
                    NextStationArrivalTime = TimeSpan.Parse("10:30:00"),
                    BusDriverID = 310468558,
                }
            };

            ListBusLines = new List<BusLine>
            {
                new BusLine
                {
                    BusLineIdentifier = 0,
                    BusLineNumber = 340,
                    Area = Enums.AREA.Center,
                    FirstBusStopKey = 60000,
                    LastBusStopKey = 20333,
                }
            };

            ListBusLineStations = new List<BusLineStation>
            {
                new BusLineStation
                {
                    BusLineID = 0,
                    BusStopKey = 60000,
                    LineStationIndex = 0,
                    PrevStation = 0,
                    NextStation = 21323
                }
            };
            ListBusStops = new List<BusStop>
            {
                new BusStop
                {
                    BusStopKey = 60000,
                    BusStopName = "גן ילדים שושנה",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                }

            };

            ListConsecutiveStations = new List<ConsecutiveStations>
            {
                new ConsecutiveStations
                {
                    BusStopKeyA = 60000,
                    BusStopKeyB = 21323,
                    Distance = 1.3,
                    TravelTime = TimeSpan.Parse("00:03:10")
                }
            };

            ListLineDepartures = new List<LineDeparture>
             {
                 new LineDeparture
                 {
                    DepartureID = 0,
                    BusLineID = 0,
                    StartTime = TimeSpan.Parse("10:00:00"),
                    EndTime = TimeSpan.Parse("23:00:00"),
                    Frequency = 50,
                 }
             };

            ListUsers = new List<User>
              {
                  new User
                  {
                       UserName = "Maga4",
                       Password = "Matate6",
                       ManageAccess = false,
                  },

                  new User
                  {
                       UserName = "Director",
                       Password = "Menael10",
                       ManageAccess = true,
                  }
              };
        }
    }
}