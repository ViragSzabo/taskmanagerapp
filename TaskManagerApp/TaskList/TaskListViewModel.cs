using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Timers;
using System.Windows.Data;
using System.Xml.Serialization;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    public class TaskListViewModel
    {
        public ICollectionView FilteredTasksView { get; }
        public ObservableCollection<Task> Tasks { get; set; }

        private Task? _selectedTask;
        public Task? SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private Priority? _selectedPriority = Priority.All;
        private Status? _selectedStatus = Status.All;

        public TaskList TaskList { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private System.Timers.Timer autoSaveTimer;

        public TaskListViewModel(TaskList list)
        {
            TaskList = list ?? throw new ArgumentNullException(nameof(list));
            Tasks = new ObservableCollection<Task>(list.Tasks);
            FilteredTasksView = CollectionViewSource.GetDefaultView(Tasks);
            FilteredTasksView.Filter = FilterTasks;

            autoSaveTimer = new System.Timers.Timer(60000);
            autoSaveTimer.Elapsed += AutoSaveTasks;
            autoSaveTimer.AutoReset = true;
            autoSaveTimer.Start();
        }

        public void AddTask(Task task)
        {
            if (task == null) return;
            Tasks.Add(task);
            TaskList.AddTask(task);
            FilteredTasksView.Refresh();
            SaveTasks();
        }

        public void RemoveTask(Task task)
        {
            if (task == null) return;
            Tasks.Remove(task);
            TaskList.RemoveTask(task);
            FilteredTasksView?.Refresh(); // 🔥 Force a refresh
            SaveTasks();
        }

        public void SaveTasks()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(TaskList));
                using var stream = new FileStream("TaskLists.txt", FileMode.Create);
                serializer.Serialize(stream, this.TaskList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving TaskList: {ex.Message}");
            }
        }

        public void LoadTasks(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(TaskList));
                using var stream = new FileStream(filePath, FileMode.Open);
                TaskList? loadedList = (TaskList?)serializer.Deserialize(stream);

                if (loadedList != null)
                {
                    this.TaskList = loadedList;
                    this.Tasks.Clear();
                    foreach (var task in this.TaskList.Tasks)
                    {
                        this.Tasks.Add(task);
                    }
                    this.FilteredTasksView.Refresh();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskList: {ex.Message}");
            }
        }

        public void FilterTasksByPriority(Priority? selectedPriority)
        {
            _selectedPriority = selectedPriority;
            FilteredTasksView.Refresh(); // 🔥 Force a refresh
        }

        public void FilterTasksByStatus(Status? selectedStatus)
        {
            _selectedStatus = selectedStatus;
            FilteredTasksView.Refresh(); // 🔥 Force a refresh
        }

        private bool FilterTasks(object obj)
        {
            if (obj is not Task task) return false;
            return (_selectedPriority == Priority.All || task.Priority == _selectedPriority)
                && (_selectedStatus == Status.All || task.Status == _selectedStatus);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void AutoSaveTasks(object sender, ElapsedEventArgs e)
        {
            SaveTasks();
        }

        public void StopAutoSave()
        {
            autoSaveTimer.Stop();
        }
    }
}