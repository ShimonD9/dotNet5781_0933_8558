using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using System.IO;

namespace DL
{

    static class XMLConfig
    {
        /// <summary>
        /// class XML config helper for the running number
        /// </summary>

        static XElement root;
        static string dir = @"xml\";
        static string configPath = @"configXml.xml";
        static XMLConfig()
        {
            if (!Directory.Exists(dir))                 //in case there is still no file, so open new one
                Directory.CreateDirectory(dir);
            if (!File.Exists(dir + configPath))         //in case the filr not exist yet
            {                                           //create new file for running number
                root = new XElement("config",
                    new XElement("BusInTravelCounter",0),
                    new XElement("BusLineCounter",0),
                     new XElement("PassengTravelCounter",0)
                     );
                root.Save(dir + configPath);
            }

            else
                try
                {
                    root = XElement.Load(dir + configPath);         //load file
                }
                catch
                {
                    throw new ExceptionDAL_XMLFileLoadCreateException(configPath, "File upload problem");
                }
        }

        public static int BusAtTravelCounter()          //running number (ID) of the bus at travel
        {

            int counter = int.Parse(root.Element("BusInTravelCounter").Value);
            counter++;
            root.Element("BusInTravelCounter").Value = counter.ToString();
            root.Save(dir + configPath);
            return counter;

        }

        public static int BusLineCounter()              //running number (ID) of the bus line
        {
            int counter = int.Parse(root.Element("BusLineCounter").Value);
            counter++;
            root.Element("BusLineCounter").Value = counter.ToString();
            root.Save(dir + configPath);
            return counter;
        }

        public static int LineDepartureCounter()        //running number (ID) of the line departure
        {
            int counter = int.Parse(root.Element("PassengTravelCounter").Value);
            counter++;
            root.Element("PassengTravelCounter").Value = counter.ToString();
            root.Save(dir + configPath);
            return counter;
        }
    }
}
