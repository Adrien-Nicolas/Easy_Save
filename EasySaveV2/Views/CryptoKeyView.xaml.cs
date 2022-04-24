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
using EasySave.Others;
using EasySaveV2.ViewModels;

namespace EasySaveV2.Views
{

    /// <summary>
    /// Logique d'interaction pour CryptoKey.xaml
    /// </summary>
    public partial class CryptoKey : Page
    {  
        private TranslateDico translateDico = TranslateDico.getDico();

        private readonly CryptoKeyViewModel _viewModel;
        private Window window;
        public CryptoKey(Window w)
        {
            InitializeComponent();
            window = w;
            window.Content = this;
            _viewModel = new CryptoKeyViewModel();
            DataContext = _viewModel;
            GetCryptoKey();
        }

        private void DeleteMethod(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteCryptoKey();
            GetCryptoKey();
        }
        private void DefCrytpoKeyMethod(object sender, RoutedEventArgs e)
        {
            _viewModel.AddCryptoKey();

        }
        private void BackMethod(object sender, RoutedEventArgs e)
        {
            window.Content = new ParametersView(window);
        }


        public void GetCryptoKey()
        {
            _viewModel.GetCryptoKey();
        }



    }
}
