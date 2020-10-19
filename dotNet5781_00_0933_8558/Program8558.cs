using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_00_0933_8558
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome8558();
            Welcome0933();
            Console.ReadKey();
        }

        static partial void Welcome0933();

        private static void Welcome8558()
        {
            Console.WriteLine("Enter your name:");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
            // Now it WORKS
            //finish ex0
        }
    }
}
