using System;
using Microsoft.Win32;

namespace HeatMapApp.ViewModel
{
    /// <summary>
    /// Service for opening .csv file
    /// </summary>
    public class OpenFileService
    {
        /// <summary>
        /// Method for opening data file
        /// </summary>
        /// <returns>Return file path or empty string If file don't selected</returns>
        public string openFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {Filter = "Text files (*.csv)|*.csv"};
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.InitialDirectory = desktop;
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return "";
            }
        }
    }
}
