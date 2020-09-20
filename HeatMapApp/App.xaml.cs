using System.Windows;
using System.Windows.Media.Imaging;
using HeatMapApp.ViewModels;

namespace HeatMapApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dataContext = new MainWindowViewModel();
            MainWindow = new MainWindow(){DataContext = dataContext};
            MainWindow.Show();
        }
    }
}