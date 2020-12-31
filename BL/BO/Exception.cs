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

    public class ExceptionBLBadUserId : Exception
    {
        public string ID;
        public ExceptionBLBadUserId(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDALBadIdUser)innerException).ID;
        public override string ToString() => base.ToString() + $", bad lisence id: {ID}";
    }

    public class ExceptionBLLinesStopHere : Exception
    {
        public string ID;
        public ExceptionBLLinesStopHere(string id) : base() => ID = id;
        public ExceptionBLLinesStopHere(string id, string message) : base(message) => ID = id;
        public ExceptionBLLinesStopHere(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bus stop : {ID} serves bus lines";
    }
    public class ExceptionBLBusLineExist : Exception
    {
        public string ID;
        public ExceptionBLBusLineExist(string id) : base() => ID = id;
        public ExceptionBLBusLineExist(string id, string message) : base(message) => ID = id;
        public ExceptionBLBusLineExist(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bus stop : {ID} serves bus lines";
    }

}

