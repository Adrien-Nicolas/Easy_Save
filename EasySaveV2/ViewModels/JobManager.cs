using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;

using EasySave.Models;
using EasySave.Others;
using System.Threading;
using System.Windows;

namespace EasySave.ViewModels
{


    /// <summary>
    /// Class to manage saves
    /// </summary>
    class JobManager
    {


        public static string pathFileJob = AppDomain.CurrentDomain.BaseDirectory + "/Job/Job.json";


        /// <summary>
        /// Method to create a Job object and return it
        /// </summary>
        /// <param name="sourcePath">string</param>
        /// <param name="targetPath">string</param>
        /// <param name="nbr">int</param>
        /// <returns></returns>
        public static Job createJob(string sourcePath, string targetPath, int nbr)
        {

            JSON json = JSON.getJson();
            Job jobBase = new Job(sourcePath, targetPath);
            int nbrJob = 0;
            int nbrJobIdem = 0;
            DateTime lastSave = new DateTime();

            //findFileJob(AppDomain.CurrentDomain.BaseDirectory + "/Job/Job.json", jobBase, ref nbrJob, ref nbrJobIdem, ref lastSave);

            jobBase.name = "Save" + (nbrJob+nbr);
            jobBase.nbrSave = nbrJobIdem;
            jobBase.lastSave = (lastSave == DateTime.MinValue ? DateTime.Now : lastSave);


            return jobBase;

        }


        public static Mutex updateMutex = new Mutex();


        /// <summary>
        /// Method allowing to update a job
        /// </summary>
        /// <param name="job">Job</param>
        public static void Update(Job job)
        {
            
            List<Job> listJobExist = getAllJob();
            int index = FindIndexJobInList(job, listJobExist);


            if (index != -1)
            {
                listJobExist[index] = job;

                JSON json = JSON.getJson();

                updateMutex.WaitOne();
                json.WriteList<Job>(pathFileJob, listJobExist, true);
                updateMutex.ReleaseMutex();
            }


        
        }

        /// <summary>
        /// Method allowing to return a list of all Job
        /// </summary>
        /// <returns>List(Job)</returns>
        public static List<Job> getAllJob()
        {
            JSON json = JSON.getJson();
            return json.Read<Job>(AppDomain.CurrentDomain.BaseDirectory + "/Job/Job.json");

        }

        /// <summary>
        /// Method allowing to verify if a sourcePath of a Job exist
        /// </summary>
        /// <param name="job">Job</param>
        /// <returns>bool</returns>
        public static bool verifPath(Job job)
        {
            return File.Exists(job.sourcePath);
        }

        /// <summary>
        /// method allowing to add a new Job 
        /// </summary>
        public static void addNewJob()
        {
            Job job = new Job();
            List<Job> listJobExist = getAllJob();


            int incI = 1;

            do
            {
                job.name = "Save " + incI;
                incI++;
            } while (IsJobInList(job, listJobExist));

            JSON json = JSON.getJson();
            json.Write<Job>(pathFileJob, job);

        }

        /// <summary>
        /// Method allowing to verif if a job is in List of Job.
        /// </summary>
        /// <param name="jobBase">Job</param>
        /// <param name="listJob">List(job)</param>
        /// <returns>bool</returns>
        public static bool IsJobInList(Job jobBase, List<Job> listJob)
        {
            bool isIn = false;
            listJob.ForEach(delegate (Job job)
            {

                if (job.name == jobBase.name)
                    isIn = true;

            });

            return isIn;

        }


        /// <summary>
        /// Method alolowing to return index of a Job in a List of Job.
        /// </summary>
        /// <param name="jobBase">Job</param>
        /// <param name="listJob">List(Job)</param>
        /// <returns>int</returns>
        public static int FindIndexJobInList(Job jobBase, List<Job> listJob)
        {

            int index = -1;
            int inc = 0;
            listJob.ForEach(delegate (Job job)
            {
                if (job.name == jobBase.name)
                    index = inc;

                inc++;
            });

            return index;
        }


        /// <summary>
        /// Method allowing to delete Job with his index in the list of Job.
        /// </summary>
        /// <param name="jobToDel">Job</param>
        public static void DeleteJob(Job jobToDel)
        {

            List<Job> listJobExist = getAllJob();
            int index = FindIndexJobInList(jobToDel, listJobExist);


            if (index != -1)
            {
                listJobExist.RemoveAt(index);

                JSON json = JSON.getJson();
                json.WriteList<Job>(pathFileJob, listJobExist, true);
            }
            


        }

    }
}
