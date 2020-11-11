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
                    int busLineNumber, area, stopKey;
                    string address;
                    double distanceFromPrev, minutesFromPrev;

                    foreach (BusLine bus in busCompany)
                    {
                        Console.WriteLine(bus.ToString());
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
                                if (!int.TryParse(Console.ReadLine(), out area))
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

                                    Console.WriteLine("Please enter the key of the second bus stop:");
                                    if (!int.TryParse(Console.ReadLine(), out stopKey))
                                        throw new ArgumentException("Invalid input!");
                                    BusStop second = findExistBusStop(stopKey);

                                    Console.WriteLine("Please enter the distance from the previous bus stop (of this line):");
                                    if (!double.TryParse(Console.ReadLine(), out distanceFromPrev))
                                        throw new ArgumentException("Invalid input!");
                                    Console.WriteLine("Please enter the minutes of travel (for example: 20.5) from the previous bus stop (of this line):");
                                    if (!double.TryParse(Console.ReadLine(), out minutesFromPrev))
                                        throw new ArgumentException("Invalid input!");

                                    BusLineStation firstLineStation = new BusLineStation(first, 0, 0);
                                    BusLineStation secondLineStation = new BusLineStation(second, distanceFromPrev, minutesFromPrev);

                                    BusLine newBusLine = new BusLine(busLineNumber, firstLineStation, secondLineStation, area);
                                    busCompany.busLineCollectionsList.Add(newBusLine);
                                }
                                else if (checkRequest == 'B')
                                {
                                    Console.WriteLine("First bus stop creation:");
                                    BusStop first = addNewBusStop(); 
                                    
                                    Console.WriteLine("Second bus stop creation:");
                                    BusStop second = addNewBusStop();
                                    
                                    Console.WriteLine("Please enter the distance from the previous bus stop (of this line):\n");
                                    if (!double.TryParse(Console.ReadLine(), out distanceFromPrev))
                                        throw new ArgumentException("Invalid input!");

                                    Console.WriteLine("Please enter the minutes of travel (for example: 20.5) from the previous bus stop (of this line):\n");
                                    if (!double.TryParse(Console.ReadLine(), out minutesFromPrev))
                                        throw new ArgumentException("Invalid input!");

                                    
                                    busStops.Insert(0, second);

                                    BusLineStation firstLineStation = new BusLineStation(first, 0, 0);
                                    BusLineStation secondLineStation = new BusLineStation(second, distanceFromPrev, minutesFromPrev);

                                    BusLine newBusLine = new BusLine(busLineNumber, firstLineStation, secondLineStation, area);
                                    busCompany.busLineCollectionsList.Add(newBusLine);
                                }
                                else throw new ArgumentException("Invalid input!");
                            }
                            else if (checkRequest == 'B')
                            {
                                Console.WriteLine("Please enter the number of the line bus, you want to add a bus stop to:");
                                if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                    throw new ArgumentException("Invalid input!");
                                int index = busCompany.searchIndex(busLineNumber); // If there is no such bys line, the searchIndex will throw exception
                                
                                Console.WriteLine("Are you wish to create a new bus stop, or to add an existing one?\n " +
                                            "Enter A for adding an existing bus stop, " +
                                            "B for creating and adding a new bus stop,");

                                char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest
                                if (checkRequest == 'A')
                                {
                                    Console.WriteLine("Please enter the existing bus stop number, you want to add to the bus line");
                                    if (!int.TryParse(Console.ReadLine(), out stopKey))
                                        throw new ArgumentException("Invalid input!");
                                    BusStop newBuwStop = findExistBusStop(stopKey);
                                }
                                else if (checkRequest == 'B')
                                {
                                    BusStop newBusStop = addNewBusStop();
                                }
                                else throw new ArgumentException("Invalid input!");

                                Console.WriteLine("Where do you wish to add the bus stop?\n " +
                                                    "Enter A for adding it in the beggining, " +
                                                    "B for adding it at the middle, " +
                                                    " C for adding it at the end");
                                char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores in checkRequest
                                if (checkRequest == 'A')
                                {
                                    Console.WriteLine("Please enter the existing bus stop number, you want to add to the bus line");
                                    if (!int.TryParse(Console.ReadLine(), out stopKey))
                                        throw new ArgumentException("Invalid input!");
                                    BusStop newBuwStop = findExistBusStop(stopKey);
                                }
                                else if (checkRequest == 'B')
                                {
                                    BusStop newBusStop = addNewBusStop();
                                }
                                else if (checkRequest == 'C')
                                {

                                }
                                else throw new ArgumentException("Invalid input!");

                                




                            }
                            else throw new ArgumentException("Invalid input!");
                            break;
                        case CHOICE.DELETE:
                            break;
                        case CHOICE.SEARCH:
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
            return newBus);
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
