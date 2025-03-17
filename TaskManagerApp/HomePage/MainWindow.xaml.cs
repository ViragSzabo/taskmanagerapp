using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;
using TaskManagerApp.TaskList;

namespace TaskManagerApp.HomePage
{
    public partial class MainWindow : Window
    {
        public readonly MainViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel(); // Properly instantiate the ViewModel
            DataContext = ViewModel; // Set it as the DataContext
            //this.Closing += MainWindow_Closing;
            Debug.WriteLine("Tasks: " + ViewModel.ListOfLists.Count);
        }

        // Add Task List Button Click Event
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel == null)
            {
                Debug.WriteLine("ViewModel is null!"); // Debug check
                return;
            }

            ListInputDialog inputDialog = new ListInputDialog();

            if (inputDialog.ShowDialog() == true)
            {
                string newListName = inputDialog.ListName;

                if (!string.IsNullOrEmpty(newListName))
                {
                    ViewModel.AddTaskList(newListName);
                }
            }
            Debug.WriteLine("Task List: " + inputDialog.ListName + ", with the size of " + ViewModel.ListOfLists.Count);
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
                    await ViewModel.SaveAllTaskLists(saveFileDialog.FileName); // Save the task lists
                    MessageBox.Show("All task lists saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while downloading all task lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList.TaskList selectedTaskList;
            Dictionary<string, TaskListView> OpenTaskListViews = new();
            TaskListViewModel? CurrentTaskListViewModel;

            if (TaskListBox.SelectedItem is TaskList.TaskList selected)
            {
                selectedTaskList = selected;

                // Check if the window for the selected task list is already open
                if (!OpenTaskListViews.ContainsKey(selected.Name!))
                {
                    // Create a new TaskListViewModel, passing TaskList and null for the TaskView
                    CurrentTaskListViewModel = new TaskListViewModel(selected, null); // This is correct! Ensure `selected` is TaskList

                    // Create a new window to display the task list and its tasks
                    var taskListView = new TaskListView(CurrentTaskListViewModel);
                    OpenTaskListViews[selected.Name!] = taskListView;

                    // When the window is closed, remove it from the dictionary
                    taskListView.Closed += (s, args) => OpenTaskListViews.Remove(selected.Name!);

                    // Show the new window
                    taskListView.Show();
                }
                else
                {
                    // If the window is already open, bring it to the foreground
                    OpenTaskListViews[selected.Name!].Activate();
                }
            }
            else
            {
                selectedTaskList = new TaskList.TaskList("Unknown name");
            }
        }

        // Loading all task lists from XML file
        public void LoadAllTaskLists(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(TaskListViewModel)); // List of TaskList
                using (var reader = new StreamReader(filePath))
                {
                    var taskLists = (TaskListViewModel)serializer.Deserialize(reader); // Deserialize the XML file into a list of TaskList objects
                    ViewModel.ListOfListsView = (ICollectionView)taskLists; // Assign to the ViewModel (or appropriate collection)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading task lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            await ViewModel.SaveAllTaskLists("TaskLists.xml");
        }
    }
}