﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]

    //---------------------buses and station bl exception-----------------------

    public class ExceptionBL_KeyNotFound : Exception
    {
        public int ID;
        public ExceptionBL_KeyNotFound(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDAL_KeyNotFound)innerException).ID;
        public override string ToString() => base.ToString() + $", bad license id: {ID}";
    }

    public class ExceptionBL_Inactive : Exception
    {
        public int ID;
        public ExceptionBL_Inactive(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDAL_Inactive)innerException).ID;
        public override string ToString() => base.ToString() + $", inactive: {ID}";
    }

    public class ExceptionBL_KeyAlreadyExist : Exception
    {
        public int ID;
        public ExceptionBL_KeyAlreadyExist(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDAL_KeyAlreadyExist)innerException).ID;
        public override string ToString() => base.ToString() + $", unexist: {ID}";
    }

    public class ExceptionBL_LinesStopHere : Exception
    {
        public string ID;
        public ExceptionBL_LinesStopHere(string id) : base() => ID = id;
        public ExceptionBL_LinesStopHere(string id, string message) : base(message) => ID = id;
        public ExceptionBL_LinesStopHere(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", bus stop : {ID} serves bus lines";
    }



    public class ExceptionBL_WrongDetails: Exception
    {
        public string ID;
        public ExceptionBL_WrongDetails(string id) : base() => ID = id;
        public ExceptionBL_WrongDetails(string id, string message) : base(message) => ID = id;
        public ExceptionBL_WrongDetails(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", The {ID} details are wrong";
    }

    public class ExceptionBL_BusStopNotExistInTheRoute : Exception
    {
        public string ID;
        public ExceptionBL_BusStopNotExistInTheRoute(string id) : base() => ID = id;
        public ExceptionBL_BusStopNotExistInTheRoute(string id, string message) : base(message) => ID = id;
        public ExceptionBL_BusStopNotExistInTheRoute(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", the bus stop code : {ID} doesn't exist in the bus line route";
    }

    /// <summary>
    /// Throws exception when the manager tries to delete station, with only two station left
    /// </summary>
    public class ExceptionBL_LessThanThreeStations : Exception
    {
        public int ID;
        public string ID1;

        public ExceptionBL_LessThanThreeStations(string id) : base() => ID1 = id;
        public ExceptionBL_LessThanThreeStations(int id) : base() => ID = id;
        public ExceptionBL_LessThanThreeStations(int id, string message) : base(message) => ID = id;
        public ExceptionBL_LessThanThreeStations(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", less than two station: {ID}";
    }

    /// <summary>
    /// Throws exception when the given bus stop coordinates are not in Israeli range
    /// </summary>
    public class ExceptionBL_Incorrect_coordinates : Exception
    {
        public string ID;

        public ExceptionBL_Incorrect_coordinates(string id) : base() => ID = id;
        public ExceptionBL_Incorrect_coordinates(string id, string message) : base(message) => ID = id;
        public ExceptionBL_Incorrect_coordinates(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $", The longitude or the latitude are incorrect: {ID}";
    }

    /// <summary>
    /// Throws exception when the mileage values of a bus are in logical conflict
    /// </summary>
    public class ExceptionBL_MileageValuesConflict : Exception
    {
        public string ID;

        public ExceptionBL_MileageValuesConflict(string id) : base() => ID = id;
        public ExceptionBL_MileageValuesConflict(string id, string message) : base(message) => ID = id;
        public ExceptionBL_MileageValuesConflict(string id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",  Conflict between mileage values: {ID}";
    }

    public class ExceptionBL_NoNeedToRefuel : Exception
    {
        public int ID;

        public ExceptionBL_NoNeedToRefuel(int id) : base() => ID = id;
        public ExceptionBL_NoNeedToRefuel(int id, string message) : base(message) => ID = id;
        public ExceptionBL_NoNeedToRefuel(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",  The gas tank is already full: {ID}";
    }

    public class ExceptionBL_NoNeedToTreat : Exception
    {
        public int ID;

        public ExceptionBL_NoNeedToTreat(int id) : base() => ID = id;
        public ExceptionBL_NoNeedToTreat(int id, string message) : base(message) => ID = id;
        public ExceptionBL_NoNeedToTreat(int id, string message, Exception innerException) : base(message, innerException) => ID = id;
        public override string ToString() => base.ToString() + $",  The bus doesn't need a refuel yet: {ID}";
    }

    //----------------------------user bl exception------------------------------
    public class ExceptionBL_UserKeyNotFound : Exception
    {
        public string ID;
        public ExceptionBL_UserKeyNotFound(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDAL_UserKeyNotFound)innerException).ID;
        public override string ToString() => base.ToString() + $", user not found: {ID}";
    }

    public class ExceptionBL_UserAlreadyExist : Exception
    {
        public string ID;
        public ExceptionBL_UserAlreadyExist(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDAL_UserAlreadyExist)innerException).ID;
        public override string ToString() => base.ToString() + $", user alredy exist: {ID}";
    }

    //----------------------------Unexpected problem------------------------------
    public class ExceptionBL_UnexpectedProblem : Exception
    {
        public string ID;
        public ExceptionBL_UnexpectedProblem(string id) : base() => ID = id;
        public ExceptionBL_UnexpectedProblem(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.ExceptionDAL_UnexpectedProblem)innerException).ID;
        public override string ToString() => base.ToString() + $", Unexpected Problem: {ID}";
    }


}

