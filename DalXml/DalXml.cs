//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;
//using DalApi;
//using DO;



//namespace DL
//{
//    sealed class DalXml : IDal    //internal
//    {
//        #region singelton
//        static readonly DalXml instance = new DalXml();
//        static DalXml() { }// static ctor to ensure instance init is done just before first usage
//        DalXml() { } // default => private
//        public static DalXml Instance { get => instance; }// The public Instance property to use
//        #endregion

//        #region DS XML Files

//        string busPath = @"BusXml.xml"; //XElement

//        string busAtTravelPath = @"BusAtTravelXml.xml"; //XMLSerializer
//        string busLinePath = @"BusLineXml.xml"; //XMLSerializer
//        string busLineStationPath = @"BusLineStationXml.xml"; //XMLSerializer
//        string busStopPath = @"BusStopXml.xml"; //XMLSerializer
//        string consecutiveStationPath = @"ConsecutiveStationXml.xml"; //XMLSerializer
//        string lineDeparturePath = @"LineDepartureXml.xml"; //XMLSerializer
//        string userPath = @"UserXml.xml"; //XMLSerializer



//        #endregion

//        #region Bus
//        public DO.Bus GetBus(int license)
//        {
//            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

//            Bus bus = (from b in busRootElem.Elements()
//                       where int.Parse(b.Element("License").Value) == license
//                       select new Bus()
//                       {
//                           License = Int32.Parse(b.Element("License").Value),
//                           LicenseDate = DateTime.Parse(b.Element("LicenseDate").Value),
//                           Mileage = double.Parse(b.Element("Mileage").Value),
//                           Fuel = double.Parse(b.Element("Fuel").Value),
//                           BusStatus = (Enums.BUS_STATUS)Enum.Parse(typeof(Enums.BUS_STATUS), b.Element("BusStatus").Value),
//                           LastTreatmentDate = DateTime.Parse(b.Element("LastTreatmentDate").Value),
//                           MileageAtLastTreat = double.Parse(b.Element("MileageAtLastTreat").Value),
//                           ObjectActive = bool.Parse(b.Element("ObjectActive").Value)
//                       }
//                        ).FirstOrDefault();

//            if (bus == null)
//                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus license not found: {license}");
//            else if (bus != null && !bus.ObjectActive)
//                throw new DO.ExceptionDAL_Inactive(license, $"the bus is  inactive");
//            return bus;
//        }
//        public IEnumerable<DO.Bus> GetAllBuses()
//        {
//            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

//            return (from b in busRootElem.Elements()
//                    where bool.Parse(b.Element("ObjectActive").Value) == true
//                    select new Bus()
//                    {
//                        License = Int32.Parse(b.Element("License").Value),
//                        LicenseDate = DateTime.Parse(b.Element("LicenseDate").Value),
//                        Mileage = double.Parse(b.Element("Mileage").Value),
//                        Fuel = double.Parse(b.Element("Fuel").Value),
//                        BusStatus = (Enums.BUS_STATUS)Enum.Parse(typeof(Enums.BUS_STATUS), b.Element("BusStatus").Value),
//                        LastTreatmentDate = DateTime.Parse(b.Element("LastTreatmentDate").Value),
//                        MileageAtLastTreat = double.Parse(b.Element("MileageAtLastTreat").Value),
//                        ObjectActive = bool.Parse(b.Element("ObjectActive").Value)
//                    }
//                    );
//        }

//        public IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate)
//        {
//            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

//            return from b in busRootElem.Elements()
//                   let bus = new Bus()
//                   {
//                       License = Int32.Parse(b.Element("License").Value),
//                       LicenseDate = DateTime.Parse(b.Element("LicenseDate").Value),
//                       Mileage = double.Parse(b.Element("Mileage").Value),
//                       Fuel = double.Parse(b.Element("Fuel").Value),
//                       BusStatus = (Enums.BUS_STATUS)Enum.Parse(typeof(Enums.BUS_STATUS), b.Element("BusStatus").Value),
//                       LastTreatmentDate = DateTime.Parse(b.Element("LastTreatmentDate").Value),
//                       MileageAtLastTreat = double.Parse(b.Element("MileageAtLastTreat").Value),
//                       ObjectActive = bool.Parse(b.Element("ObjectActive").Value)
//                   }
//                   where predicate(bus)
//                   select bus;
//        }
//        public void AddBus(DO.Bus bus)
//        {
//            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

