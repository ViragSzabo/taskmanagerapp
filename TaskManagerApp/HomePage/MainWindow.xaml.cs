using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using TaskManagerApp.TaskList;

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
            _viewModel.AddNewList();
        }

        // Download Button Click Event
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var taskList = button?.CommandParameter as TaskList.TaskList;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Save Task List"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    _viewModel.SaveTaskListAsync(taskList, saveFileDialog.FileName);
                    MessageBox.Show(
                        "Task list saved successfully!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"An error occurred while downloading the task list: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        // Handle selection changed event for TaskListBox
        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskList.TaskList selectedTaskList)
            {
                var taskListView = new TaskListView(selectedTaskList);
                taskListView.Show();
                this.Hide();
            }
        }
    }
}