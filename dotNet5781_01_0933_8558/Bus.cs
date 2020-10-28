using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
	public class Bus
	{
		public Bus(DateTime dateInput, string licenseInput,double mile) // Bus constructor
		{
			DateOfAbsorption = dateInput;
			License = licenseInput;
			mileageSinceRefill = 1200;
			Mileage = mile;
		}

		public DateTime DateOfAbsorption { get; set; }

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
					suffix = license.Substring(4, 2);
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

			set
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

            set { mileage = value; }
        }

        private double mileageSinceRefill;

        public double MileageSinceRefill
		{
            get { return mileageSinceRefill; }
			set
			{
				if (mileageSinceRefill > value)
				{ mileageSinceRefill -= value; }
				else
				{
					throw new Exception("There is not enough fuel for the ride!");
				}
			}
        }

		public bool compareLicenses(String str)
        {
			return (this.license == str);
        }

        /*
        private bool fuelIsEmpty;

        public bool FuelIsEmpty
		{
			get { return  fuelIsEmpty; }
            set { }
        }

		private bool buslIsDangerous;

        public bool buslIsDangerous
		{
			get { return buslIsDangerous; }
            set { }
        }
		*/

        public override string ToString()
		{
			return string.Format("License = {0} Date = {1}, Km left to ride = {2},mileage = {3} ", License, DateOfAbsorption.ToShortDateString(), MileageSinceRefill,Mileage);
		}
	}
}
