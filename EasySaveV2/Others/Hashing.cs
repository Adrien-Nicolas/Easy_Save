using EasySave.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EasySave.Others
{
    /// <summary>
    /// Class handling Hash
    /// </summary>
    class Hashing
    {


        /// <summary>
        /// Class allowing to return the hash of a file
        /// </summary>
        /// <param name="filename">String(path)</param>
        /// <returns>string</returns>
        public static string GetHashFile(string filename)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// Method allowing to return a dictionary with the last hash job 
        /// </summary>
        /// <param name="nameJob">string</param>
        /// <returns>Dictionary(string, string)/returns>
        public static Dictionary<string, string> GetLastHashJob(string nameJob)
        {
            string file = InteractFile.FindFileInFolder(AppDomain.CurrentDomain.BaseDirectory + "/Hashing", nameJob + ".json");

            if (file == "")
                return new Dictionary<string, string>();
            else
            {
                JSON json = JSON.getJson();

                return json.Read(file); ;
            }

        }



        /// <summary>
        /// Method allowing to return a Dictionnary with all hash from a folder
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>Dictionary(string, string)</returns>
        public static Dictionary<string, string> GetAllHashFolder(string path)
        {

            Dictionary<string, string> dicoHash = new Dictionary<string, string>();

            if (path == null || !Directory.Exists(path))
                return dicoHash;

            foreach (string sourceFilePath in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {

                dicoHash.Add(sourceFilePath, GetHashFile(sourceFilePath));

            }

            return dicoHash;


        }

        /// <summary>
        /// Method allowing to write on a file all hash from a Job
        /// </summary>
        /// <param name="nameJob">string</param>
        /// <param name="dicoHash">Dictionary(string, string)</param>
        public static void WriteAllHash(string nameJob, Dictionary<string, string> dicoHash)
        {

            string file = AppDomain.CurrentDomain.BaseDirectory + "/Hashing/" + nameJob + ".json";



            JSON json = JSON.getJson();

            json.Write(file,dicoHash); ;



        }

        /// <summary>
        /// Method allowing to Update an hash dictionnary
        /// </summary>
        /// <param name="dicoJobHashFile">Dictionary(string, string) </param>
        /// <param name="fileToCopy"> List(string)</param>
        /// <param name="fileToDelete">List(string)</param>
        /// <param name="job">Job</param>
        /// <returns>Dictionary(string, string)</returns>
        public static Dictionary<string, string> UpdateHashDico(Dictionary<string, string> dicoJobHashFile, List<string> fileToCopy, List<string> fileToDelete, Job job)
        {

            Dictionary<string, string> dicoJobHashFileTemp = dicoJobHashFile;

            fileToCopy.ForEach((string file) =>
            {

                string hashFile = Hashing.GetHashFile(file);

                if (hashFile != "error")
                {

                    if (dicoJobHashFileTemp.ContainsKey(file.Replace(job.sourcePath, job.targetPath)))
                    {
                        dicoJobHashFileTemp[file.Replace(job.sourcePath, job.targetPath)] = hashFile;
                    }
                    else
                    {
                        dicoJobHashFileTemp.Add(file.Replace(job.sourcePath, job.targetPath), hashFile);
                    }

                }

            });



            fileToDelete.ForEach((string file) =>
            {

                dicoJobHashFileTemp.Remove(file.Replace(job.sourcePath, job.targetPath));

            });




            return dicoJobHashFileTemp;

        }
    }
}
