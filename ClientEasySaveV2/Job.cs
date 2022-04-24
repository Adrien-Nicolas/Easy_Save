using System;
using System.Collections.Generic;
using System.Text;

namespace ClientEasySaveV2
{

    /// <summary>
    /// Save Job class
    /// </summary>
    public class Job
    {

        public string targetPath { get; set; } = "";
        public string sourcePath { get; set; } = "";
        public bool fullMode { get; set; }
        public string name { get; set; } = "";
        public int nbrSave { get; set; }
        public DateTime lastSave { get; set; }
        public LogEtat logEtat { get; set; }
        public int progress { get; set; } = 0;

        public Job()
        {

        }


        /// <summary>
        /// Constructor of the Job class
        /// </summary>
        /// <param name="sourcePath">string</param>
        /// <param name="targetPath">string</param>
        public Job(string sourcePath, string targetPath)
        {
            this.sourcePath = sourcePath;
            this.targetPath = targetPath;
        }



        /// <summary>
        /// Method to read the values of the attributes of the Job method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Source Path : {sourcePath}\n" +
                $"Target Path : {targetPath}\n" +
                $"Full Mode : {fullMode}\n" +
                $"Name : {name}\n" +
                $"nbrSave : {nbrSave}\n" +
                $"last Save : {(lastSave == DateTime.MinValue ? "" : lastSave.ToString())}\n";

        }


    }
}
