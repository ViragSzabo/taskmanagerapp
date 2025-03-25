using System;
using System.Windows;

namespace TaskManagerApp.TasksBenefits
{
    public partial class TaskView : Window
    {
        public Task SelectedTask { get; set; }
        public event Action<Task>? TaskCompleted;
        public event Action<Task>? TaskEdited;

        public TaskView(Task task)
        {
            InitializeComponent();
            SelectedTask = task;
            DataContext = this;
        }

        public void MarkAsCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask.Status != Status.Completed)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to mark '{SelectedTask.Name}' as complete?",
                    "Confirm Completion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        SelectedTask.MarkAsComplete();
                        TaskCompleted?.Invoke(SelectedTask);
                        MessageBox.Show($"Task '{SelectedTask.Name}' marked as complete.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error marking task as complete: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show($"Task '{SelectedTask.Name}' is already marked as complete.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedTask.Name))
            {
                MessageBox.Show(SelectedTask.Name);
                return;
            }

            var editDialog = new EditDialog
            {
                Task = SelectedTask
            };

            editDialog.LoadTaskDetails(SelectedTask);

            if (editDialog.ShowDialog() == true)
            {
                try
                {
                    SelectedTask.EditTask(
                        editDialog.Task.Name,
                        editDialog.Task.Description,
                        editDialog.Task.DueDateTime,
                        editDialog.Task.Priority,
                        editDialog.Task.Status);

                    MessageBox.Show($"Task '{SelectedTask.Name}' updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    TaskEdited?.Invoke(SelectedTask);
                    DataContext = null; // Reset the DataContext to update UI bindings
                    DataContext = this; // Rebind the updated task
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Just close without saving
        }
    }
}