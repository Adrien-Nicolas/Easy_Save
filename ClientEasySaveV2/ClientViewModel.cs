using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientEasySaveV2
{
    class ClientViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private List<Job> _listJob;
        /// <summary>
        /// Method allowing to get value of a list
        /// </summary>
        public List<Job> ListJob
        {
            get
            {
                return _listJob;
            }
            set
            {
                if (_listJob == value || value == null)
                    return;

                _listJob = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ListJob)));
            }
        }
        /// <summary>
        /// Refrsh the listJob of the view
        /// </summary>
        /// <param name="listJobTemp"></param>
        public void RefreshList(List<Job> listJobTemp)
        {
            ListJob = listJobTemp;
        }

    }
}
