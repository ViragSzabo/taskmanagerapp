using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskManagerApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private TaskManager _taskManager;

        public ObservableCollection<TaskList> TaskLists => _taskManager.TaskLists;

        public MainViewModel()
        {
            _taskManager = new TaskManager();
            string filePath = "tasks.xml";
            _taskManager.LoadData(filePath);
            _taskManager.CreateList("Default List");
            _taskManager.AddActiveTasks();
        }

        public void SaveTasks()
        {
            string filePath = "tasks.xml";
            _taskManager.SaveData(filePath);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}