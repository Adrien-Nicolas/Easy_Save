using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EasySaveV2.Models;

using EasySave.Others;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EasySaveV2.ViewModels
{
    class SelectExtensionModelView
    {
        /// <summary>
        /// Method allowing to get List of extension
        /// </summary>
        /// <returns> List(string)</returns>
        public List<string> GetListExtension()
        {
            
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Extension.json"))
            {
                var oui = File.Create(AppDomain.CurrentDomain.BaseDirectory + "/Extension.json");
                oui.Close();
                initFile();
            }
            var myJsonString = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Extension.json");
            List<Extension>ExtOrderListe = JsonConvert.DeserializeObject<List<Extension>>(myJsonString);
            List<string> ExtListeString = new List<string>();
            foreach (var item in ExtOrderListe)
            {
                ExtListeString.Add(String.Join("",item));
            }
            return ExtListeString;
        }

        /// <summary>
        /// Method allowing to add extension in list and txt file
        /// </summary>
        /// <param name="ext">string</param>
        public void AppendinList(string ext)
        {
            Extension extension = new Extension();
            JSON json = JSON.getJson();
            extension.name = ext;
            json.Write<Extension>(AppDomain.CurrentDomain.BaseDirectory + "/Extension.json", extension);
        }


        public void initFile()
        {
             File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Extension.json", "[]");
        }
        /// <summary>
        /// Method allowing to delete extension in a list.
        /// </summary>
        /// <param name="ext">string</param>
        public void DeleteinList(string ext)
        {
            Extension extension = new Extension();
            JSON json = JSON.getJson();
            List<Extension> ExtOrderListe = new List<Extension>();
            List<string> extensionList = GetListExtension();
            extensionList.Remove(ext);

            foreach (var item in extensionList)
            {
                extension.name = item;
                ExtOrderListe.Add(extension);
            }

            var jsonFile = JsonConvert.SerializeObject(ExtOrderListe);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Extension.json", jsonFile);
        }


    }
}
