using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaskManagerApp.TaskList;

namespace TaskManagerApp.HomePage
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public TaskManager TaskManager { get; set; }

        public MainViewModel()
        {
            TaskManager = new TaskManager
            {
                TaskLists = new ObservableCollection<TaskList.TaskList>()
            };

            // Add some sample task lists for testing
            TaskManager.TaskLists.Add(new TaskList.TaskList("Groceries"));
            TaskManager.TaskLists.Add(new TaskList.TaskList("Work Projects"));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddNewList()
        {
            ListInputDialog listInput = new ListInputDialog();
            if (listInput.ShowDialog() == true && !string.IsNullOrWhiteSpace(listInput.ListName))
            {
                TaskManager.TaskLists?.Add(new TaskList.TaskList(listInput.ListName));
                OnPropertyChanged(nameof(TaskManager.TaskLists)); // Notify UI
            }
        }

        public void LoadTaskListsFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<TaskList.TaskList>));
                    using StreamReader reader = new StreamReader(filePath);
                    TaskManager.TaskLists = (ObservableCollection<TaskList.TaskList>)serializer.Deserialize(reader)!;
                    OnPropertyChanged(nameof(TaskManager.TaskLists)); // Notify UI 
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error loading task lists: {ex.Message}", ex);
                }
            }
        }

        // Save Task Lists
        public async void SaveAllTaskLists(TaskManager taskManager, string filePath)
        {
            await Task.Run(() =>
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<TaskList.TaskList>));
                    using StreamWriter writer = new StreamWriter(filePath);
                    serializer.Serialize(writer, taskManager.TaskLists);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error saving task lists: {ex.Message}", ex);
                }
            });
        }
    }
}