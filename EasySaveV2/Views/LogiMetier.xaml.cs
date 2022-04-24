using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace EasySaveV2.Views
{
    /// <summary>
    /// Logique d'interaction pour LogiMetier.xaml
    /// </summary>
    public partial class LogiMetierView : Page

    {
        private Window window;

        private readonly LogMetierViewModel _viewModel;
        private TranslateDico translateDico = TranslateDico.getDico();

        /// <summary>
        /// Method allowing to load traduction for LogiMetierView
        /// </summary>
        private void LoadLogiMetier()
        {
            DefLogMetier.Content = translateDico.getValue("LogiMetier");
            Back.Content = translateDico.getValue("Back");
            BaseLabel.Content = translateDico.getValue("LogiMetierSelect");
           
        }

        /// <summary>
        /// Init page LogiMetier
        /// </summary>
        /// <param name="w"></param>
        public LogiMetierView(Window w)
        {

            InitializeComponent();
           
            window = w;
            window.Content = this;
            _viewModel = new LogMetierViewModel();
            _viewModel.DisplayTextBox();
            DataContext = _viewModel;
            GetProcessComboBox();
            string cal = GetLogicielMetier();
            bool Run = IsRunning(cal);
             LoadLogiMetier();
        }


        /// <summary>
        /// Method to get Logiciel Metier et return it
        /// </summary>
        /// <returns>string</returns>
        public string GetLogicielMetier()
        {
            string cal = _viewModel.GetLogicielMetier();
            TextBox1.Text = cal;
            return cal;

        }

        /// <summary>
        /// Method to call ViewModel method
        /// </summary>
        /// <param name="name">string</param>
        /// <returns>bool</returns>
        public bool IsRunning(string name)
        {
            bool Run = _viewModel.IsRunning(name);
            return Run;
        }


        /// <summary>
        /// Method to add Process in combobox
        /// </summary>
        public void GetProcessComboBox()
        {
            Process[] AllProcess = Process.GetProcesses();
            foreach (var item in AllProcess)
            {
                ComboBox1.Items.Add(item.ProcessName);
            }
        }

        /// <summary>
        /// Method allowing to listen event ONclick on button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        public void OnClick1(object sender, RoutedEventArgs e)
        {
            var item = ComboBox1.SelectedItem;
            
            string itemString = item.ToString();
            foreach (var process in Process.GetProcessesByName(itemString))
            {
                var processSelected = process;
            }
            _viewModel.AddLogiMetier(itemString);
            _viewModel.DisplayTextBox();
        }

        /// <summary>
        /// Method allowing to listen event Onclick on button exit
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        public void OnClick2(object sender, RoutedEventArgs e)
        {
            window.Content = new ParametersView(window);
        }
         public void OnClick3(object sender, RoutedEventArgs e)
        {
            TextBox1.Text = String.Empty;
            _viewModel.DeleteLogiMetier();
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
