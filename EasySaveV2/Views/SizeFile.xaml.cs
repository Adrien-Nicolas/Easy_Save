using EasySave.Others;
using EasySaveV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logique d'interaction pour SizeFile.xaml
    /// </summary>
    public partial class SizeFile : Page
    {
        private TranslateDico translateDico = TranslateDico.getDico();

        private readonly SizeFileViewModel _viewModel;
        private Window window;
        public SizeFile(Window w)
        {
            InitializeComponent();
            window = w;
            window.Content = this;
            _viewModel = new SizeFileViewModel();
            DataContext = _viewModel;
            GetSize();
            _viewModel.CreateCombo();

        }

    

        private void DeleteMethod(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteSize();
            GetSize();
        }
        private void BackMethod(object sender, RoutedEventArgs e)
        {

            window.Content = new ParametersView(window);
        }
        private void DefSizeMethod(object sender, RoutedEventArgs e)
        {
            var selectedSizeobj = ComboSize.SelectedItem;
            string selectedSize = selectedSizeobj.ToString();
            _viewModel.AddSize(selectedSize);
        }

        public void GetSize()
        {
            _viewModel.GetSize();
            ComboSize.SelectedIndex = 0;
        }

    }
}
