using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    public partial class TaskListView
    {
        public TaskListViewModel TaskListViewModel { get; set; }

        public TaskListView(TaskListViewModel viewModel)
        {
            InitializeComponent();
            TaskListViewModel = viewModel;
            DataContext = TaskListViewModel;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();

            if (addTaskWindow.ShowDialog() == true)
            {
                try
                {
                    Task? newTask = addTaskWindow.NewTask;

                    if (newTask != null)
                    {
                        TaskListViewModel.AddTask(newTask);
                    }
                }
                catch (ValidationException ex)
                {
                    ShowErrorMessage(ex.Message);
                    LogError(ex.Message);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("An unexpected error occurred while adding the task.");
                    LogError(ex.Message);
                }
            }
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListViewModel.SelectedTask == null)
            {
                ShowErrorMessage("Please select a task to remove.");
                return;
            }

            if (ShowConfirmationDialog("Are you sure you want to remove this task?"))
            {
                TaskListViewModel.RemoveTask(TaskListViewModel.SelectedTask);
            }
        }

        private void PriorityFilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PriorityFilterComboBox.SelectedItem is not ComboBoxItem selectedItem) return;
            var selectedPriority =
                (Priority?)Enum.Parse(typeof(Priority),
                selectedItem.Tag.ToString()
                ?? throw new InvalidOperationException());
            TaskListViewModel.FilterTasksByPriority(selectedPriority);
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (StatusFilterComboBox.SelectedItem is not ComboBoxItem selectedItem) return;
            var selectedStatus =
                (Status?)Enum.Parse(typeof(Status),
                    selectedItem.Tag.ToString()
                    ?? throw new InvalidOperationException());
            TaskListViewModel.FilterTasksByStatus(selectedStatus);
        }

        private void TasksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksListView.SelectedItem is Task selectedTask)
            {
                TaskListViewModel.SelectedTask = selectedTask;
                SelectedTaskDetails.Text = $"Selected Task: {selectedTask.Name} (Status: {selectedTask.Status})";

                var taskView = new TaskView(selectedTask);
                taskView.TaskCompleted += (task) =>
                {
                    selectedTask.Status = Status.Completed;
                    TaskListViewModel.FilteredTasksView.Refresh();
                };
                taskView.TaskEdited += (task) =>
                {
                    TaskListViewModel.FilteredTasksView.Refresh();
                };
                taskView.ShowDialog();
            }
            else
            {
                SelectedTaskDetails.Text = "No task selected.";
            }
        }

        private static void ShowErrorMessage(string message) => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        private static bool ShowConfirmationDialog(string message) => MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        private static void LogError(string message) => File.AppendAllText("error_log.txt", $"{DateTime.Now}: {message}\n");

        private void GoBackButton_Click(object sender, RoutedEventArgs e) => Close();

        private void TaskListView_Closed(object? sender, EventArgs e)
        {
            Application.Current.MainWindow?.Show();
        }
    }
}