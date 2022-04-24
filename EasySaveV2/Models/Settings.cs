using EasySave.Others;
using EasySave.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Models
{
    /// <summary>
    /// Settings class
    /// </summary>
    class Settings
    {

        static private Settings setting;


        public string language { get; set; } = "FRANCAIS.json";
        public string logiMetier { get; set; } = "";
        public string formatLogJournalier { get; set; } = "JSON";
        public string crytpoKey { get; set; } = "Key";
        public long size { get; set; } = 5000000;


        /// <summary>
        /// Constructor of Settings Class
        /// </summary>
        /// <param name="language">string</param>
        /// <param name="nbrEtat">int</param>
        /// <param name="logiciel">string</param>
        /// <param name="crytpoKey">string</param>
        /// <param name="size">string</param>
        /// 
        private Settings()
        {
        }



        static public Settings getSettings()
        {

            if (setting == null)
            {

                JSON json = new JSON();
                try
                {
                    setting = json.Read<Settings>(AppDomain.CurrentDomain.BaseDirectory + "conf.json")[0];
                }
                catch
                {
                    setting = new Settings();
                    json.Write<Settings>(AppDomain.CurrentDomain.BaseDirectory + "conf.json", setting);
                }

                    
                

            }

            return setting;
        }

        internal void Update()
        {
            JSON json = JSON.getJson();
            LogJournalierManager.GetLogJournalierManager().MajInteractFile(this.formatLogJournalier);
            InteractFile.DeleteFile(AppDomain.CurrentDomain.BaseDirectory + "/conf.json");
            json.Write(AppDomain.CurrentDomain.BaseDirectory + "/conf.json", setting);
        }


    }
}
