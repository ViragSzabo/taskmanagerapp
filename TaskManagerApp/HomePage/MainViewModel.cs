using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace TaskManagerApp.HomePage
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Observable collection for task lists
        public TaskManager TaskManager { get; set; } = new();

        // Expose TaskLists directly from TaskManager
        public ObservableCollection<TaskList.TaskList>? DesignTaskLists => TaskManager.TaskLists;

        // Property for the selected task list
        private TaskList.TaskList? _selectedTaskList;
        public TaskList.TaskList? SelectedTaskList
        {
            get => _selectedTaskList;
            set
            {
                if (_selectedTaskList != value)
                {
                    _selectedTaskList = value;
                    OnPropertyChanged(nameof(SelectedTaskList));
                    // Notify that the selected task list has changed
                    OnSelectedTaskListChanged();
                }
            }
        }

        // Method to add a new task list
        public void AddNewTaskList(string listName)
        {
            TaskManager.AddTaskList(new TaskList.TaskList(listName));
            OnPropertyChanged(nameof(DesignTaskLists)); // Notify UI of changes
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSelectedTaskListChanged()
        {
            // Handle logic when the selected task list changes
            // This can trigger a UI update in the view.
        }

        public void SaveTaskList(TaskList.TaskList list, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TaskList.TaskList));
                using StreamWriter writer = new StreamWriter(filePath);
                serializer.Serialize(writer, list);
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