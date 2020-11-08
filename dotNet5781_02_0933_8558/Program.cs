using System;
using System.Collections.Generic;
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
            // Bus stop randomizer routine:

            Stack<BusStation> busStops = new Stack<BusStation>();
            var chars = "ABCDEF GHIJKLMN OPQRS TUVWXY Zabcd efghij klmnopqrs tuv wxyz";
            for (int i = 0; i < 40; ++i)
            {
                char[] stringChars = new char[8];
                for (int j = 0; j < stringChars.Length; j++)
                {
                    stringChars[i] = chars[rnd.Next(chars.Length)];
                }
                string address = new string(stringChars);
                int stationKey = rnd.Next(100000);
                while (busStops.Any(item => item.BusStationKey == stationKey)) // This loop makes sures there is no duplicateded station key
                {
                    stationKey = rnd.Next(100000);
                }
                BusStation newStation = new BusStation(stationKey, address);
                busStops.Push(newStation);
            }

            // Bus lines collection routine:

            BusLineCollections busCompany;

            for (int i = 0; i < 10; i++)
            {
                int busNumber = rnd.Next(999);

                // First station build:
                BusLineStation first = (BusLineStation)busStops.Pop();
                first.DistanceFromPreviousStation = 0;
                first.TravelTimeFromPreviousStation = TimeSpan.FromMinutes(0);

                // Last station build:
                BusLineStation last = (BusLineStation)busStops.Pop();
                last.DistanceFromPreviousStation = 100 * rnd.NextDouble() + 1; ;
                last.TravelTimeFromPreviousStation = TimeSpan.FromMinutes(200 * rnd.NextDouble() + 1);
            }

            CHOICE choice;
            bool success;
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
                        // busCompany.add();
                        Console.WriteLine("The gas tank was filled succesfuly!");
                    }

                    else if (checkRequest == 'B')
                    {
                        busFound.Treatment();
                        Console.WriteLine("The bus recieved a treatment!");
                    }

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

        private static bool BusStopExists(int stopKey)
        {
            return BusLine;
        }
    }
}
