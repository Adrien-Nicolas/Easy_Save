using System;
using System.Collections.Generic;
using System.Text;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasySave.Models
{
    /// <summary>
    /// LogEtat class model
    /// </summary>
    public class LogEtat
    {
        public int progress { get; set; }
        public string sourceFilePath { get; set; }
        public string targetFilePath { get; set; }
        public string state { get; set; }
        public int totalFileToCopy { get; set; }
        public double totalFileSize { get; set; }
        public int totalFileLeft { get; set; }
        public string nameJob { get; set; }
        public DateTime date { get; set; }

        /// <summary>
        /// Constructor of LogEtat Class
        /// </summary>
        public LogEtat()
        {
            this.progress = 0;
            this.state = "Non-Actif";
            this.sourceFilePath = "";
            this.targetFilePath = "";
        }





    }
}
