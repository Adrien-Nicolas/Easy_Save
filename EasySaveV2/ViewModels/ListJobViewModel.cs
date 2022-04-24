using EasySave.Models;
using EasySave.Others;
using EasySave.ViewModels;
using EasySaveV2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace EasySaveV2.ViewModels
{
    class ListJobViewModel : INotifyPropertyChanged
    {
        private TranslateDico translateDico = TranslateDico.getDico();
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };


        private List<Job> _listJob;
        /// <summary>
        /// Method allowing to get value of a list
        /// </summary>
        public List<Job> ListJob
        {
            get
            {
                return _listJob;
            }
            set
            {
                if (_listJob == value)
                    return;

                _listJob = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListJob)));
            }
        }




        static private ListJobViewModel listJobViewModelInstance;

        /// <summary>
        /// Constructor of ListJob2ViewModel
        /// </summary>
        private ListJobViewModel()
        {
            ListJob = JobManager.getAllJob();

        }

        public static ListJobViewModel GetListJobViewModel()
        {
            if (listJobViewModelInstance == null)
                listJobViewModelInstance = new ListJobViewModel();

            return listJobViewModelInstance;
        }


        public static Mutex refreshMutex = new Mutex();

        /// <summary>
        /// Method allowing to refresh List
        /// </summary>
        internal void RefreshList()
        {
            if (refreshMutex.WaitOne(10))
            {
                ListJob = JobManager.getAllJob();
                refreshMutex.ReleaseMutex();
            }
        }


        public static Mutex ModifyList = new Mutex();
        /// <summary>
        /// Change the job in the list
        /// </summary>
        /// <param name="job"></param>
        internal void RefreshJob(Job job)
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {

                if (ModifyList.WaitOne(200))
                {
                    List<Job> newList = new List<Job>();

                    foreach (var item in ListJob)
                    {
                        if (item.name == job.name)
                            item.progress = job.progress;

                        newList.Add(item);
                    }

                    ListJob = newList;
                    Thread.Sleep(200);
                    ModifyList.ReleaseMutex();

                }
                
            });

        }

        /// <summary>
        /// Launch JobProcess if not exit, and continue it if exist and in pause
        /// </summary>
        /// <param name="job"></param>
        public void StartOrContinueJob(Job job)
        {
            LaunchJobService launchJobServiceTemp = LaunchJobService.getLaunchJobServiceFromJob(job);
            if (launchJobServiceTemp.job.name == "")
            {
                StartJob(job, false);
            }
            else if (launchJobServiceTemp.suspend == true)
            {
                ContinueJob(job);
            }

        }

        /// <summary>
        /// Launch JobProcess
        /// </summary>
        /// <param name="job"></param>
        private void StartJob(Job job, bool modeLoad)
        {
            LaunchJobService launchJobService = new LaunchJobService(job);

            launchJobService.OnLogiMetierDetected += (LaunchJobService l) =>
            {
                l.Suspend = true;
                LaunchJobService.listJobServiceActif.Remove(l);
                MessageBox.Show(translateDico.getValue("BusinessSoftware"));
            };

            launchJobService.OnProgressJobChanged += (Job job) =>
            {
                RefreshJob(job);
            };

            launchJobService.OnJobFinished += () =>
            {
                LaunchJobService.listJobServiceActif.Remove(launchJobService);

                RefreshJob(job);
                PlaySoundFinish();


            };

            LaunchJobService.listJobServiceActif.Add(launchJobService);

            Thread t = new Thread(new ThreadStart(() =>
            {
                launchJobService.Launch(false, modeLoad);
            }));
            t.Start();
        }

        private void PlaySoundFinish()
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(@"./Assets/Sounds/work_finish.mp3", UriKind.Relative));
            mediaPlayer.Position = TimeSpan.Zero;

            mediaPlayer.Play();
        }


        /// <summary>
        /// Pause Job if in Process
        /// </summary>
        /// <param name="job"></param>
        public void PauseJob(Job job)
        {
            LaunchJobService launchJobServiceTemp = LaunchJobService.getLaunchJobServiceFromJob(job);
            if (launchJobServiceTemp.job.name != "")
            {
                launchJobServiceTemp.Suspend = true;
            }
        }


        /// <summary>
        /// Stop Job if exist
        /// </summary>
        /// <param name="job"></param>
        public void StopJob(Job job)
        {
            LaunchJobService launchJobServiceTemp = LaunchJobService.getLaunchJobServiceFromJob(job);
            if (launchJobServiceTemp.job.name != "")
            {
                launchJobServiceTemp.Suspend = true;
                LaunchJobService.listJobServiceActif.Remove(launchJobServiceTemp);
            }


        }

        /// <summary>
        /// Continue a job if exist
        /// </summary>
        /// <param name="job"></param>
        private void ContinueJob(Job job)
        {
            LaunchJobService launchJobServiceTemp = LaunchJobService.getLaunchJobServiceFromJob(job);
            if (launchJobServiceTemp.job.name != "")
            {
                Thread t = new Thread(new ThreadStart(() =>
                {
                    launchJobServiceTemp.Launch(true);
                }));
                t.Start();
            }
        }

        /// <summary>
        /// Launch Job process in Load Mode
        /// </summary>
        /// <param name="job"></param>
        public void LoadJob(Job job)
        {
            LaunchJobService launchJobServiceTemp = LaunchJobService.getLaunchJobServiceFromJob(job);
            if (launchJobServiceTemp.job.name == "")
            {
                StartJob(job, true);
            }
        }


    }
}
