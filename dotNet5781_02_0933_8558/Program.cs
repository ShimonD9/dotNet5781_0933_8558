﻿/*
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

                    int total = 0;
                    foreach (BusLine bus in busCompany)
                    {
                        if (total < 2)
                            Console.WriteLine(bus.ToString());
                        total++;
                    }


                    Console.WriteLine("Bus management:\n\n" +
                            "Enter one of the following:\n" +
                            "ADD: To add a new line or station\n" +
                            "DELETE: To delete line or station\n" +
                            "SEARCH: To search lines or print options between two stations\n" +
                            "PRINT: To print all lines or stations\n" +
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
                            Console.WriteLine("Please enter A for adding a new line," +
                                               " B for adding a new bus line station to an existing line," +
                                               " or other key to return to the menu:");
                            char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest

                            if (checkRequest == 'A')
                            {
                                // Absorbing the bus line number and checking if already exist
                                Console.WriteLine("Please enter new line number:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                                if (!busCompany.searchBusLine(busLineNumber))
                                    throw new ArgumentException("Bus line is already exist twice!");

                                // Absorbing the bus line area 
                                Console.WriteLine("Please enter the area number 0 - 4 (General, North, South, Center, Jerusalem):");
                                if (!int.TryParse(Console.ReadLine(), out int area))
                                    throw new ArgumentException("Invalid input!");
                                if (area < 0 || area > 4)
                                    throw new ArgumentException("Invalid area number!");

                                Console.WriteLine("A new bus line must containe at least two bus stops.\n " +
                                              "Please enter A for adding existing 2 bus stops, " +
                                              "B for creating 2 new bus stops:");
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
                                    busCompany.busLineCollectionsList.Add(newBusLine);
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
                                    busCompany.busLineCollectionsList.Add(newBusLine);
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

                                Console.WriteLine("Are you wish to add an existing one, or to create a new bus stop?\n " +
                                            "Enter A for adding an existing bus stop, " +
                                            "B for creating and adding a new bus stop,");

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
                                                    "Enter A for adding it to the beginning of the line, " +
                                                    "B for adding it to the middle of the line, " +
                                                    "C for adding it to the end of the line:");
                                char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest

                                double distToNext = 0, minToNext = 0;
                                if (checkRequest == 'A')
                                {
                                    BusLineStation newBusLineStation = new BusLineStation(newBusStop, 0, 0);
                                    askForDistanceAndMinutesToNext(ref distToNext, ref minToNext);
                                    busCompany.busLineCollectionsList[index].addBusStation(newBusLineStation, 0, distToNext, minToNext);
                                }
                                else if (checkRequest == 'B')
                                {
                                    int prevKey;
                                    Console.WriteLine("Please, enter the key of the previous station.");
                                    if (!int.TryParse(Console.ReadLine(), out prevKey))
                                        throw new ArgumentException("Invalid input!");
                                    if (!busCompany.busLineCollectionsList[index].stationExist(prevKey))
                                        throw new KeyNotFoundException("The key for the previous station is incorrect");
                                    askForDistanceAndMinutesFromPrevious(ref distance, ref minutes);
                                    BusLineStation newBusLineStation = new BusLineStation(newBusStop, distance, minutes);
                                    askForDistanceAndMinutesToNext(ref distToNext, ref minToNext);
                                    busCompany.busLineCollectionsList[index].addBusStation(newBusLineStation, prevKey, distToNext, minToNext);
                                }
                                else if (checkRequest == 'C')
                                {
                                    askForDistanceAndMinutesFromPrevious(ref distance, ref minutes);
                                    BusLineStation newBusLineStation = new BusLineStation(newBusStop, distance, minutes);
                                    busCompany.busLineCollectionsList[index].addBusStationToEnd(newBusLineStation);
                                }
                                else throw new ArgumentException("Invalid input!");
                            }
                            else throw new ArgumentException("Invalid input!");
                            break;

                        case CHOICE.DELETE:
                            // For the A or B input
                            Console.WriteLine("Please enter A for delete a bus line," +
                                               " B for delete station," +
                                               " or other key to return to the menu:");
                            char.TryParse(Console.ReadLine(), out checkRequest);
                            if (checkRequest == 'A')
                            {
                                Console.WriteLine("Please enter the bus line number you would like to delete:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                               // index = busCompany.searchIndex(busLineNumber);
                               // BusLine deleteBusLineStation = busCompany.busLineCollectionsList[index];
                                busCompany.DeleteBusLine(busLineNumber);
                            }
                            else if (checkRequest == 'B')
                            {
                                Console.WriteLine("Please enter the bus line number:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                                index = busCompany.searchIndex(busLineNumber);
                                BusLine busLineHelp = busCompany.busLineCollectionsList[index];
                                Console.WriteLine("Please enter the station number you would like to delete:");
                                if (!int.TryParse(Console.ReadLine(), out stopKey))
                                    throw new ArgumentException("Invalid input!");
                                index = busLineHelp.findIndexStation(stopKey);
                                if (index == 0)
                                {
                                    busLineHelp.BusStationList[1].DistanceFromPreviousStation = 0;
                                    busLineHelp.BusStationList[1].TimeTravelFromPreviousStation = TimeSpan.FromMinutes(0);
                                    busLineHelp.deleteBusStation(stopKey);
                                }
                                else if (index == busLineHelp.BusStationList.Count - 1)
                                {
                                    busLineHelp.deleteBusStation(stopKey);
                                }
                                else
                                {
                                    double newDistance, newMinutes;
                                    Console.WriteLine("You need to update the distance and the time of next stop from the previous station that you wish to delete:\n" +
                                        "Please enter the new Distance:\n");
                                    if (!double.TryParse(Console.ReadLine(), out newDistance))
                                        throw new ArgumentException("Invalid input!");
                                    Console.WriteLine("Please enter the new Time:\n");
                                    if (!double.TryParse(Console.ReadLine(), out newMinutes))
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
                            Console.WriteLine("Please enter A for printing all the bus lines who are passing trough a specific bus stop, " +
                                               "B for printing the options to travel from one station to another, sorted by travel time, " +
                                               "or other key to return to the menu:");
                            char.TryParse(Console.ReadLine(), out checkRequest);
                            if (checkRequest == 'A')
                            {
                                Console.WriteLine("Please enter the key of the bus station:");
                                if (!int.TryParse(Console.ReadLine(), out stopKey))
                                    throw new ArgumentException("Invalid input!");
                                BusStop busStop = findExistBusStop(stopKey);
                                bool flag = false;
                                foreach (BusLine bus in busCompany)
                                {
                                    if (bus.stationExist(stopKey))
                                    { 
                                        Console.WriteLine("Bus line number {0} stops at bus station number {1}", bus.BusLineNumber, stopKey);
                                        flag = true;
                                    }
                                }
                                if (!flag)
                                    Console.WriteLine("No bus line stops at the given bus station");
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
                                            busLinesBetweenTwoStops.busLineCollectionsList.Add(bus.Track(stopKeyA, stopKeyB));
                                }
                                busLinesBetweenTwoStops.sortBusList();
                                foreach (BusLine bus in busLinesBetweenTwoStops)
                                {
                                    Console.WriteLine("Bus number {0} travel time from {1} to {2} is: {3}", bus.BusLineNumber, stopKeyA, stopKeyB, bus.TotalTimeTravel());
                                }
                            }
                            else throw new ArgumentException("Invalid input!");
                            break;

                        case CHOICE.PRINT:
                          
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
                busCompany.busLineCollectionsList.Add(newBusLine);

                //  Middle stations build:

                minutes = 20 * rnd.NextDouble() + 1;
                BusLineStation middle = new BusLineStation(busStops[i + 20], 1.5 * minutes, minutes);
                minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLineCollectionsList[i].addBusStation(middle, first.BusStopKey, 1.5 * minutes, minutes);

                minutes = 20 * rnd.NextDouble() + 1;
                BusLineStation secondMiddle = new BusLineStation(busStops[i + 30], 1.5 * minutes, minutes);
                minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLineCollectionsList[i].addBusStation(secondMiddle, first.BusStopKey, 1.5 * minutes, minutes);
            } // Ending this loop, we initialized 10 bus lines, using 40 bus stations

            for (int i = 0; i < 10; i++)
            {
                BusLineStation first = new BusLineStation(busStops[i + 1], 0, 0);
                double minutes = 20 * rnd.NextDouble() + 1;
                busCompany.busLineCollectionsList[i].addBusStation(first, 0, 1.5 * minutes, minutes);
            } // This loop makes sure that at least 10 bus stations will be used for two different bus lines
        }
    }
}