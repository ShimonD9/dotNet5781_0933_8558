using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dotNet5781_02_0933_8558
{
    class Program
    {

        public static Random rnd = new Random(DateTime.Now.Millisecond);

        static void Main(string[] args)
        {
            try
            {
                
                BusLinesCollection busCompany = new BusLinesCollection();
                List<BusStop> busStops = new List<BusStop>();
                // Calling the 40 bus stops and 10 bus lines randomizer routine:
                busCompanyInitializer(ref busCompany, ref busStops);
                foreach(BusLine bus in busCompany)
                {
                    Console.WriteLine(bus.ToString());
                }
                CHOICE choice;
                bool success;
                int busLineNumber;
                int area;
                string address;
                int stationKey;
                Console.WriteLine("Bus management:\n\n" +
                        "Enter one of the following:\n" +
                        "ADD: To add a new linr or station\n" +
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
                        Console.WriteLine("Please enter A for adding a new line" +
                                           "B for adding a new bus line stop to an existing line" +
                                           "or other key to return to the menu:\n");
                        char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores checkRequest

                        if (checkRequest == 'A')
                        {

                            Console.WriteLine("plese enter new line number:\n");
                            if (!int.TryParse(Console.ReadLine(), out busLineNumber))
                                throw new ArgumentException("Invalid input!");

                            if(!busCompany.searchBusLine(busLineNumber))
                                throw new ArgumentException("Bus line is already exist twice!");

                            Console.WriteLine("plese enter the area number 0 - 4:\n");
                            if (!int.TryParse(Console.ReadLine(), out area))                         
                                throw new ArgumentException("Invalid input!");

                            if (area < 0 || area > 4)
                                throw new ArgumentException("Invalid area number!");

                            Console.WriteLine("plese enter first and last station of the bus:\n");
                            Console.WriteLine("enter first station:\n");
                            

                            
                            //BusLineStation newStation = new BusLineStation(stationKey, address);
                           // busStops.Insert(0, newStation); ;
                            //busCompany.add();
                            Console.WriteLine("The gas tank was filled succesfuly!");
                        }

                        //else if (checkRequest == 'B')
                        //{
                        //    busFound.Treatment();
                        //    Console.WriteLine("The bus recieved a treatment!");
                        //}

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
            catch(ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
            }
            catch (KeyNotFoundException exception)
            {
                Console.WriteLine(exception.Message);
            };
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
                double minutes = 200 * rnd.NextDouble() + 1;
                // Assuming the bus travels 1.5 km at a minute, so there will be a logic connection between the minutes and the distance
                BusLineStation last = new BusLineStation(busStops[i + 10], 1.5 * minutes, minutes);
                
                BusLine newLine = new BusLine(busNumber, first, last, area);
                busCompany.busLineCollectionsList.Add(newLine);

                //  Middle stations build:

                minutes = 200 * rnd.NextDouble() + 1;
                BusLineStation middle = new BusLineStation(busStops[i + 20], 1.5 * minutes, minutes);
                minutes = 200 * rnd.NextDouble() + 1;
                busCompany.busLineCollectionsList[i].addBusStation(middle, first.BusStop.BusStopKey, 1.5 * minutes, minutes);

                minutes = 200 * rnd.NextDouble() + 1;
                BusLineStation secondMiddle = new BusLineStation(busStops[i + 30], 1.5 * minutes, minutes);
                minutes = 200 * rnd.NextDouble() + 1;
                busCompany.busLineCollectionsList[i].addBusStation(secondMiddle, first.BusStop.BusStopKey, 1.5 * minutes, minutes);
            } // Ending this loop, we initialized 10 bus lines, using 40 bus stations

            for (int i = 0; i < 10; i++)
            {
                BusLineStation first = new BusLineStation(busStops[i + 1], 0, 0);
                double  minutes = 200 * rnd.NextDouble() + 1;
                busCompany.busLineCollectionsList[i].addBusStation(first, 0, 1.5 * minutes, minutes);
            } // This loop makes sure that at least 10 bus stations will be used for two different bus lines
        }
    }
}
