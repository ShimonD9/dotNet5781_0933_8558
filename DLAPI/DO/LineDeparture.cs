using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DO
{
    public class LineDeparture
    {
        public int DepartureID { get; set; }  // Entity Key A - Running number
        public int BusLineID { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public bool ObjectActive { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("Bus Identifier = {0}, Start Time= {1}", BusLineID, DepartureTime);
        }
    }
}