//            XElement existBus = (from b in busRootElem.Elements()
//                               where int.Parse(b.Element("License").Value) == bus.License
//                               select b).FirstOrDefault();

//            if (existBus != null && bool.Parse(existBus.Element("ObjectActive").Value))
//                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.License, "License already exist");
//            if (existBus != null && !bool.Parse(existBus.Element("ObjectActive").Value))
//                throw new DO.ExceptionDAL_Inactive(bus.License, "Bus is inactive");

//            XElement newBusElem = new XElement("Bus",
//                                   new XElement("License", bus.License),
//                                   new XElement("LicenseDate", bus.LicenseDate),
//                                   new XElement("Mileage", bus.Mileage),
//                                   new XElement("Fuel", bus.Fuel),
//                                   new XElement("BusStatus", bus.BusStatus),
//                                   new XElement("LastTreatmentDate", bus.LastTreatmentDate),
//                                   new XElement("MileageAtLastTreat", bus.MileageAtLastTreat),
//                                   new XElement("ObjectActive", bus.ObjectActive = true));

//            busRootElem.Add(newBusElem);

//            XMLTools.SaveListToXMLElement(busRootElem, busPath);
//        }

//        public void DeleteBus(int license)
//        {
//            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);
//            Bus busTodelete1 = GetBus(license);
//            XElement busTodelete = (from b in busRootElem.Elements()
//                            where int.Parse(b.Element("License").Value) == license
//                            select b).FirstOrDefault();

//            if (busTodelete != null && bool.Parse(busTodelete.Element("ObjectActive").Value))
//            {
//                busTodelete.Element("ObjectActive").Value = busTodelete1.ObjectActive;
//                XMLTools.SaveListToXMLElement(busRootElem, busPath);
//            }
//            else
//                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus not found: {license}");
//        }

////        public void UpdatePerson(DO.Person person)
////        {
////            XElement personsRootElem = XMLTools.LoadListFromXMLElement(personsPath);

////            XElement per = (from p in personsRootElem.Elements()
////                            where int.Parse(p.Element("ID").Value) == person.ID
////                            select p).FirstOrDefault();

////            if (per != null)
////            {
////                per.Element("ID").Value = person.ID.ToString();
////                per.Element("Name").Value = person.Name;
////                per.Element("Street").Value = person.Street;
////                per.Element("HouseNumber").Value = person.HouseNumber.ToString();
////                per.Element("City").Value = person.City;
////                per.Element("BirthDate").Value = person.BirthDate.ToString();
////                per.Element("PersonalStatus").Value = person.PersonalStatus.ToString();

////                XMLTools.SaveListToXMLElement(personsRootElem, personsPath);
////            }
////            else
////                throw new DO.BadPersonIdException(person.ID, $"bad person id: {person.ID}");
////        }

////        public void UpdatePerson(int id, Action<DO.Person> update)
////        {
////            throw new NotImplementedException();
////        }

////        #endregion Person

////        #region Student
////        public DO.Student GetStudent(int id)
////        {
////            List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

////            DO.Student stu = ListStudents.Find(p => p.ID == id);
////            if (stu != null)
////                return stu; //no need to Clone()
////            else
////                throw new DO.BadPersonIdException(id, $"bad student id: {id}");
////        }
////        public void AddStudent(DO.Student student)
////        {
////            List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

////            if (ListStudents.FirstOrDefault(s => s.ID == student.ID) != null)
////                throw new DO.BadPersonIdException(student.ID, "Duplicate student ID");

////            if (GetPerson(student.ID) == null)
////                throw new DO.BadPersonIdException(student.ID, "Missing person ID");

////            ListStudents.Add(student); //no need to Clone()

////            XMLTools.SaveListToXMLSerializer(ListStudents, studentsPath);

////        }
////        public IEnumerable<DO.Student> GetAllStudents()
////        {
////            List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

////            return from student in ListStudents
////                   select student; //no need to Clone()
////        }
////        public IEnumerable<object> GetStudentFields(Func<int, string, object> generate)
////        {
////            List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

////            return from student in ListStudents
////                   select generate(student.ID, GetPerson(student.ID).Name);
////        }

