using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DO.Enums;

namespace DO
{
    public class Bus
    {
        public int License { get; set; } // Entity Identifier (Key)
        public DateTime LicenseDate { get; set; }
        public double Mileage { get; set; }
        public double Fuel { get; set; }
        public BUS_STATUS BusStatus { get; set; }
        public DateTime LastTreatmentDate { get; set; }
        public double MileageAtLastTreat { get; set; }
        public bool ObjectActive { get; set; }
        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("License = {0}, License date = {1}, KM left to ride = {2} km, Total mileage = {3} km", License, LicenseDate.ToShortDateString(), Fuel, Mileage);
        }
    }
}
