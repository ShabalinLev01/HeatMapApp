using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HeatMapApp.ViewModel;

namespace HeatMapApp.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly OpenFileService _openFileService; 
        private readonly SaveFileService _saveFileService;
        private readonly HeatMapService _heatMapService; 
        private BitmapSource _imageSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand CommandOpen => new DelegateCommand(OpenCommand);
        /// <summary>
        /// 
        /// </summary>
        public ICommand CommandSave => new DelegateCommand(SaveCommand);

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public MainWindowViewModel()
        {
            _openFileService = new OpenFileService();
            _saveFileService = new SaveFileService();
            _heatMapService = new HeatMapService();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
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
