using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
namespace TaskManagerApp
{
    public partial class MainWindow : Window
    {
        private readonly int characterLimit = 15;
        private TaskManager taskManager;
        private TaskListViewModel defaultTaskListViewModel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTaskManager();
            SetUpDefaultTask();
            SetInitialButtonVisibility();
        }

        private void SetInitialButtonVisibility()
        {
            addButton.Visibility = Visibility.Visible;
            removeButton.Visibility = Visibility.Visible;
        }

        private void SetUpDefaultTask()
        {
            defaultTaskListViewModel = new TaskListViewModel(new TaskList());
            taskListBox.ItemsSource = defaultTaskListViewModel.Tasks;
            defaultTaskListViewModel.AddTask(new Task("default"));
        }

        private void InitializeTaskManager()
        {
            taskManager = new TaskManager();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newName = taskInput.Text.Trim();

            if (!string.IsNullOrEmpty(newName) && newName.Length <= characterLimit)
            {
                defaultTaskListViewModel.AddTask(new Task(newName));
                MessageBox.Show("Task added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid task name! Please enter a name up to 15 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            taskInput.Clear();
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem != null)
            {
                defaultTaskListViewModel.RemoveSelectedTask();
                SetInitialButtonVisibility();
                MessageBox.Show("Task removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a task to remove.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            defaultTaskListViewModel.ClearFilters();
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem != null)
            {
                string selectedStatus = ((ComboBoxItem)box.SelectedItem).Content.ToString();
                defaultTaskListViewModel.FilterTasksByStatus(selectedStatus);
            }
        }

        private void Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem != null)
            {
                string selectedPriority = ((ComboBoxItem)box.SelectedItem).Content.ToString();
                defaultTaskListViewModel.FilterTasksByPriority(selectedPriority);
            }
        }
    }

    public class TaskListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private TaskList taskList;
        private TaskViewModel selectedTask;

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        public TaskViewModel SelectedTask
        {
            get => selectedTask;
            set
            {
                selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public TaskListViewModel(TaskList taskList)
        {
            this.taskList = taskList;
            Tasks = new ObservableCollection<TaskViewModel>();
            foreach (var task in taskList.Tasks)
            {
                Tasks.Add(new TaskViewModel(task));
            }
        }

        public void AddTask(Task task)
        {
            taskList.AddTask(task);
            Tasks.Insert(0, new TaskViewModel(task));
        }

        public void EditTask(string name, DateTime? date, Priority priority, Status status)
        {
            if (SelectedTask != null)
            {
                //SelectedTask.SaveChanges(name, date, priority, status);
                SelectedTask.Name = name;
                SelectedTask.DueDate = date;
                SelectedTask.Priority = priority;
                SelectedTask.Status = status;
            }
        }

        public void RemoveSelectedTask()
        {
            if (SelectedTask != null)
            {
                taskList.RemoveTask(SelectedTask.Task);
                Tasks.Remove(SelectedTask);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Add this method to refresh the task list
        private void RefreshTasks()
        {
            // FilteredTasks is a placeholder for your filtered tasks collection
            ObservableCollection<TaskViewModel> filteredTasks = new ObservableCollection<TaskViewModel>();

            foreach (var task in Tasks)
            {
                if (task.IsVisible)
                {
                    filteredTasks.Add(task);
                }
            }

            Tasks = filteredTasks;
            OnPropertyChanged(nameof(Tasks));
        }

        public void ClearFilters()
        {
            foreach (var task in Tasks)
            {
                task.IsVisible = true;
            }
        }

        public void FilterTasksByStatus(string status)
        {
            foreach (var task in Tasks)
            {
                if (task.Status == Status.Completed)
                {
                    task.IsVisible = task.Status == Status.Completed;
                }
                else if (task.Status == Status.InProgress)
                {
                    task.IsVisible = task.Status == Status.InProgress;
                }
                else
                {
                    task.IsVisible = task.Status == Status.NotStarted;
                }
            }
            RefreshTasks();
        }

        public void FilterTasksByPriority(string priority)
        {
            foreach (var task in Tasks)
            {
                if (task.Priority == Priority.High)
                {
                    task.IsVisible = task.Priority == Priority.High;
                }
                else if (task.Priority == Priority.Medium)
                {
                    task.IsVisible = task.Priority == Priority.Medium;
                }
                else
                {
                    task.IsVisible = task.Priority == Priority.Low;
                }
            }
            RefreshTasks();
        }

        public void FilterByDate(DateTime date)
        {
            foreach (var task in Tasks)
            {
                if (task.DueDate == date)
                {
                    task.IsVisible = task.DueDate == date;
                }
            }
            RefreshTasks();
        }
    }

    public class TaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Task task;
        private TaskList TaskList;

        public Task Task
        {
            get => task;
            set
            {
                task = value;
                OnPropertyChanged(nameof(Task));
            }
        }

        public DateTime? DueDate
        {
            get => Task.DueDate;
            set
            {
                Task.DueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }

        public Priority Priority
        {
            get => Task.Priority;
            set
            {
                Task.Priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public Status Status
        {
            get => Task.Status;
            set
            {
                Task.Status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string Name
        {
            get => Task.Name;
            set
            {
                Task.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public void EditTask(string name, DateTime? date, Priority priority, Status status)
        {
            if (task != null)
            {
                task.Name = name;
                task.DueDate = date;
                task.Priority = priority;
                task.Status = status;

                // Update the task in the TaskList
                TaskList.UpdateTask(task);
            }
        }

        public bool IsVisible { get; set; } = true;

        public TaskViewModel(Task task)
        {
            this.task = task;
        }

        public void SaveChanges(string name, DateTime? date, Priority priority, Status status)
        {
            Name = name;
            DueDate = date;
            Priority = priority;
            Status = status;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Priority
    {
        High,
        Medium,
        Low
    }

    public enum Status
    {
        Completed,
        InProgress,
        NotStarted
    }

    public class TaskList
    {
        public List<Task> Tasks { get; set; }

        public TaskList()
        {
            Tasks = new List<Task>();
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            Tasks.Remove(task);
        }
        public void UpdateTask(Task task)
        {
            // Find the task in the list and update it
            Task existingTask = Tasks.Find(t => t.Name == task.Name);
            if (existingTask != null)
            {
                existingTask.DueDate = task.DueDate;
                existingTask.Priority = task.Priority;
                existingTask.Status = task.Status;
            }
        }
    }

    public class Task
    {
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        public Task(string name)
        {
            Name = name;
            DueDate = DateTime.Today;
            Priority = Priority.Medium;
            Status = Status.NotStarted;
        }
    }

    public class TaskManager
    {
        public TaskList TaskList { get; set; }

        public TaskManager()
        {
            this.TaskList = new TaskList();
        }
    }
}