////        public IEnumerable<object> GetStudentListWithSelectedFields(Func<DO.Student, object> generate)
////        {
////            List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

////            return from student in ListStudents
////                   select generate(student);
////        }
////        public void UpdateStudent(DO.Student student)
////        {
////            List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

////            DO.Student stu = ListStudents.Find(p => p.ID == student.ID);
////            if (stu != null)
////            {
////                ListStudents.Remove(stu);
////                ListStudents.Add(student); //no nee to Clone()
////            }
////            else
////                throw new DO.BadPersonIdException(student.ID, $"bad student id: {student.ID}");

////            XMLTools.SaveListToXMLSerializer(ListStudents, studentsPath);
////        }

////        public void UpdateStudent(int id, Action<DO.Student> update)
////        {
////            throw new NotImplementedException();
////        }

////        public void DeleteStudent(int id)
////        {
////            List<Student> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

////            DO.Student stu = ListStudents.Find(p => p.ID == id);

////            if (stu != null)
////            {
////                ListStudents.Remove(stu);
////            }
////            else
////                throw new DO.BadPersonIdException(id, $"bad student id: {id}");

////            XMLTools.SaveListToXMLSerializer(ListStudents, studentsPath);
////        }
////        #endregion Student

////        #region StudentInCourse
////        public IEnumerable<DO.StudentInCourse> GetStudentsInCourseList(Predicate<DO.StudentInCourse> predicate)
////        {
////            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

////            return from sic in ListStudInCourses
////                   where predicate(sic)
////                   select sic; //no need to Clone()
////        }
////        public void AddStudentInCourse(int perID, int courseID, float grade = 0)
////        {
////            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

////            if (ListStudInCourses.FirstOrDefault(cis => (cis.PersonId == perID && cis.CourseId == courseID)) != null)
////                throw new DO.BadPersonIdCourseIDException(perID, courseID, "person ID is already registered to course ID");

////            DO.StudentInCourse sic = new DO.StudentInCourse() { PersonId = perID, CourseId = courseID, Grade = grade };

////            ListStudInCourses.Add(sic);

////            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);
////        }

////        public void UpdateStudentGradeInCourse(int perID, int courseID, float grade)
////        {
////            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

////            DO.StudentInCourse sic = ListStudInCourses.Find(cis => (cis.PersonId == perID && cis.CourseId == courseID));

////            if (sic != null)
////            {
////                sic.Grade = grade;
////            }
////            else
////                throw new DO.BadPersonIdCourseIDException(perID, courseID, "person ID is NOT registered to course ID");

////            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);
////        }

////        public void DeleteStudentInCourse(int perID, int courseID)
////        {
////            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

////            DO.StudentInCourse sic = ListStudInCourses.Find(cis => (cis.PersonId == perID && cis.CourseId == courseID));

////            if (sic != null)
////            {
////                ListStudInCourses.Remove(sic);
////            }
////            else
////                throw new DO.BadPersonIdCourseIDException(perID, courseID, "person ID is NOT registered to course ID");

////            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);

////        }
////        public void DeleteStudentFromAllCourses(int perID)
////        {
////            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

////            ListStudInCourses.RemoveAll(p => p.PersonId == perID);

////            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);

////        }

////        #endregion StudentInCourse

////        #region Course
////        public DO.Course GetCourse(int id)
////        {
////            List<Course> ListCourses = XMLTools.LoadListFromXMLSerializer<Course>(coursesPath);

////            return ListCourses.Find(c => c.ID == id); //no need to Clone()

////            //if not exist throw exception etc.
////        }

////        public IEnumerable<DO.Course> GetAllCourses()
////        {
////            List<Course> ListCourses = XMLTools.LoadListFromXMLSerializer<Course>(coursesPath);

////            return from course in ListCourses
////                   select course; //no need to Clone()
////        }

////        #endregion Course

////        #region Lecturer
////        public IEnumerable<DO.LecturerInCourse> GetLecturersInCourseList(Predicate<DO.LecturerInCourse> predicate)
////        {
////            List<LecturerInCourse> ListLectInCourses = XMLTools.LoadListFromXMLSerializer<LecturerInCourse>(lectInCoursesPath);

////            return from sic in ListLectInCourses
////                   where predicate(sic)
////                   select sic; //no need to Clone()
////        }
////        #endregion


////    }
////}