using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveV2.ViewModels
{
    class ExitViewModel
    {

        public ExitViewModel()
        {
            System.Windows.Application.Current.Shutdown();

        }
        
    }
}
