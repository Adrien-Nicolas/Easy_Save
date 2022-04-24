using EasySave.Others;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasySaveV2.Others
{
    public class XML : InteractFile
    {
        /// <summary>
        /// Contructeur de la classe json
        /// </summary>
        public XML() { }



        /// <summary>
        /// Method to read a file and return a dictionary
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>Dictionary string, string</returns>
        public Dictionary<string, string> Read(string path)
        {

            var doc = XDocument.Load(path);
            var rootNodes = doc.Root.DescendantNodes().OfType<XElement>();
            return rootNodes.ToDictionary(n => n.Name.ToString(), n => n.Value);

        }

        public List<string> ReadToList(string path)
        {
            string chemin = @path;
            var myJsonString = File.ReadAllText(chemin);
            List<string> values = new List<string>();
            //values = 
            return values;

        }

        /// <summary>
        /// Method to write on a JSON file
        /// </summary>
        /// <typeparam name="T">Objet</typeparam>
        /// <param name="path">string</param>
        /// <param name="t">instance</param>
        /// <returns></returns>
        public override bool Write<T>(string path, T t)
        {

            bool i = false;
            List<T> tabListItems = new List<T>();
            string chemin = @path;
            if (!File.Exists(chemin))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(chemin));
                using (var writer = new FileStream(path, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(List<T>),
                        new XmlRootAttribute(nameof(List<T>)));
                    ser.Serialize(writer, tabListItems);
                }
            }

            using (var reader = new StreamReader(chemin))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<T>),
                    new XmlRootAttribute(nameof(List<T>)));
                tabListItems = (List<T>)deserializer.Deserialize(reader);
            }


            if (tabListItems == null)
            {
                tabListItems = new List<T>();
            }

            tabListItems.Add(t);

            //write string to file
            using (var writer = new FileStream(path, FileMode.Create))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<T>),
                    new XmlRootAttribute(nameof(List<T>)));
                ser.Serialize(writer, tabListItems);
            }


            
            i = true;
            return i;
        }


        /// <summary>
        /// Method allowing to Write in a List of object.
        /// </summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="path">string</param>
        /// <param name="listT">List(T)</param>
        /// <param name="del">bool</param>
        /// <returns>bool</returns>
        public override bool WriteList<T>(string path, List<T> listT, bool del = false)
        {
            if (del)
                DeleteFile(path);

            bool i = true;
            listT.ForEach(delegate (T t)
            {

                if (!Write<T>(path, t))
                    i = false;

            });

            return i;
        }




        /// <summary>
        /// Method to return a List of a JSON file
        /// </summary>
        /// <typeparam name="Saveworker">objet</typeparam>
        /// <param name="path">string</param>
        /// <returns></returns>
        public List<T> Read<T>(string path)
        {

            string chemin = @path;
            if (File.Exists(chemin))
            {
                var myJsonString = File.ReadAllText(chemin);
                //List<T> ObjOrderList = JsonConvert.DeserializeObject<List<T>>(myJsonString);
                //return ObjOrderList;
            }
            return new List<T>();
        }


        /// <summary>
        /// Method to read an object file from a json file
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="pos">int</param>
        /// <returns></returns>
        public static object readPos<T>(string path, int pos)
        {
            string chemin = @path;
            var myJsonString = File.ReadAllText(path);
            List<T> ObjOrderList = new List<T>();
            //JsonConvert.DeserializeObject<List<T>>(myJsonString);
            return ObjOrderList[pos];

        }



        /// <summary>
        /// Method allowing to Write
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="dico">Dictionary(string, string)</param>
        public void Write(string path, Dictionary<string, string> dico)
        {
            string chemin = @path;
            //string json = JsonConvert.SerializeObject(dico, Formatting.Indented);


            Directory.CreateDirectory(Path.GetDirectoryName(path));

            //write string to file
            //System.IO.File.WriteAllText(chemin, json);
        }


    }

}
