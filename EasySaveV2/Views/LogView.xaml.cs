using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using EasySave.Others;
using EasySaveV2.Views;

namespace EasySaveV2.Views
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : Page
    {

        private Window window;
        private TranslateDico translateDico = TranslateDico.getDico();
       
        /// <summary>
        /// Method allowing to load traduction of LogView page
        /// </summary>
        private void LoadLogView()
        {
            OpenLogJournalier.Content = translateDico.getValue("OpenLogJournalier");
            OpenLogEtat.Content = translateDico.getValue("OpenLogEtat");
            back.Content = translateDico.getValue("Back");

        }

        /// <summary>
        /// Method allowing to init LogViewPage
        /// </summary>
        /// <param name="w">Window</param>
        public LogView(Window w)
        {
            window = w;
            InitializeComponent();
            window.Content = this;
            LoadLogView();
        }

        /// <summary>
        /// Method allowing to openLogJournalier on click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OpenLogJournalier_Click(object sender, RoutedEventArgs e)
        {
            ShowLog(AppDomain.CurrentDomain.BaseDirectory + "/LogJournalier");
        }

        /// <summary>
        /// Method allowing to OpenLogEtat on click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OpenLogEtat_Click(object sender, RoutedEventArgs e)
        {
            ShowLog(AppDomain.CurrentDomain.BaseDirectory + "/LogEtat/");
        }

        /// <summary>
        /// Method allowing to go back on last page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new MainView(window);
        }


        /// <summary>
        /// Method to get a filled path file
        /// </summary>
        /// <param name="path">string</param>
        private void ShowLog(string path)
        {
            Process processus = new Process();
            processus.StartInfo.FileName = path;
            processus.StartInfo.UseShellExecute = true;
            processus.Start();
            processus.Close();

        }
    }
}
