using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
    class Program
    {
        public static Random kmForRide = new Random(DateTime.Now.Millisecond);
        static void Main(string[] args)
        {
            String license;
            Bus busFound = null;
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
                    Console.WriteLine("Try again");
                }
            }
            while (!success);

            do
            {
                switch (choice)
                {
                    case CHOICE.ADD_BUS:
                        bool successMile, successDate;
                        double mile;
                        DateTime date;
                        Console.WriteLine("Enter the license number, please:");
                        license = Console.ReadLine();
                        Console.WriteLine("Enter the mileage of the bus, please:");
                        successMile = double.TryParse(Console.ReadLine(), out mile);
                        Console.WriteLine("Enter date of absorption, please:");
                        successDate = DateTime.TryParse(Console.ReadLine(), out date);

                        if (successDate && successMile)
                        {
                            try
                            {
                                // Check if there is already such a bus:
                                foreach (Bus bus in buses)
                                {
                                    if (bus.compareLicenses(license))
                                    {
                                        throw new Exception("There is already a bus with such a license!");
                                    }
                                }
                                buses.Add(new Bus(date, license, mile));
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine(exception.Message);
                            }

                            foreach (Bus bus in buses)
                            {
                                Console.WriteLine(bus);
                            }
                        }
                        else
                            Console.WriteLine("Wrong input for Date or Mile.");
                        break;

                    case CHOICE.PICK_BUS:
                        {
                            double kmRand = 1200 * kmForRide.NextDouble() + 1; // Minimum 1 kilometer ride
                            Console.WriteLine("Enter the license number of the bus for ride, please:");
                            license = Console.ReadLine();

                            // Find the bus with the license:
                            foreach (Bus bus in buses)
                            {
                                if (bus.compareLicenses(license))
                                {
                                    busFound = bus;
                                    break;
                                }
                            }

                            if (busFound == null)
                            {
                                Console.WriteLine("The bus doesn't exist!");
                            }
                            else try
                                {
                                    busFound.KMLeftToRide = kmRand; // Check if there are km left to go to this ride, if left, the kmLeftToRide will be updated in the setter
                                    busFound.Mileage += kmRand; // Add the km of the ride to the toal mileage
                                }
                                catch (Exception exception)
                                {
                                    Console.WriteLine(exception.Message);
                                }

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

                            // Find the bus with the license:
                            foreach (Bus bus in buses)
                            {
                                if (bus.compareLicenses(license))
                                {
                                    busFound = bus;
                                    break;
                                }
                            }

                            if (busFound == null)
                            {
                                Console.WriteLine("The bus doesn't exist!");
                            }
                            else
                            {
                                char checkRequest; 
                                Console.WriteLine("Please enter A for refuel or B for treatment:");
                                char.TryParse(Console.ReadLine(), out checkRequest);
                                if (checkRequest == 'A')
                                {
                                    busFound.ReFuel();
                                    Console.WriteLine("Success!");
                                }
                                else if (checkRequest == 'B')
                                {
                                    busFound.Treatment();
                                }
                                else
                                    Console.WriteLine("Success!");
                                break;
                            }
                        }
                        break;
                    case CHOICE.SHOW_MILEAGE:
                        {
                            foreach (Bus bus in buses)
                            {
                                Console.WriteLine("License = {0} ,Mileage = {1},Mileage since last treatment = {2} ", bus.License,bus.Mileage ,bus.Mileage - bus.MileageAtLastTreat);
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

                Console.WriteLine("enter your choice:");
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