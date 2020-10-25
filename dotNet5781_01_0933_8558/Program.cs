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
				Console.WriteLine("enter etc.");
				string kelet = Console.ReadLine();
				success = Enum.TryParse(kelet, out choice);
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
						Consoe.WriteLine("ten li pratim");
						string rishuy;
						DateTime.taarich;
						Console.WriteLine("enter date");
						success = DateTime.TryPrase(Console.ReadLine(), out taarich);
						Console.WriteLine("teb rishuy");
						rishuy = Console.ReadLine();
						if (success)
						{
							try
							{
								buses.Add(new Bus(taarich, rishuy)); // surround with try
								foreach (Bus bus in buses)
								{
									Console.WriteLine(bus);
								}
						catch (Exception exception)
							{
								console.WriteLine(exception.Message);
							}
						}
						break;
				}

			} while (success);
		}
    }
}
