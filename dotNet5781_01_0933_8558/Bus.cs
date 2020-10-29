using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
    public class Bus
    {
        public Bus(DateTime dateInput, string licenseInput, double km) // Bus constructor
        {
            DateOfAbsorption = dateInput;
            License = licenseInput;
            kmLeftToRide = 1200;
            Mileage = km;
            treatmentDate = DateTime.Now;
            mileageAtLastTreat = km;
        }

        public readonly DateTime DateOfAbsorption;

        private String license;

        public String License
        {
            get
            {
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
                if (DateOfAbsorption.Year >= 2018 && value.Length == 8)
                {
                    license = value;
                }
                else if (DateOfAbsorption.Year < 2018 && value.Length == 7)
                {
                    license = value;
                }
                else
                {
                    throw new Exception("The license number is incorrect.");
                }
            }
        }

        private double mileage = 0;

        public double Mileage
        {
            get { return mileage; }

            set
            {
                if (value < 0) throw new Exception("The mileage input is incorrect.");
                mileage = value;
            }
        }

        private double kmLeftToRide;

        public void ReFuel()
        {
            kmLeftToRide = 1200;
        }

        private DateTime treatmentDate;

        public void Treatment()
        {
            treatmentDate = DateTime.Now;
            MileageAtLastTreat = mileage;
        }

        public double KMLeftToRide
        {
            get { return kmLeftToRide; }
            set
            {
                if (kmLeftToRide > value)
                { kmLeftToRide -= value; }
                else
                {
                    throw new Exception("There is not enough fuel for the ride!");
                }
            }
        }

        private double mileageAtLastTreat;

        public double MileageAtLastTreat
        {
            get { return mileageAtLastTreat; }
            set { mileageAtLastTreat = Mileage; }
        }

        public bool CheckIfDangerous(double kmForNextRide = 0)
        {         
            if ((MileageFromLastTreat() + kmForNextRide > 20000)|| treatmentDate.AddYears(1).CompareTo(DateTime.Now) <= 0)
                return true;
            return false;
        }

        public double MileageFromLastTreat()
        {
            return Mileage - MileageAtLastTreat;
        }
       
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
