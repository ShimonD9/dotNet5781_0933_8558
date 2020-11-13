/*
 Exercise 2 - Mendi Ben Ezra (311140933), Shimon Dyskin (310468558)
 Description: The program manage a collection of bus lines, with bus stations, offering to add, delete, search and print.
 Note: According to the lecturer we decided that two round-trip lines would not pass through stations with the same code (even the first and the last ones, as it is in reality.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace dotNet5781_02_0933_8558
{
    class Program
    {
        static BusLinesCollection busCompany = new BusLinesCollection();
        static List<BusStop> busStops = new List<BusStop>();
        public static Random rnd = new Random(DateTime.Now.Millisecond);

        static void Main(string[] args)
        {

            busCompanyInitializer(ref busCompany, ref busStops); // Calling the 40 bus stops and 10 bus lines randomizer routine:
            CHOICE choice = CHOICE.EXIT;

            do
            {
                try
                {
                    bool success;
                    int busLineNumber, stopKey, index;
                    double distance = 0, minutes = 0;

                    Console.WriteLine("====================BUS COMPANY====================\n\n" +
                            "Enter one of the following:\n" +
                            "ADD: To add a new line or station\n" +
                            "DELETE: To delete line or station\n" +
                            "SEARCH: To search lines of a bus station, or to print travel options between two stations\n" +
                            "PRINT: To print all the lines or the stations\n" +
                            "EXIT: exit\n");
                    do // The choice input loop
                    {
                        Console.WriteLine("Enter your choice please:");
                        string answer = Console.ReadLine();
                        success = Enum.TryParse(answer, out choice);
                        if (!success) // If the conversion of the string to enum didn't succeed - print message and run the loop again
                            Console.WriteLine("There is no such option in the menu, please enter your choice again.");
                    } while (!success);

                    switch (choice)
                    {
                        case CHOICE.ADD:
                            char checkRequest; // For the A or B input
                            Console.WriteLine("Please enter A - for adding a new line, " +
                                               "B - for adding a new bus line station to an existing line, " +
                                               "or other key to return to the menu:");
                            char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest

                            if (checkRequest == 'A')
                            {
                                // Absorbing the bus line number and checking if already exist
                                Console.WriteLine("Please enter new line number:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                                if (!busCompany.searchBusLine(busLineNumber))
                                    throw new ArgumentException("The bus line number is already exist in the company twice!");

                                // Absorbing the bus line area 
                                Console.WriteLine("Please enter the area number 0 - 4 (General, North, South, Center, Jerusalem):");
                                if (!int.TryParse(Console.ReadLine(), out int area))
                                    throw new ArgumentException("Invalid input!");
                                if (area < 0 || area > 4)
                                    throw new ArgumentException("Invalid area number!");

                                Console.WriteLine("A new bus line must containe at least two bus stops.\n" +
                                              "Enter A - for adding existing 2 bus stops, " +
                                              "B - for creating 2 new bus stops:");
                                char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest
                                if (checkRequest == 'A')
                                {
                                    Console.WriteLine("Please enter the key of the first existing bus stop:");
                                    if (!int.TryParse(Console.ReadLine(), out stopKey))
                                        throw new ArgumentException("Invalid input!");
                                    BusStop first = findExistBusStop(stopKey);
                                    BusLineStation firstLineStation = new BusLineStation(first, 0, 0);

                                    Console.WriteLine("Please enter the key of the second bus stop:");
                                    if (!int.TryParse(Console.ReadLine(), out stopKey))
                                        throw new ArgumentException("Invalid input!");
                                    BusStop second = findExistBusStop(stopKey);
                                    askForDistanceAndMinutesFromPrevious(ref distance, ref minutes);
                                    BusLineStation secondLineStation = new BusLineStation(second, distance, minutes);

                                    BusLine newBusLine = new BusLine(busLineNumber, firstLineStation, secondLineStation, area);
                                    busCompany.busLinesList.Add(newBusLine);
                                }
                                else if (checkRequest == 'B')
                                {
                                    Console.WriteLine("First bus stop creation:");
                                    BusStop first = addNewBusStop();
                                    BusLineStation firstLineStation = new BusLineStation(first, 0, 0);

                                    Console.WriteLine("Second bus stop creation:");
                                    BusStop second = addNewBusStop();
                                    askForDistanceAndMinutesFromPrevious(ref distance, ref minutes);
                                    BusLineStation secondLineStation = new BusLineStation(second, distance, minutes);

                                    BusLine newBusLine = new BusLine(busLineNumber, firstLineStation, secondLineStation, area);
                                    busCompany.busLinesList.Add(newBusLine);
                                }
                                else throw new ArgumentException("Invalid input!");
                            }
                            else if (checkRequest == 'B')
                            {
                                BusStop newBusStop;
                                Console.WriteLine("Please enter the number of the line bus, you want to add a bus stop to:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                                index = busCompany.searchIndex(busLineNumber); // If there is no such bus line, the searchIndex will throw exception

                                Console.WriteLine("Enter A - for adding an existing bus stop, " +
                                            "B - for creating and adding a new bus stop");

                                char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest

                                if (checkRequest == 'A')
                                {
                                    Console.WriteLine("Please enter the existing bus stop number, you want to add to the bus line");
                                    if (!int.TryParse(Console.ReadLine(), out stopKey))
                                        throw new ArgumentException("Invalid input!");
                                    newBusStop = findExistBusStop(stopKey);
                                }
                                else if (checkRequest == 'B')
                                {
                                    newBusStop = addNewBusStop();
                                }
                                else throw new ArgumentException("Invalid input!");

                                Console.WriteLine("Where do you wish to add the bus stop?\n " +
                                                    "Enter A - for adding it to the beginning of the line, " +
                                                    "B - for adding it to the middle of the line, " +
                                                    "C - for adding it to the end of the line:");
                                char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest

                                double distToNext = 0, minToNext = 0;
                                if (checkRequest == 'A')
                                {
                                    BusLineStation newBusLineStation = new BusLineStation(newBusStop, 0, 0);
                                    askForDistanceAndMinutesToNext(ref distToNext, ref minToNext);
                                    busCompany.busLinesList[index].addBusStation(newBusLineStation, 0, distToNext, minToNext);
                                }
                                else if (checkRequest == 'B')
                                {
                                    Console.WriteLine("Please, enter the key of the previous station.");
                                    if (!int.TryParse(Console.ReadLine(), out int prevKey))
                                        throw new ArgumentException("Invalid input!");
                                    if (!busCompany.busLinesList[index].stationExist(prevKey))
                                        throw new KeyNotFoundException("The key for the previous station is incorrect");
                                    askForDistanceAndMinutesFromPrevious(ref distance, ref minutes);
                                    BusLineStation newBusLineStation = new BusLineStation(newBusStop, distance, minutes);
                                    askForDistanceAndMinutesToNext(ref distToNext, ref minToNext);
                                    busCompany.busLinesList[index].addBusStation(newBusLineStation, prevKey, distToNext, minToNext);
                                }
                                else if (checkRequest == 'C')
                                {
                                    askForDistanceAndMinutesFromPrevious(ref distance, ref minutes);
                                    BusLineStation newBusLineStation = new BusLineStation(newBusStop, distance, minutes);
                                    busCompany.busLinesList[index].addBusStationToEnd(newBusLineStation);
                                }
                                else throw new ArgumentException("Invalid input!");
                            }
                            else throw new ArgumentException("Invalid input!");
                            break;

                        case CHOICE.DELETE:
                            // For the A or B input
                            Console.WriteLine("Please enter A - for delete a bus line, " +
                                               "B - for delete station, " +
                                               "or other key to return to the menu:");
                            char.TryParse(Console.ReadLine(), out checkRequest);
                            if (checkRequest == 'A')
                            {
                                Console.WriteLine("Please enter the bus line number you would like to delete:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                                busCompany.DeleteBusLine(busLineNumber);
                            }
                            else if (checkRequest == 'B')
                            {
                                Console.WriteLine("Please enter the bus line number:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                                index = busCompany.searchIndex(busLineNumber);
                                BusLine busLineHelp = busCompany.busLinesList[index];
                                Console.WriteLine("Please enter the station number you would like to delete:");
                                if (!int.TryParse(Console.ReadLine(), out stopKey))
                                    throw new ArgumentException("Invalid input!");
                                index = busLineHelp.findIndexStation(stopKey);
                                if (index == 0)    //in case its the first station to delete
                                {   //update the first station
                                    busLineHelp.BusStationList[1].DistanceFromPreviousStation = 0;
                                    busLineHelp.BusStationList[1].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(0);
                                    busLineHelp.FirstStation = busLineHelp.BusStationList[1];   
                                    busLineHelp.deleteBusStation(stopKey);
                                }
                                else if (index == busLineHelp.BusStationList.Count - 1) //in case of delete last station
                                {   //update the last station
                                    busLineHelp.LastStation = busLineHelp.BusStationList[busLineHelp.BusStationList.Count - 1];
                                    busLineHelp.deleteBusStation(stopKey);
                                }
                                else                                                    //in case of delete middle station
                                {
                                    Console.WriteLine("You need to update the distance and the time (from the previous station) of the next bus station.\n" +
                                        "Please enter the new Distance:");
                                    if (!double.TryParse(Console.ReadLine(), out double newDistance))
                                        throw new ArgumentException("Invalid input!");
                                    Console.WriteLine("Please enter the new Time:");
                                    if (!double.TryParse(Console.ReadLine(), out double newMinutes))
                                        throw new ArgumentException("Invalid input!");
                                    busLineHelp.BusStationList[index + 1].DistanceFromPreviousStation = newDistance;
                                    busLineHelp.BusStationList[index + 1].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(newMinutes);
                                    busLineHelp.deleteBusStation(stopKey);
                                }
                            }
                            else throw new ArgumentException("Invalid input!");
                            break;

                        case CHOICE.SEARCH:
                            // For the A or B input
                            Console.WriteLine("Please enter A - for printing all the bus lines who are passing trough a specific bus stop, " +
                                               "B - for printing the options to travel from one station to another, sorted by travel time, " +
                                               "or other key to return to the menu:");
                            char.TryParse(Console.ReadLine(), out checkRequest);
                            if (checkRequest == 'A')
                            {
                                Console.WriteLine("Please enter the key of the bus station:");
                                if (!int.TryParse(Console.ReadLine(), out stopKey))
                                    throw new ArgumentException("Invalid input!");
                                BusLinesCollection linesContaining = busCompany.BusLinesContainStation(stopKey);
                                foreach (BusLine bus in linesContaining)
                                {
                                    Console.WriteLine("Bus line number {0} stops at bus station number {1}", bus.BusLineNumber, stopKey);
                                }
                            }

                            else if (checkRequest == 'B')
                            {
                                int stopKeyA, stopKeyB;
                                Console.WriteLine("Please enter the key of the first bus station:");
                                if (!int.TryParse(Console.ReadLine(), out stopKeyA))
                                    throw new ArgumentException("Invalid input!");
                                Console.WriteLine("Please enter the key of the second bus station:");
                                if (!int.TryParse(Console.ReadLine(), out stopKeyB))
                                    throw new ArgumentException("Invalid input!");
                                BusLinesCollection busLinesBetweenTwoStops = new BusLinesCollection();
                                foreach (BusLine bus in busCompany)
                                {
                                    BusLineStation first = bus.findAndReturnStation(stopKeyA);
                                    BusLineStation last = bus.findAndReturnStation(stopKeyB);
                                    if (first != null && last != null)
                                        if (bus.BusStationList.IndexOf(bus.findAndReturnStation(stopKeyA)) < bus.BusStationList.IndexOf(bus.findAndReturnStation(stopKeyB)))
                                            busLinesBetweenTwoStops.busLinesList.Add(bus.Track(stopKeyA, stopKeyB));
                                }

                                busLinesBetweenTwoStops.SortBusLinesList();

                                Console.WriteLine("Options to travel:");
                                foreach (BusLine bus in busLinesBetweenTwoStops)
                                {
                                    Console.WriteLine("Bus number {0} travel time from {1} to {2} is: {3}", bus.BusLineNumber, stopKeyA, stopKeyB, bus.TotalTimeTravel());
                                }
                            }
                            else throw new ArgumentException("Invalid input!");
                            break;

                        case CHOICE.PRINT:
                            // For the A or B input
                            Console.WriteLine("Please enter A - for printing all the bus lines in the company, " +
                                               "B - for printing all the bus stations, and the bus lines stoping at them, " +
                                               "or other key to return to the menu:");
                            char.TryParse(Console.ReadLine(), out checkRequest);
                            if (checkRequest == 'A')
                            {
                                foreach (BusLine bus in busCompany)
                                {
                                    Console.WriteLine(bus);
                                }
                            }
                            else if (checkRequest == 'B')
                            {
                                foreach (BusStop busStop in busStops)
                                {
                                    BusLinesCollection linesContaining = busCompany.BusLinesContainStation(busStop.BusStopKey);
                                    Console.Write("Bus lines numbers which stop at bus station number {0} =>", busStop.BusStopKey);
                                    foreach (BusLine bus in linesContaining)
                                    {
                                        Console.Write(" {0,-5}", bus.BusLineNumber, busStop.BusStopKey);
                                   
                                    }
                                    Console.WriteLine("\n====");
                                }
                            }
                            else throw new ArgumentException("Invalid input!");
                            break;

                        case CHOICE.EXIT:
                            break;

                        default:
                            break;
                    }

                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine(exception.Message);
                }
                catch (KeyNotFoundException exception)
                {
                    Console.WriteLine(exception.Message);
                };
                Console.WriteLine("\n");
            } while (choice != CHOICE.EXIT);

        }

        public static BusStop addNewBusStop()
        {
            int stopKey;
            string address;
            Console.WriteLine("Please enter the key of the bus stop:\n");
            if (!int.TryParse(Console.ReadLine(), out stopKey))
                throw new ArgumentException("Invalid input!");
            foreach (BusStop busStop in busStops)
            {
                if (busStop.BusStopKey == stopKey)
                {
                    throw new KeyNotFoundException("The bus stop already exist!");
                }
            }
            Console.WriteLine("Please enter the address of the bus stop:\n");
            address = Console.ReadLine();
            BusStop newBus = new BusStop(stopKey, address);
            busStops.Insert(0, newBus);
            return newBus;
        }

        public static void askForDistanceAndMinutesFromPrevious(ref double distance, ref double minutes)
        {
            Console.WriteLine("Please enter the distance from the previous bus stop (of this line):\n");
            if (!double.TryParse(Console.ReadLine(), out distance))
                throw new ArgumentException("Invalid input!");

            Console.WriteLine("Please enter the minutes of travel (for example: 20.5) from the previous bus stop (of this line):\n");
            if (!double.TryParse(Console.ReadLine(), out minutes))
                throw new ArgumentException("Invalid input!");
        }

        public static void askForDistanceAndMinutesToNext(ref double distance, ref double minutes)
        {
            Console.WriteLine("Please enter the distance to the next bus stop (of this line):\n");
            if (!double.TryParse(Console.ReadLine(), out distance))
                throw new ArgumentException("Invalid input!");

            Console.WriteLine("Please enter the minutes of travel (for example: 20.5) to the next bus stop (of this line):\n");
            if (!double.TryParse(Console.ReadLine(), out minutes))
                throw new ArgumentException("Invalid input!");
        }

        public static BusStop findExistBusStop(int stopKey)
        {
            foreach (BusStop busStop in busStops)
            {
                if (busStop.BusStopKey == stopKey)
                {
                    return busStop;
                }
            }
            throw new KeyNotFoundException("The bus stop doesn't exist!");
        }


        public static void busCompanyInitializer(ref BusLinesCollection busCompany, ref List<BusStop> busStops)
        {
            var bigChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var smallChars = "abcdefghijklmnopqrstuvwxyz";
            var numbers = "0123456789";
            for (int i = 0; i < 40; ++i)
            {
                char[] stringChars = new char[8];
                for (int j = 0; j < stringChars.Length; j++)
                {
                    if (j == 0) stringChars[j] = bigChars[rnd.Next(bigChars.Length - 1)];
                    else if (j == 5) stringChars[j] = ' ';
                    else if (j < 5) stringChars[j] = smallChars[rnd.Next(smallChars.Length - 1)];
                    else stringChars[j] = numbers[rnd.Next(numbers.Length - 1)];
                }
                string address = new string(stringChars);
                int stationKey = rnd.Next(100000);
                while (busStops.Any(item => item.BusStopKey == stationKey)) // This loop makes sures there is no duplicateded station key
                {
                    stationKey = rnd.Next(100000);
                }
                BusStop newStation = new BusStop(stationKey, address);
                busStops.Insert(0, newStation); ;
            } // Ending this loop, 40 bus station are initialized

            // Bus lines collection routine:



            for (int i = 0; i < 10; i++)
            {
                int busNumber = i * 100 + 1;
                int area = rnd.Next(0, 5);

                // First station build:

                BusLineStation first = new BusLineStation(busStops[i], 0, 0);

                // Last station build:
                double minutes = 20 * rnd.NextDouble() + 1;
                // Assuming the bus travels 1.5 km at a minute, so there will be a logic connection between the minutes and the distance
                BusLineStation last = new BusLineStation(busStops[i + 10], 1.5 * minutes, minutes);

                BusLine newBusLine = new BusLine(busNumber, first, last, area);
                busCompany.busLinesList.Add(newBusLine);

                //  Middle stations build:

                minutes = 20 * rnd.NextDouble() + 1;
                BusLineStation middle = new BusLineStation(busStops[i + 20], 1.5 * minutes, minutes);
                minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLinesList[i].addBusStation(middle, first.BusStopKey, 1.5 * minutes, minutes);

                minutes = 20 * rnd.NextDouble() + 1;
                BusLineStation secondMiddle = new BusLineStation(busStops[i + 30], 1.5 * minutes, minutes);
                minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLinesList[i].addBusStation(secondMiddle, first.BusStopKey, 1.5 * minutes, minutes);
            } // Ending this loop, we initialized 10 bus lines, using 40 bus stations

            for (int i = 0; i < 10; i++)
            {
                BusLineStation first = new BusLineStation(busStops[i + 1], 0, 0);
                double minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLinesList[i].addBusStation(first, 0, 1.5 * minutes, minutes);
            } // This loop makes sure that at least 10 bus stations will be used for two different bus lines
        }
    }
}

/*
Bus line details:
Bus line = 1, Area line = Center, busStationList: =
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:00:00, Km from last station = 0
Bus Station Code:
BusStationKey = 1395, Latitude = 32.2513984280878, Longitude = 34.3174921842373, Time from last station = 00:02:14.7330000, Km from last station = 3.36831361701168
Bus Station Code:
BusStationKey = 75409, Latitude = 33.2613128496619, Longitude = 34.7456978721757, Time from last station = 00:05:29.8560000, Km from last station = 8.24638964084275
Bus Station Code:
BusStationKey = 50987, Latitude = 32.0458440956407, Longitude = 35.2926303139853, Time from last station = 00:06:03.0310000, Km from last station = 9.07578664346402
Bus Station Code:
BusStationKey = 54672, Latitude = 33.2505180454583, Longitude = 35.1654375467754, Time from last station = 00:18:08.2010000, Km from last station = 27.2050262138737

Bus line details:
Bus line = 101, Area line = General, busStationList: =
Bus Station Code:
BusStationKey = 99641, Latitude = 31.6551010035701, Longitude = 35.0913000473712, Time from last station = 00:00:00, Km from last station = 0
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:08:49.1060000, Km from last station = 13.2276403967885
Bus Station Code:
BusStationKey = 21930, Latitude = 31.5479610886182, Longitude = 35.3497510886983, Time from last station = 00:04:53.3230000, Km from last station = 7.33306468363528
Bus Station Code:
BusStationKey = 72878, Latitude = 32.3575119236752, Longitude = 35.4232365984112, Time from last station = 00:14:33.5370000, Km from last station = 21.8384193733048
Bus Station Code:
BusStationKey = 59064, Latitude = 31.7871417542394, Longitude = 35.3085964956361, Time from last station = 00:18:07.6560000, Km from last station = 27.1913919447415

Bus management:

Enter one of the following:
ADD: To add a new line or station
DELETE: To delete line or station
SEARCH: To search lines or print options between two stations
PRINT: To print all lines or stations
EXIT: exit

Enter your choice please:
ADD
Please enter A for adding a new line, B for adding a new bus line station to an existing line, or other key to return to the menu:
B
Please enter the number of the line bus, you want to add a bus stop to:
1
Are you wish to add an existing one, or to create a new bus stop?
 Enter A for adding an existing bus stop, B for creating and adding a new bus stop,
A
Please enter the existing bus stop number, you want to add to the bus line
59064
Where do you wish to add the bus stop?
 Enter A for adding it to the beginning of the line, B for adding it to the middle of the line, C for adding it to the end of the line:
B
Please, enter the key of the previous station.
66489
Please enter the distance from the previous bus stop (of this line):

12.3
Please enter the minutes of travel (for example: 20.5) from the previous bus stop (of this line):

20.4
Please enter the distance to the next bus stop (of this line):

11
Please enter the minutes of travel (for example: 20.5) to the next bus stop (of this line):

3.4
Bus line details:
Bus line = 1, Area line = Center, busStationList: =
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:00:00, Km from last station = 0
Bus Station Code:
BusStationKey = 59064, Latitude = 31.7871417542394, Longitude = 35.3085964956361, Time from last station = 00:20:24, Km from last station = 12.3
Bus Station Code:
BusStationKey = 1395, Latitude = 32.2513984280878, Longitude = 34.3174921842373, Time from last station = 00:03:24, Km from last station = 11
Bus Station Code:
BusStationKey = 75409, Latitude = 33.2613128496619, Longitude = 34.7456978721757, Time from last station = 00:05:29.8560000, Km from last station = 8.24638964084275
Bus Station Code:
BusStationKey = 50987, Latitude = 32.0458440956407, Longitude = 35.2926303139853, Time from last station = 00:06:03.0310000, Km from last station = 9.07578664346402
Bus Station Code:
BusStationKey = 54672, Latitude = 33.2505180454583, Longitude = 35.1654375467754, Time from last station = 00:18:08.2010000, Km from last station = 27.2050262138737

Bus line details:
Bus line = 101, Area line = General, busStationList: =
Bus Station Code:
BusStationKey = 99641, Latitude = 31.6551010035701, Longitude = 35.0913000473712, Time from last station = 00:00:00, Km from last station = 0
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:08:49.1060000, Km from last station = 13.2276403967885
Bus Station Code:
BusStationKey = 21930, Latitude = 31.5479610886182, Longitude = 35.3497510886983, Time from last station = 00:04:53.3230000, Km from last station = 7.33306468363528
Bus Station Code:
BusStationKey = 72878, Latitude = 32.3575119236752, Longitude = 35.4232365984112, Time from last station = 00:14:33.5370000, Km from last station = 21.8384193733048
Bus Station Code:
BusStationKey = 59064, Latitude = 31.7871417542394, Longitude = 35.3085964956361, Time from last station = 00:18:07.6560000, Km from last station = 27.1913919447415

Bus management:

Enter one of the following:
ADD: To add a new line or station
DELETE: To delete line or station
SEARCH: To search lines or print options between two stations
PRINT: To print all lines or stations
EXIT: exit

Enter your choice please:
SEAECH
There is no such option in the menu, please enter your choice again.
Enter your choice please:
SEARCH
Please enter A for printing all the bus lines who are passing trough a specific bus stop, B for printing the options to travel from one station to another, sorted by travel time, or other key to return to the menu:
B
Please enter the key of the first bus station:
66489
Please enter the key of the second bus station:
59064
Bus line details:
Bus line = 1, Area line = Center, busStationList: =
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:00:00, Km from last station = 0
Bus Station Code:
BusStationKey = 59064, Latitude = 31.7871417542394, Longitude = 35.3085964956361, Time from last station = 00:20:24, Km from last station = 12.3

Bus line details:
Bus line = 101, Area line = General, busStationList: =
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:08:49.1060000, Km from last station = 13.2276403967885
Bus Station Code:
BusStationKey = 21930, Latitude = 31.5479610886182, Longitude = 35.3497510886983, Time from last station = 00:04:53.3230000, Km from last station = 7.33306468363528
Bus Station Code:
BusStationKey = 72878, Latitude = 32.3575119236752, Longitude = 35.4232365984112, Time from last station = 00:14:33.5370000, Km from last station = 21.8384193733048
Bus Station Code:
BusStationKey = 59064, Latitude = 31.7871417542394, Longitude = 35.3085964956361, Time from last station = 00:18:07.6560000, Km from last station = 27.1913919447415

Bus number 1 travel time from 66489 to 59064 is: 00:20:24
Bus number 101 travel time from 66489 to 59064 is: 00:37:34.5160000
Bus line details:
Bus line = 1, Area line = Center, busStationList: =
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:00:00, Km from last station = 0
Bus Station Code:
BusStationKey = 59064, Latitude = 31.7871417542394, Longitude = 35.3085964956361, Time from last station = 00:20:24, Km from last station = 12.3
Bus Station Code:
BusStationKey = 1395, Latitude = 32.2513984280878, Longitude = 34.3174921842373, Time from last station = 00:03:24, Km from last station = 11
Bus Station Code:
BusStationKey = 75409, Latitude = 33.2613128496619, Longitude = 34.7456978721757, Time from last station = 00:05:29.8560000, Km from last station = 8.24638964084275
Bus Station Code:
BusStationKey = 50987, Latitude = 32.0458440956407, Longitude = 35.2926303139853, Time from last station = 00:06:03.0310000, Km from last station = 9.07578664346402
Bus Station Code:
BusStationKey = 54672, Latitude = 33.2505180454583, Longitude = 35.1654375467754, Time from last station = 00:18:08.2010000, Km from last station = 27.2050262138737

Bus line details:
Bus line = 101, Area line = General, busStationList: =
Bus Station Code:
BusStationKey = 99641, Latitude = 31.6551010035701, Longitude = 35.0913000473712, Time from last station = 00:00:00, Km from last station = 0
Bus Station Code:
BusStationKey = 66489, Latitude = 32.006602273186, Longitude = 35.1777311466996, Time from last station = 00:08:49.1060000, Km from last station = 13.2276403967885
Bus Station Code:
BusStationKey = 21930, Latitude = 31.5479610886182, Longitude = 35.3497510886983, Time from last station = 00:04:53.3230000, Km from last station = 7.33306468363528
Bus Station Code:
BusStationKey = 72878, Latitude = 32.3575119236752, Longitude = 35.4232365984112, Time from last station = 00:14:33.5370000, Km from last station = 21.8384193733048
Bus Station Code:
BusStationKey = 59064, Latitude = 31.7871417542394, Longitude = 35.3085964956361, Time from last station = 00:18:07.6560000, Km from last station = 27.1913919447415

Bus management:

Enter one of the following:
ADD: To add a new line or station
DELETE: To delete line or station
SEARCH: To search lines or print options between two stations
PRINT: To print all lines or stations
EXIT: exit

Enter your choice please:
*/