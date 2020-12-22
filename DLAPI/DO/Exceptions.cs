using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class BadIdException : Exception
    {
        public int ID;
        public BadIdException(int id) : base() => ID = id;
        public BadIdException(int id, string message) : base(message) => ID = id;
        public BadIdException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id: {ID}";
    }
    public class BadIdUserException : Exception
    {
        public string ID;
        public BadIdUserException(string id) : base() => ID = id;
        public BadIdUserException(string id, string message) : base(message) => ID = id;
        public BadIdUserException(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad dog : {ID}";
    }
    public class InactiveBusException : Exception
    {
        public int ID;
        public InactiveBusException(int id) : base() => ID = id;
        public InactiveBusException(int id, string message) : base(message) => ID = id;
        public InactiveBusException(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad dog : {ID}";
    }
    public class InactiveUserException : Exception
    {
        public string ID;
        public InactiveUserException(string id) : base() => ID = id;
        public InactiveUserException(string id, string message) : base(message) => ID = id;
        public InactiveUserException(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad dog : {ID}";
    }

}