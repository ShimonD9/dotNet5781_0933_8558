using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
    public class Bus
    {
		public Bus(DateTime date, string license) // Bus constructor
		{
			DateOfAbsorption = date;
			busLicense = license;
		}

		public DateTime DateOfAbsorption { get; set; }

		private String busLicense;

		public String License
		{
			get
			{
				string prefix, middle, suffix, result;
				if (busLicense.Length == 7)
				{
					prefix = busLicense.Substring(0, 2);
					middle = busLicense.Substring(2, 3);
					suffix = busLicense.Substring(4, 2);
					result = string.Format("{0}-{1}-{2}", prefix, middle, suffix);

				}
				else
				{
					prefix = busLicense.Substring(0, 3);
					middle = busLicense.Substring(3, 2);
					suffix = busLicense.Substring(5, 3);
					result = string.Format("{0}-{1}-{2}", prefix, middle, suffix);
				}
				return result;
			}

			set
			{
				if (DateOfAbsorption.Year >= 2018 && License.Length == 8)
				{
					busLicense = value;
				}
				else if (License.Length == 7)
				{
					busLicense = value;
				}
				else
				{
					throw new Exception("kuku");
				}

			}
		}

		public override string ToString()
		{
			return string.Format("rishuy = {0}; date = {1}", License, DateOfAbsorption.ToShortDateString());
		}
	}
}
