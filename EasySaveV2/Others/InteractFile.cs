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
using EasySaveV2.Others;

namespace EasySave.Others
{
    /// <summary>
    /// Classe
    /// </summary>
    public class InteractFile
    {
        
        //private InteractFile()
        //{

        //}

        //private 






        /// <summary>
        /// Method to write on a JSON file
        /// </summary>
        /// <typeparam name="T">Objet</typeparam>
        /// <param name="path">string</param>
        /// <param name="t">instance</param>
        /// <returns></returns>
        public virtual bool Write<T>(string path, T t)
        {
            return true;
        }

        public virtual bool WriteList<T>(string path, List<T> listT, bool delete = false)
        {
            return true;
        }



        /// <summary>
        /// Method to return a List of a JSON file
        /// </summary>
        /// <typeparam name="T">objet</typeparam>
        /// <param name="path">string</param>
        /// <returns></returns>
        public List<T> Read<T>(string path)
        {

            string chemin = @path;
            if (File.Exists(chemin))
            {
                var myJsonString = File.ReadAllText(chemin);
                List<T> ObjOrderList = JsonConvert.DeserializeObject<List<T>>(myJsonString);
                return ObjOrderList;
            }
            return new List<T>();
        }


        




        /// <summary>
        /// Method to copy files from a folder to another folder
        /// </summary>
        /// <param name="pathSource">string</param>
        /// <param name="pathDest">string</param>
        /// <returns>bool</returns>
        public static void Copy(string pathSource, string pathDest)
        {

            Directory.CreateDirectory(Path.GetDirectoryName(pathDest));
            File.Copy(pathSource, pathDest, true);

        }



        /// <summary>
        /// Delete a file and return true if success
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>bool</returns>
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }



        }




        /// <summary>
        /// Delete a file and return true if success
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>bool</returns>
        public static bool DeleteFolder(string path,bool rcursif)
        {
            try
            {
                Directory.Delete(path,rcursif);
                return true;
            }
            catch
            {
                return false;
            }



        }



        /// <summary>
        /// Method to compare two files 
        /// </summary>
        /// <param name="first">string</param>
        /// <param name="second">string</param>
        /// <returns>bool</returns>

        public static bool isFileDifferents(string first, string second)
        {
            if (!File.Exists(first) || !File.Exists(second))
                return true;

            if (first == second)
                return true;


            FileStream fs1;
            FileStream fs2;
            int file1byte;
            int file2byte;
            fs1 = new FileStream(first, FileMode.Open);
            fs2 = new FileStream(second, FileMode.Open);

            do
            {
                // Read one byte from each file.             
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            if (file1byte != file2byte)
            {
                fs1.Close();
                fs2.Close();
                return true;
            }
            else
            {
                fs1.Close();
                fs2.Close();
                return false;
            }


        }


        /// <summary>
        /// Method to read an object from a json file from a position
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="pos">int</param>
        /// <returns>object</returns>
        public static object readPos<T>(string path, int pos)
        {
            string chemin = @path;
            var myJsonString = File.ReadAllText(path);
            List<T> ObjOrderList = JsonConvert.DeserializeObject<List<T>>(myJsonString);
            return ObjOrderList[pos];

        }

        /// <summary>
        /// Method to get the last file in a folder
        /// </summary>
        /// <param name="pathFolder">string</param>
        /// <returns>string</returns>
        public static string getLastFileInFolder(string pathFolder)
        {

            try
            {
                var directory = new DirectoryInfo(pathFolder);

                return directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First().ToString();

            }
            catch
            {
                return "";
            }



        }




        /// <summary>
        /// Method allowing to return file from a specific folder
        /// </summary>
        /// <param name="folderPath">string</param>
        /// <param name="fileName">string</param>
        /// <returns></returns>
        public static string FindFileInFolder(string folderPath, string fileName)
        {

            try
            {
                return Directory.GetFiles(folderPath, fileName)[0];
            }
            catch
            {
                return "";
            }
        }

        public static InteractFile getInstanceFromName(string interactFileName)
        {

            if (interactFileName == "JSON")
                return new JSON();

            if (interactFileName == "XML")
                return new XML();


            //Default
            return new JSON();

        }
    }
}
