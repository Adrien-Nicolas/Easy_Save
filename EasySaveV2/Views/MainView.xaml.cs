using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using System.Text;
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
using EasySaveV2.ViewModels;
using EasySaveV2.Views;

namespace EasySaveV2.Views
{
    /// <summary>
    /// Logique d'interaction pour MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        private TranslateDico translateDico = TranslateDico.getDico();
        private Window window;
        public MainView(Window w)
        {
            window = w;
            InitializeComponent();


            initDico();

        }
        /// <summary>
        /// Method allowing to init dico
        /// </summary>
        private void initDico()
        {

            Save.Content = translateDico.getValue("SaveWork");
            Log.Content = translateDico.getValue("Logs");
            Parameters.Content = translateDico.getValue("Parameters");
            Quit.Content = translateDico.getValue("Quit");

        }

        /// <summary>
        /// Method allowing to call the exitViewModel
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void QuitView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ExitViewModel();
        }

        /// <summary>
        /// Method allowing to call ListJobView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void SaveView_Clicked(object sender, RoutedEventArgs e)
        {

            window.Content = new ListJobView(window);
        }

        /// <summary>
        /// Method allowing to call LogView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void LogView_Clicked(object sender, RoutedEventArgs e)
        {
            window.Content = new LogView(window);
        }
        /// <summary>
        /// Method allowing to call ParametersView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void ParametersView_Clicked(object sender, RoutedEventArgs e)
        {
            window.Content = new ParametersView(window);
        }
        /// <summary>
        /// Method allowing to exit Application
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void Quit_Clicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);

        }
    }
}
