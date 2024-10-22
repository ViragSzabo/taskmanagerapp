using System.Windows;

namespace TaskManagerApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // If you want to pass a TaskList object to ListView
            MainWindow win = new MainWindow();
            TaskList taskList = new TaskList("My Tasks"); // Example initialization
            ListView listView = new ListView(taskList); // Pass the taskList to the ListView
            win.Show();
            //listView.Show(); // Show the ListView
        }
    }
}
