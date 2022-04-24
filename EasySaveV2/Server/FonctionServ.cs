using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Text;
using EasySaveV2.Services;
using Newtonsoft.Json;
using System.Windows;

/// <summary>
/// Class and methods allow to use by our clients
/// </summary>
namespace EasySaveV2.Server.FonctionServ
{

    /// <summary>
    /// Class to modify the Job list
    /// </summary>
    class ListJobClient
    {
        ViewModels.ListJobViewModel listJobViewModel = ViewModels.ListJobViewModel.GetListJobViewModel();
        
        /// <summary>
        /// Return the actual list of job who exist with their progress bar
        /// </summary>
        /// <returns></returns>
        public string GetListJob()
        {
            return JsonConvert.SerializeObject(listJobViewModel.ListJob);
        }

        /// <summary>
        /// Add a job to the app
        /// </summary>
        /// <returns></returns>
        public string AddJob()
        {
            EasySave.ViewModels.JobManager.addNewJob();
            listJobViewModel.RefreshList();
            return "done";
        }


        /// <summary>
        /// Return all job in progress ONLY
        /// </summary>
        /// <returns></returns>
        public string GetListJobLaunchOnly()
        {
            List<Job> newList = new List<Job>();
            foreach (var jobServiceActif in LaunchJobService.listJobServiceActif)
            {
                newList.Add(jobServiceActif.job);
            }
            return JsonConvert.SerializeObject(newList);
        }

    }

    /// <summary>
    /// Class allow us to launch, stop and pause JobProgress using the Singleton ListJobViewModel
    /// </summary>
    class LaunchJobController
    {
        ViewModels.ListJobViewModel listJobViewModel = ViewModels.ListJobViewModel.GetListJobViewModel();

        /// <summary>
        /// Play a specific Job
        /// </summary>
        /// <param name="jobStr"></param>
        /// <returns></returns>
        public string PlayJob(string jobStr)
        {
            listJobViewModel.StartOrContinueJob(JsonConvert.DeserializeObject<Job>(jobStr));
            return "done";
        }

        /// <summary>
        /// Stop a specific Job
        /// </summary>
        /// <param name="jobStr"></param>
        /// <returns></returns>
        public string StopJob(string jobStr)
        {
            //listJobViewModel.StopJob(JsonConvert.DeserializeObject<Job>(jobStr));
            PauseJob(jobStr);
            return "done";
        }

        /// <summary>
        /// Pause a specific Job
        /// </summary>
        /// <param name="jobStr"></param>
        /// <returns></returns>
        public string PauseJob(string jobStr)
        {
            listJobViewModel.PauseJob(JsonConvert.DeserializeObject<Job>(jobStr));
            return "done";
        }


        /// <summary>
        /// Load a specific Job
        /// </summary>
        /// <param name="jobStr"></param>
        /// <returns></returns>
        public string LoadJob(string jobStr)
        {
            listJobViewModel.LoadJob(JsonConvert.DeserializeObject<Job>(jobStr));
            return "done";
        }
    }




}
