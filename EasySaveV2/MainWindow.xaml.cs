using EasySaveV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasySaveV2.Views;
using EasySaveV2.Others;
using EasySave.Models;
using EasySave.ViewModels;
using System.ComponentModel;
using System.Threading;
using Server;
namespace EasySaveV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
          
                InitializeComponent();

                LogJournalierManager logJournalierManager = LogJournalierManager.GetLogJournalierManager();
                logJournalierManager.Write(new LogJournalier());

                //Trace.WriteLine(uri.AbsolutePath);

                this.Content = new MainView(this);

            Listener listener = new Listener();
            listener.StartListening();
        }
    }

        
        
 
}

