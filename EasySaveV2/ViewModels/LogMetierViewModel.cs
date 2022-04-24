using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using EasySave.Others;
using EasySave.Models;

namespace EasySaveV2.ViewModels
{
    sealed class LogMetierViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method allowing to add a string in doc txt
        /// </summary>
        /// <param name="logicielMetier"></param>
        public void AddLogiMetier(string logicielMetier)
        {

            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();

            settings.logiMetier = logicielMetier;

            settings.Update();
        }

      

        /// <summary>
        /// Method allowing to read Logiciel metier in to a doc txt 
        /// </summary>
        /// <returns>string</returns>
        public string GetLogicielMetier()
        {

            JSON json = JSON.getJson();
            string LogicielMetier = Settings.getSettings().logiMetier;
            return LogicielMetier;

        }

        /// <summary>
        /// Method allowing to show if a process is running
        /// </summary>
        /// <param name="name">string</param>
        /// <returns>bool</returns>
        public bool IsRunning(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Method allowing to display text in text box
        /// </summary>
        public void DisplayTextBox()
        {
            TextBox = GetLogicielMetier();
        }


        public void DeleteLogiMetier()
        {
            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();

            settings.logiMetier = "";

            settings.Update();
        }

        private string _textBox;
        public string TextBox
        {
            get
            {
                return _textBox;
            }
            set
            {
                if (value == null)
                    return;
                _textBox = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(TextBox)));
                }
            }
        }


    }
}
