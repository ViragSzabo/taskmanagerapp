using Microsoft.Win32;
using System;
using System.Windows;

namespace TaskManagerApp.HomePage
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel; // Set DataContext for data binding
        }

        // Add Task List Button Click Event
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            ListInputDialog listInput = new ListInputDialog();
            if (listInput.ShowDialog() == true && !string.IsNullOrWhiteSpace(listInput.ListName))
            {
                _viewModel.AddNewTaskList(listInput.ListName); // Add task list via ViewModel
            }
            else
            {
                MessageBox.Show(
                    "Invalid task list name. Please enter a valid name.",
                    "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Download Button Click Event
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the clicked button
            var button = sender as System.Windows.Controls.Button;

            // Get the TaskList object associated with the button
            var taskList = button.CommandParameter as TaskList.TaskList;

            // Open a SaveFileDialog to select where to save the file
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Save Task List"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Save the selected task list to the specified path
                    _viewModel.SaveTaskList(taskList, saveFileDialog.FileName);
                    MessageBox.Show(
                        "Task list saved successfully!",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while downloading the task list: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void TaskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskList.TaskList selectedTaskList)
            {
                // Set the selected task list in the view model
                var viewModel = (MainViewModel)this.DataContext; // Assuming DataContext is set to MainViewModel
                viewModel.SelectedTaskList = selectedTaskList;

                // Create an instance of the TaskListView
                var taskListView = new TaskListView();
                taskListView.DataContext = selectedTaskList; // Ensure TaskListView is set up to use this DataContext

                // Replace the main content with the task list view
                MainContent.Content = taskListView; // Assuming you have a ContentControl named MainContent
            }
        }

    }
}