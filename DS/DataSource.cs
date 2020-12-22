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
                // 1
                new Bus
                {
                License=1324544,
                LicenseDate = DateTime.Parse("10.12.2016"),
                Mileage = 200030,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.12.2020"),
                MileageAtLastTreat = 200020,
                ObjectActive=true,
                },

                // 2
                new Bus
                {
                License=93029403,
                LicenseDate = DateTime.Parse("10.11.2019"),
                Mileage = 301010,
                Fuel = 0,
                BusStatus = Enums.BUS_STATUS.NEEDS_REFUEL,
                LastTreatmentDate = DateTime.Parse("10.12.2020"),
                MileageAtLastTreat = 300020,
                ObjectActive=true,
                },

                // 3 
                new Bus
                {
                License=63045719,
                LicenseDate = DateTime.Parse("10.3.2018"),
                Mileage = 130030,
                Fuel = 1100,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("11.4.2020"),
                MileageAtLastTreat = 130010,
                ObjectActive=true,
                },

                // 4 
                new Bus
                {
                License=7707707,
                LicenseDate = DateTime.Parse("10.1.2012"),
                Mileage = 500030,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.DANGEROUS,
                LastTreatmentDate = DateTime.Parse("10.12.2016"),
                MileageAtLastTreat = 300010,
                ObjectActive=true,
                },

                // 5 
                new Bus
                {
                License=11231123,
                LicenseDate = DateTime.Parse("21.2.2019"),
                Mileage = 2223,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("22.12.2020"),
                MileageAtLastTreat = 222,
                ObjectActive=true,
                },

                // 6 
                new Bus
                {
                License=32207541,
                LicenseDate = DateTime.Parse("10.3.2020"),
                Mileage = 15000,
                Fuel = 0,
                BusStatus = Enums.BUS_STATUS.NEEDS_REFUEL,
                LastTreatmentDate = DateTime.Parse("10.3.2020"),
                MileageAtLastTreat = 0,
                ObjectActive=true,
                },

                // 7 
                new Bus
                {
                License=1332465,
                LicenseDate = DateTime.Parse("10.12.1999"),
                Mileage = 933302,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.12.2018"),
                MileageAtLastTreat = 913999,
                ObjectActive=true,
                },

                // 8 
                new Bus
                {
                License=80412356,
                LicenseDate = DateTime.Parse("30.7.2018"),
                Mileage = 100500,
                Fuel = 695,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.11.2019"),
                MileageAtLastTreat = 85000,
                ObjectActive=true,
                },

                // 9
                new Bus
                {
                License=9360505,
                LicenseDate = DateTime.Parse("17.1.2004"),
                Mileage = 366666,
                Fuel = 1070,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("27.1.2020"),
                MileageAtLastTreat = 348999,
                ObjectActive=true,
                },

                // 10
                new Bus
                {
                License=10050823,
                LicenseDate = DateTime.Parse("3.4.2020"),
                Mileage = 1113,
                Fuel = 55,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.4.2020"),
                MileageAtLastTreat = 0,
                ObjectActive=true,
                },
                
                // 11
                new Bus
                {
                License=3025133,
                LicenseDate = DateTime.Parse("11.11.2011"),
                Mileage = 11134,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.12.2020"),
                MileageAtLastTreat = 11100,
                ObjectActive=true,
                },

                // 12
                new Bus
                {
                License=20021120,
                LicenseDate = DateTime.Parse("10.1.2018"),
                Mileage = 19320,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("9.3.2020"),
                MileageAtLastTreat = 2000,
                ObjectActive=true,
                },

                // 13
                new Bus
                {
                License=7820522,
                LicenseDate = DateTime.Parse("3.3.2013"),
                Mileage = 9999,
                Fuel = 950,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.1.2020"),
                MileageAtLastTreat = 8230,
                ObjectActive=true,
                },

                // 14
                new Bus
                {
                License=98769852,
                LicenseDate = DateTime.Parse("10.12.2019"),
                Mileage = 30520,
                Fuel = 200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("18.12.2020"),
                MileageAtLastTreat = 10520,
                ObjectActive=true,
                },

                // 15
                new Bus
                {
                License=9603250,
                LicenseDate = DateTime.Parse("10.12.1993"),
                Mileage = 2002030,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.12.2020"),
                MileageAtLastTreat = 1909996.3,
                ObjectActive=true,
                },

                // 16
                new Bus
                {
                License=96503201,
                LicenseDate = DateTime.Parse("11.11.2018"),
                Mileage = 19230,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("11.11.2020"),
                MileageAtLastTreat = 19200,
                ObjectActive=true,
                },

                // 17
                new Bus
                {
                License=7052033,
                LicenseDate = DateTime.Parse("12.7.2006"),
                Mileage = 999666,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("12.7.2020"),
                MileageAtLastTreat = 979666,
                ObjectActive=true,
                },

                // 18
                new Bus
                {
                License=10203055,
                LicenseDate = DateTime.Parse("23.3.2019"),
                Mileage = 23320,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("10.10.2020"),
                MileageAtLastTreat = 20000,
                ObjectActive=true,
                },

                // 19
                new Bus
                {
                License=9036541,
                LicenseDate = DateTime.Parse("5.5.2015"),
                Mileage = 19852,
                Fuel = 0,
                BusStatus = Enums.BUS_STATUS.NEEDS_REFUEL,
                LastTreatmentDate = DateTime.Parse("5.5.2020"),
                MileageAtLastTreat = 18999,
                ObjectActive=true,
                },

                // 20
                new Bus
                {
                License=12987053,
                LicenseDate = DateTime.Parse("11.11.2019"),
                Mileage = 11112,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate = DateTime.Parse("11.11.2020"),
                MileageAtLastTreat = 9992,
                ObjectActive=true,
                },

            };

            ListBusLines = new List<BusLine>
            {
                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 340,
                    Area = Enums.AREA.Center,
                    FirstBusStopKey = 60000,
                    LastBusStopKey = 20333,
                    ObjectActive=true,
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
                    ObjectActive=true,
                }

            };

            ListBusLineStations = new List<BusLineStation>
            {
                new BusLineStation
                {
                    BusLineID = ListBusLines[0].BusLineID,
                    BusStopKey = 60000,
                    LineStationIndex = 0,
                    PrevStation = 0,
                    NextStation = 21323,
                    ObjectActive=true,
                }
            };

            ListConsecutiveStations = new List<ConsecutiveStations>
            {
                new ConsecutiveStations
                {
                    BusStopKeyA = 60000,
                    BusStopKeyB = 21323,
                    Distance = 1.3,
                    TravelTime = TimeSpan.Parse("00:03:10"),
                     ObjectActive=true,
                }
            };

            ListBusAtTravels = new List<BusAtTravel>
            {
                new BusAtTravel
                {
                    BusAtTravelID = Config.RunningNumBusAtTravel,
                    License = 93029403,
                    BusLineNumberIdentifier = 101,
                    FormalDepartureTime = TimeSpan.Parse("10:20:00"),
                    ActualDepartureTime = TimeSpan.Parse("10:22:00"),
                    PrevBusLineStationNumber = 66033,
                    PrevStationArrivalTime = TimeSpan.Parse("10:15:00"),
                    NextStationArrivalTime = TimeSpan.Parse("10:30:00"),
                    BusDriverID = 310468558,
                    ObjectActive=true,
                }
            };

            ListLineDepartures = new List<LineDeparture>
             {
                 new LineDeparture
                 {
                    DepartureID = Config.RunningNumLineDeparture,
                    BusLineID = ListBusLines[0].BusLineID,
                    StartTime = TimeSpan.Parse("10:00:00"),
                    EndTime = TimeSpan.Parse("23:00:00"),
                    Frequency = 50,
                    ObjectActive=true,
                 }
             };

            ListUsers = new List<User>
              {
                  new User
                  {
                       UserName = "Maga4",
                       Password = "1234",
                       ManageAccess = false,
                       ObjectActive=true,
                  },

                  new User
                  {
                       UserName = "Director",
                       Password = "7890",
                       ManageAccess = true,
                       ObjectActive=true,
                  }
              };
        }
    }
}