using EasySave.Models;
using EasySave.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveV2.ViewModels
{
    public class ParametersViewModel : INotifyPropertyChanged
    {
        private TranslateDico translateDico = TranslateDico.getDico();

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Constructor of ParametersViewModel
        /// </summary>
        public ParametersViewModel()
        {
            RefreshDico();
        }

        /// <summary>
        /// Method allowing to refresh dico in language Combobox
        /// </summary>
        public void RefreshDico()
        {
            List<string> _languages = new List<string>();

            foreach (var item in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory+"Languages/"))
            {
                string str = Path.GetFileNameWithoutExtension(item) + Path.GetExtension(item).ToLower();

                _languages.Add(str);

                if (str == Path.GetFileName(translateDico.getDicoPath()))
                    LanguageSelected = str;


            }


            Languages = _languages;
        }


        private IList<string> _languages;

        /// <summary>
        /// Interface of Languages List
        /// </summary>
        public IList<string> Languages
        {
            get
            {
                return _languages;
            }
            set
            {
                if (value == _languages)
                    return;

                _languages = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Languages)));
            }
        }


        private string _languageSelected;

        /// <summary>
        /// Method allowing to get value of languageSelected
        /// </summary>
        public string LanguageSelected
        {
            get
            {
                return _languageSelected;
            }
            set
            {
                if (value == _languageSelected)
                    return;

                _languageSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LanguageSelected)));
            }
        }


        public void ToXml()
        {
            Settings settings = Settings.getSettings();

            settings.formatLogJournalier = "XML";
            settings.Update();
        }
        
        public void ToJson()
        {
            Settings settings = Settings.getSettings();

            settings.formatLogJournalier = "JSON";
            settings.Update();
        }
        public string GetFormat()
        {
            Settings settings = Settings.getSettings();
            return settings.formatLogJournalier;     
        }
        




    }
}
