using EasySave.Models;
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
using EasySaveV2.ViewModels;
using EasySave.ViewModels;
using EasySave.Others;
using EasySaveV2.Others;
using System.Threading;
using System.Diagnostics;

namespace EasySaveV2.Views
{
    /// <summary>
    /// Logique d'interaction pour ListJob2View.xaml
    /// </summary>
    public partial class ListJobView : Page
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private Window window;
        private ListJobViewModel _viewModel = ListJobViewModel.GetListJobViewModel();

        //<Button Content = "Save" HorizontalAlignment="Left" Margin="435,536,0,0" Grid.Row="1" VerticalAlignment="Top" Click="Save_Click" Height="49" Width="240" Background="#FF97FF00"/>
        //<Button Content = "Add Job" Click="Click_Add_Job" HorizontalAlignment="Left" Margin="171,541,0,0" Grid.Row="1" VerticalAlignment="Top" Height="39" Width="177" Background="Yellow"/>
        //<Button Content = "Delete" Name="DelButton" HorizontalAlignment="Left" Margin="794,536,0,0" Grid.Row="1" VerticalAlignment="Top" Visibility="Hidden" Click="DelButton_Click" Height="42" Width="163" Background="Red"/>
        //<Button Content = "Back" Name="Back" Margin="171,605,852,129" Grid.Row="1" Width="177" Background="Brown" Click="Back_Click"/>
        //<Button Content = "Launch Job(s)" Name="LaunchButton" HorizontalAlignment="Left" Margin="423,630,0,0" Grid.Row="1" VerticalAlignment="Top" Visibility="Hidden" Height="97" Width="275" Background="Aqua" RenderTransformOrigin="0.5,0.5" Click="LaunchButton_Click"/>
        //<Button Content = "Load Job(s)" Name="LoadButton" HorizontalAlignment="Left" Margin="667,630,0,0" Grid.Row="1" VerticalAlignment="Top" Visibility="Hidden" Height="97" Width="275" Background="Orange" RenderTransformOrigin="0.5,0.5" Click="LoadButton_Click"></Button>
        
        private TranslateDico translateDico = TranslateDico.getDico();

        /// <summary>
        /// Method allowing to load traduction
        /// </summary>
        private void LoadListJob2View()
        {
            AddButton.Content = translateDico.getValue("AddJob");
            DelButton.Content = translateDico.getValue("Suppr");
            Back.Content = translateDico.getValue("Back");
        }

        /// <summary>
        /// Method allowing to init view
        /// </summary>
        /// <param name="w">Window</param>
        public ListJobView(Window w)
        {
            window = w;
            InitializeComponent();
            DataContext = _viewModel;

            mediaPlayer.Open(new Uri(@"./Assets/Sounds/encore_du_travail.mp3", UriKind.Relative));
            

            JobGrid.SelectionChanged += JobGrid_Clicked;

            LoadListJob2View();
            window.Content = this;
        }



        /// <summary>
        /// Method allowing to add a job on button push
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void Click_Add_Job(object sender, RoutedEventArgs e)
        {


            JobManager.addNewJob();
            _viewModel.RefreshList();


            mediaPlayer.Stop();
            mediaPlayer.Position = TimeSpan.Zero;
            
            mediaPlayer.Play();



        }

        

        /// <summary>
        /// Method allowing to show button when click on job
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">SelectionChangedEventArgs</param>
        private void JobGrid_Clicked(object sender, SelectionChangedEventArgs e)
        {
            DelButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Method allowing to delete a save Job.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (var item in JobGrid.SelectedItems)
            {
                Job job = item as Job;
                JobManager.DeleteJob(job);
            }

            _viewModel.RefreshList();

        }

        /// <summary>
        /// Method allowing to save a job on button push
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void SaveJob()
        {
           
            foreach (var item in JobGrid.Items)
            {
                Job job = item as Job;
                JobManager.Update(job);
            }
            _viewModel.RefreshList();


        }



        /// <summary>
        /// Method allowing to go back
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">RoutedEventArgs</param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new MainView(window);
        }


        /// <summary>
        /// Detect and change button value from an user choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateBoxPath(object sender, RoutedEventArgs e)
        {
            Button buttonTargetPath = sender as Button;
            buttonTargetPath.Content = WindowsElements.OpenDirectoryDialog(buttonTargetPath.Content.ToString());
            SaveJob();

        }

        /// <summary>
        /// Detect if user change checkbox value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            SaveJob();
        }

        /// <summary>
        /// Pause specific job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseJob_Click(object sender, RoutedEventArgs e)
        {
            Job jobToPause = (sender as Button).DataContext as Job;
            _viewModel.PauseJob(jobToPause);

        }

        /// <summary>
        /// Play specific Job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayJob_Click(object sender, RoutedEventArgs e)
        {

            Job jobToLaunch = (sender as Button).DataContext as Job;
            _viewModel.StartOrContinueJob(jobToLaunch);

        }


        /// <summary>
        /// Stop specific Job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopJob_Click(object sender, RoutedEventArgs e)
        {
            Job jobToStop = (sender as Button).DataContext as Job;
            _viewModel.StopJob(jobToStop);

        }

        /// <summary>
        /// Load specific Job
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadJob_Click(object sender, RoutedEventArgs e)
        {
            Job jobToStop = (sender as Button).DataContext as Job;
            _viewModel.LoadJob(jobToStop);
        }


        
    }
}
