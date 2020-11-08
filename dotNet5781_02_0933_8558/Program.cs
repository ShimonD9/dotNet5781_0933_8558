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
            List<BusLineStation> busStops = new List<BusLineStation>();
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
                double dist = 100 * rnd.NextDouble() + 1;
                double minutes = 200 * rnd.NextDouble() + 1;
                BusLineStation newStation = new BusLineStation(dist, minutes, stationKey, address);
                busStops.Add(newStation);
            }

            BusLineCollections busCompany;


            var finalString = new String(stringChars);
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
             } while (!success) ;
             switch (choice) { 
                case CHOICE.ADD:
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
    }
}
