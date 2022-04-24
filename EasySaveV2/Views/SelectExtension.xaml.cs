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
    /// Logique d'interaction pour SelectExtension.xaml
    /// </summary>
    public partial class SelectExtensionView : Page
    {


        //  <Label Margin="74,113,726,139">Entrez une extension à exclure</Label>
        //  <TextBox Name = "ExtensionTexte"  Margin="74,144,726,108" RenderTransformOrigin="0.484,1.469"  />
        //  <Button Name = "btn1" Click="OnClick1" Margin="351,144,449,108" >Ajouter</Button>
        //  <Button Name = "btn2" Click="OnClick2" Margin="351,23,449,325" Grid.Row="1" >Supprimer</Button>
        //  <Button Name = "btn3" Content="Back" Click="btn3_Click"  Margin="351,148,449,200" Grid.Row="1" ></Button>

        private TranslateDico translateDico = TranslateDico.getDico();
        /// <summary>
        /// Method allowing to have traduction on the page
        /// </summary>
        private void LoadSelectExtensionView()
        {
            btn1.Content = translateDico.getValue("Add"); 
            btn2.Content = translateDico.getValue("Delete");
            btn3.Content = translateDico.getValue("Back");
            ExtensionTexte.Text = translateDico.getValue("ExtensionText");
        }

        private readonly SelectExtensionModelView _viewModel;
        private Window window;
        /// <summary>
        /// Method allowing to display SelectExtensionView
        /// </summary>
        /// <param name="w">Window</param>
        public SelectExtensionView(Window w)
        {
            InitializeComponent();
            window = w;
            window.Content = this;
            _viewModel = new SelectExtensionModelView();
            DataContext = _viewModel;
            LoadSelectExtensionView();
            ShowListView();
        }
        /// <summary>
        /// MEthod allowing to get value of textBox
        /// </summary>
        /// <returns>string</returns>
        public string GetValueOfTextBox()
        {
            string extension = ExtensionTexte.Text;
            return extension;
        }
        /// <summary>
        /// Method allowing to get list of extension
        /// </summary>
        /// <returns>List(string)</returns>
        public List<string> GetListExtension()
        {
            List<string> line = _viewModel.GetListExtension();
            return line;
        }

        /// <summary>
        /// Method allowing to append extension in list
        /// </summary>
        /// <param name="ext">string</param>
        public void AppendinList(string ext)
        {
            _viewModel.AppendinList(ext);
        }

        /// <summary>
        /// Method allowing to add List extension in list
        /// </summary>
        public void ShowListView()
        {
            ListView1.Items.Clear();
            List<string> extension = new List<string>();
            extension = GetListExtension();
            foreach (var item in extension)
            {
                ListView1.Items.Add(item);
            }
        }

        /// <summary>
        /// Method allowing to delete extension in list
        /// </summary>
        /// <param name="ext">string</param>
        public void DeleteinList(string ext)
        {
            _viewModel.DeleteinList(ext);
        }

        /// <summary>
        /// method allowing to get text of textbox
        /// </summary>
        /// <returns></returns>
        public string GetTextBox()
        {
            string TextBox = ExtensionTexte.Text;
            return TextBox;

        }
        /// <summary>
        /// Method allowing to append text of text box in list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OnClick1(object sender, RoutedEventArgs e)
        {
            string Text = GetTextBox();
            AppendinList(Text);
            ShowListView();
        }

        /// <summary>
        /// Method allowing to delete text of text box in list
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void OnClick2(object sender, RoutedEventArgs e)
        {
            var text = ListView1.SelectedItems;
            foreach (string item in text)
            {
                DeleteinList(item);
            }
            ShowListView();
        }

        /// <summary>
        /// Method allowing to return on last view
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new ParametersView(window);
        }
    }
}
