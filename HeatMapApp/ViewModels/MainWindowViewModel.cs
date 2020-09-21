using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HeatMapApp.ViewModel;

namespace HeatMapApp.ViewModels
{
    /// <summary>
    /// The class that implements the ViewModel for the MainWindow
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly OpenFileService _openFileService; 
        private readonly SaveFileService _saveFileService;
        private readonly HeatMapService _heatMapService;
        private BitmapSource _imageSource;

        /// <summary>
        /// Accepts delegate for CommandOpen
        /// </summary>
        public ICommand CommandOpen => new DelegateCommand(OpenCommand);

        /// <summary>
        /// Accepts delegate for CommandSave
        /// </summary>
        public ICommand CommandSave => new DelegateCommand(SaveCommand);

        /// <summary>
        /// Informs a view that the properties of some object was changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor of MainWindowViewModel
        /// </summary>
        public MainWindowViewModel()
        {
            _openFileService = new OpenFileService();
            _saveFileService = new SaveFileService();
            _heatMapService = new HeatMapService();
        }

        /// <summary>
        /// The method called when it is necessary to inform that the object has changed
        /// </summary>
        /// <param name="propertyName">Name of the changed object</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Property bound to an object in a view
        /// </summary>
        public BitmapSource ImageSource
        {
            get => _imageSource;

            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        private void OpenCommand()
        {
            string fileName = _openFileService.openFile();
            if (fileName != "")
            {
                BitmapSource tempBitmapSource;
                Task.Run(() =>
                {
                    tempBitmapSource = _heatMapService.GenerateHeatMap(fileName);
                    tempBitmapSource.Freeze();
                    ImageSource = tempBitmapSource;
                });
            }
        }
        private void SaveCommand()
        {
            if (ImageSource != null)
            {
                _saveFileService.SaveFile(ImageSource);
            }
        }
    }
}
