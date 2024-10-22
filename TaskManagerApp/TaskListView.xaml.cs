using System;
using System.Windows;

namespace TaskManagerApp
{
    public partial class TaskListView : Window
    {
        public TaskList TaskList { get; private set; }

        public TaskListView(TaskList taskList)
        {
            InitializeComponent();
            TaskList = taskList;
            DataContext = this;

            RefreshTaskList();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var newTaskDialog = new TaskInputDialog();
            if (newTaskDialog.ShowDialog() == true)
            {
                try
                {
                    ValidateTask(newTaskDialog.Task);
                    TaskList.AddTask(newTaskDialog.Task);
                    RefreshTaskList();
                }
                catch (ValidationException ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }

        private void RemoveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListView.SelectedItem is Task selectedTask)
            {
                if (ShowConfirmationDialog("Are you sure you want to remove this task?"))
                {
                    TaskList.RemoveTask(selectedTask);
                    RefreshTaskList();
                }
            }
            else
            {
                ShowErrorMessage("Please select a task to remove.");
            }
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksListView.SelectedItem is Task selectedTask)
            {
                var editTaskDialog = new TaskInputDialog();
                if (editTaskDialog.ShowDialog() == true)
                {
                    try
                    {
                        ValidateTask(editTaskDialog.Task);
                        TaskList.UpdateTask(selectedTask, editTaskDialog.Task);
                        RefreshTaskList();
                    }
                    catch (ValidationException ex)
                    {
                        ShowErrorMessage(ex.Message);
                    }
                }
            }
            else
            {
                ShowErrorMessage("Please select a task to edit.");
            }
        }

        private void RefreshTaskList()
        {
            TasksListView.ItemsSource = null;
            var tasks = TaskList.GetTasks();
            TasksListView.ItemsSource = tasks;

            NoTasksMessage.Visibility = tasks.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ValidateTask(Task task)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new ValidationException("Task name cannot be empty.");
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

        private void TasksListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TasksListView.SelectedItem is Task selectedTask)
            {
                SelectedTaskDetails.Text = $"Task: {selectedTask.Name}\n" +
                                           $"Description: {selectedTask.Description}\n" +
                                           $"Due Date: {selectedTask.DueDateTime}\n" +
                                           $"Priority: {selectedTask.Priority}\n" +
                                           $"Status: {selectedTask.Status}";
            }
            else
            {
                SelectedTaskDetails.Text = string.Empty;
            }
        }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}