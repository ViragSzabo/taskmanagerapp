using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TaskManagerApp.HomePage;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    public partial class TaskListView : Window
    {
        public TaskListViewModel ViewModel { get; set; }

        public TaskManagerApp.TaskList.TaskList SelectedTaskList { get; set;  }

        public TaskListView(TaskManagerApp.TaskList.TaskList taskList)
        {
            InitializeComponent();
            ViewModel = new TaskListViewModel(taskList);
            DataContext = ViewModel;
            SelectedTaskList = taskList;
            this.Closed += TaskListView_Closed;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow(); 
            if (addTaskWindow.ShowDialog() == true) 
            {
                try
                {
                    var newTask = addTaskWindow.TaskToEdit; 
                    ViewModel.AddTask(newTask);
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
            if (ViewModel.SelectedTask != null 
                && ShowConfirmationDialog("Are you sure you want to remove this task?"))
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

        private void PriorityFilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PriorityFilterComboBox.SelectedItem is not ComboBoxItem selectedItem) return;
            var selectedPriority = 
                (Priority?)Enum.Parse(typeof(Priority),
                selectedItem.Tag.ToString() 
                ?? throw new InvalidOperationException());
            ViewModel.FilterTasksByPriority(selectedPriority);
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (StatusFilterComboBox.SelectedItem is not ComboBoxItem selectedItem) return;
            var selectedStatus = 
                (Status?)Enum.Parse(typeof(Status),
                    selectedItem.Tag.ToString() 
                    ?? throw new InvalidOperationException());
            ViewModel.FilterTasksByStatus(selectedStatus);
        }

        private void TasksListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TasksListView.SelectedItem is Task selectedTask)
            {
                ViewModel.SelectedTask = selectedTask;
                SelectedTaskDetails.Text = $"Selected Task: {selectedTask.Name} is {selectedTask.Status}";

                var taskView = new TaskView(selectedTask);
                taskView.TaskCompleted += (completedTask) =>
                {
                    ViewModel.RemoveTask(completedTask);
                };
                taskView.ShowDialog(); 
            }
            else
            {
                ViewModel.SelectedTask = null;
                SelectedTaskDetails.Text = "No task has been selected.";
            }
        }

        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static bool ShowConfirmationDialog(string message)
        {
            return MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                   MessageBoxResult.Yes;
        }

        private static void LogError(string message)
        {
            File.AppendAllText("error_log.txt", $"{DateTime.Now}: {message}\n");
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TaskListView_Closed(object? sender, EventArgs e)
        {
            Application.Current.MainWindow?.Show(); // Show the existing MainWindow
        }
    }
}