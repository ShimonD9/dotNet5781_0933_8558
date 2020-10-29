using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
    public class Bus
    {
        /// <summary>
        /// Bus constructor, initializing 6 fields in the ctor, based on 3 paramters
        /// </summary>
        /// <param name="dateInput"></param>
        /// <param name="licenseInput"></param>
        /// <param name="km" - equals 0 if not initialized by user></param>
        public Bus(DateTime dateInput, string licenseInput, double km = 0) // Bus constructor
        {
            DateOfAbsorption = dateInput;
            License = licenseInput;
            Mileage = km;

            // It is assumed that the treatment and the gas tank refill were done on the day of the entry to the database:
            treatmentDate = DateTime.Now;
            mileageAtLastTreat = km;
            kmLeftToRide = 1200;                            
        }

        private readonly DateTime DateOfAbsorption;

        private String license;

        /// <summary>
        /// License property.
        /// The getter returns string of right formatted license in accordance to the year and the number of digits.
        /// The setter puts the value in the license field, not before checking if the input is appropriate
        /// </summary>
        public String License
        {
            get
            {
                // Firstly - cutting the string to 3 parts, and then formatting them into a new one, adding dashes
                string prefix, middle, suffix, result;
                if (license.Length == 7)
                {
                    prefix = license.Substring(0, 2);
                    middle = license.Substring(2, 3);
                    suffix = license.Substring(5, 2);
                    result = string.Format("{0}-{1}-{2}", prefix, middle, suffix);
                }
                else
                {
                    prefix = license.Substring(0, 3);
                    middle = license.Substring(3, 2);
                    suffix = license.Substring(5, 3);
                    result = string.Format("{0}-{1}-{2}", prefix, middle, suffix);
                }
                return result;
            }

            private set
            {
                if (DateOfAbsorption.Year >= 2018 && value.Length == 8) // 8 digits only after 2018
                {
                    license = value;
                }
                else if (DateOfAbsorption.Year < 2018 && value.Length == 7) // 7 digits only before 2018
                {
                    license = value;
                }
                else
                {
                    throw new Exception("The license number is incorrect."); // Throws message if the input is incorrect
                }
            }
        }

        private double mileage; // Total mileage (kilometraj) field

        /// <summary>
        /// The mileage property - returns the mileage, or sets it with the value (only if it not less than zero)
        /// </summary>
        public double Mileage
        {
            get { return mileage; }

            set
            {
                if (value < 0) throw new Exception("The mileage input is incorrect."); // Throws message if the input is incorrect
                mileage = value;
            }
        }

        private double kmLeftToRide; // kmLeftToRide (equivalent to fuel condition)

        /// <summary>
        /// The property of the kmLeftToRide field -
        /// the getters returns the field,
        /// and the setter reduces the kilometers by the value, if the ride is possible
        /// if the km left is not enough for the new ride, it throws appropriate message
        /// </summary>
        public double KMLeftToRide
        {
            get { return kmLeftToRide; }
            set
            {
                if (kmLeftToRide > value)
                { kmLeftToRide -= value; }
                else
                {
                    throw new Exception("There is not enough fuel for the travel.");
                }
            }
        }

        /// <summary>
        /// The function restarts the kmLeftToRide with 1200 km
        /// </summary>
        public void Refuel()
        {
            kmLeftToRide = 1200;
        }

        private DateTime treatmentDate; // treatment date field

        /// <summary>
        /// The function stores the date of the treatment and the mileage at the time of treatment
        /// </summary>
        public void Treatment()
        {
            treatmentDate = DateTime.Now;
            MileageAtLastTreat = mileage;
        }


        private double mileageAtLastTreat; // mileage at last treat field

        /// <summary>
        /// Property of mileageAtLastTreat field (simple get and set)
        /// </summary>
        public double MileageAtLastTreat
        {
            get { return mileageAtLastTreat; }
            set { mileageAtLastTreat = Mileage; }
        }

        /// <summary>
        /// The function decides if the bus can travel, based on the mileage or the time passed since the last treatment
        /// </summary>
        /// <param name="kmForNextRide" - used to decide if the bus can travel the requested km></param>
        /// <returns></returns>
        public bool CheckIfDangerous(double kmForNextRide = 0)
        {         
            if ((MileageFromLastTreat() + kmForNextRide > 20000)         // The bus cannot travel more than 20,000 km since the last treatment
                ||                                                       // OR, if the last treatment happened more than a year a ago 
            (treatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0))    // To check this, we add a year to the last treatment date, and compare it to the current date. If the value returned is 0 or -1, it means a year (or more) passed since the last treatment
                return true;                                             // Returns true, meaning the bus is dangerous for ride
            return false;                                                // Else, returns false
        }

        /// <summary>
        /// The function returns the total mileage minus the mileage at last treat; 
        /// </summary>
        /// <returns></returns>
        public double MileageFromLastTreat()
        {
            return Mileage - MileageAtLastTreat;
        }
       
        /// <summary>
        /// The functions compares two license numbers
        /// </summary>
        /// <param name="str"> the given license for comprison</param>
        /// <returns></returns>
        public bool CompareLicenses(String str)
        {
            return (this.license == str);
        }

        /// <summary>
        /// Formats a string which represents the Bus object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("License = {0}, Date = {1}, KM left to ride = {2} km, Total mileage = {3} km", License, DateOfAbsorption.ToShortDateString(), KMLeftToRide, Mileage);
        }
    }
}
