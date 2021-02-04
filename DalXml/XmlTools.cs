using DO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace DL
{

    class XMLTools     //xmlTools(save and load)
    {
        static string dir = @"xml\";        //the head path

        static XMLTools()

        {
            if (!Directory.Exists(dir))                  //in case the file not created yet
                Directory.CreateDirectory(dir);          //open new one  
        }

        /// <summary>
        /// function that save changes in to the file
        /// works with Xelement 
        /// </summary>
        /// <param name="rootElem"></param>
        /// <param name="filePath"></param>
        #region SaveLoadWithXElement
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                throw new DO.ExceptionDAL_XMLFileLoadCreate(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        /// <summary>
        /// function that load details from file
        /// works with Xelement 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                {
                    XElement rootElem = new XElement(filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.ExceptionDAL_XMLFileLoadCreate(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        #region SaveLoadWithXMLSerializer

        /// <summary>
        /// function that save changes in to the file
        /// using XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.ExceptionDAL_XMLFileLoadCreate(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        /// <summary>
        /// function that load details from file
        /// using XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.ExceptionDAL_XMLFileLoadCreate(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion
    }
}