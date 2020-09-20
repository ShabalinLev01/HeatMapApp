using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace HeatMapApp.ViewModel
{
    /// <summary>
    /// Service for save Image.
    /// </summary>
    public class SaveFileService
    {
        /// <summary>
        /// Method for save Image in format .jpeg
        /// </summary>
        /// <param name="imageSource">Image for save</param>
        public void SaveFile(BitmapSource imageSource)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                FileName = "HeatMap",
                Filter = "JPEG image(*.jpeg, .jpg)|*.jpeg",
                RestoreDirectory = true
            };
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream =
                    new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageSource));
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
