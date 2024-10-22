using System;
using System.Windows;

namespace TaskManagerApp
{
    public partial class TaskView : Window
    {
        public Task SelectedTask { get; set; }
        public event Action<Task>? TaskCompleted; // Make it nullable

        public TaskView(Task task)
        {
            InitializeComponent();
            SelectedTask = task;
            DataContext = this;
        }

        private void MarkAsCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                try
                {
                    SelectedTask.MarkAsComplete();
                    TaskCompleted?.Invoke(SelectedTask);
                    MessageBox.Show($"Task '{SelectedTask.Name}' marked as complete.");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error marking task as complete: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                EditDialog editDialog = new EditDialog(SelectedTask.Name, SelectedTask.Description)
                {
                    TaskName = null,
                    TaskDescription = null
                }; // Initialize with existing task details
                editDialog.LoadTaskDetails(SelectedTask); // Load existing task details

                if (editDialog.ShowDialog() == true)
                {
                    try
                    {
                        // Update the task with the edited values
                        SelectedTask.EditTask(editDialog.TaskName, editDialog.TaskDescription, editDialog.DueDate, Priority.High, Status.InProgress);
                        MessageBox.Show($"Task '{SelectedTask.Name}' updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No task selected for editing.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}