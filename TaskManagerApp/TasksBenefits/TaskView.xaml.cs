using System;
using System.Windows;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp
{
    public partial class TaskView : Window
    {
        public Task SelectedTask { get; set; }
        public event Action<Task>? TaskCompleted;

        public TaskView(Task task)
        {
            InitializeComponent();
            SelectedTask = task;
            DataContext = this;
        }

        private void MarkAsCompleteButton_Click(object sender, RoutedEventArgs e)
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TaskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Get the selected task from the ListBox
            if (TaskListBox.SelectedItem is Task selectedTask)
            {
                SelectedTask = selectedTask;  // Update the SelectedTask property
                DataContext = null;           // Reset the DataContext to update UI bindings
                DataContext = this;           // Rebind the updated task
            }
        }

        private async void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            EditDialog editDialog = new EditDialog
            {
                Task = SelectedTask
            };

            editDialog.LoadTaskDetails(SelectedTask);

            if (editDialog.ShowDialog() == true)
            {
                try
                {
                    await SelectedTask.EditTask(
                        editDialog.Task.Name,
                        editDialog.Task.Description,
                        editDialog.Task.DueDateTime,
                        editDialog.Task.Priority,
                        editDialog.Task.Status);

                    MessageBox.Show($"Task '{SelectedTask.Name}' updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    DataContext = null; // Reset the DataContext to update UI bindings
                    DataContext = this; // Rebind the updated task
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Just close without saving
        }
    }
}