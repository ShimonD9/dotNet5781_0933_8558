﻿using System;
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
        static XElement root;
        static string dir = @"xml\";
        static string configPath = @"configXml.xml";
        static XMLConfig()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (!File.Exists(dir + configPath))
            {
                root = new XElement("config",
                    new XElement("BusInTravelCounter", 0),
                    new XElement("BusLineCounter", 0),
                     new XElement("PassengTravelCounter", 0)
                     );
                root.Save(dir + configPath);
            }

            else
                try
                {
                    root = XElement.Load(dir + configPath);
                }
                catch
                {
                    throw new ExceptionDAL_XMLFileLoadCreateException(configPath, "File upload problem");
                }
        }

        public static int BusAtTravelCounter()
        {

            int counter = int.Parse(root.Element("BusInTravelCounter").Value);
            counter++;
            root.Element("BusInTravelCounter").Value = counter.ToString();
            root.Save(dir + configPath);
            return counter;

        }

        public static int BusLineCounter()
        {
            int counter = int.Parse(root.Element(" BusLineCounter").Value);
            counter++;
            root.Element("BusInTravelCounter").Value = counter.ToString();
            root.Save(dir + configPath);
            return counter;
        }

        public static int LineDepartureCounter()
        {
            int counter = int.Parse(root.Element("PassengTravelCounter").Value);
            counter++;
            root.Element("BusInTravelCounter").Value = counter.ToString();
            root.Save(dir + configPath);
            return counter;
        }
    }
}
