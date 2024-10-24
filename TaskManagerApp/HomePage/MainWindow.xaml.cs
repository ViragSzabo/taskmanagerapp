using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManagerApp.TaskList;

namespace TaskManagerApp.HomePage
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private const string FilePath = "application.xml";
        private TaskList.TaskList? selectedTaskList;
        private CancelEventHandler MainWindow_Closing;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            _viewModel.LoadTaskListsFromFile(FilePath);
            selectedTaskList = TaskManager.LoadTasks();
            this.Closing += MainWindow_Closing;
        }

        // Add Task List Button Click Event
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddNewList();
        }

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
                    // Make sure to call SaveAllTaskLists asynchronously
                    _viewModel.SaveAllTaskLists(_viewModel.TaskManager, FilePath);
                    await Task.Run(() => _viewModel.SaveAllTaskLists(_viewModel.TaskManager, saveFileDialog.FileName));
                    MessageBox.Show("All task lists saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while downloading all task lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Handle selection changed event for TaskListBox
        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskList.TaskList selected)
            {
                selectedTaskList = selected; // Store the selected task list
                var taskListView = new TaskListView(selected);
                taskListView.Show();
                this.Hide();
            }
            else
            {
                selectedTaskList = null; // Reset if nothing is selected
            }
        }

        protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            // Call SaveAllTaskLists asynchronously to avoid blocking the UI thread
            await Task.Run(() => _viewModel.SaveAllTaskLists(_viewModel.TaskManager, FilePath));
            TaskManager.SaveTasks(selectedTaskList);
        }
    }
}