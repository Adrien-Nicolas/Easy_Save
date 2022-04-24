using EasySave.Others;
using EasySaveV2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace EasySaveV2.ViewModels
{
    class PriorityFileViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private string _prioFile;
        public string PrioFileText
        {
            get
            {
                return _prioFile;
            }
            set
            {
                if (value == null)
                    return;
                _prioFile = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(PrioFileText)));
                }
            }
        }

        /// <summary>
        /// Method allowing to get List of extension
        /// </summary>
        /// <returns> List(string)</returns>
        public List<string> GetListPrioExtension()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/PriorityFile.json"))
            {
                var oui = File.Create(AppDomain.CurrentDomain.BaseDirectory + "/PriorityFile.json");
                oui.Close();
                initFile();
            }
            var myJsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/PriorityFile.json");
            List<Extension> PrioFileList = JsonConvert.DeserializeObject<List<Extension>>(myJsonString);
            List<string> PrioFileListString = new List<string>();
            foreach (var item in PrioFileList)
            {
                PrioFileListString.Add(String.Join("", item));
            }
            return PrioFileListString;
        }

        /// <summary>
        /// Method allowing to add extension in list and txt file
        /// </summary>
        /// <param name="ext">string</param>
        public void AppendinList()
        {
            PriorityFile prioFile = new PriorityFile();
            JSON json = JSON.getJson();
            prioFile.name = PrioFileText;
            json.Write<PriorityFile>(AppDomain.CurrentDomain.BaseDirectory + "/PriorityFile.json", prioFile);
        }


        public void initFile()
        {
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/PriorityFile.json", "[]");
        }
        /// <summary>
        /// Method allowing to delete extension in a list.
        /// </summary>
        /// <param name="ext">string</param>
        public void DeleteinList(string ext)
        {   
            JSON json = JSON.getJson();
            PriorityFile prioFile = new PriorityFile();
            List<PriorityFile> prioFileList = new List<PriorityFile>();
            List<string> prioFileListString = GetListPrioExtension();
            prioFileListString.Remove(ext);

            foreach (var item in prioFileListString)
            {
                prioFile.name= item;
                prioFileList.Add(prioFile);
            }

            var jsonFile = JsonConvert.SerializeObject(prioFileList);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/PriorityFile.json", jsonFile);
        }


    }
}
