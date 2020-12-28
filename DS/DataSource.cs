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

            ListBusStops = new List<BusStop>
            {
                new BusStop
                {
                    BusStopKey = 26475,
                    BusStopName = "חזון אי''ש/מרום נווה",
                    BusStopAddress = "חזון אי''ש 90 בני ברק",
                    Latitude = 32.07311,
                    Longitude = 34.832921,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 20150,
                    BusStopName = "אבו חצירא/הרב פתאיה",
                    BusStopAddress = "הרב ישראל אבו חצירא 24 בני ברק",
                    Latitude = 32.095152,
                    Longitude = 34.830981,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60204,
                    BusStopName = "משך חוכמה ז",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = false,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60278,
                    BusStopName = "רשב''י א",
                    BusStopAddress = "רבי שמעון בר יוחאי 47 מודיעין עילית",
                    Latitude = 31.937852,
                    Longitude = 35.045995,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 20164,
                    BusStopName = "אזור התעשייה",
                    BusStopAddress = "מבצע קדש בני ברק",
                    Latitude = 32.10235,
                    Longitude = 34.827827,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60403,
                    BusStopName = "מעלות החכמה",
                    BusStopAddress = "רשי מודיעין עילית",
                    Latitude = 31.930906,
                    Longitude = 35.049892,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 21165,
                    BusStopName = "מגדל ויטה/דרך בן גוריון",
                    BusStopAddress = "דרך בן גוריון 11 בני ברק",
                    Latitude = 32.094143,
                    Longitude = 34.822193,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 4009,
                    BusStopName = "צומת רמות/גולדה",
                    BusStopAddress = "שדרות גולדה מאיר ירושלים",
                    Latitude = 31.808253,
                    Longitude = 35.204001,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 61416,
                    BusStopName = "רמב''ן/שדרות בית שמאי",
                    BusStopAddress = "רמב''ן מודיעין עילית",
                    Latitude = 31.934343,
                    Longitude = 35.052361,
                    Sunshade = false,
                    DigitalPanel = false,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60817,
                    BusStopName = "שדרות בית שמאי/שדרות בית הלל",
                    BusStopAddress = "שדרות בית שמאי 1 מודיעין עילית",
                    Latitude = 31.934442,
                    Longitude = 35.050766,
                    Sunshade = true,
                    DigitalPanel = false,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60226,
                    BusStopName = "אבני נזר/אור החיים",
                    BusStopAddress = "אבני נזר 32 מודיעין עילית",
                    Latitude = 31.931486,
                    Longitude = 35.03977,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 65411,
                    BusStopName = "אבי עזרי ז",
                    BusStopAddress = "אבי עזרי 28 מודיעין עילית",
                    Latitude = 31.923308,
                    Longitude = 35.053342,
                    Sunshade = true,
                    DigitalPanel = false,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 63988,
                    BusStopName = "מסילת ישרים/דרך קרית ספר",
                    BusStopAddress = "מסילת ישרים 1 מודיעין עילית",
                    Latitude = 31.935677,
                    Longitude = 35.037737,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 61378,
                    BusStopName = "אבי עזרי/מסוף",
                    BusStopAddress = "אבי עזרי 32 מודיעין עילית",
                    Latitude = 31.923647,
                    Longitude = 35.054196,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 12284,
                    BusStopName = "שדרות הפלמח/חטיבת כרמלי",
                    BusStopAddress = "שדרות הפלמ''ח 2 אשדוד",
                    Latitude = 31.804129,
                    Longitude = 34.660742,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60226,
                    BusStopName = "אבני נזר/אור החיים",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60227,
                    BusStopName = "מרכז קסם/אבני נזר",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60228,
                    BusStopName = "חפץ חיים/קצות חושן",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60229,
                    BusStopName = "חפץ חיים ז",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60233,
                    BusStopName = "מסילת יוסף ב",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60234,
                    BusStopName = "מסילת יוסף/מרומי שדה",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60237,
                    BusStopName = "גן ילדים/נתיבות המשפט",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60238,
                    BusStopName = "נתיבות המשפט ח",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60239,
                    BusStopName = "נתיבות המשפט י''ג",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60240,
                    BusStopName = "נתיבות המשפט/מסילת יוסף",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60241,
                    BusStopName = "נתיבות המשפט ז",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60243,
                    BusStopName = "כיכר בית תפילה/נתיבות המשפט",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60244,
                    BusStopName = "נתיבות המשפט ה",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60245,
                    BusStopName = "נתיבות המשפט י''ז",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60247,
                    BusStopName = "נתיבות המשפט/כיכר גפן",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60248,
                    BusStopName = "נתיבות המשפט/מרומי שדה",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60249,
                    BusStopName = "נתיבות המשפט ל",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60250,
                    BusStopName = "נתיבות המשפט כ",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60252,
                    BusStopName = "נתיבות המשפט/חת''ם סופר",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60254,
                    BusStopName = "חפץ חיים/אבני נזר",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60255,
                    BusStopName = "שדרות בית הלל/שדרות בית שמאי",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60256,
                    BusStopName = "אבי עזרי ב",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60257,
                    BusStopName = "אבי עזרי א",
                    BusStopAddress = "אבי עזרי 9 מודיעין עילית",
                    Latitude = 31.923385,
                    Longitude = 35.053287,
                    Sunshade = true,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60258,
                    BusStopName = "כיכר נאות הפסגה/רש''י",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60261,
                    BusStopName = "חפץ חיים ג",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60262,
                    BusStopName = "חפץ חיים/קצות חושן",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60263,
                    BusStopName = "חפץ חיים/אבני נזר",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60266,
                    BusStopName = "מסילת ישרים ג",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60268,
                    BusStopName = "מרכז מסחרי/שדרות יחזקאל",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60270,
                    BusStopName = "צומת אלעזר",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                new BusStop
                {
                    BusStopKey = 60271,
                    BusStopName = "שד. יחזקאל/רב ושמואל",
                    BusStopAddress = "יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 33.212,
                    Longitude = 33.32,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },




            };

            ListBusLines = new List<BusLine>
            {
                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 210,
                    Area = Enums.AREA.Center,
                    FirstBusStopKey = 26475,
                    LastBusStopKey = 60204,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 220,
                    Area = Enums.AREA.Center,
                    FirstBusStopKey = 65299,
                    LastBusStopKey = 20150,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 230,
                    Area = Enums.AREA.Center,
                    FirstBusStopKey = 60278,
                    LastBusStopKey = 20163,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 240,
                    Area = Enums.AREA.Center,
                    FirstBusStopKey = 60403,
                    LastBusStopKey = 21165,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 310,
                    Area = Enums.AREA.Jerusalem,
                    FirstBusStopKey = 4009,
                    LastBusStopKey = 60204,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 340,
                    Area = Enums.AREA.Jerusalem,
                    FirstBusStopKey = 4009,
                    LastBusStopKey = 61416,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 1,
                    Area = Enums.AREA.Jerusalem,
                    FirstBusStopKey = 60257,
                    LastBusStopKey = 61416,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 2,
                    Area = Enums.AREA.Jerusalem,
                    FirstBusStopKey = 60817,
                    LastBusStopKey = 60226,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 6,
                    Area = Enums.AREA.North,
                    FirstBusStopKey = 60817,
                    LastBusStopKey = 65411,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 5,
                    Area = Enums.AREA.Center,
                    FirstBusStopKey = 63988,
                    LastBusStopKey = 61378,
                    ObjectActive=true,
                },

                new BusLine
                {
                    BusLineID = Config.RunningNumBusLine,
                    BusLineNumber = 891,
                    Area = Enums.AREA.South,
                    FirstBusStopKey = 12284,
                    LastBusStopKey = 65411,
                    ObjectActive=true,
                },


            };

            ListBusLineStations = new List<BusLineStation> { };

            for (int i = 0; i < 10; i++) // Creating 20 bus line stations according to the 10 bus lines (with first bus stop and last bus stop, and 8 bus stops between)
            {
                // #1 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID, 
                        BusStopKey = ListBusLines[i].FirstBusStopKey,
                        LineStationIndex = 0,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #2 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 1,
                        PrevStation = ListBusLines[i].FirstBusStopKey,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #3 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 2,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #4 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 3,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #5 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 4,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #6 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 5,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #7 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 6,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #8 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 7,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                // #9 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = 0,
                        LineStationIndex = 8,
                        PrevStation = 0,
                        NextStation = ListBusLines[i].LastBusStopKey,
                        ObjectActive = true
                    }
                );

                // #10 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusLines[i].LastBusStopKey,
                        LineStationIndex = 9,
                        PrevStation = 0,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );
            }

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