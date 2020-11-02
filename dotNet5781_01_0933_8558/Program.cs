/*
 Exercise 1 - Mendi Ben Ezra (311140933), Shimon Dyskin (310468558)
 Description: The program offers to add a bus, pick it to a travel and treat and refuel it.
 */


using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
    class Program
    {
        public static Random kmForRide = new Random(DateTime.Now.Millisecond); // Booting the random sequence field

        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>();
            BUS_CHOICE choice;
            bool success;
            Console.WriteLine("Bus management:\n\n"+
                "Enter one of the following:\n" +
                "ADD: To add a new bus\n" +
                "PICK: To pick a bus for a ride\n" +
                "TREAT: to refuel or treat the bus\n" +
                "SHOW_MILEAGE: to show the milage since the last treatment\n" +
                "EXIT: exit\n");

            do // Loops until the user puts EXIT
            {
                do // The first choice input loop
                {
                    Console.WriteLine("enter your choice:");
                    string answer = Console.ReadLine();
                    success = Enum.TryParse(answer, out choice);  // Trys to convert the answer to one of the ENUM CHOICE
                    if (!success) // If the conversion of the string to enum didn't succeed - print message and run the loop again
                    {
                        Console.WriteLine("There is no such option in the menu, please enter your choice again.");
                    }
                }
                while (!success);

                try // Try and catch for all the error cases in the switch case
                {
                    String license;
                    Bus busFound = null;

                    switch (choice)
                    {
                        case BUS_CHOICE.ADD: // The add bus option
                            Console.WriteLine("Enter the license number, please:");
                            license = Console.ReadLine(); 
                            if (!int.TryParse(license, out int number) || license.Length > 8 || license.Length < 7) // Checks if the input can be converted to int, and if the length is appropriate
                                throw new Exception("Wrong input of license number.");

                            if (FindIfBusExist(buses, license)) // Checks if the license already in the list
                                throw new Exception("There is already a bus with such a license.");

                            Console.WriteLine("Enter the date of absorption, please:"); 
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date) || date > DateTime.Now) // Checks if the input can be converted to a date 
                                throw new Exception("Wrong input for date (enter a legit date, for example: 1/1/2001, or 1.1.2001, and not a future date).");
                            else if ((date.Year >= 2018 && license.Length == 7) || (date.Year < 2017 && license.Length == 8)) // Checks if the date match the license
                                throw new Exception("The date does not match the license (8 digits for 2018 and above buses, and 7 digits for less than 2017.)");
                            
                            Console.WriteLine("Enter the mileage of the bus, please:");
                            if (!double.TryParse(Console.ReadLine(), out double mile)) // Checks if the input can be converted to a double
                                throw new Exception("Wrong input for mileage.");

                            buses.Add(new Bus(date, license, mile)); // If didn't throw anything, it means we can add the bus to the list
                            break;

                        case BUS_CHOICE.PICK:
                            {
                                double kmRand = 1200 * kmForRide.NextDouble(); // Choosing a random number base on the sequence created above - a double number between 0-1200 km (a ride above 1200 isn't possible)
                                kmRand = Math.Round(kmRand, 2); // Round up the double to two decimal places
                                Console.WriteLine("Please, enter the license number of the bus for travel:");
                                license = Console.ReadLine();
                                if (!int.TryParse(license, out number) || license.Length > 8 || license.Length < 7) // If the license input is incorecct - throws exception
                                    throw new Exception("Wrong input of license number.");

                                // Find the bus with the license (using the FindBus function) and store to busFound:
                                busFound = FindBus(buses, license);

                                if (busFound == null) // If didn't found, throws an exception
                                {
                                    throw new Exception("The bus doesn't exist.");
                                }
                                else
                                {
                                    if (!busFound.CheckIfDangerous(kmRand)) // Calling the Bus method to check if the bus is dangerous
                                    {
                                        busFound.KMLeftToRide = kmRand; // Check if there are km left to go to this ride, if left, the kmLeftToRide will be updated in the setter, if not the setter throws exception                                 
                                        busFound.Mileage += kmRand; // Add the km of the ride to the toal mileage                                       
                                    }
                                    else
                                        throw new Exception("The bus you chose is dangerous, please take it to treatment!");
                                }
                            }
                            break;
                        case BUS_CHOICE.TREAT: // The bus treatment (refuel or treatment)
                            {
                                
                                Console.WriteLine("Enter the license number, please:");
                                license = Console.ReadLine();
                                if (!int.TryParse(license, out number) || license.Length > 8 || license.Length < 7) // Checks if the input is correct
                                    throw new Exception("Wrong input of license number");

                                // Find the bus with the license (using the FindBus function) and store to busFound:
                                busFound = FindBus(buses, license);

                                if (busFound == null) // If didn't found, throws an exception
                                {
                                    throw new Exception("The bus doesn't exist!");
                                }
                                else
                                {
                                    char checkRequest; // For the A or B input
                                    Console.WriteLine("Please enter A for refuel" +
                                                       "B for treatment" +
                                                       "or other key to return to the menu:\n");
                                    char.TryParse(Console.ReadLine(), out checkRequest); // Checks if the input legit and stores checkRequest
                                    if (checkRequest == 'A')
                                    {
                                        busFound.Refuel();
                                        Console.WriteLine("The gas tank was filled succesfuly!");
                                    }
                                    else if (checkRequest == 'B')
                                    {
                                        busFound.Treatment();
                                        Console.WriteLine("The bus recieved a treatment!");
                                    }
                                    break;
                                }
                            }

                        case BUS_CHOICE.SHOW_MILEAGE: // Shows the total mileage, since last treatment (using the method from BUS class), and license
                            {
                                foreach (Bus bus in buses)
                                {
                                    Console.WriteLine("License = {0}, Total mileage = {1} km, Mileage since last treatment = {2} km", bus.License, bus.Mileage, bus.MileageFromLastTreat());
                                }
                            }
                            break;

                        case BUS_CHOICE.EXIT:
                            {
                                Console.WriteLine("Good Bye ☺");
                                Console.ReadKey();
                            }
                            break;
                        default:
                            Console.WriteLine("Wrong Choice");
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                
                Console.WriteLine("Returning to the menu.");

            } while (choice != BUS_CHOICE.EXIT);
        }


        /// <summary>
        /// Searchs trough a given list of buses if a bus exist with the given string, and returns true if yes
        /// </summary>
        /// <param name="buses"></param>
        /// <param name="license"></param>
        /// <returns></returns>
        public static bool FindIfBusExist(List<Bus> buses, string license)
        {
            foreach (Bus bus in buses)
            {
                if (bus.CompareLicenses(license))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds a bus with the given string in the given list of buses, and returns reference to the object
        /// </summary>
        /// <param name="buses"></param>
        /// <param name="license"></param>
        /// <returns></returns>
        public static Bus FindBus(List<Bus> buses, string license)
        {
            foreach (Bus bus in buses)
            {
                if (bus.CompareLicenses(license))
                {
                    return bus;
                }
            }
            return null;
        }

    }
}


