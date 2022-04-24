using EasySave.Models;
using EasySave.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasySaveV2.ViewModels
{

    public class Size
    {
        public string Name { get; set; }
    }
    class SizeFileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private long _size;
        public long Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (value == null)
                    return;
                _size = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Size)));
                }
            }
        } 
        
        private List<string> _sizeSource;
        public List<string> SizeSource
        {
            get
            {
                return _sizeSource;
            }
            set
            {
                if (value == null)
                    return;
                _sizeSource = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SizeSource)));
                }
            }
        } 
        

        public void CreateCombo()
        {
            List<string> sizeWordList = new List<string>();
            sizeWordList.Add("Octet");
            sizeWordList.Add("Kilooctet");
            sizeWordList.Add("Megaoctet");
            sizeWordList.Add("Gigaoctet");
            SizeSource = sizeWordList;
        }



        public void AddSize(string sizeWord)
        {

            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();

            if(sizeWord == "o")
            {
                Size = Size;
            }else if (sizeWord == "ko")
            {
                Size = Size * 1000;
            }
            else if (sizeWord == "mo")
            {
                Size = Size * 1000000;
            }
            else if (sizeWord == "go")
            {
                Size = Size * 1000000000;
            }

            settings.size = Size;

            settings.Update();
        }

        public void GetSize()
        {
            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();
            Size = settings.size;
        }

        public void DeleteSize()
        {
            JSON json = JSON.getJson();

            Settings settings = Settings.getSettings();

            settings.size = 0;

            settings.Update();
        }



    }
}
