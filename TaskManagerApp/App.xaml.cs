using System.Windows;
using TaskManagerApp.HomePage;

namespace TaskManagerApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create and show MainWindow
            MainWindow mainWindow = new MainWindow();

            // Set the DataContext to a new instance of MainViewModel
            mainWindow.DataContext = new MainViewModel();

            // Show the main window
            mainWindow.Show();
        }
    }
}