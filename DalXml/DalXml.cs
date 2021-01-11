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
//                   );
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
//                                 where int.Parse(b.Element("License").Value) == bus.License
//                                 select b).FirstOrDefault();

//            if (existBus != null && bool.Parse(existBus.Element("ObjectActive").Value))
//                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.License, "License already exist");
//            if (existBus != null && !bool.Parse(existBus.Element("ObjectActive").Value))
//            {
//                existBus.Element("License").Value = bus.License.ToString();
//                existBus.Element("LicenseDate").Value = bus.LicenseDate.ToString();
//                existBus.Element("Mileage").Value = bus.Mileage.ToString();
//                existBus.Element("Fuel").Value = bus.Fuel.ToString();
//                existBus.Element("BusStatus").Value = bus.BusStatus.ToString();
//                existBus.Element("LastTreatmentDate").Value = bus.LastTreatmentDate.ToString();
//                existBus.Element("MileageAtLastTreat").Value = bus.MileageAtLastTreat.ToString();
//                //UpdateBus(bus);
//                existBus.Element("ObjectActive").Value = true.ToString();
//            }
//            else
//            {
//                XElement newBusElem = new XElement("Bus",
//                                       new XElement("License", bus.License),
//                                       new XElement("LicenseDate", bus.LicenseDate),
//                                       new XElement("Mileage", bus.Mileage),
//                                       new XElement("Fuel", bus.Fuel),
//                                       new XElement("BusStatus", bus.BusStatus),
//                                       new XElement("LastTreatmentDate", bus.LastTreatmentDate),
//                                       new XElement("MileageAtLastTreat", bus.MileageAtLastTreat),
//                                       new XElement("ObjectActive", bus.ObjectActive = true));
//                busRootElem.Add(newBusElem);
//            }

//            XMLTools.SaveListToXMLElement(busRootElem, busPath);
//        }

//        public void DeleteBus(int license)
//        {
//            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

//            XElement busToDelete = (from b in busRootElem.Elements()
//                                    where int.Parse(b.Element("License").Value) == license
//                                    select b).FirstOrDefault();

//            if (busToDelete == null)
//                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus not found: {license}");

//            if (!bool.Parse(busToDelete.Element("ObjectActive").Value))
//                throw new DO.ExceptionDAL_Inactive(license, $"Bus is alredy deleted: {license}");

//            if (bool.Parse(busToDelete.Element("ObjectActive").Value))
//            {
//                busToDelete.Element("ObjectActive").Value = false.ToString();
//                XMLTools.SaveListToXMLElement(busRootElem, busPath);
//            }
//            else
//                throw new DO.ExceptionDAL_UnexpectedProblem("Unexpected Problem");
//        }

//        public void UpdateBus(DO.Bus bus)
//        {
//            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

//            XElement busToUpdate = (from b in busRootElem.Elements()
//                                    where int.Parse(b.Element("License").Value) == bus.License
//                                    select b).FirstOrDefault();
//            if (busToUpdate == null)
//                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"Bus not found: {bus.License}");
//            if (!bool.Parse(busToUpdate.Element("ObjectActive").Value))
//                throw new DO.ExceptionDAL_Inactive(bus.License, $"The bus is inactive: {bus.License}");

//            if (bool.Parse(busToUpdate.Element("ObjectActive").Value))
//            {
//                busToUpdate.Element("License").Value = bus.License.ToString();
//                busToUpdate.Element("LicenseDate").Value = bus.LicenseDate.ToString();
//                busToUpdate.Element("Mileage").Value = bus.Mileage.ToString();
//                busToUpdate.Element("Fuel").Value = bus.Fuel.ToString();
//                busToUpdate.Element("BusStatus").Value = bus.BusStatus.ToString();
//                busToUpdate.Element("LastTreatmentDate").Value = bus.LastTreatmentDate.ToString();
//                busToUpdate.Element("MileageAtLastTreat").Value = bus.MileageAtLastTreat.ToString();
//                busToUpdate.Element("ObjectActive").Value = bus.ObjectActive.ToString();

//                XMLTools.SaveListToXMLElement(busRootElem, busPath);
//            }
//        }

//        public void UpdateBus(int id, Action<DO.Bus> update)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion Bus

//        #region BusAtTravel
//        public DO.BusAtTravel GetBusAtTravel(int license)
//        {
//            List<BusAtTravel> ListBusAtravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

//            DO.BusAtTravel bus = ListBusAtravel.Find(b => b.License == license);
//            if (bus == null)
//                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus not found: {license}");
//            if (!bus.ObjectActive)
//                throw new DO.ExceptionDAL_Inactive(license, $"Bus is inactive: {license}");
//            return bus;  //no need to Clone()          
//        }
//        public void AddBusAtTravel(DO.BusAtTravel bus)
//        {
//            List<BusAtTravel> ListBusAtravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

//            BusAtTravel existBus = ListBusAtravel.FirstOrDefault(b => b.License == bus.License);
//            if (existBus != null && existBus.ObjectActive)
//                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.License, "Bus already in travel");

//            if (existBus != null && !existBus.ObjectActive)
//            {
//                existBus.ObjectActive = true;
//                existBus = bus;
//            }
//            else
//            {
//                bus.ObjectActive = true;
//                ListBusAtravel.Add(bus); //no need to Clone()
//            }
//            XMLTools.SaveListToXMLSerializer(ListBusAtravel, busAtTravelPath);

