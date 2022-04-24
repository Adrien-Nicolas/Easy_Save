using EasySave.Models;
using EasySave.Others;
using EasySave.ViewModels;
using EasySaveV2.Others;
using EasySaveV2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace EasySaveV2.Services
{
    /// <summary>
    /// Use it to Manage and Launch job
    /// </summary>
    class LaunchJobService
    {
        static public Semaphore sizeMaxMutex = new Semaphore(2,2);

        static public List<LaunchJobService> listJobServiceActif = new List<LaunchJobService>();

        public Job job;
        public bool load;

        public delegate void ProgressJobChange(Job j);
        public event ProgressJobChange OnProgressJobChanged;

        public delegate void LogiMetierDetect(LaunchJobService l);
        public event LogiMetierDetect OnLogiMetierDetected;

        public delegate void JobFinish();
        public event JobFinish OnJobFinished;



        private int _progressBarSave;

        /// <summary>
        /// Method allowing to have value of progress bar 
        /// </summary>
        public int ProgressBarSave
        {
            get
            {
                return _progressBarSave;
            }
            set
            {
                if (_progressBarSave == value)
                    return;

                _progressBarSave = value;

                this.job.progress = _progressBarSave;

                OnProgressJobChanged(this.job);
            }
        }


        //private string _textInfos;

        ///// <summary>
        ///// method allowing to have value of textinfos
        ///// </summary>
        //public string TextInfos
        //{
        //    get
        //    {
        //        return _textInfos;
        //    }
        //    set
        //    {
        //        if (_textInfos == value)
        //            return;

        //        _textInfos = value;

        //        PropertyChanged(this, new PropertyChangedEventArgs(nameof(TextInfos)));
        //    }
        //}



        /// <summary>
        /// Check if the Work Processus is Running
        /// </summary>
        /// <returns></returns>
        public bool isLogicielMetierRunning()
        {
            LogMetierViewModel logMetierViewModel = new LogMetierViewModel();
            return logMetierViewModel.IsRunning(logMetierViewModel.GetLogicielMetier());
        }




        private TranslateDico dico = TranslateDico.getDico();


        public bool suspend { get; set; } = false;
        public bool Suspend
        {
            set { suspend = value; }
        }


        /// <summary>
        /// Get LaucnhJobService link to a job if exist
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public static LaunchJobService getLaunchJobServiceFromJob(Job job)
        {

            foreach (LaunchJobService launchJobService in LaunchJobService.listJobServiceActif)
            {

                if (launchJobService.job.name == job.name)
                {
                    return launchJobService;
                }
            }

            //Return empty laucnService with empty job
            return new LaunchJobService(new Job());
        }

        public LaunchJobService(Job job)
        {
            this.job = job;
        }


        /// <summary>
        /// Method allowing to lauch All save in a list of Job. We alson hash all file, delete and recreate targetfolder
        /// </summary>
        /// <param name="tabJob">List(Job)</param>
        public void Launch(bool reprendreInstant = false, bool l = false)
        {
            this.load = l;
            Suspend = false;

            if (isLogicielMetierRunning())
            {
                //MessageBox.Show(dico.getValue("BusinessSoftware"));
                OnLogiMetierDetected(this);
                return;

            }


            if (load)
                InverseJobPath();
            


            job.logEtat = new LogEtat();
            job.logEtat.nameJob = job.name;
            job.logEtat.state = "Non-Actif";


            Dictionary<string, string> dicoHashSourcePath = Hashing.GetAllHashFolder(job.sourcePath);
            Dictionary<string, string> dicoJobHashFile = Hashing.GetLastHashJob(job.name);

            List<string> fileToCopy = new List<string>();
            List<string> fileToDelete = new List<string>();


            List<string> oldFileList = new List<string>();


            List<string> listErrorDelete = new List<string>();
            List<string> listErrorCopy = new List<string>();



            //Define files to delete / copy with the different mode
            if (job.fullMode)
            {

                InteractFile.DeleteFolder(job.targetPath, true);
                fileToCopy = new List<string>(dicoHashSourcePath.Keys);

            }
            else
            {

                UpdateFilesToCopyDelete(out fileToCopy, out fileToDelete, dicoHashSourcePath, dicoJobHashFile, job);

            }


            fileToCopy = OrderFileByPriority(fileToCopy);
            bool reprendre = reprendreInstant ? true : CheckOldSave(job);


            job.logEtat = LogEtatManager.InitLogEtatFilesInfo(job, fileToCopy);

            //If we continue an old save, not reset logEtat
            if (!reprendre)
            {
                //Write "Non-Actif" state
                LogEtatManager.InitFile(job.logEtat);
            }


            listErrorCopy = LaunchCopy(fileToCopy, job, reprendre);
            listErrorDelete = LaunchDelete(fileToDelete, job);


            if (!suspend)
            {
                //Maj du hash en fct des supp et add
                dicoJobHashFile = Hashing.UpdateHashDico(dicoJobHashFile, fileToCopy, fileToDelete, job);

                //And write it
                Hashing.WriteAllHash(job.name, dicoJobHashFile);

                LogEtatManager.EndLogEtat(job.logEtat);

                job.nbrSave++;
                job.lastSave = DateTime.Now;
                //set logEtat to null to not write it in the file
                job.logEtat = null;
                job.progress = 0;

                ProgressBarSave = 100;

                if (load)
                    InverseJobPath();

                JobManager.Update(job);
            }

            if (!suspend)
                OnJobFinished();
            //View.ShowJobFinish(job, listErrorCopy, listErrorDelete);

        }

        /// <summary>
        /// Inverse the source and target path of the job
        /// </summary>
        private void InverseJobPath()
        {
            string pathTemp = job.sourcePath;
            job.sourcePath = job.targetPath;
            job.targetPath = pathTemp;
        }



        /// <summary>
        /// Check last logEtat and ask if user want to continue
        /// </summary>
        /// <param name="fileToCopy"></param>
        /// <param name="job"></param>
        private bool CheckOldSave(Job job)
        {
            List<LogEtat> listOldLogEtat = LogEtatManager.verifLastLogEtat(job.name);
            //If more than 1, it means that we have an old save
            if (listOldLogEtat.Count >= 1)
            {

                //Ask user if he wants to continue
                if (WindowsElements.VerifUserButton("Continue last Save for " + job.name + " ? " + listOldLogEtat[listOldLogEtat.Count - 1].progress + "%", job.name))
                {
                    return true;
                }


            }

            return false;
        }









        /// <summary>
        /// Method allowing to updateFiles to copy
        /// </summary>
        /// <param name="fileToCopy">out List(string)</param>
        /// <param name="fileToDelete">out List(string)</param>
        /// <param name="dicoHashSourcePath">Dictionary(string,string)</param>
        /// <param name="dicoJobHashFile">Dictionary(string,string)</param>
        /// <param name="job">Job</param>
        private void UpdateFilesToCopyDelete(out List<string> fileToCopy, out List<string> fileToDelete, Dictionary<string, string> dicoHashSourcePath, Dictionary<string, string> dicoJobHashFile, Job job)
        {

            List<string> fileToCopyTemp = new List<string>();
            List<string> fileToDeleteTemp = new List<string>();

            string hashTemp = "";

            foreach (var hash in dicoJobHashFile)
            {

                //string Get
                if (dicoHashSourcePath.TryGetValue(hash.Key.Replace(job.targetPath, job.sourcePath), out hashTemp))
                {


                    if (hashTemp != hash.Value)
                    {
                        fileToCopyTemp.Add(hash.Key.Replace(job.targetPath, job.sourcePath));
                    }
                }

                else
                {
                    fileToDeleteTemp.Add(hash.Key.Replace(job.sourcePath, job.targetPath));
                }


            }


            foreach (var hash in dicoHashSourcePath)
            {

                //string Get
                if (!dicoJobHashFile.TryGetValue(hash.Key.Replace(job.sourcePath, job.targetPath), out hashTemp))
                    fileToCopyTemp.Add(hash.Key);

            }

            fileToCopy = fileToCopyTemp;
            fileToDelete = fileToDeleteTemp;


        }


        /// <summary>
        /// Create a new list orderer by extension propriety
        /// </summary>
        /// <param name="fileToCopyTemp"></param>
        /// <returns></returns>
        private List<string> OrderFileByPriority(List<string> fileToCopyTemp)
        {
            PriorityFileViewModel prioViewModel = new PriorityFileViewModel();

            List<string> listOrdered = new List<string>();
            foreach (string extPrio in prioViewModel.GetListPrioExtension())
            {
                foreach (string fileTemp in fileToCopyTemp)
                {
                    if (Path.GetExtension(fileTemp) == extPrio)
                    {
                        listOrdered.Add(fileTemp);

                    }

                }
            }

            foreach (string item in listOrdered)
            {
                fileToCopyTemp.Remove(item);
            }
            listOrdered.AddRange(fileToCopyTemp);


            return listOrdered;


        }




        /// <summary>
        /// Method allowing to launch copy
        /// </summary>
        /// <param name="fileToCopy">List(string)</param>
        /// <param name="job">Job</param>
        /// <returns></returns>
        public List<string> LaunchCopy(List<string> fileToCopy, Job job, bool reprendre = false)
        {
            Settings settings = Settings.getSettings();
            List<string> listErrorCopy = new List<string>();

            SelectExtensionModelView viewModelExtension = new SelectExtensionModelView();
            //Get except extension list
            List<string> listExtensionForbidden = viewModelExtension.GetListExtension();


            List<string> listOldFileToCopy = new List<string>();
            if (reprendre)
                listOldFileToCopy = LogEtatManager.ListLogEtatToListSourcePath(LogEtatManager.getLastLogEtat(job.name));


            job.logEtat.state = "Actif";


            List<string> fileToFinish = new List<string>();

            foreach (string filePath in fileToCopy)
            {
                if (isLogicielMetierRunning())
                {
                    //dico.getValue("BusinessSoftware");
                    OnLogiMetierDetected(this);
                    Suspend = true;

                }
                //if suspend stop copy
                if (suspend == true)
                {
                    break;
                }
                else
                {
                    FileInfo file = new FileInfo(filePath);

                    if (file.Length > settings.size)
                    {
                        if (sizeMaxMutex.WaitOne(2000))
                        {
                            CryptOrCopyFile(filePath, listOldFileToCopy, listExtensionForbidden, job, settings);
                            sizeMaxMutex.Release();
                        }
                        else
                        {
                            fileToFinish.Add(filePath);
                        }

                    }
                    else
                    {
                        CryptOrCopyFile(filePath, listOldFileToCopy, listExtensionForbidden, job, settings);
                    }



                }
            }
            if (fileToFinish.Count > 0)
                LaunchCopy(fileToFinish, job, reprendre);

            if (!suspend)
                ProgressBarSave = 100;

            return listErrorCopy;
        }


        /// <summary>
        /// Lunch Copy or Crypt Copy according to the extension file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="listOldFileToCopy"></param>
        /// <param name="listExtensionCrypt"></param>
        /// <param name="job"></param>
        /// <param name="settings"></param>
        private void CryptOrCopyFile(string filePath, List<string> listOldFileToCopy, List<string> listExtensionCrypt, Job job, Settings settings)
        {


            string fileSourcePath = filePath;
            string fileTargetPath = filePath.Replace(job.sourcePath, job.targetPath);

            double cryptTime = 0;

            FileInfo file = new FileInfo(fileSourcePath);
            DateTime copyTime = DateTime.Now;

            //Maj View
            //TextInfos = Path.GetFileName(fileSourcePath);

            //If file was not copy before
            if (!listOldFileToCopy.Contains(filePath))
            {
                //We try the copy or the crypt + copy 
                try
                {


                    //Cryptt if extension is not in teh extension list
                    if (listExtensionCrypt.Contains(Path.GetExtension(fileSourcePath)))
                    {


                        //Crypt AND COPY
                        cryptTime = CryptoSoft.LaunchProcessCrypto(settings.crytpoKey, fileSourcePath, Path.GetDirectoryName(fileTargetPath));

                    }
                    else
                    {
                        //Else ONLY COPY and not set cryptTime
                        InteractFile.Copy(fileSourcePath, fileTargetPath);

                    }


                    LogJournalierManager logJournalierManager = LogJournalierManager.GetLogJournalierManager();
                    logJournalierManager.Write(logJournalierManager.createFromJob(job, fileSourcePath, fileTargetPath, -copyTime.Subtract(DateTime.Now).TotalMilliseconds, cryptTime));

                    job.logEtat.totalFileSize = file.Length;
                    job.logEtat.sourceFilePath = fileSourcePath;
                    job.logEtat.targetFilePath = fileTargetPath;

                    LogEtatManager.Write(job.logEtat);


                }
                //If error, add in listError
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    //listErrorCopy.Add(fileSourcePath);
                }
            }

            job.logEtat.totalFileLeft--;
            job.logEtat.progress = ((job.logEtat.totalFileToCopy - job.logEtat.totalFileLeft)) * 100 / (job.logEtat.totalFileToCopy == 0 ? 1 : job.logEtat.totalFileToCopy);


            //Draw Progress on View
            ProgressBarSave = job.logEtat.progress;

        }







        /// <summary>
        /// Method allowing to delete file with one list of path in parameter
        /// </summary>
        /// <param name="fileToDelete">list(string)</param>
        /// <param name="job">Job</param>
        /// <returns></returns>
        internal List<string> LaunchDelete(List<string> fileToDelete, Job job)
        {

            List<string> listErrorDelete = new List<string>();

            fileToDelete.ForEach((string file) =>
            {
                if (suspend)
                    return;
                else if (!InteractFile.DeleteFile(file))
                    listErrorDelete.Add(file);

            });

            return listErrorDelete;
        }




    }

}
