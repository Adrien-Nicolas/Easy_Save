using EasySave.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave.Others
{
    /// <summary>
    /// Dictionary controller, allowing to manage the language change.    
    /// </summary>
    class TranslateDico
    {
        private string dicoPath;
        private static TranslateDico SingleDico;

        /// <summary>
        /// Constructor of the TranslateDico class
        /// </summary>
        private TranslateDico()
        {

            JSON json = JSON.getJson();

            Settings setting = Settings.getSettings();

            if(!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Languages/" + setting.language))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/Languages");
                throw new Exception("LanguageFileMissing");
            }


            this.dicoPath = AppDomain.CurrentDomain.BaseDirectory + "/Languages/" + setting.language;
        }
        /// <summary>
        /// Method to retrieve an object of the TranslateDico class (singleton)
        /// </summary>
        /// <returns>Object of Dicocontrolleur </returns>
        public static TranslateDico getDico()
        {
            
            if (SingleDico == null)
                SingleDico = new TranslateDico();

            return SingleDico;
        }

        /// <summary>
        /// Method to return the value of a dictionary key
        /// </summary>
        /// <param name="val">string</param>
        /// <returns>string</returns>
        public string getValue(string val)
        {
            string value;

            try
            {
                JSON json = JSON.getJson();
                value = json.Read(this.dicoPath)[val];

            }
            catch
            {
                value = val+"??";
            }


            return value;
        }


        /// <summary>
        /// Method allowing to return a dico path with language
        /// </summary>
        /// <param name="language">string</param>
        public void GoingLanguage(string language)
        {
            MajConfigLanguage(language);
            this.dicoPath = AppDomain.CurrentDomain.BaseDirectory + "/Languages/" + language;
        }

        /// <summary>
        /// Method allowing tu update language configuration
        /// </summary>
        /// <param name="name">string</param>
        private void MajConfigLanguage(string name)
        {
            JSON json = JSON.getJson();
            Settings settings = Settings.getSettings();

            settings.language = name;

            settings.Update();
        } 

        /// <summary>
        /// Method allowing to return dico path
        /// </summary>
        /// <returns></returns>
        public string getDicoPath()
        {
            return this.dicoPath;
        }


    }
}
