using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class ExceptionDALBadLicense : Exception
    {
        public int ID;
        public ExceptionDALBadLicense(int id) : base() => ID = id;
        public ExceptionDALBadLicense(int id, string message) : base(message) => ID = id;
        public ExceptionDALBadLicense(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id: {ID}";
    }

    public class ExceptionDALunexist : Exception
    {
        public int ID, ID_B;
        public ExceptionDALunexist(int id) : base() => ID = id;
        public ExceptionDALunexist(int id, string message) : base(message) => ID = id;
        public ExceptionDALunexist(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public ExceptionDALunexist(int id, int idB, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", doesn't exist: {ID}";
    }

    public class ExceptionDALBadIdUser : Exception
    {
        public string ID;
        public ExceptionDALBadIdUser(string id) : base() => ID = id;
        public ExceptionDALBadIdUser(string id, string message) : base(message) => ID = id;
        public ExceptionDALBadIdUser(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id : {ID}";
    }
    public class ExceptionDALInactive : Exception
    {
        public int ID;
        public ExceptionDALInactive(int id) : base() => ID = id;
        public ExceptionDALInactive(int id, string message) : base(message) => ID = id;
        public ExceptionDALInactive(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id : {ID}";
    }
    public class ExceptionDALInactiveUser : Exception
    {
        public string ID;
        public ExceptionDALInactiveUser(string id) : base() => ID = id;
        public ExceptionDALInactiveUser(string id, string message) : base(message) => ID = id;
        public ExceptionDALInactiveUser(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id : {ID}";
    }

}