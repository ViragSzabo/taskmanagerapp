using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManagerApp.TaskList;

namespace TaskManagerApp.HomePage
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private TaskList.TaskList? _selectedTaskList;
        private readonly CancelEventHandler _mainWindowClosing;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            if (_selectedTaskList != null)
            {
                _viewModel?.AddTaskList(_selectedTaskList);
            }
            this.Closing += _mainWindowClosing;
        }

        // Add Task List Button Click Event
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            ListInputDialog dialog = new ListInputDialog(); // Create the dialog instance
            if (dialog.ShowDialog() != true) return; // Show the dialog and check if OK was clicked
            string listName = dialog.ListName; // Get the list name from the dialog
            _viewModel.AddTaskList(listName); // Call the AddTaskList method from the ViewModel
        }

        /*
        // Download Button Click Event for All Task Lists
        private async void DownloadAllTaskLists_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                Title = "Save All Task Lists"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    await Task.Run(() => _viewModel.SaveAllTaskLists());
                    MessageBox.Show("All task lists saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while downloading all task lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        */

        // Handle selection changed event for TaskListBox
        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskList.TaskList selected)
            {
                _selectedTaskList = selected;
                var taskListView = new TaskListView(selected);
                taskListView.Show();
                this.Hide();
            }
            else
            {
                _selectedTaskList = null; 
            }
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            //await _viewModel.SaveAllTaskLists(); // Save async when closing
            base.OnClosing(e);
        }
    }
}