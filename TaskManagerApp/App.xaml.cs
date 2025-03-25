using System.Windows;
using TaskManagerApp.HomePage;

namespace TaskManagerApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Create the startup (base.OnStartup(e))
            MainWindow mainWindow = new MainWindow();

            // Set the DataContext to a new instance of MainViewModel
            // mainWindow.DataContext = new MainWindow();

            // Show the main window
            mainWindow.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            await ((MainViewModel)MainWindow.DataContext).SaveOnExist();
        }

    }
}