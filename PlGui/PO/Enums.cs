using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PO
{
    public class Enums
    {
        //enum for the bus status
        public enum BUS_STATUS
        {
            READY_FOR_TRAVEL, NEEDS_REFUEL, DANGEROUS, AT_TRAVEL, AT_TREATMENT, AT_REFUEL
        }

        public enum AREA             //enum choices for aera buses
        {
            General, North, South, Center, Jerusalem
        }
    }
}