//        }
//        public IEnumerable<DO.BusAtTravel> GetAllBusesAtTravel()
//        {
//            List<BusAtTravel> ListBusAtravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

//            return from bus in ListBusAtravel
//                   where bus.ObjectActive == true
//                   select bus; //no need to Clone()
//        }
//        IEnumerable<BusAtTravel> GetAllBusesAtTravelBy(Predicate<BusAtTravel> predicate)
//        {
//            throw new NotImplementedException();
//        }

//        public void UpdateBusAtTravel(DO.BusAtTravel bus)
//        {
//            List<BusAtTravel> ListBusAtTravels = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);
//            int index = ListBusAtTravels.FindIndex(bus1 => bus1.License == bus.License);  //get index to update bus
//            if (ListBusAtTravels[index] == null)
//                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"The bus not found: {bus.License}");
//            if (!ListBusAtTravels[index].ObjectActive)
//                throw new DO.ExceptionDAL_Inactive(bus.License, $"The bus is inactive: {bus.License}");

//                ListBusAtTravels[index] = bus;

//            XMLTools.SaveListToXMLSerializer(ListBusAtTravels, busAtTravelPath);
//        }

//        void UpdateBusAtTravel(int license, Action<BusAtTravel> update) // method that knows to updt specific fields in bus at travel
//        {
//            throw new NotImplementedException();
//        }


//        public void DeleteBusAtTravel(int license)
//        {
//            List<Student> ListBusAtTravel = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

//            DO.Student stu = ListBusAtTravel.Find(p => p.ID == license);

//            if (stu != null)
//            {
//                ListBusAtTravel.Remove(stu);
//            }
//            else
//                throw new DO.BadPersonIdException(license, $"bad student id: {license}");

//            XMLTools.SaveListToXMLSerializer(ListBusAtTravel, studentsPath);
//        }
//        #endregion Student

//        #region StudentInCourse
//        public IEnumerable<DO.StudentInCourse> GetStudentsInCourseList(Predicate<DO.StudentInCourse> predicate)
//        {
//            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

//            return from sic in ListStudInCourses
//                   where predicate(sic)
//                   select sic; //no need to Clone()
//        }
//        public void AddStudentInCourse(int perID, int courseID, float grade = 0)
//        {
//            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

//            if (ListStudInCourses.FirstOrDefault(cis => (cis.PersonId == perID && cis.CourseId == courseID)) != null)
//                throw new DO.BadPersonIdCourseIDException(perID, courseID, "person ID is already registered to course ID");

//            DO.StudentInCourse sic = new DO.StudentInCourse() { PersonId = perID, CourseId = courseID, Grade = grade };

//            ListStudInCourses.Add(sic);

//            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);
//        }

//        public void UpdateStudentGradeInCourse(int perID, int courseID, float grade)
//        {
//            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

//            DO.StudentInCourse sic = ListStudInCourses.Find(cis => (cis.PersonId == perID && cis.CourseId == courseID));

//            if (sic != null)
//            {
//                sic.Grade = grade;
//            }
//            else
//                throw new DO.BadPersonIdCourseIDException(perID, courseID, "person ID is NOT registered to course ID");

//            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);
//        }

//        public void DeleteStudentInCourse(int perID, int courseID)
//        {
//            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

//            DO.StudentInCourse sic = ListStudInCourses.Find(cis => (cis.PersonId == perID && cis.CourseId == courseID));

//            if (sic != null)
//            {
//                ListStudInCourses.Remove(sic);
//            }
//            else
//                throw new DO.BadPersonIdCourseIDException(perID, courseID, "person ID is NOT registered to course ID");

//            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);

//        }
//        public void DeleteStudentFromAllCourses(int perID)
//        {
//            List<StudentInCourse> ListStudInCourses = XMLTools.LoadListFromXMLSerializer<StudentInCourse>(studInCoursesPath);

//            ListStudInCourses.RemoveAll(p => p.PersonId == perID);

//            XMLTools.SaveListToXMLSerializer(ListStudInCourses, studInCoursesPath);

//        }

//        #endregion StudentInCourse

//        #region Course
//        public DO.Course GetCourse(int id)
//        {
//            List<Course> ListCourses = XMLTools.LoadListFromXMLSerializer<Course>(coursesPath);

//            return ListCourses.Find(c => c.ID == id); //no need to Clone()

//            //if not exist throw exception etc.
//        }

//        public IEnumerable<DO.Course> GetAllCourses()
//        {
//            List<Course> ListCourses = XMLTools.LoadListFromXMLSerializer<Course>(coursesPath);

//            return from course in ListCourses
//                   select course; //no need to Clone()
//        }

//        #endregion Course

//        #region Lecturer
//        public IEnumerable<DO.LecturerInCourse> GetLecturersInCourseList(Predicate<DO.LecturerInCourse> predicate)
//        {
//            List<LecturerInCourse> ListLectInCourses = XMLTools.LoadListFromXMLSerializer<LecturerInCourse>(lectInCoursesPath);

//            return from sic in ListLectInCourses
//                   where predicate(sic)
//                   select sic; //no need to Clone()
//        }
//        #endregion


//    }
//}