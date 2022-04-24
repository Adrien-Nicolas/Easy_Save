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
    /// Logique d'interaction pour ParametersView.xaml
    /// </summary>
    public partial class ParametersView : Page
    {
        private Window window;
        private ParametersViewModel parametersViewModel = new ParametersViewModel();
        private TranslateDico translateDico = TranslateDico.getDico();
        
        /// <summary>
        /// Method allowing to init and display ParametersView 
        /// </summary>
        /// <param name="w">Window</param>
        public ParametersView(Window w)
        {
            window = w;
            InitializeComponent();

            DataContext = parametersViewModel;

            loadLanguageView();

            window.Content = this;
            InitRadioButton();
        }

        /// <summary>
        /// Method allowing to load LanguageView traduction
        /// </summary>
        private void loadLanguageView()
        {
            HavingLanguage.Text = translateDico.getValue("HavingLanguage");
            LogiMetier.Content = translateDico.getValue("LogiMetier");
            Back.Content = translateDico.getValue("Back");
            SelectExtension.Content = translateDico.getValue("SelectExtension");
        }

        /// <summary>
        /// Method allowing to call LogiMetier View
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void LogiMetier_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new LogiMetierView(window);
           
        }

       
        private void Languages_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Method allowing to come back on last page
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new MainView(window);
        }

        /// <summary>
        /// Method allowing to call SelectExtensionView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void SelectExtension_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new SelectExtensionView(window);
        }

        /// <summary>
        /// Method allowing to display ChangleLanguage combobox + change language
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void ChangeLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string newLanguage = ComboBoxLanguage.SelectedItem as string;
            translateDico.GoingLanguage(newLanguage);
            loadLanguageView();

        }
        /// <summary>
        /// Method allowing to call CryptoKeyView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void CryptoKey_Click(object sender, RoutedEventArgs e)
        {

            window.Content = new CryptoKey(window);

        }

        /// <summary>
        /// Method allowing to Change format in XML
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void XML(object sender, RoutedEventArgs e)
        {
            parametersViewModel.ToXml();
        }

        /// <summary>
        /// Method allowing to Change format in JSON
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void JSON(object sender, RoutedEventArgs e)
        {
            parametersViewModel.ToJson();
        }


        public void InitRadioButton()
        {
            string format = parametersViewModel.GetFormat();
            Trace.Write(format);
            if (format == "JSON")
            {
                Json.IsChecked = true;   
            }
            else
            {
                Xml.IsChecked = true;
            }
        }

        /// <summary>
        /// Method allowing to call SizeFileView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void SizeFile_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new SizeFile(window);
        }

        /// <summary>
        /// Method allowing to call PriorityFileView
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void PrioFile_Click(object sender, RoutedEventArgs e)
        {
           window.Content = new PriorityFile(window);
        }

        

    }
}
