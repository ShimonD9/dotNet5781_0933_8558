/*
 Exercise 2 - Mendi Ben Ezra (311140933), Shimon Dyskin (310468558)
 Description: The program manages a collection of bus lines, with bus stations,
offering to add, delete, search and print.
 ===Note: According to the lecturer we decided, that two round-trip lines would not pass through stations with the same code (even the first and the last ones, as it is in reality.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_0933_8558
{
    public enum CHOICE          //enum choices for the main
    {
        ADD, DELETE, SEARCH, PRINT, EXIT = -1
    }
    public enum AREA             //enum choices for aera buses
    {
        General,North,South,Center,Jerusalem
    }
}
