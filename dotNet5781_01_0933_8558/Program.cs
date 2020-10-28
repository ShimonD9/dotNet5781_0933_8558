﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
						String license;
						DateTime date;
						Console.WriteLine("enter date");
						success = DateTime.TryParse(Console.ReadLine(), out date);
						Console.WriteLine("teb rishuy");
						license = Console.ReadLine();
						if (success)
						{
							try
							{
								buses.Add(new Bus(date, license)); // surround with try
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