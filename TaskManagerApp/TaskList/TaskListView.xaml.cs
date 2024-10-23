using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using TaskManagerApp.TaskList;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp
{
    public partial class TaskListView : Window
    {
        public TaskListViewModel ViewModel { get; set; }

        public TaskListView(TaskList.TaskList taskList)
        {
            InitializeComponent();
            ViewModel = new TaskListViewModel(taskList);
            DataContext = ViewModel;
            TaskListName.Text = taskList.Name; // Bind the list name to the header
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow(); // Open the AddTaskWindow
            if (addTaskWindow.ShowDialog() == true) // Show the dialog
            {
                try
                {
                    var newTask = addTaskWindow.TaskToEdit; // Get the new task from the dialog
                    ViewModel.AddTask(newTask); // Add the new task to the ViewModel
                }
                catch (ValidationException ex)
                {
                    ShowErrorMessage(ex.Message); // Show validation error
                    LogError(ex.Message); // Log the error
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("An unexpected error occurred while adding the task."); // Show generic error
                    LogError(ex.Message); // Log the error
                }
            }
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedTask != null && ShowConfirmationDialog("Are you sure you want to remove this task?"))
            {
                ViewModel.RemoveTask(ViewModel.SelectedTask);
            }
            else
            {
                string message = "Please select a task to remove.";
                ShowErrorMessage(message);
                LogError(message);
            }
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedTask != null)
            {
                var editTaskWindow = new AddTaskWindow(ViewModel.SelectedTask);
                if (editTaskWindow.ShowDialog() == true)
                {
                    // Optionally refresh the view or navigate to the task view
                }
            }
            else
            {
                ShowErrorMessage("No task selected to edit.");
            }
        }

        private void PriorityFilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PriorityFilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedPriority = (Priority)Enum.Parse(typeof(Priority), selectedItem.Tag.ToString());
                ViewModel.FilterTasksByPriority(selectedPriority);
            }
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (StatusFilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedStatus = (Status)Enum.Parse(typeof(Status), selectedItem.Tag.ToString());
                ViewModel.FilterTasksByStatus(selectedStatus);
            }
        }

        private void TasksListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TasksListView.SelectedItem is Task selectedTask)
            {
                ViewModel.SelectedTask = selectedTask;
                SelectedTaskDetails.Text = $"Selected Task: {selectedTask.Name} (Status: {selectedTask.Status})";
            }
            else
            {
                ViewModel.SelectedTask = null;
                SelectedTaskDetails.Text = "No task selected.";
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool ShowConfirmationDialog(string message)
        {
            return MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        private void LogError(string message)
        {
            Console.WriteLine($"ERROR: {message}"); // Placeholder for actual logging
        }
    }
}