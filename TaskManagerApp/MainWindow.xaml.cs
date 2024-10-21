using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManagerApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<TaskList> TaskLists { get; set; }
        public ObservableCollection<Task> HighPriorityTasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            TaskLists = new ObservableCollection<TaskList>();
            HighPriorityTasks = new ObservableCollection<Task>();
            DataContext = this; // Set the DataContext for data binding
        }

        // Add Task Button Click Event
        private void AddTaskList_Click(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.ShowDialog() == true)
            {
                // Assume you want to add to the first task list for simplicity
                if (TaskLists.Count > 0)
                {
                    TaskList newList = new TaskList(inputDialog.Name);
                    TaskLists.Add(newList);
                    newList.AddTask(new Task("Default", "Default task", DateTime.Today, Priority.LOW, Status.NotStarted));


                    // Add to high priority Tasks if applicable
                    foreach (Task task in newList.Tasks)
                    {
                        if (task.Priority == Priority.HIGH && task.Status == Status.InProgress)
                        {
                            HighPriorityTasks.Add(task);
                        }
                    }
                }
                OnPropertyChanged(nameof(TaskLists));
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is Task selectedTask)
            {
                // Find the task list that contains this task
                foreach (var taskList in TaskLists)
                {
                    if (taskList.Tasks.Contains(selectedTask))
                    {
                        taskList.RemoveTask(selectedTask); // Remove task from task list
                        break;
                    }
                }
                HighPriorityTasks.Remove(selectedTask);
            }
            else if (TaskListBox.SelectedItem is TaskList selectedList)
            {
                TaskLists.Remove(selectedList); // Remove the entire list
            }

            OnPropertyChanged(nameof(HighPriorityTasks));
            OnPropertyChanged(nameof(TaskLists)); // Notify changes
        }

        // Clear Filter Button Click Event
        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            // LOGIC later
        }

        private void TaskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is Task selectedTask)
            {
                TaskView taskView = new TaskView(selectedTask);
                taskView.TaskCompleted += TaskView_TaskCompleted; // Subscribe to the event
                taskView.ShowDialog(); // Show the task view as a dialog
            }
        }

        private void TaskView_TaskCompleted(Task completedTask)
        {
            // Remove from high priority if necessary
            if (HighPriorityTasks.Contains(completedTask))
            {
                HighPriorityTasks.Remove(completedTask);
            }

            // You can also notify changes here if necessary
            OnPropertyChanged(nameof(HighPriorityTasks));
        }

        // Property changed notification for data binding
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}