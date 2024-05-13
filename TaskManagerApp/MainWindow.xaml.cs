using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

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
            taskManager = new TaskManager();
            defaultTaskListViewModel = new TaskListViewModel(taskManager.TaskList);
            defaultTaskListViewModel.AddTask(new TaskItem("default"));
            taskListBox.ItemsSource = defaultTaskListViewModel.Tasks;
        }

        private void HighlightButton(Button button)
        {
            // Change the background color temporarily
            button.Background = Brushes.LightGreen;

            // Reset the background color after a short delay
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5); // Adjust the duration as needed
            timer.Tick += (sender, e) =>
            {
                button.Background = Brushes.Transparent;
                timer.Stop();
            };
            timer.Start();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newName = taskInput.Text.Trim();

            if (!string.IsNullOrEmpty(newName) && newName.Length <= characterLimit)
            {
                defaultTaskListViewModel.AddTask(new TaskItem(newName));
                MessageBox.Show("Task added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid task name! Please enter a name up to 15 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            taskInput.Clear();
            HighlightButton(addButton);
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
            HighlightButton(removeButton);
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            defaultTaskListViewModel.ClearFilters();
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedItem != null && defaultTaskListViewModel != null)
            {
                string selectedStatus = ((ComboBoxItem)box.SelectedItem).Content.ToString();
                defaultTaskListViewModel.FilterTasksByStatus(selectedStatus);
            } else
            {
                MessageBox.Show("Please, run it again!", "Error", MessageBoxButton.OK, MessageBoxImage.None);
            }
        }

        private void Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (defaultTaskListViewModel != null && box.SelectedItem != null)
            {
                string selectedPriority = ((ComboBoxItem)box.SelectedItem).Content.ToString();
                defaultTaskListViewModel.FilterTasksByPriority(selectedPriority);
            }
            else
            {
                MessageBox.Show("Please, run it again!", "Error", MessageBoxButton.OK, MessageBoxImage.None);
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

            foreach (var task in taskList.Tasks) // Iterate over taskList.Tasks instead of Tasks
            {
                Tasks.Add(new TaskViewModel(task, this));
            }
        }

        public void AddTask(TaskItem task)
        {
            taskList.AddTask(task);
            Tasks.Insert(0, new TaskViewModel(task, this));
        }

        public void EditTask(string name, DateTime? date, Priority priority, Status status)
        {
            if (SelectedTask != null)
            {
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
                SelectedTask.RemoveTask(SelectedTask.Task);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        }
    }

    public class TaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private TaskItem task;
        private TaskListViewModel taskListViewModel;

        public TaskItem Task
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

        public bool IsVisible { get; set; } = true;

        public TaskViewModel(TaskItem task, TaskListViewModel taskListViewModel)
        {
            this.task = task;
            this.taskListViewModel = taskListViewModel;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RemoveTask(TaskItem task)
        {
            if (task != null && taskListViewModel != null)
            {
                taskListViewModel.RemoveSelectedTask();
            }
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
        public List<TaskItem> Tasks { get; set; }

        public TaskList()
        {
            Tasks = new List<TaskItem>();
        }

        public void AddTask(TaskItem task)
        {
            Tasks.Add(task);
        }

        public void RemoveTask(TaskItem task)
        {
            Tasks.Remove(task);
        }
    }

    public class TaskItem
    {
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        public TaskItem(string name)
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
