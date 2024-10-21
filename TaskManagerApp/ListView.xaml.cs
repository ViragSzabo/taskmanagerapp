using System.Windows;

namespace TaskManagerApp
{
    public partial class ListView : Window
    {
        public TaskList TaskList { get; set; }

        public ListView(TaskList taskList)
        {
            InitializeComponent();
            TaskList = taskList;
            DataContext = this; // Set the DataContext to the current instance

            // Populate the ListView
            TasksListView.ItemsSource = TaskList.GetTasks(); // Assuming GetTasks returns ObservableCollection<Task>
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to add a new task
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to remove the selected task
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to edit the selected task
        }

        private void TasksListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TasksListView.SelectedItem is Task selectedTask)
            {
                SelectedTaskDetails.Text = $"Selected Task: {selectedTask.Name} (Due: {selectedTask.DueDateTime})";

                // Open the TaskView for the selected task
                TaskView taskView = new TaskView(selectedTask);
                taskView.ShowDialog(); // Show TaskView as a dialog
            }
        }
    }
}