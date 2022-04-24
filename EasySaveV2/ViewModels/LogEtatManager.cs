using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EasySave.Others;
using EasySave.Models;

namespace EasySave.ViewModels
{
    /// <summary>
    /// class of the controller log etat
    /// </summary>
    class LogEtatManager
    {
        /// <summary>
        /// function to write the logetat.json file
        /// </summary>
        /// <param name="logEtat">LogEtat</param>
        public static void Write(LogEtat logEtat)
        {
            JSON json = JSON.getJson();
            logEtat.date = DateTime.Now;

            json.Write<LogEtat>(AppDomain.CurrentDomain.BaseDirectory + "/LogEtat/" + logEtat.nameJob + ".json", logEtat);
        }

        /// <summary>
        /// Method allowing to init LogEtat File
        /// </summary>
        /// <param name="logEtat">LogEtat</param>
        public static void InitFile(LogEtat logEtat)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "/LogEtat/" + logEtat.nameJob + ".json";

            JSON json = JSON.getJson();
            InteractFile.DeleteFile(path);
            LogEtatManager.Write(logEtat);
        }


        /// <summary>
        ///  Function to check the last logEtat if its End status. Return logEtat if not verif
        /// </summary>
        /// <returns>bool</returns>
        public static List<LogEtat> verifLastLogEtat(string jobName)
        {

            List<LogEtat> listLogEtat = getLastLogEtat(jobName);
            bool verif = false;
            listLogEtat.ForEach((LogEtat logEtat) =>
            {

                if (logEtat.state == "End")
                    verif = true;

            });

            if (verif)
                return new List<LogEtat>();

            return listLogEtat;

        }






        public static List<LogEtat> getLastLogEtat(string jobName)
        {
            List<LogEtat> listLogEtat = new List<LogEtat>();
            string file = AppDomain.CurrentDomain.BaseDirectory + $"/LogEtat/{jobName}.json";

            if (File.Exists(file))
            {

                JSON json = JSON.getJson();
                listLogEtat = json.Read<LogEtat>(file);

            }

            return listLogEtat;


        }




        /// <summary>
        /// Method allowing to write logEtat state = "END" in LogEtat file
        /// </summary>
        /// <param name="logEtat">LogEtat</param>
        public static void EndLogEtat(LogEtat logEtat)
        {

            logEtat.targetFilePath = "";
            logEtat.sourceFilePath = "";
            logEtat.state = "End";

            LogEtatManager.Write(logEtat);
        }

        /// <summary>
        /// Method allowing to initLogEtat file. We go through each folder, and check if the files are different 
        /// and the mode is differential, if the mode is complete, 
        /// and write the information in the status log according to these parameters
        /// </summary>
        /// <param name="job">Job</param>
        /// <returns>LogEtat</returns>
        public static LogEtat InitLogEtatFilesInfo(Job job, List<string> listFileToCopy)
        {
            string sourcePath = job.sourcePath;
            string targetPath = job.targetPath;

            int totalFile = 0;
            double totalFileSize = 0;


            listFileToCopy.ForEach((string file) =>
            {

                try
                {

                    totalFile++;
                    FileInfo fi = new FileInfo(file);
                    totalFileSize += fi.Length;


                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getFilesInfo : " + ex.ToString());
                }

            });

            LogEtat logEtat = new LogEtat();

            //MAJ du logEtat
            logEtat.nameJob = job.name;
            logEtat.totalFileSize = totalFileSize;
            logEtat.totalFileLeft = totalFile;
            logEtat.totalFileToCopy = totalFile;

            return logEtat;


        }


        /// <summary>
        /// Get list file from a list LogEtat
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static List<string> ListLogEtatToListSourcePath(List<LogEtat> listLogEtat)
        {
            List<string> finalList = new List<string>();

            listLogEtat.ForEach((LogEtat logEtat) =>
            {
                finalList.Add(logEtat.sourceFilePath);
            });

            return finalList;
        }
    }



}
