using EasySave.Others;
using EasySaveV2.ViewModels;
using System;
using System.Collections.Generic;
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

namespace EasySaveV2.Views
{
    /// <summary>
    /// Logique d'interaction pour PriorityFile.xaml
    /// </summary>
    /// 
    public partial class PriorityFile : Page
    {

        private TranslateDico translateDico = TranslateDico.getDico();
        private readonly PriorityFileViewModel _viewModel;
        private Window window;
        public PriorityFile(Window w)
        {
            InitializeComponent();
            window = w;
            window.Content = this;
            _viewModel = new PriorityFileViewModel();
            DataContext = _viewModel;
            ShowListView();

        }
        /// <summary>
        /// Method allowing to add List extension in list
        /// </summary>
        public void ShowListView()
        {
            ListView1.Items.Clear();
            List<string> extension = new List<string>();
            extension = GetListPrioExtension();
            foreach (var item in extension)
            {
                ListView1.Items.Add(item);
            }
        }
        private void DeleteMethod(object sender, RoutedEventArgs e)
        {
            var text = ListView1.SelectedItems;
            foreach (string item in text)
            {
                DeleteinList(item);
            }
            ShowListView(); 
        }
        private void BackMethod(object sender, RoutedEventArgs e)
        {
            window.Content = new ParametersView(window);
        }
        private void DefProFileMethod(object sender, RoutedEventArgs e)
        {
            _viewModel.AppendinList();
            ShowListView();
        }

        /// <summary>
        /// Method allowing to get list of extension
        /// </summary>
        /// <returns>List(string)</returns>
        public List<string> GetListPrioExtension()
        {
            List<string> line = _viewModel.GetListPrioExtension();
            return line;
        }

        /// <summary>
        /// Method allowing to delete extension in list
        /// </summary>
        /// <param name="ext">string</param>
        public void DeleteinList(string ext)
        {
            _viewModel.DeleteinList(ext);
        }


    }
}
