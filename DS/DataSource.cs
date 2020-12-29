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
                LicenseDate = new DateTime(2016,12,10),           
                Mileage = 200030,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,10,15),
                MileageAtLastTreat = 200020,
                ObjectActive=true,
                },

                // 2
                new Bus
                {
                License=93029403,
                LicenseDate = new DateTime(2019,10,11),
                Mileage = 301010,
                Fuel = 0,
                BusStatus = Enums.BUS_STATUS.NEEDS_REFUEL,
                LastTreatmentDate =new DateTime(2020,10,12),
                MileageAtLastTreat = 300020,
                ObjectActive=true,
                },

                // 3 
                new Bus
                {
                License=63045719,
                LicenseDate = new DateTime(2018,3,10),
                Mileage = 130030,
                Fuel = 1100,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,11,4),
                MileageAtLastTreat = 130010,
                ObjectActive=true,
                },

                // 4 
                new Bus
                {
                License=7707707,
                LicenseDate = new DateTime(2012,1,10),
                Mileage = 500030,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.DANGEROUS,
                LastTreatmentDate =new DateTime(2016,10,17),
                MileageAtLastTreat = 300010,
                ObjectActive=true,
                },

                // 5 
                new Bus
                {
                License=11231123,
                LicenseDate = new DateTime(2019,2,2),
                Mileage = 2223,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,2,3),
                MileageAtLastTreat = 222,
                ObjectActive=true,
                },

                // 6 
                new Bus
                {
                License=32207541,
                LicenseDate = new DateTime(2018,10,3),
                Mileage = 15000,
                Fuel = 0,
                BusStatus = Enums.BUS_STATUS.NEEDS_REFUEL,
                LastTreatmentDate =new DateTime(2020,3,1),
                MileageAtLastTreat = 0,
                ObjectActive=true,
                },

                // 7 
                new Bus
                {
                License=1332465,
                LicenseDate = new DateTime(1999,10,12),
                Mileage = 933302,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2019,12,30),
                MileageAtLastTreat = 913999,
                ObjectActive=true,
                },

                // 8 
                new Bus
                {
                License=80412356,
                LicenseDate = new DateTime(2018,3,7),
                Mileage = 100500,
                Fuel = 695,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2019,10,11),
                MileageAtLastTreat = 85000,
                ObjectActive=true,
                },

                // 9
                new Bus
                {
                License=9360505,
                LicenseDate = new DateTime(2004,3,1),
                Mileage = 366666,
                Fuel = 1070,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,4,3),
                MileageAtLastTreat = 348999,
                ObjectActive=true,
                },

                // 10
                new Bus
                {
                License=10050823,
                LicenseDate = new DateTime(2019,6,3),
                Mileage = 1113,
                Fuel = 55,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,8,23),
                MileageAtLastTreat = 0,
                ObjectActive=true,
                },
                
                // 11
                new Bus
                {
                License=3025133,
                LicenseDate = new DateTime(2011,11,11),
                Mileage = 11134,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,9,20),
                MileageAtLastTreat = 11100,
                ObjectActive=true,
                },

                // 12
                new Bus
                {
                License=20021120,
                LicenseDate = new DateTime(2018,9,11),
                Mileage = 19320,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,9,3),
                MileageAtLastTreat = 2000,
                ObjectActive=true,
                },

                // 13
                new Bus
                {
                License=7820522,
                LicenseDate = new DateTime(2013,3,3),
                Mileage = 9999,
                Fuel = 950,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,1,10),
                MileageAtLastTreat = 8230,
                ObjectActive=true,
                },

                // 14
                new Bus
                {
                License=98769852,
                LicenseDate = new DateTime(2019,7,7),
                Mileage = 30520,
                Fuel = 200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,12,18),
                MileageAtLastTreat = 10520,
                ObjectActive=true,
                },

                // 15
                new Bus
                {
                License=9603250,
                LicenseDate = new DateTime(1993,7,17),
                Mileage = 2002030,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,10,18),
                MileageAtLastTreat = 1909996.3,
                ObjectActive=true,
                },

                // 16
                new Bus
                {
                License=96503201,
                LicenseDate = new DateTime(2018,10,17),
                Mileage = 19230,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,11,11),
                MileageAtLastTreat = 19200,
                ObjectActive=true,
                },

                // 17
                new Bus
                {
                License=7052033,
                LicenseDate = new DateTime(2006,6,7),
                Mileage = 999666,
                Fuel = 1200,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,7,12),
                MileageAtLastTreat = 979666,
                ObjectActive=true,
                },

                // 18
                new Bus
                {
                License=10203055,
                LicenseDate = new DateTime(2019,5,3),
                Mileage = 23320,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,10,10),
                MileageAtLastTreat = 20000,
                ObjectActive=true,
                },

                // 19
                new Bus
                {
                License=9036541,
                LicenseDate = new DateTime(2015,9,6),
                Mileage = 19852,
                Fuel = 0,
                BusStatus = Enums.BUS_STATUS.NEEDS_REFUEL,
                LastTreatmentDate =new DateTime(2020,5,5),
                MileageAtLastTreat = 18999,
                ObjectActive=true,
                },

                // 20
                new Bus
                {
                License=12987053,
                LicenseDate = new DateTime(2019,6,26),
                Mileage = 11112,
                Fuel = 1150,
                BusStatus = Enums.BUS_STATUS.READY_FOR_TRAVEL,
                LastTreatmentDate =new DateTime(2020,8,22),
                MileageAtLastTreat = 9992,
                ObjectActive=true,
                },

            };

            ListBusStops = new List<BusStop>
            {
                // #1
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

                 // #2
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

                 // #3
                new BusStop
                {
                    BusStopKey = 65299,
                    BusStopName = "שדרות בית הלל/מרכז מסחרי",
                    BusStopAddress = "שדרות בית הלל 2 מודיעין עילית",
                    Latitude = 31.936436,
                    Longitude = 35.039463,
                    Sunshade = false,
                    DigitalPanel = false,
                    ObjectActive=true,
                },

                 // #4
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

                 // #5
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

                 // #6
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

                 // #7
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

                // #8
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

                // #9
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

                // #10
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

                 // #10
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

                 // #11
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

                 // #12
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

                 // #13
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

                 // #14
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

                 // #15 - The middle bus line station init will start from here (busStop[14])
                new BusStop
                {
                    BusStopKey = 60225,
                    BusStopName = "אור החיים/אוהלי ספר",
                    BusStopAddress = "אור החיים 29 מודיעין עילית",
                    Latitude = 31.931206,
                    Longitude = 35.041019,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #16
                new BusStop
                {
                    BusStopKey = 60227,
                    BusStopName = "מרכז קסם/אבני נזר",
                    BusStopAddress = "אבני נזר 44 מודיעין עילית",
                    Latitude = 31.930259,
                    Longitude = 35.039163,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #17
                new BusStop
                {
                    BusStopKey = 60228,
                    BusStopName = "חפץ חיים/קצות חושן",
                    BusStopAddress = "מעלות שמחה 2 מודיעין עילית",
                    Latitude = 31.930062,
                    Longitude = 35.041693,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #18
                new BusStop
                {
                    BusStopKey = 60229,
                    BusStopName = "חפץ חיים ז",
                    BusStopAddress = "חפץ חיים 17 מודיעין עילית",
                    Latitude = 31.930769,
                    Longitude = 35.043667,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #19
                new BusStop
                {
                    BusStopKey = 60233,
                    BusStopName = "מסילת יוסף ב",
                    BusStopAddress = "מסילת יוסף 28 מודיעין עילית",
                    Latitude = 31.930097,
                    Longitude = 35.043896,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #20
                new BusStop
                {
                    BusStopKey = 60234,
                    BusStopName = "מסילת יוסף/מרומי שדה",
                    BusStopAddress = "מסילת יוסף 46 מודיעין עילית",
                    Latitude = 31.927997,
                    Longitude = 35.043064,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #21
                new BusStop
                {
                    BusStopKey = 60237,
                    BusStopName = "גן ילדים/נתיבות המשפט",
                    BusStopAddress = "נתיבות המשפט 95 מודיעין עילית",
                    Latitude = 31.927233,
                    Longitude = 35.040124,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #22
                new BusStop
                {
                    BusStopKey = 60238,
                    BusStopName = "נתיבות המשפט ח",
                    BusStopAddress = "נתיבות המשפט 100 מודיעין עילית",
                    Latitude = 31.92613,
                    Longitude = 35.041169,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #23
                new BusStop
                {
                    BusStopKey = 60239,
                    BusStopName = "נתיבות המשפט י''ג",
                    BusStopAddress = "נתיבות המשפט 87 מודיעין עילית",
                    Latitude = 31.925009,
                    Longitude = 35.041581,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #24
                new BusStop
                {
                    BusStopKey = 60240,
                    BusStopName = "נתיבות המשפט/מסילת יוסף",
                    BusStopAddress = " נתיבות המשפט 90 מודיעין עילית",
                    Latitude = 31.926719,
                    Longitude = 35.040952,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #25
                new BusStop
                {
                    BusStopKey = 60241,
                    BusStopName = "נתיבות המשפט ז",
                    BusStopAddress = "נתיבות המשפט 76 מודיעין עילית",
                    Latitude = 31.924946,
                    Longitude = 35.042655,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #26
                new BusStop
                {
                    BusStopKey = 60243,
                    BusStopName = "כיכר בית תפילה/נתיבות המשפט",
                    BusStopAddress = "נתיבות המשפט 66 מודיעין עילית",
                    Latitude = 31.925653,
                    Longitude = 35.043741,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #27
                new BusStop
                {
                    BusStopKey = 60244,
                    BusStopName = "נתיבות המשפט ה",
                    BusStopAddress = "נתיבות המשפט 50 מודיעין עילית",
                    Latitude = 31.926916,
                    Longitude = 35.045071,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #28
                new BusStop
                {
                    BusStopKey = 60245,
                    BusStopName = "נתיבות המשפט י''ז",
                    BusStopAddress = "נתיבות המשפט 56 מודיעין עילית",
                    Latitude = 31.926849,
                    Longitude = 35.045163,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #29
                new BusStop
                {
                    BusStopKey = 60247,
                    BusStopName = "נתיבות המשפט/כיכר גפן",
                    BusStopAddress = "נתיבות המשפט 46 מודיעין עילית",
                    Latitude = 31.928316,
                    Longitude = 35.044702,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #30
                new BusStop
                {
                    BusStopKey = 60248,
                    BusStopName = "נתיבות המשפט/מרומי שדה",
                    BusStopAddress = "נתיבות המשפט 34 מודיעין עילית",
                    Latitude = 31.930243,
                    Longitude = 35.045065,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #31
                new BusStop
                {
                    BusStopKey = 60249,
                    BusStopName = "נתיבות המשפט ל",
                    BusStopAddress = "נתיבות המשפט 31 מודיעין עילית",
                    Latitude = 31.931,
                    Longitude = 35.045806,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #32
                new BusStop
                {
                    BusStopKey = 60250,
                    BusStopName = "נתיבות המשפט כ",
                    BusStopAddress = "נתיבות המשפט 13 מודיעין עילית",
                    Latitude = 31.932529,
                    Longitude = 35.047188,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #10
                new BusStop
                {
                    BusStopKey = 60252,
                    BusStopName = "נתיבות המשפט/חת''ם סופר",
                    BusStopAddress = "נתיבות המשפט 6 מודיעין עילית",
                    Latitude = 31.933294,
                    Longitude = 35.047424,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #33
                new BusStop
                {
                    BusStopKey = 60254,
                    BusStopName = "חפץ חיים/אבני נזר",
                    BusStopAddress = "חפץ חיים 2 מודיעין עילית",
                    Latitude = 31.934342,
                    Longitude = 35.046634,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #34
                new BusStop
                {
                    BusStopKey = 60255,
                    BusStopName = "שדרות בית הלל/שדרות בית שמאי",
                    BusStopAddress = "שדרות בית הלל 1 מודיעין עילית",
                    Latitude = 31.934638,
                    Longitude = 35.047961,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #35
                new BusStop
                {
                    BusStopKey = 60256,
                    BusStopName = "אבי עזרי ב",
                    BusStopAddress = "אבי עזרי 3 מודיעין עילית",
                    Latitude = 31.923148,
                    Longitude = 35.051371,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #36
                new BusStop
                {
                    BusStopKey = 60319,
                    BusStopName = "גרין פארק/הרב מפוניבז",
                    BusStopAddress = "שדרות הרב מפוניביז 4 מודיעין עילית",
                    Latitude = 31.924332,
                    Longitude = 35.046591,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #37
                new BusStop
                {
                    BusStopKey = 60258,
                    BusStopName = "כיכר נאות הפסגה/רש''י",
                    BusStopAddress = "רש''י 1 מודיעין עילית",
                    Latitude = 31.928996,
                    Longitude = 35.046171,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #38
                new BusStop
                {
                    BusStopKey = 60262,
                    BusStopName = "חפץ חיים/קצות חושן",
                    BusStopAddress = "מעלות שמחה 30 מודיעין עילית",
                    Latitude = 31.930206,
                    Longitude = 35.041693,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #39
                new BusStop
                {
                    BusStopKey = 60263,
                    BusStopName = "חפץ חיים/אבני נזר",
                    BusStopAddress = "מעלות שמחה 23 מודיעין עילית",
                    Latitude = 31.929456,
                    Longitude = 35.039434,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #40
                new BusStop
                {
                    BusStopKey = 60266,
                    BusStopName = "מסילת ישרים ג",
                    BusStopAddress = " מסילת ישרים 1 מודיעין עילית",
                    Latitude = 31.931171,
                    Longitude = 35.037825,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #41
                new BusStop
                {
                    BusStopKey = 60268,
                    BusStopName = "מרכז מסחרי/שדרות יחזקאל",
                    BusStopAddress = "שדרות יחזקאל 2 מודיעין עילית",
                    Latitude = 31.93743,
                    Longitude = 35.037612,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #42
                new BusStop
                {
                    BusStopKey = 60270,
                    BusStopName = "שד. יחזקאל/רב ושמואל",
                    BusStopAddress = "שדרות יחזקאל 1 מודיעין עילית",
                    Latitude = 31.93854,
                    Longitude = 35.037621,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                 // #43
                new BusStop
                {
                    BusStopKey = 60271,
                    BusStopName = "מכבי אש/שדרות יחזקאל",
                    BusStopAddress = "שדרות יחזקאל 9 מודיעין עילית",
                    Latitude = 31.937521,
                    Longitude = 35.036854,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #44
                new BusStop
                {
                    BusStopKey = 60272,
                    BusStopName = "רשב''י ד",
                    BusStopAddress = "רבי שמעון בר יוחאי 7 מודיעין עילית",
                    Latitude = 31.937482,
                    Longitude = 35.038868,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #45
                new BusStop
                {
                    BusStopKey = 60276,
                    BusStopName = "רשב''י ב",
                    BusStopAddress = "רבי שמעון בר יוחאי 31 מודיעין עילית",
                    Latitude = 31.937254,
                    Longitude = 35.043655,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #46
                new BusStop
                {
                    BusStopKey = 60277,
                    BusStopName = "רשב''י ו",
                    BusStopAddress = "רבי שמעון בר יוחאי 30 מודיעין עילית",
                    Latitude = 31.937103,
                    Longitude = 35.044334,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #47
                new BusStop
                {
                    BusStopKey = 60261,
                    BusStopName = "חפץ חיים ג",
                    BusStopAddress = "חפץ חיים 22 מודיעין עילית",
                    Latitude = 31.930696,
                    Longitude = 35.043362,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #48
                new BusStop
                {
                    BusStopKey = 60279,
                    BusStopName = "יהודה הנשיא/יחזקאל",
                    BusStopAddress = "רבי יהודה הנשיא 2 מודיעין עילית",
                    Latitude = 31.938428,
                    Longitude = 35.039605,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #49
                new BusStop
                {
                    BusStopKey = 60280,
                    BusStopName = "תיבות דואר/יהודה הנשיא",
                    BusStopAddress = "רבי יהודה הנשיא 12 מודיעין עילית",
                    Latitude = 31.939044,
                    Longitude = 35.040886,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #50
                new BusStop
                {
                    BusStopKey = 60281,
                    BusStopName = "יהודה הנשיא ה",
                    BusStopAddress = "רבי יהודה הנשיא 22 מודיעין עילית",
                    Latitude = 31.938451,
                    Longitude = 35.043513,
                    Sunshade = false,
                    DigitalPanel = true,
                    ObjectActive=true,
                },

                // #51
                new BusStop
                {
                    BusStopKey = 60282,
                    BusStopName = "יהודה הנשיא / רשב''י",
                    BusStopAddress = "רבי יהודה הנשיא 30 מודיעין עילית",
                    Latitude = 31.93904,
                    Longitude = 35.045744,
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
                    LastBusStopKey = 65299,
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
                    LastBusStopKey = 26475,
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
                    LastBusStopKey = 65299,
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

            ListConsecutiveStations = new List<ConsecutiveStations> { };

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
                        NextStation = ListBusStops[2 * i + 14].BusStopKey,
                        ObjectActive = true
                    }
                );


                // #2 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 14].BusStopKey,
                        LineStationIndex = 1,
                        PrevStation = ListBusLines[i].FirstBusStopKey,
                        NextStation = ListBusStops[2 * i + 15].BusStopKey,
                        ObjectActive = true
                    }
                );

                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusLines[i].FirstBusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 14].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #3 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 15].BusStopKey,
                        LineStationIndex = 2,
                        PrevStation = ListBusStops[2 * i + 14].BusStopKey,
                        NextStation = ListBusStops[2 * i + 16].BusStopKey,
                        ObjectActive = true
                    }
                );

                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 14].BusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 15].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #4 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 16].BusStopKey,
                        LineStationIndex = 3,
                        PrevStation = ListBusStops[2 * i + 15].BusStopKey,
                        NextStation = ListBusStops[2 * i + 17].BusStopKey,
                        ObjectActive = true
                    }
                );

                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 15].BusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 16].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #5 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 17].BusStopKey,
                        LineStationIndex = 4,
                        PrevStation = ListBusStops[2 * i + 16].BusStopKey,
                        NextStation = ListBusStops[2 * i + 18].BusStopKey,
                        ObjectActive = true
                    }
                );

                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 16].BusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 17].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #6 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 18].BusStopKey,
                        LineStationIndex = 5,
                        PrevStation = ListBusStops[2 * i + 17].BusStopKey,
                        NextStation = ListBusStops[2 * i + 19].BusStopKey,
                        ObjectActive = true
                    }
                );

                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 17].BusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 18].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #7 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 19].BusStopKey,
                        LineStationIndex = 6,
                        PrevStation = ListBusStops[2 * i + 18].BusStopKey,
                        NextStation = ListBusStops[2 * i + 20].BusStopKey,
                        ObjectActive = true
                    }
                );


                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 18].BusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 19].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #8 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 20].BusStopKey,
                        LineStationIndex = 7,
                        PrevStation = ListBusStops[2 * i + 19].BusStopKey,
                        NextStation = ListBusStops[2 * i + 21].BusStopKey,
                        ObjectActive = true
                    }
                );


                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 19].BusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 20].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #9 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusStops[2 * i + 21].BusStopKey,
                        LineStationIndex = 8,
                        PrevStation = ListBusStops[2 * i + 20].BusStopKey,
                        NextStation = ListBusLines[i].LastBusStopKey,
                        ObjectActive = true
                    }
                );



                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 20].BusStopKey,
                        BusStopKeyB = ListBusStops[2 * i + 21].BusStopKey,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );

                // #10 busLineStation
                ListBusLineStations.Add(
                    new BusLineStation
                    {
                        BusLineID = ListBusLines[i].BusLineID,
                        BusStopKey = ListBusLines[i].LastBusStopKey,
                        LineStationIndex = 9,
                        PrevStation = ListBusStops[2 * i + 21].BusStopKey,
                        NextStation = 0,
                        ObjectActive = true
                    }
                );

                ListConsecutiveStations.Add(
                    new ConsecutiveStations
                    {
                        BusStopKeyA = ListBusStops[2 * i + 21].BusStopKey,
                        BusStopKeyB = ListBusLines[i].BusLineID,
                        Distance = 1.3,
                        TravelTime = TimeSpan.Parse("00:03:10"),
                        ObjectActive = true,
                    }
                );
            }

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
