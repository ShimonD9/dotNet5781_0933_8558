using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]

    //---------------------Buses and station dl exception-----------------------
    public class ExceptionDAL_KeyNotFound : Exception
    {
        public int ID;
        public string ID1;

        public ExceptionDAL_KeyNotFound(string id) : base() => ID1 = id;
        public ExceptionDAL_KeyNotFound(int id) : base() => ID = id;
        public ExceptionDAL_KeyNotFound(int id, string message) : base(message) => ID = id;
        public ExceptionDAL_KeyNotFound(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id: {ID}";
    }
    public class ExceptionDAL_KeyAlreadyExist : Exception
    {
        public int ID;

        public string ID1;

        public ExceptionDAL_KeyAlreadyExist(string id) : base() => ID1 = id;
        public ExceptionDAL_KeyAlreadyExist(int id) : base() => ID = id;
        public ExceptionDAL_KeyAlreadyExist(int id, string message) : base(message) => ID = id;
        public ExceptionDAL_KeyAlreadyExist(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public ExceptionDAL_KeyAlreadyExist(int id, int idB, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", doesn't exist: {ID}";
    } 
    public class ExceptionDAL_Inactive : Exception
    {
        public int ID;

        public string ID1;
        public ExceptionDAL_Inactive(int id) : base() => ID = id;

        public ExceptionDAL_Inactive(string id) : base() => ID1 = id;

        public ExceptionDAL_Inactive(int id, string message) : base(message) => ID = id;
        public ExceptionDAL_Inactive(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id : {ID}";
    }
    public class ExceptionDAL_ExistConsStations : Exception
    {
        public string ID;

        public ExceptionDAL_ExistConsStations(string id) : base() => ID = id;
        public ExceptionDAL_ExistConsStations(string id, string message) : base(message) => ID = id;
        public ExceptionDAL_ExistConsStations(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", Exist Conscutive Stations : {ID}";
    }

    //----------------------------User dl exception------------------------------
    public class ExceptionDAL_UserKeyNotFound : Exception
    {
        public string ID;
        public ExceptionDAL_UserKeyNotFound(string id) : base() => ID = id;
        public ExceptionDAL_UserKeyNotFound(string id, string message) : base(message) => ID = id;
        public ExceptionDAL_UserKeyNotFound(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id : {ID}";
    }
    public class ExceptionDAL_UserAlreadyExist : Exception
    {
        public string ID;
        public ExceptionDAL_UserAlreadyExist(string id) : base() => ID = id;
        public ExceptionDAL_UserAlreadyExist(string id, string message) : base(message) => ID = id;
        public ExceptionDAL_UserAlreadyExist(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id : {ID}";
    }
    public class ExceptionDAL_InactiveUser : Exception
    {
        public string ID;
        public ExceptionDAL_InactiveUser(string id) : base() => ID = id;
        public ExceptionDAL_InactiveUser(string id, string message) : base(message) => ID = id;
        public ExceptionDAL_InactiveUser(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bad id : {ID}";
    }

    //----------------------------Unexpected problem------------------------------
    public class ExceptionDAL_UnexpectedProblem : Exception
    {
        public string ID;
        public ExceptionDAL_UnexpectedProblem(string id) : base() => ID = id;
        public ExceptionDAL_UnexpectedProblem(string id, string message) : base(message) => ID = id;
        public ExceptionDAL_UnexpectedProblem(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",Unexpected Problem : {ID}";
    }

    //----------------------------Xml------------------------------
    public class ExceptionDAL_XMLFileLoadCreate : Exception
    {
        public string ID;
        public ExceptionDAL_XMLFileLoadCreate(string id) : base() => ID = id;
        public ExceptionDAL_XMLFileLoadCreate(string id, string message) : base(message) => ID = id;
        public ExceptionDAL_XMLFileLoadCreate(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",Load fail : {ID}";
    }
    public class ExceptionDAL_XMLFileLoadCreateException : Exception
    {
        public string ID;
        public ExceptionDAL_XMLFileLoadCreateException(string id) : base() => ID = id;
        public ExceptionDAL_XMLFileLoadCreateException(string id, string message) : base(message) => ID = id;
        public ExceptionDAL_XMLFileLoadCreateException(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",Load fail: {ID}";
    }
}