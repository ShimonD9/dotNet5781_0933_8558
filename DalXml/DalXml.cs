using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DalApi;
using DO;



namespace DL
{
    sealed class DalXml : IDal    //internal
    {
        #region singelton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }// static ctor to ensure instance init is done just before first usage
        DalXml() { } // default => private
        public static DalXml Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        string busPath = @"BusXml.xml"; //XElement

        string busAtTravelPath = @"BusAtTravelXml.xml"; //XMLSerializer
        string busLinePath = @"BusLineXml.xml"; //XMLSerializer
        string busLineStationPath = @"BusLineStationXml.xml"; //XMLSerializer
        string busStopPath = @"BusStopXml.xml"; //XMLSerializer
        string consecutiveStationPath = @"ConsecutiveStationXml.xml"; //XMLSerializer
        string lineDeparturePath = @"LineDepartureXml.xml"; //XMLSerializer
        string userPath = @"UserXml.xml"; //XMLSerializer

        #endregion

        #region Bus
        public DO.Bus GetBus(int license)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

            Bus bus = (from b in busRootElem.Elements()
                       where int.Parse(b.Element("License").Value) == license
                       select new Bus()
                       {
                           License = Int32.Parse(b.Element("License").Value),
                           LicenseDate = DateTime.Parse(b.Element("LicenseDate").Value),
                           Mileage = double.Parse(b.Element("Mileage").Value),
                           Fuel = double.Parse(b.Element("Fuel").Value),
                           BusStatus = (Enums.BUS_STATUS)Enum.Parse(typeof(Enums.BUS_STATUS), b.Element("BusStatus").Value),
                           LastTreatmentDate = DateTime.Parse(b.Element("LastTreatmentDate").Value),
                           MileageAtLastTreat = double.Parse(b.Element("MileageAtLastTreat").Value),
                           ObjectActive = bool.Parse(b.Element("ObjectActive").Value)
                       }
                        ).FirstOrDefault();

            if (bus == null)
                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus license not found: {license}");
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(license, $"the bus is  inactive");
            return bus;
        }
        public IEnumerable<DO.Bus> GetAllBuses()
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

