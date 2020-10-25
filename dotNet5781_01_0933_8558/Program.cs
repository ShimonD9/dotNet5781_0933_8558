using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
    class Program
    {
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
						Console.WriteLine("put the details in please:");
						string license;
						DateTime Date;
						Console.WriteLine("enter date");
						success = DateTime.TryParse(Console.ReadLine(), out Date);
						Console.WriteLine("teb rishuy");
						license = Console.ReadLine();
						if (success)
						{
							try
							{
								buses.Add(new Bus(Date, license)); // surround with try
								foreach (Bus bus in buses)
								{
									Console.WriteLine(bus);
								}
							}
							catch (Exception exception)
							{
								Console.WriteLine(exception.Message);
							}
						}
						break;
				}

			} while (success);
		}
    }
}
