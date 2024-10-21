using System;
using System.Windows;

namespace TaskManagerApp
{
    public partial class TaskView : Window
    {
        public Task SelectedTask { get; set; }
        public event Action<Task> TaskCompleted;

        public TaskView(Task task)
        {
            InitializeComponent();
            SelectedTask = task;
            DataContext = this; // Set the DataContext to the current instance
        }

        private void MarkAsCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                SelectedTask.Status = Status.Completed; // Update status
                TaskCompleted?.Invoke(SelectedTask); // Raise the event
                MessageBox.Show($"Task '{SelectedTask.Name}' marked as complete.");
                Close(); // Close the TaskView after marking as complete
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the TaskView
        }

        private void taskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is Task selectedTask)
            {
                TaskView taskView = new TaskView(selectedTask);
                taskView.ShowDialog(); // Show the task view as a dialog
            }
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                EditDialog editDialog = new EditDialog(SelectedTask);
                if (editDialog.ShowDialog() == true)
                {
                    // Update the task with the edited values
                    SelectedTask.EditTask(editDialog.TaskName, editDialog.TaskDescription, editDialog.DueDate, editDialog.TaskPriority);
                    MessageBox.Show($"Task '{SelectedTask.Name}' updated successfully.");
                }
            }
        }
    }
}