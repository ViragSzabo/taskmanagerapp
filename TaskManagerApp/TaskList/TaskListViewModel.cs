using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Xml.Serialization;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    public class TaskListViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<Task> _allTasks;
        public ICollectionView FilteredTasksView { get; }

        private Task _selectedTask;
        public Task SelectedTask 
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public TaskListViewModel(TaskList taskList)
        {
            _allTasks = new ObservableCollection<Task>(taskList.Tasks);
            FilteredTasksView = CollectionViewSource.GetDefaultView(_allTasks);
        }

        public void AddTask(Task task)
        {
            _allTasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            _allTasks.Remove(task);
        }

        public void Clear()
        {
            _allTasks.Clear();
        }

        public bool Contains(Task task)
        {
            return _allTasks.Contains(task);
        }

        public void FilterTasksByPriority(Priority? selectedPriority)
        {
            FilteredTasksView.Filter = task =>
            {
                var currentTask = task as Task;
                if (selectedPriority is null or Priority.All)
                {
                    return true;
                }
                return currentTask.Priority == selectedPriority;
            };
            FilteredTasksView.Refresh();
        }

        public void FilterTasksByStatus(Status? selectedStatus)
        {
            FilteredTasksView.Filter = task =>
            {
                var currentTask = task as Task;
                if (selectedStatus is null or Status.All)
                {
                    return true;
                }
                return currentTask.Status == selectedStatus;
            };
            FilteredTasksView.Refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}