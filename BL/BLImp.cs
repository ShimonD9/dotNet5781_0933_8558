using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BLApi;
using System.Threading;

using BO;


namespace BL
{
    sealed class BLImp : IBL //internal
    {
       IDal dl = DalFactory.GetDL();
        BO.Bus busDoBoAdapter(DO.Bus busDO)
        {
            BO.Bus busBO = new BO.Bus();
            int id = busDO.License;
            busDO.CopyPropertiesTo(busBO);
            return busBO;
        }

        #region Bus
        public IEnumerable<Bus> GetAllBuses()
        {
            return from doBus in dl.GetAllBuses() select busDoBoAdapter(doBus);
            throw new NotImplementedException();
            //return from bus in DS.ListBuses
            //       select bus.Clone();
        }

        public IEnumerable<Bus> GetAllBusesBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
            //return from bus in DataSource.ListBuses
            //       where predicate(bus)
            //       select bus.Clone();
        }

        public Bus GetBus(int license)
        {
            throw new NotImplementedException();
            //DO.Bus busDO;
            //try
            //{
            //    busDO = dl.GetBus(license);
            //}
            //catch (DO.BadIdException ex)
            //{
            //    throw new BO.BadIdException("Person id does not exist or he is not a student", ex);
            //}
            //return studentDoBoAdapter(studentDO);
        }

        public void AddBus(Bus bus)
        {
            throw new NotImplementedException();
            //if (DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License) != null &&
            //    DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License).ObjectActive == true)
            //    throw new DO.BadIdException(bus.License, "Duplicate bus ID");
            //else if (DataSource.ListBuses.FirstOrDefault(b => b.License == bus.License).ObjectActive == false)
            //{
            //    Bus addBus = DataSource.ListBuses.Find(b => b.License == bus.License);
            //    addBus.ObjectActive = true;
            //    addBus = bus.Clone();
            //}
            //else
            //    DataSource.ListBuses.Add(bus.Clone());
        }
        public void UpdateBus(Bus bus) //busUpdate
        {
            throw new NotImplementedException();
            //Bus busUpdate = DataSource.ListBuses.Find(b => b.License == bus.License);
            //if (busUpdate != null && busUpdate.ObjectActive)
            //    busUpdate = bus.Clone();
            //else if (!busUpdate.ObjectActive)
            //    throw new DO.InactiveBusException(bus.License, $"the bus is  inactive");
            //else
            //    throw new DO.BadIdException(bus.License, $"bad id: {bus.License}");
        }
        public void UpdateBus(int licenseNumber, Action<Bus> update)  // method that knows to update specific fields in Person
        {
            throw new NotImplementedException();
            //Bus busUpdate = GetBus(licenseNumber);
            //update(busUpdate);
        }
        public void DeleteBus(int license)
        {
            throw new NotImplementedException();
            //Bus bus = DataSource.ListBuses.Find(b => b.License == license);
            //if (bus != null && bus.ObjectActive)
            //    bus.ObjectActive = false;
            //else if (!bus.ObjectActive)
            //    throw new DO.InactiveBusException(bus.License, $"the bus is  inactive");
            //else
            //    throw new DO.BadIdException(bus.License, $"bad id: {bus.License}");
        }
        #endregion



        //public BO.Student GetStudent(int id)
        //{
        //    BO.Student studentBO = new BO.Student();

        //    DO.Person personDO;
        //    try
        //    {
        //        personDO = dl.GetPerson(id);
        //    }
        //    catch (DO.BadPersonIdException ex)
        //    {
        //        throw new BO.BadStudentIdException("Student ID is illegal", ex);
        //    }
        //    personDO.Clone(studentBO);
        //    //studentBO.ID = personDO.ID;
        //    //studentBO.BirthDate = personDO.BirthDate;
        //    //studentBO.City = personDO.City;
        //    //studentBO.Name = personDO.Name;
        //    //studentBO.HouseNumber = personDO.HouseNumber;
        //    //studentBO.Street = personDO.Street;
        //    //studentBO.PersonalStatus = (BO.PersonalStatus)(int)personDO.PersonalStatus;

        //    DO.Student studentDO = dl.GetStudent(id);
        //    studentDO.Clone(studentBO);
        //    //studentBO.StartYear = studentDO.StartYear;
        //    //studentBO.Status = (BO.StudentStatus)(int)studentDO.Status;
        //    //studentBO.Graduation = (BO.StudentGraduate)(int)studentDO.Graduation;

        //    studentBO.ListOfCourses = from sic in dl.GetStudentInCourseList(sic => sic.PersonId == id)
        //                              let course = dl.GetCourse(sic.CourseId)
        //                              select course.CloneToStudentCourse(sic);
        //    //new BO.StudentCourse()
        //    //{
        //    //    ID = course.ID,
        //    //    Number = course.Number,
        //    //    Name = course.Name,
        //    //    Year = course.Year,
        //    //    Semester = (BO.Semester)(int)course.Semester,
        //    //    Grade = sic.Grade
        //    //};
        //    return studentBO;
        //}


        //public IEnumerable<BO.Student> GetAllStudents()
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<BO.Student> GetStudentsBy(Predicate<BO.Student> predicate)
        //{
        //    throw new NotImplementedException();
        //}


        //public IEnumerable<BO.ListedPerson> GetStudentIDs()
        //{
        //    return from item in dl.GetStudentIDs((id, name) =>
        //    {
        //        try { Thread.Sleep(1500); } catch (ThreadInterruptedException e) { }
        //        return new BO.ListedPerson() { ID = id, Name = name };
        //    })
        //           let student = item as BO.ListedPerson
        //           orderby student.ID
        //           select student;
        //}
    }
}