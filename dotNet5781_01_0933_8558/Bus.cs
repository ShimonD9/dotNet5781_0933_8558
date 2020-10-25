using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_01_0933_8558
{
    public class Bus
    {
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
				else if (value.Length == 7)
				{
					license = value;
				}
				else
				{
					throw new Exception("kuku");
				}

			}
		}
		public Bus(DateTime date, string license)
		{
			DateOfAbsorption = date;
			License = license;
		}
		public override string ToString()
		{
			return string.Format("rishuy = {0}; date = {1}", License, DateOfAbsorption.ToShortDateString());
		}
	}
}
