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
        static void Main(string[] args)
        {
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
                {
                    Console.WriteLine("There is no such option in the menu, please enter your choice again.");
                }
            }
            while (!success);
            switch (choice)
            {
                case CHOICE.ADD:
                    // adding new station:
                    // בודק אם הרשימה ריקה. אם כן, אז מבקש להוסיף שתי תחנות
                    // אם לא ריקה, אז שואל איפה להוסיף.
                    // אם להוסיף בהתחלה - אז לשלוח עם פרמטר 0 של תחנה קודמת
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
