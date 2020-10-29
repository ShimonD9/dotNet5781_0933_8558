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

        public static bool FindIfBusExist(List<Bus> buses, string license)
        {
            foreach (Bus bus in buses)
            {
                if (bus.compareLicenses(license))
                {
                    return true;
                }
            }
            return false;
        }

        public static Bus FindBus(List<Bus> buses, string license)
        {
            foreach (Bus bus in buses)
            {
                if (bus.compareLicenses(license))
                {
                    return bus;
                }
            }
            return null;
        }

        static void Main(string[] args)
        {
            List<Bus> buses = new List<Bus>();
            CHOICE choice;
            bool success;
            Console.WriteLine(@"Bus management
				Enter one of the following:
				ADD_BUS: To add a new bus
				PICK_BUS: To pick a bus for a ride
				TREAT_BUS: to refuel or treat the bus
				SHOW_MILEAGE: to show the milage since the last treatment
				EXIT: exit");
            do
            {
                Console.WriteLine("enter your choice:");
                string answer = Console.ReadLine();
                success = Enum.TryParse(answer, out choice);
                if (!success)
                {
                    Console.WriteLine("There is no such option in the menu, please enter your choice again.");
                }
            }
            while (!success);

            do
            {
                try
                {
                    String license;
                    Bus busFound = null;

                    switch (choice)
                    {
                        case CHOICE.ADD_BUS:
                            Console.WriteLine("Enter the license number, please:");
                            license = Console.ReadLine();
                            if (!int.TryParse(license, out int number) || license.Length > 8 || license.Length < 7)
                                throw new Exception("Wrong input of license number");

                            if (FindIfBusExist(buses, license))
                                throw new Exception("There is already a bus with such a license");

                            Console.WriteLine("Enter the date of absorption, please:");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
                                throw new Exception("Wrong input for date");

                            Console.WriteLine("Enter the mileage of the bus, please:");
                            if (!double.TryParse(Console.ReadLine(), out double mile))
                                throw new Exception("Wrong input for mileage");

                            buses.Add(new Bus(date, license, mile));

                            // למחוק לפני הגשה:
                            foreach (Bus bus in buses)
                            {
                                Console.WriteLine(bus);
                            }
                            break;

                        case CHOICE.PICK_BUS:
                            {
                                double kmRand = 1200 * kmForRide.NextDouble(); // Choosing a random number base on the sequence created above - a double number between 0-1200 km (a ride above 1200 isn't possible)
                                Console.WriteLine("Please, enter the license number of the bus for travel:");
                                license = Console.ReadLine();
                                if (!int.TryParse(license, out number) || license.Length > 8 || license.Length < 7)
                                    throw new Exception("Wrong input of license number");

                                // Find the bus with the license:
                                busFound = FindBus(buses, license);

                                if (busFound == null)
                                {
                                    throw new Exception("The bus doesn't exist!");
                                }
                                else
                                {
                                    busFound.KMLeftToRide = kmRand; // Check if there are km left to go to this ride, if left, the kmLeftToRide will be updated in the setter
                                    busFound.Mileage += kmRand; // Add the km of the ride to the toal mileage
                                }

                                // למחוק לפני הגשה:
                                foreach (Bus bus in buses)
                                {
                                    Console.WriteLine(bus);
                                }
                            }
                            break;
                        case CHOICE.TREAT_BUS:
                            {
                                Console.WriteLine("Enter the license number, please:");
                                license = Console.ReadLine();
                                if (!int.TryParse(license, out number) || license.Length > 8 || license.Length < 7)
                                    throw new Exception("Wrong input of license number");

                                // Find the bus with the license:
                                busFound = FindBus(buses, license);

                                if (busFound == null)
                                {
                                    throw new Exception("The bus doesn't exist!");
                                }
                                else
                                {
                                    char checkRequest;
                                    Console.WriteLine(@"Please enter A for refuel
                                                                     B for treatment
                                                                     other key to return to the menu:");
                                    char.TryParse(Console.ReadLine(), out checkRequest);
                                    if (checkRequest == 'A')
                                    {
                                        busFound.ReFuel();
                                        Console.WriteLine("Success!");
                                    }
                                    else if (checkRequest == 'B')
                                    {
                                        busFound.Treatment();
                                        Console.WriteLine("Success!");
                                    }
                                    break;
                                }
                            }

                        case CHOICE.SHOW_MILEAGE:
                            {
                                foreach (Bus bus in buses)
                                {
                                    Console.WriteLine("License = {0}, Mileage = {1}, Mileage since last treatment = {2} ", bus.License, bus.Mileage, bus.MileageFromLastTreat());
                                }
                            }
                            break;

                        case CHOICE.EXIT:
                            {
                                Console.WriteLine("Good Bye ☺");
                                return;
                            }
                        default:
                            Console.WriteLine("Wrong Choice");
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                Console.WriteLine("Enter your choice:");
                string answer = Console.ReadLine();
                success = Enum.TryParse(answer, out choice);
                if (!success)
                {
                    Console.WriteLine("Try again");
                }
            } while (success);
        }
    }
}

/*
			Bus pupik = null;
			try
			{
				pupik = new Bus(new DateTime(2014, 2, 2), "5233435");
				pupik.MileageSinceRefill = 1199;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
			Console.WriteLine("{0}", pupik); // pupik = pupik.toString();
*/

/*
 								foreach (Bus bus in buses)
								{
									Console.WriteLine(bus);
								}
 */