/* RUN EXAMPLE:
Bus management:

Enter one of the following:
ADD: To add a new bus
PICK: To pick a bus for a ride
TREAT: to refuel or treat the bus
SHOW_MILEAGE: to show the milage since the last treatment
EXIT: exit

enter your choice:
ADD
Enter the license number, please:
1234567
Enter the date of absorption, please:
10 / 12 / 2020
Wrong input for date (enter a legit date, for example: 1 / 1 / 2001, or 1.1.2001, and not a future date).
Returning to the menu.
enter your choice:
ADD
Enter the license number, please:
1234567
Enter the date of absorption, please:
10 / 1 / 2001
Enter the mileage of the bus, please:
1000
Returning to the menu.
enter your choice:
ADD
Enter the license number, please:
12345678
Enter the date of absorption, please:
10 / 2 / 2020
Enter the mileage of the bus, please:
2000
Returning to the menu.
enter your choice:
PICK
Please, enter the license number of the bus for travel:
12345666
The bus doesn't exist.
Returning to the menu.
enter your choice:
PICK
Please, enter the license number of the bus for travel:
1234567
Returning to the menu.
enter your choice:
ADD
Enter the license number, please:
1234567
There is already a bus with such a license.
Returning to the menu.
enter your choice:
PICK
Please, enter the license number of the bus for travel:
1234567
There is not enough fuel for the travel.
Returning to the menu.
enter your choice:
TREAT
Enter the license number, please:
1234567
Please enter A for refuel, B for treatment or other key to return to the menu:
A
The gas tank was filled succesfuly!
Returning to the menu.
enter your choice:
ADD
Enter the license number, please:
ABSBSBS
Wrong input of license number.
Returning to the menu.
enter your choice:
ADD
Enter the license number, please:
12345677
Enter the date of absorption, please:
1 / 2 / 2020
Enter the mileage of the bus, please:
-0.3
The mileage input is incorrect.
Returning to the menu.
enter your choice:
EXIT
Good Bye ☺
*/