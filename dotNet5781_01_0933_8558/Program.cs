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
		List<Bus> buses = new List<Bus>();
			CHOICE choice;
			bool success;
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
						String license;
						DateTime date;
						Console.WriteLine("Enter the license number, please:");
						license = Console.ReadLine();
						Console.WriteLine("Enter date of absorption, please:");
						success = DateTime.TryParse(Console.ReadLine(), out date);
						if (success)
						{
							try
							{
								buses.Add(new Bus(date, license)); // surround with try
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

						case CHOICE.PICK_BUS:
                        {
							Console.WriteLine("Enter the license number of the bus for ride, please:");
							license = Console.ReadLine();
							double kmRand = 1200 * kmForRide.NextDouble();
							Bus busFound = null;
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
								Console.WriteLine("We are sorry, go search your mama!");
							}
							else try
							{
								busFound.MileageSinceRefill = kmRand;
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