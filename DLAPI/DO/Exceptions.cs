using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class ExceptionDALBadLicsens : Exception
    {
        public int ID;
        public ExceptionDALBadLicsens(int id) : base() => ID = id;
        public ExceptionDALBadLicsens(int id, string message) : base(message) => ID = id;
        public ExceptionDALBadLicsens(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id: {ID}";
    }
    public class ExceptionDALBadIdUser : Exception
    {
        public string ID;
        public ExceptionDALBadIdUser(string id) : base() => ID = id;
        public ExceptionDALBadIdUser(string id, string message) : base(message) => ID = id;
        public ExceptionDALBadIdUser(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad dog : {ID}";
    }
    public class ExceptionDALInactiveBus : Exception
    {
        public int ID;
        public ExceptionDALInactiveBus(int id) : base() => ID = id;
        public ExceptionDALInactiveBus(int id, string message) : base(message) => ID = id;
        public ExceptionDALInactiveBus(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad dog : {ID}";
    }
    public class ExceptionDALInactiveUser : Exception
    {
        public string ID;
        public ExceptionDALInactiveUser(string id) : base() => ID = id;
        public ExceptionDALInactiveUser(string id, string message) : base(message) => ID = id;
        public ExceptionDALInactiveUser(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad dog : {ID}";
    }

}