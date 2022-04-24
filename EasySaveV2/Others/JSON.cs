using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using EasySave.ViewModels;



namespace EasySave.Others
{
    /// <summary>
    /// Json class inheriting from the InteractFile class
    /// </summary>
    public class JSON : InteractFile
    {
   
        static public JSON json;
        /// <summary>
        /// Contructeur de la classe json
        /// </summary>
        public JSON() { }

        /// <summary>
        /// Singleton of the JSON class
        /// </summary>
        /// <returns>JSON</returns>
        static public JSON getJson()
        {
            if(JSON.json == null)
            {
                JSON.json = new JSON();
            }

            return JSON.json;
        }



        /// <summary>
        /// Method to read a file and return a dictionary
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>Dictionary string, string</returns>
        public Dictionary<string, string> Read(string path)
        {
            string chemin = @path;
            var myJsonString = File.ReadAllText(chemin);
            Dictionary<string, string> values = new Dictionary<string, string>();
            values = JsonConvert.DeserializeObject<Dictionary<string, string>>(myJsonString);
            return values;
        }

        public List<string> ReadToList(string path)
        {
            string chemin = @path;
            var myJsonString = File.ReadAllText(chemin);
            List<string> values = new List<string>();
            values = JsonConvert.DeserializeObject <List<string>>(myJsonString);
            return values;

        }

        /// <summary>
        /// Method to write on an existant JSON file (not overwrite)
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
            if(!File.Exists(chemin))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(chemin));
                var myFile = System.IO.File.Create(chemin);
                myFile.Close();
            }
                
            var myJsonString = File.ReadAllText(chemin);

            List<T> ObjOrderListe = new List<T>();
            ObjOrderListe = JsonConvert.DeserializeObject<List<T>>(myJsonString);

            if(ObjOrderListe == null)
            {
                ObjOrderListe = new List<T>();
            }

            List<T> _data = new List<T>();
            _data.Add(t
                );

            ObjOrderListe.AddRange(_data);
            string json = JsonConvert.SerializeObject(ObjOrderListe, Formatting.Indented);


            //write string to file
            System.IO.File.WriteAllText(chemin, json);
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
           if(File.Exists(chemin))
            {
              var myJsonString = File.ReadAllText(chemin);
                List<T> ObjOrderList = JsonConvert.DeserializeObject<List<T>>(myJsonString);
                return ObjOrderList;
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
            List<T> ObjOrderList = JsonConvert.DeserializeObject<List<T>>(myJsonString);
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
            string json = JsonConvert.SerializeObject(dico, Formatting.Indented);


                Directory.CreateDirectory(Path.GetDirectoryName(path));
            
            //write string to file
            System.IO.File.WriteAllText(chemin, json);
        }


    }
}
