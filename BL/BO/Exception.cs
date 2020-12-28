using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class ExceptionBLBadLicense : Exception
    {
        public int ID;
        public ExceptionBLBadLicense(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDALBadLicsens)innerException).ID;
        public override string ToString() => base.ToString() + $", bad lisence id: {ID}";
    }
    
}

