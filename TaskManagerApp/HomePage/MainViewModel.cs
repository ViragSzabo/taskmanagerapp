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
        // Observable collection for task lists
        public TaskManager TaskManager { get; set; }

        // Expose TaskLists directly from TaskManager
        public ObservableCollection<TaskList.TaskList>? DesignTaskLists => TaskManager.TaskLists;

        public MainViewModel()
        {
            TaskManager = new TaskManager();
            // Add some sample task lists for testing
            TaskManager.TaskLists?.Add(new TaskList.TaskList("Groceries"));
            TaskManager.TaskLists?.Add(new TaskList.TaskList("Work Projects"));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Method to add a new task list
        public void AddNewTaskList(string listName)
        {
            if (string.IsNullOrWhiteSpace(listName))
                throw new ArgumentException("Task list name cannot be null or empty.", nameof(listName));

            TaskManager.TaskLists.Add(new TaskList.TaskList(listName));
            OnPropertyChanged(nameof(TaskManager.TaskLists)); // Notify UI of changes
        }

        public void AddNewList()
        {
            ListInputDialog listInput = new ListInputDialog();
            if (listInput.ShowDialog() == true && !string.IsNullOrWhiteSpace(listInput.ListName))
            {
                AddNewTaskList(listInput.ListName);
            }
        }

        // Method to save a task list to a specified file path asynchronously
        public async Task SaveTaskListAsync(TaskList.TaskList list, string filePath)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list), "Task list cannot be null.");
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TaskList.TaskList));
                using StreamWriter writer = new StreamWriter(filePath);
                await Task.Run(() => serializer.Serialize(writer, list));
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Serialization error: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving task list: {ex.Message}", ex);
            }
        }
    }
}