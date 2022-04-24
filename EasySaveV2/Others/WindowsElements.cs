using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasySaveV2.Others
{

    /// <summary>
    /// Class to define methods to interract with the user
    /// </summary>
    class WindowsElements
    {

        /// <summary>
        /// Open a folder and ask user folder path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string OpenDirectoryDialog(string filePath = "")
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            openFileDlg.SelectedPath = filePath;

            var result = openFileDlg.ShowDialog();

            if (result.ToString() != string.Empty)
            {
                 return openFileDlg.SelectedPath;
            }


            return filePath;
        }


        /// <summary>
        /// Show to the user, a dialog yes or no
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool VerifUserButton(string message, string title)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
