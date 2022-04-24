using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EasySave.Others;
using EasySave.Models;
using System.Threading;

namespace EasySave.ViewModels
{
    /// <summary>
    /// Daily logs controller class
    /// </summary>
    class LogJournalierManager
    {
        private TranslateDico translateDico = TranslateDico.getDico();
        private InteractFile formatFile;

        static LogJournalierManager logJournalierManager;

        private LogJournalierManager(string interactFileName)
        {
            formatFile = InteractFile.getInstanceFromName(interactFileName);
        }

        static public LogJournalierManager GetLogJournalierManager()
        {

           if (logJournalierManager == null)
            {
                Settings setting = Settings.getSettings();
                logJournalierManager = new LogJournalierManager(setting.formatLogJournalier);
            }


            return logJournalierManager;
                

        }

        public void MajInteractFile(string interactFileName)
        {
            formatFile = InteractFile.getInstanceFromName(interactFileName);
        }



        /// <summary>
        /// Function to create a daily log 
        /// </summary>
        /// <param name="job">Job</param>
        /// <param name="fileSourcePath">string</param>
        /// <param name="fileTargetPath">string</param>
        /// <param name="transfertTime">double</param>
        /// <returns>LogJournalier</returns>
        public LogJournalier createFromJob(Job job, string fileSourcePath, string fileTargetPath, double transfertTime, double cryptTime)
        {
            LogJournalier logJournalier = new LogJournalier();

            logJournalier.cryptTime = cryptTime < 0 ? "error" : (cryptTime == 0 ? "" : cryptTime.ToString());


            logJournalier.name = Path.GetFileName(fileSourcePath);
            logJournalier.fileSource = fileSourcePath;
            logJournalier.fileTarget = fileTargetPath;
            logJournalier.destPath = job.targetPath;
            FileInfo fi = new FileInfo(fileSourcePath);
            logJournalier.fileSize = fi.Length;
            logJournalier.fileTransfertTimeMS = transfertTime;
          


            return logJournalier;
        }


        public static Mutex WriteLogMutex = new Mutex();

        /// <summary>
        /// Method to write the logJournal.json file with the created object LogJournal 
        /// </summary>
        /// <param name="logJournalier">LogJournalier</param>
        public void Write(LogJournalier logJournalier)
        {
            WriteLogMutex.WaitOne();
            formatFile.Write<LogJournalier>(AppDomain.CurrentDomain.BaseDirectory + "/LogJournalier/" + DateTime.Now.ToString("dd-MM-yyyy") + "."+formatFile.GetType().Name.ToLower(), logJournalier);
            WriteLogMutex.ReleaseMutex();
        }
    }
}
