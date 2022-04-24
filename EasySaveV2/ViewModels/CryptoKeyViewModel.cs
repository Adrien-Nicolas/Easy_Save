using EasySave.Models;
using EasySave.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySaveV2.ViewModels
{
    /// <summary>
    /// ViewModel of the parameter cryptoKey
    /// </summary>
    class CryptoKeyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private string _cryptoKey;
        public string CryptoKey
        {
            get
            {
                return _cryptoKey;
            }
            set
            {
                if (value == null)
                    return;
                _cryptoKey = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(CryptoKey)));
                }
            }
        }


        /// <summary>
        /// Change Crypto Key
        /// </summary>
        public void AddCryptoKey()
        {

            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();

            settings.crytpoKey = CryptoKey;

            settings.Update();
        }


        /// <summary>
        /// Get the old cryptoKey
        /// </summary>
        public void GetCryptoKey()
        {
            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();
            CryptoKey = settings.crytpoKey;
        }
        

        /// <summary>
        /// Delete the actual crypto key
        /// </summary>
        public void DeleteCryptoKey()
        {
            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();

            settings.crytpoKey = "";

            settings.Update();
        }



    }


    }

