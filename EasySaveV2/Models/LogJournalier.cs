using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave.Models
{
    /// <summary>
    /// LogJournalier Class
    /// </summary>
    public class LogJournalier
    {

        public string name { get; set; }
        public string fileSource { get; set; }
        public string fileTarget { get; set; }
        public string destPath { get; set; }
        public double fileSize { get; set; }
        public double fileTransfertTimeMS { get; set; }
        public string cryptTime { get; set; }
        public DateTime time { get; set; }

        /// <summary>
        /// Contructor of LogJournalier class
        /// </summary>
        public LogJournalier()
        {
            this.time = DateTime.Now;
        }




    }
}
