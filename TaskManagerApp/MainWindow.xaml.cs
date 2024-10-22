using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
            TaskLists = new ObservableCollection<TaskList>(); // Initialize task lists
            HighPriorityTasks = new ObservableCollection<Task>(); // Initialize high-priority tasks
            DataContext = this; // Set the DataContext for data binding
        }

        // Add Task List Button Click Event
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            TaskInputDialog inputDialog = new TaskInputDialog(); // Open dialog to get task list name
            if (inputDialog.ShowDialog() == true)
            {
                TaskList newList = new TaskList(inputDialog.Task.Name); // Use the correct property for the name
                TaskLists.Add(newList); // Add new list to the collection
                UpdateHighPriorityTasks(); // Update high-priority tasks
                OnPropertyChanged(nameof(TaskLists)); // Notify the UI of changes
            }
        }

        // Remove Task or Task List Button Click Event
        private void RemoveList_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskList selectedList)
            {
                TaskLists.Remove(selectedList); // Remove the entire task list
                UpdateHighPriorityTasks(); // Update high-priority tasks
                OnPropertyChanged(nameof(TaskLists)); // Notify the UI of changes
            }
        }

        // Task List Selection Changed Event
        private void TaskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskList selectedList)
            {
                TaskListView taskListView = new TaskListView(selectedList); // Open task list view dialog
                taskListView.ShowDialog(); // Show the task list view
            }
        }

        // Property changed notification for data binding
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Method to retrieve high-priority tasks and update the collection
        private void UpdateHighPriorityTasks()
        {
            HighPriorityTasks.Clear();
            var highPriorityTasks = TaskLists.SelectMany(t => t.Tasks)
                .Where(task => task.Priority == Priority.High);
            foreach (var task in highPriorityTasks)
            {
                HighPriorityTasks.Add(task);
            }
        }
    }
}