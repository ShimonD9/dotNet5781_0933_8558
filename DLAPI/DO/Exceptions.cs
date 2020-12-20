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
}