            return (from b in busRootElem.Elements()
                    where bool.Parse(b.Element("ObjectActive").Value) == true
                    select new Bus()
                    {
                        License = Int32.Parse(b.Element("License").Value),
                        LicenseDate = DateTime.Parse(b.Element("LicenseDate").Value),
                        Mileage = double.Parse(b.Element("Mileage").Value),
                        Fuel = double.Parse(b.Element("Fuel").Value),
                        BusStatus = (Enums.BUS_STATUS)Enum.Parse(typeof(Enums.BUS_STATUS), b.Element("BusStatus").Value),
                        LastTreatmentDate = DateTime.Parse(b.Element("LastTreatmentDate").Value),
                        MileageAtLastTreat = double.Parse(b.Element("MileageAtLastTreat").Value),
                        ObjectActive = bool.Parse(b.Element("ObjectActive").Value)
                    }
                   );
        }
        public IEnumerable<DO.Bus> GetAllBusesBy(Predicate<DO.Bus> predicate)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

            return from b in busRootElem.Elements()
                   let bus = new Bus()
                   {
                       License = Int32.Parse(b.Element("License").Value),
                       LicenseDate = DateTime.Parse(b.Element("LicenseDate").Value),
                       Mileage = double.Parse(b.Element("Mileage").Value),
                       Fuel = double.Parse(b.Element("Fuel").Value),
                       BusStatus = (Enums.BUS_STATUS)Enum.Parse(typeof(Enums.BUS_STATUS), b.Element("BusStatus").Value),
                       LastTreatmentDate = DateTime.Parse(b.Element("LastTreatmentDate").Value),
                       MileageAtLastTreat = double.Parse(b.Element("MileageAtLastTreat").Value),
                       ObjectActive = bool.Parse(b.Element("ObjectActive").Value)
                   }
                   where predicate(bus)
                   select bus;
        }
        public void AddBus(DO.Bus bus)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

            XElement existBus = (from b in busRootElem.Elements()
                                 where int.Parse(b.Element("License").Value) == bus.License
                                 select b).FirstOrDefault();

            if (existBus != null && bool.Parse(existBus.Element("ObjectActive").Value))
                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.License, "License already exist");
            if (existBus != null && !bool.Parse(existBus.Element("ObjectActive").Value))
            {
                existBus.Element("License").Value = bus.License.ToString();
                existBus.Element("LicenseDate").Value = bus.LicenseDate.ToString();
                existBus.Element("Mileage").Value = bus.Mileage.ToString();
                existBus.Element("Fuel").Value = bus.Fuel.ToString();
                existBus.Element("BusStatus").Value = bus.BusStatus.ToString();
                existBus.Element("LastTreatmentDate").Value = bus.LastTreatmentDate.ToString();
                existBus.Element("MileageAtLastTreat").Value = bus.MileageAtLastTreat.ToString();
                //UpdateBus(bus);
                existBus.Element("ObjectActive").Value = true.ToString();
            }
            else
            {
                XElement newBusElem = new XElement("Bus",
                                       new XElement("License", bus.License),
                                       new XElement("LicenseDate", bus.LicenseDate),
                                       new XElement("Mileage", bus.Mileage),
                                       new XElement("Fuel", bus.Fuel),
                                       new XElement("BusStatus", bus.BusStatus),
                                       new XElement("LastTreatmentDate", bus.LastTreatmentDate),
                                       new XElement("MileageAtLastTreat", bus.MileageAtLastTreat),
                                       new XElement("ObjectActive", bus.ObjectActive = true));
                busRootElem.Add(newBusElem);
            }

            XMLTools.SaveListToXMLElement(busRootElem, busPath);
        }
        public void DeleteBus(int license)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

            XElement busToDelete = (from b in busRootElem.Elements()
                                    where int.Parse(b.Element("License").Value) == license
                                    select b).FirstOrDefault();

            if (busToDelete == null)
                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus not found: {license}");

            if (!bool.Parse(busToDelete.Element("ObjectActive").Value))
                throw new DO.ExceptionDAL_Inactive(license, $"Bus is alredy deleted: {license}");

            if (bool.Parse(busToDelete.Element("ObjectActive").Value))
            {
                busToDelete.Element("ObjectActive").Value = false.ToString();
                XMLTools.SaveListToXMLElement(busRootElem, busPath);
            }
            else
                throw new DO.ExceptionDAL_UnexpectedProblem("Unexpected Problem");
        }
        public void UpdateBus(DO.Bus bus)
        {
            XElement busRootElem = XMLTools.LoadListFromXMLElement(busPath);

            XElement busToUpdate = (from b in busRootElem.Elements()
                                    where int.Parse(b.Element("License").Value) == bus.License
                                    select b).FirstOrDefault();
            if (busToUpdate == null)
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"Bus not found: {bus.License}");
            if (!bool.Parse(busToUpdate.Element("ObjectActive").Value))
                throw new DO.ExceptionDAL_Inactive(bus.License, $"The bus is inactive: {bus.License}");

            if (bool.Parse(busToUpdate.Element("ObjectActive").Value))
            {
                busToUpdate.Element("License").Value = bus.License.ToString();
                busToUpdate.Element("LicenseDate").Value = bus.LicenseDate.ToString();
                busToUpdate.Element("Mileage").Value = bus.Mileage.ToString();
                busToUpdate.Element("Fuel").Value = bus.Fuel.ToString();
                busToUpdate.Element("BusStatus").Value = bus.BusStatus.ToString();
                busToUpdate.Element("LastTreatmentDate").Value = bus.LastTreatmentDate.ToString();
                busToUpdate.Element("MileageAtLastTreat").Value = bus.MileageAtLastTreat.ToString();
                busToUpdate.Element("ObjectActive").Value = bus.ObjectActive.ToString();

                XMLTools.SaveListToXMLElement(busRootElem, busPath);
            }
        }
        public void UpdateBus(int id, Action<DO.Bus> update)
        {
            throw new NotImplementedException();
        }

        #endregion Bus

        #region BusAtTravel
        public IEnumerable<DO.BusAtTravel> GetAllBusesAtTravel()
        {
            List<BusAtTravel> ListBusAtravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

            return from bus in ListBusAtravel
                   where bus.ObjectActive == true
                   select bus; //no need to Clone()
        }
        public IEnumerable<BusAtTravel> GetAllBusesAtTravelBy(Predicate<BusAtTravel> predicate)
        {
            List<BusAtTravel> ListBusAtravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

            return from busAtTravel in ListBusAtravel
                   where predicate(busAtTravel)
                   select busAtTravel;
        }
        public DO.BusAtTravel GetBusAtTravel(int license)
        {
            List<BusAtTravel> ListBusAtravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

            DO.BusAtTravel bus = ListBusAtravel.Find(b => b.License == license);
            if (bus == null)
                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus not found: {license}");
            if (!bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(license, $"Bus is inactive: {license}");
            return bus;  //no need to Clone()          
        }
        public void AddBusAtTravel(DO.BusAtTravel bus)
        {
            List<BusAtTravel> ListBusAtravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

            BusAtTravel existBus = ListBusAtravel.FirstOrDefault(b => b.License == bus.License);
            if (existBus != null && existBus.ObjectActive)
                throw new DO.ExceptionDAL_KeyAlreadyExist(bus.License, "Bus already in travel");

            if (existBus != null && !existBus.ObjectActive)
            {
                existBus.ObjectActive = true;
                existBus = bus;
                XMLTools.SaveListToXMLSerializer(ListBusAtravel, busAtTravelPath);
            }
            else
            {
                bus.BusLineID = XMLConfig.BusAtTravelCounter();
                bus.ObjectActive = true;
                ListBusAtravel.Add(bus); //no need to Clone()
                XMLTools.SaveListToXMLSerializer(ListBusAtravel, busAtTravelPath);
            }
        }
        public void UpdateBusAtTravel(DO.BusAtTravel bus)
        {
            List<BusAtTravel> ListBusAtTravels = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);
            int index = ListBusAtTravels.FindIndex(bus1 => bus1.License == bus.License);  //get index to update bus
            if (ListBusAtTravels[index] == null)
                throw new DO.ExceptionDAL_KeyNotFound(bus.License, $"The bus not found: {bus.License}");
            if (!ListBusAtTravels[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.License, $"The bus is inactive: {bus.License}");

            ListBusAtTravels[index] = bus;

            XMLTools.SaveListToXMLSerializer(ListBusAtTravels, busAtTravelPath);
        }
        public void UpdateBusAtTravel(int license, Action<BusAtTravel> update) // method that knows to updt specific fields in bus at travel
        {
            BusAtTravel busUpdate = GetBusAtTravel(license);
            update(busUpdate);
            UpdateBusAtTravel(busUpdate);
        }
        public void DeleteBusAtTravel(int license)
        {
            List<BusAtTravel> ListBusAtTravel = XMLTools.LoadListFromXMLSerializer<BusAtTravel>(busAtTravelPath);

            DO.BusAtTravel bus = ListBusAtTravel.Find(b => b.License == license);
            if (bus == null)
                throw new DO.ExceptionDAL_KeyNotFound(license, $"Bus not found {license}");
            if (bus.ObjectActive)
            {
                bus.ObjectActive = false;
                XMLTools.SaveListToXMLSerializer(ListBusAtTravel, busAtTravelPath);
            }
            else
                throw new ExceptionDAL_UnexpectedProblem("Unexpected Problem");
        }
        #endregion

        #region BusLine

        public IEnumerable<DO.BusLine> GetAllBusLinesBy(Predicate<DO.BusLine> predicate)
        {
            List<BusLine> ListBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinePath);

            return from bus in ListBusLine
                   where predicate(bus)
                   select bus; //no need to Clone()
        }
        public IEnumerable<BusLine> GetAllBusLines()
        {
            List<BusLine> ListBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinePath);

            return from bus in ListBusLine
                   where bus.ObjectActive == true
                   select bus;

        }
        public BusLine GetBusLine(int busLineID)
        {
            List<BusLine> ListBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinePath);

            DO.BusLine bus = ListBusLine.Find(b => b.BusLineID == busLineID);
            if (bus == null)
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"Bus not found: {busLineID}");
            if (!bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"Bus is inactive: {busLineID}");
            return bus;  //no need to Clone()   
        }
        public int AddBusLine(BusLine busLine)
        {
            int idToReturn;
            List<BusLine> ListBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinePath);

            BusLine existBus = ListBusLine.FirstOrDefault(b => b.BusLineNumber == busLine.BusLineNumber);
            if (existBus != null && existBus.ObjectActive == true && existBus.FirstBusStopKey == busLine.FirstBusStopKey
                && existBus.LastBusStopKey == busLine.LastBusStopKey)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busLine.BusLineNumber, "Bus line is already exist");
            else if (existBus != null && !existBus.ObjectActive)
            {
                existBus.ObjectActive = true;
                idToReturn = existBus.BusLineID;
                existBus = busLine;
                XMLTools.SaveListToXMLSerializer(ListBusLine, busLinePath);
            }
            else
            {
                busLine.BusLineID = XMLConfig.BusLineCounter();
                busLine.ObjectActive = true;
                idToReturn = busLine.BusLineID;
                ListBusLine.Add(busLine);
                XMLTools.SaveListToXMLSerializer(ListBusLine, busLinePath);
            }
            return idToReturn;
        }
        public void UpdateBusLine(BusLine busLine)
        {
            List<BusLine> ListBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinePath);

            int index = ListBusLine.FindIndex(bus1 => bus1.BusLineID == busLine.BusLineID);
            if (ListBusLine[index] != null && ListBusLine[index].ObjectActive)
            {
                ListBusLine.Remove(ListBusLine[index]);
                ListBusLine.Add(busLine);
                XMLTools.SaveListToXMLSerializer(ListBusLine, busLinePath);
            }

            else if (ListBusLine[index] != null && !ListBusLine[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLine.BusLineID, $"The bus line is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLine.BusLineID, $"Bus line number not found: {busLine.BusLineID}");
        }
        public void UpdateBusLine(int busLineNumber, Action<BusLine> update) // method that knows to updt specific fields in Person
        {
            BusLine busUpdate = GetBusLine(busLineNumber);
            update(busUpdate);
            UpdateBusLine(busUpdate);
        }
        public void DeleteBusLine(int busLineID)
        {
            List<BusLine> ListBusLine = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinePath);

            BusLine bus = ListBusLine.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
            {
                bus.ObjectActive = false;
                XMLTools.SaveListToXMLSerializer(ListBusLine, busLinePath);
            }
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"the bus line alredy is deleted");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"bus line number not found: {busLineID}");

        }

        #endregion 

        #region BusLineStation
        public IEnumerable<BusLineStation> GetAllBusLineStations()
        {
            List<BusLineStation> ListBusLineStation = XMLTools.LoadListFromXMLSerializer<BusLineStation>(busLineStationPath);

            return from busLineStation in ListBusLineStation
                   where busLineStation.ObjectActive == true
                   select busLineStation;
        }
        public IEnumerable<BusLineStation> GetAllBusLineStationsBy(Predicate<BusLineStation> predicate)
        {
            List<BusLineStation> ListBusLineStation = XMLTools.LoadListFromXMLSerializer<BusLineStation>(busLineStationPath);

            return from bus in ListBusLineStation
                   where predicate(bus)
                   select bus;
        }
        public BusLineStation GetBusLineStation(int busLineID, int busStopCode)
        {
            List<BusLineStation> ListBusLineStation = XMLTools.LoadListFromXMLSerializer<BusLineStation>(busLineStationPath);

            BusLineStation bus = ListBusLineStation.Find(b => b.BusStopKey == busStopCode && b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                return bus;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopCode, $"The bus station is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopCode, $"Station key not found: {busStopCode}");
        }
        public void AddBusLineStation(BusLineStation busLineStation)
        {
            List<BusLineStation> ListBusLineStation = XMLTools.LoadListFromXMLSerializer<BusLineStation>(busLineStationPath);
            BusLineStation existBusLineStation = ListBusLineStation.FirstOrDefault(b => b.BusStopKey == busLineStation.BusStopKey
            && b.BusLineID == busLineStation.BusLineID);
            if (existBusLineStation != null && existBusLineStation.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busLineStation.BusStopKey, "Duplicate bus station key");
            else if (existBusLineStation != null && existBusLineStation.ObjectActive == false)
            {
                existBusLineStation.ObjectActive = true;
                existBusLineStation = busLineStation;
                XMLTools.SaveListToXMLSerializer(ListBusLineStation, busLineStationPath);
            }
            else
            {
                busLineStation.ObjectActive = true;
                ListBusLineStation.Add(busLineStation);
                XMLTools.SaveListToXMLSerializer(ListBusLineStation, busLineStationPath);
            }
        }
        public void UpdateBusLineStation(BusLineStation busLineStation)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(busLineStationPath);

            int index = ListBusLineStations.FindIndex(bus1 => bus1.BusLineID == busLineStation.BusLineID && bus1.BusStopKey == busLineStation.BusStopKey);
            if (ListBusLineStations[index] != null && ListBusLineStations[index].ObjectActive)
            {
                ListBusLineStations.Remove(ListBusLineStations[index]);
                ListBusLineStations.Add(busLineStation);
                XMLTools.SaveListToXMLSerializer(ListBusLineStations, busLineStationPath);
            }
            else if (ListBusLineStations[index] != null && !ListBusLineStations[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineStation.BusStopKey, $"The bus line station is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineStation.BusStopKey, $"Station key not found: {busLineStation.BusStopKey}");
        }
        public void UpdateBusLineStation(int busLineID, int busStopCode, Action<BusLineStation> update) // method that knows to updt specific fields in Person
        {
            BusLineStation busUpdate = GetBusLineStation(busLineID, busStopCode);
            update(busUpdate);
            UpdateBusLineStation(busUpdate);
        }
        public void DeleteBusLineStation(int busLineID, int busStopCode)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(busLineStationPath);

            BusLineStation bus = ListBusLineStations.Find(b => b.BusStopKey == busStopCode && b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
            {
                bus.ObjectActive = false;
                XMLTools.SaveListToXMLSerializer(ListBusLineStations, busLineStationPath);
            }
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopCode, $"The bus line station is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopCode, $"Station key not found: {busStopCode}");
        }
        public void DeleteBusLineStationsByID(int busLineID)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(busLineStationPath);

            foreach (BusLineStation item in ListBusLineStations)
            {
                if (item.BusLineID == busLineID)
                    item.ObjectActive = false;
            }
            XMLTools.SaveListToXMLSerializer(ListBusLineStations, busLineStationPath);
        }
        #endregion

        #region BusStop
        public IEnumerable<BusStop> GetAllBusStops()
        {
            List<BusStop> ListBusStop = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopPath);

            return from busLineStation in ListBusStop
                   where busLineStation.ObjectActive == true
                   select busLineStation;
        }
        public IEnumerable<BusStop> GetAllBusStopsBy(Predicate<BusStop> predicate)
        {
            List<BusStop> ListBusStop = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopPath);

            return from busStop in ListBusStop
                   where predicate(busStop)
                   select busStop;
        }
        public BusStop GetBusStop(int busStopKey)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopPath);

            BusStop bus = ListBusStops.Find(b => b.BusStopKey == busStopKey);
            if (bus != null && bus.ObjectActive)
                return bus;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopKey, $"The bus stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopKey, $"Bus stop key not found: {busStopKey}");
        }
        public void AddBusStop(BusStop busStop)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopPath);

            BusStop existStop = ListBusStops.FirstOrDefault(b => b.BusStopKey == busStop.BusStopKey);

            if (existStop != null && existStop.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(busStop.BusStopKey, "Duplicate bus stop key");
            else if (existStop != null && existStop.ObjectActive == false)
            {
                existStop.ObjectActive = true;
                existStop = busStop;
                XMLTools.SaveListToXMLSerializer(ListBusStops, busStopPath);
            }
            else
            {
                busStop.ObjectActive = true;
                ListBusStops.Insert(0, busStop);
                XMLTools.SaveListToXMLSerializer(ListBusStops, busStopPath);
            }
        }
        public void UpdateBusStop(BusStop busStop)
        {
            // If the old bus stop code didn't change, or it changed but it's new:
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopPath);

            int index = ListBusStops.FindIndex(b => b.BusStopKey == busStop.BusStopKey);
            if (ListBusStops[index] != null && ListBusStops[index].ObjectActive)
            {
                ListBusStops[index] = busStop;
                XMLTools.SaveListToXMLSerializer(ListBusStops, busStopPath);
            }
            else if (ListBusStops[index] != null && !ListBusStops[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStop.BusStopKey, $"The bus Stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStop.BusStopKey, $"Bus stop key not found");
        }
        public void UpdateBusStop(int busStopKey, Action<BusStop> update) // method that knows to updt specific fields in Person
        {
            BusStop busUpdate = GetBusStop(busStopKey);
            update(busUpdate);
            UpdateBusStop(busUpdate);
        }
        public void DeleteBusStop(int busStopKey)
        {
            List<BusStop> ListBusStops = XMLTools.LoadListFromXMLSerializer<BusStop>(busStopPath);

            BusStop bus = ListBusStops.Find(b => b.BusStopKey == busStopKey);
            if (bus != null && bus.ObjectActive)
            {
                bus.ObjectActive = false;
                XMLTools.SaveListToXMLSerializer(ListBusStops, busStopPath);
            }
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busStopKey, $"The bus stop is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busStopKey, $"Bus stop key not found: {busStopKey}");
        }
        #endregion

        #region ConsecutiveStations
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationPath);

            return from busConsecutiveStations in ListConsecutiveStations
                   where busConsecutiveStations.ObjectActive == true
                   select busConsecutiveStations;
        }
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationPath);

            return from busConsecutiveStations in ListConsecutiveStations
                   where predicate(busConsecutiveStations)
                   select busConsecutiveStations;
        }
        public ConsecutiveStations GetConsecutiveStations(int busStopCodeA, int busStopCodeB)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationPath);

            ConsecutiveStations conStations = ListConsecutiveStations.Find(b => b.BusStopKeyA == busStopCodeA && b.BusStopKeyB == busStopCodeB);
            if (conStations != null && conStations.ObjectActive)
                return conStations;
            else if (conStations != null && !conStations.ObjectActive)
                throw new DO.ExceptionDAL_Inactive("the consecutive stations is  inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("the consecutive stations not found");
        }
        public void AddConsecutiveStations(ConsecutiveStations newConsecutiveStations)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationPath);

            ConsecutiveStations existConsecutiveStations = ListConsecutiveStations.FirstOrDefault(b => b.BusStopKeyA == newConsecutiveStations.BusStopKeyA && b.BusStopKeyB == newConsecutiveStations.BusStopKeyB);
            if (existConsecutiveStations != null && existConsecutiveStations.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist("Duplicate consecutive stations object");
            else if (existConsecutiveStations != null && existConsecutiveStations.ObjectActive == false)
            {
                existConsecutiveStations.ObjectActive = true;
                XMLTools.SaveListToXMLSerializer(ListConsecutiveStations, consecutiveStationPath);
            }
            else
            {
                newConsecutiveStations.ObjectActive = true;
                ListConsecutiveStations.Add(newConsecutiveStations);
                XMLTools.SaveListToXMLSerializer(ListConsecutiveStations, consecutiveStationPath);
            }
        }
        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationPath);

            int index = ListConsecutiveStations.FindIndex(consecutive => consecutive.BusStopKeyA == consecutiveStations.BusStopKeyA);
            if (ListConsecutiveStations[index] != null && ListConsecutiveStations[index].ObjectActive)
            {
                ListConsecutiveStations[index] = consecutiveStations;
                XMLTools.SaveListToXMLSerializer(ListConsecutiveStations, consecutiveStationPath);
            }
            else if (ListConsecutiveStations[index] != null && !ListConsecutiveStations[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive("The consecutive stations is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("The consecutive stations not found");
        }
        public void UpdateConsecutiveStations(int busStopCodeA, int busStopCodeB, Action<ConsecutiveStations> update) // method that knows to updt specific fields in Person
        {
            ConsecutiveStations busConsecutiveUpdate = GetConsecutiveStations(busStopCodeA, busStopCodeB);
            update(busConsecutiveUpdate);
            UpdateConsecutiveStations(busConsecutiveUpdate);
        }
        public void DeleteConsecutiveStations(int busStopCodeA, int busStopCodeB)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationPath);

            ConsecutiveStations busConsecutive = ListConsecutiveStations.Find(b => b.BusStopKeyA == busStopCodeA && b.BusStopKeyB == busStopCodeB);
            if (busConsecutive != null && busConsecutive.ObjectActive)
            {
                busConsecutive.ObjectActive = false;
                XMLTools.SaveListToXMLSerializer(ListConsecutiveStations, consecutiveStationPath);
            }
            else if (busConsecutive != null && !busConsecutive.ObjectActive)
                throw new DO.ExceptionDAL_Inactive("The consecutive stations is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound("The consecutive stations not found");
        }
        #endregion  //

        #region LineDeparture
        public IEnumerable<LineDeparture> GetAllLineDeparture()
        {
            List<LineDeparture> ListLineDepartures = XMLTools.LoadListFromXMLSerializer<LineDeparture>(lineDeparturePath);

            return from busLineDeparture in ListLineDepartures
                   where busLineDeparture.ObjectActive == true
                   select busLineDeparture;
        }
        public IEnumerable<LineDeparture> GetAllLineDepartureBy(Predicate<LineDeparture> predicate)
        {
            List<LineDeparture> ListLineDepartures = XMLTools.LoadListFromXMLSerializer<LineDeparture>(lineDeparturePath);

            return from busLineDeparture in ListLineDepartures
                   where predicate(busLineDeparture)
                   select busLineDeparture;
        }
        public LineDeparture GetLineDeparture(int busLineID)
        {
            List<LineDeparture> ListLineDepartures = XMLTools.LoadListFromXMLSerializer<LineDeparture>(lineDeparturePath);

            LineDeparture bus = ListLineDepartures.Find(b => b.BusLineID == busLineID);
            if (bus != null && bus.ObjectActive)
                return bus;
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(busLineID, $"The line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(busLineID, $"Line departure key not found: {busLineID}");
        }
        public void AddLineDeparture(LineDeparture lineDeparture)
        {
            List<LineDeparture> ListLineDepartures = XMLTools.LoadListFromXMLSerializer<LineDeparture>(lineDeparturePath);

            LineDeparture existLineDeparture = ListLineDepartures.FirstOrDefault(b => b.BusLineID == lineDeparture.BusLineID && b.DepartureTime == lineDeparture.DepartureTime);
            if (existLineDeparture != null && existLineDeparture.ObjectActive == true)
                throw new DO.ExceptionDAL_KeyAlreadyExist(lineDeparture.BusLineID, "Duplicate line departure time");
            else if (existLineDeparture != null && existLineDeparture.ObjectActive == false)
            {
                existLineDeparture.ObjectActive = true;
                XMLTools.SaveListToXMLSerializer(ListLineDepartures, lineDeparturePath);
            }
            else
            {
                lineDeparture.DepartureID = XMLConfig.LineDepartureCounter();
                lineDeparture.ObjectActive = true;
                ListLineDepartures.Add(lineDeparture);
                XMLTools.SaveListToXMLSerializer(ListLineDepartures, lineDeparturePath);
            }
        }
        public void UpdateLineDeparture(LineDeparture lineDeparture)
        {
            List<LineDeparture> ListLineDepartures = XMLTools.LoadListFromXMLSerializer<LineDeparture>(lineDeparturePath);

            int index = ListLineDepartures.FindIndex(lineDep => lineDep.BusLineID == lineDeparture.BusLineID);
            if (ListLineDepartures[index] != null && ListLineDepartures[index].ObjectActive)
            {
                ListLineDepartures[index] = lineDeparture;
                XMLTools.SaveListToXMLSerializer(ListLineDepartures, lineDeparturePath);
            }
            else if (ListLineDepartures[index] != null && !ListLineDepartures[index].ObjectActive)
                throw new DO.ExceptionDAL_Inactive(lineDeparture.BusLineID, $"the line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(lineDeparture.BusLineID, $"line departure key not found: {lineDeparture.BusLineID}");
        }
        public void UpdateLineDeparture(int busLineID, Action<LineDeparture> update) // method that knows to updt specific fields in Person
        {
            LineDeparture busUpdate = GetLineDeparture(busLineID);
            update(busUpdate);
            UpdateLineDeparture(busUpdate);
        }
        public void DeleteLineDeparture(TimeSpan departureTime, int busLineID)
        {
            List<LineDeparture> ListLineDepartures = XMLTools.LoadListFromXMLSerializer<LineDeparture>(lineDeparturePath);

            LineDeparture bus = ListLineDepartures.Find(b => b.BusLineID == busLineID && b.DepartureTime == departureTime);
            if (bus != null && bus.ObjectActive)
            {
                bus.ObjectActive = false;
                XMLTools.SaveListToXMLSerializer(ListLineDepartures, lineDeparturePath);
            }
            else if (bus != null && !bus.ObjectActive)
                throw new DO.ExceptionDAL_Inactive(bus.DepartureID, $"The line departure is inactive");
            else
                throw new DO.ExceptionDAL_KeyNotFound(bus.DepartureID, $"Line departure key not found: {bus.DepartureID}");
        }
        public void DeleteLineDepartureByID(int busLineID)
        {
            List<LineDeparture> ListLineDepartures = XMLTools.LoadListFromXMLSerializer<LineDeparture>(lineDeparturePath);

            foreach (LineDeparture item in ListLineDepartures)
            {
                if (item.BusLineID == busLineID)
                    item.ObjectActive = false;
            }
            XMLTools.SaveListToXMLSerializer(ListLineDepartures, lineDeparturePath);
        }
        #endregion

        #region User
        public IEnumerable<User> GetAllUsers()
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(userPath);

            return from user in ListUsers
                   where user.ObjectActive == true
                   select user;
        }
        public IEnumerable<User> GetAllUsersBy(Predicate<User> predicate)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(userPath);

            return from user in ListUsers
                   where predicate(user)
                   select user;
        }
        public User GetUser(string userName)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(userPath);

            User user = ListUsers.Find(b => b.UserName == userName);
            if (user != null && user.ObjectActive)
                return user;
            else if (user != null && !user.ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(userName, $"The user is  inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(userName, $"User name not found: {userName}");
        }
        public void AddUser(User user)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(userPath);

            User existUser = ListUsers.FirstOrDefault(b => b.UserName == user.UserName);
            if (existUser != null && existUser.ObjectActive == true)
                throw new DO.ExceptionDAL_UserAlreadyExist(user.UserName, "Duplicate user name");
            else if (existUser != null && existUser.ObjectActive == false)
            {
                existUser.ObjectActive = true;
                existUser = user;
                XMLTools.SaveListToXMLSerializer(ListUsers, userPath);
            }
            else
            {
                user.ObjectActive = true;
                ListUsers.Add(user);
                XMLTools.SaveListToXMLSerializer(ListUsers, userPath);
            }
        }
        public void UpdateUser(User user)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(userPath);

            int index = ListUsers.FindIndex(user1 => user1.UserName == user.UserName);
            if (ListUsers[index] != null && ListUsers[index].ObjectActive)
            {
                ListUsers[index] = user;
                XMLTools.SaveListToXMLSerializer(ListUsers, userPath);
            }
            else if (ListUsers[index] != null && !ListUsers[index].ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(user.UserName, $"The user is inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(user.UserName, $"User name not found: {user.UserName}");
        }
        public void UpdateUser(string userName, Action<User> update)  
        {
            User userUpdate = GetUser(userName);
            update(userUpdate);
            UpdateUser(userUpdate);
        }
        public void DeleteUser(string userName)
        {
            List<User> ListUsers = XMLTools.LoadListFromXMLSerializer<User>(userPath);

            User user = ListUsers.Find(b => b.UserName == userName);
            if (user != null && user.ObjectActive)
            {
                user.ObjectActive = false;
                XMLTools.SaveListToXMLSerializer(ListUsers, userPath);
            }
            else if (user != null && !user.ObjectActive)
                throw new DO.ExceptionDAL_InactiveUser(userName, $"The user is inactive");
            else
                throw new DO.ExceptionDAL_UserKeyNotFound(userName, $"User name not found: {userName}");
        }
        #endregion

    }
}