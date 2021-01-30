using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ManageAccess { get; set; }
        public bool ObjectActive { get; set; }
        /// <summary>
        /// Formats a string which represents the User object
        /// </summary>
        /// <returns> Returns the string to print the object </returns>
        public override string ToString()
        {
            return string.Format("User Name = {0}, Password= {1},Manage Access = {2}", UserName, Password, ManageAccess);
        }
    }
}
