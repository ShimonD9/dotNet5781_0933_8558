using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class BadIdException : Exception
    {
        public int ID;
        public BadIdException(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.BadIdException)innerException).ID;
        public override string ToString() => base.ToString() + $", bad lisence id: {ID}";
    }
    
}

