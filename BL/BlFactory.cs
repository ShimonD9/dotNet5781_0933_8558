﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL;

namespace BLApi
{
    public static class BLFactory
    {
        /// <summary>
        /// Gets the singelton of the bl
        /// </summary>
        /// <param name="type"></param>
        /// <returns>BL Singelton</returns>
        public static IBL GetBL(string type)
        {
            switch (type)
            {
                case "1":
                    return BLImp.Instance;
                default:
                    return BLImp.Instance;
            }
        }
    }
}
