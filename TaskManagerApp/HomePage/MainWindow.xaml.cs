using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;
using TaskManagerApp.TaskList;

namespace TaskManagerApp.HomePage
{
    public partial class MainWindow : Window
    {
        public readonly MainViewModel ViewModel;
        public TaskListViewModel? CurrentTaskListViewModel { get; set; }
        private readonly Dictionary<string, TaskListView> OpenTaskListViews = new();

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
            Debug.WriteLine("Tasks: " + ViewModel.ListOfLists.Count);
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            var inputDialog = new ListInputDialog();

            if (inputDialog.ShowDialog() == true)
            {
                var newListName = inputDialog.ListName;

                if (!string.IsNullOrEmpty(newListName))
                {
                    ViewModel.AddTaskList(newListName);
                    ViewModel.UpdateHighPriorityTasks();
                }
            }
            Debug.WriteLine("Task List: " + inputDialog.ListName + ", with the size of " + ViewModel.ListOfLists.Count);
        }

        private void RemoveList_Click(Object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskList.TaskList selectedTaskList)
            {
                ViewModel.ListOfLists.Remove(selectedTaskList);
                ViewModel.ListOfListsView.Refresh();
                ViewModel.UpdateHighPriorityTasks();
                MessageBox.Show($"Remove Task List: {selectedTaskList.Tasks} with {selectedTaskList.SizeOfTheList} task(s).");
            }
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListBox.SelectedItem is not TaskList.TaskList selectedTaskList
                || string.IsNullOrWhiteSpace(selectedTaskList.Name)) return;

            if (TaskListBox.SelectedItem is TaskList.TaskList selected)
            {
                selectedTaskList = selected;

                if (!string.IsNullOrWhiteSpace(selectedTaskList.Name) &&
                    !OpenTaskListViews.TryGetValue(selectedTaskList.Name, out TaskListView? taskListView))
                {
                    CurrentTaskListViewModel = new TaskListViewModel(selectedTaskList);
                    taskListView = new TaskListView(CurrentTaskListViewModel);
                    OpenTaskListViews[selectedTaskList.Name] = taskListView;

                    taskListView.Closed += (s, args) => OpenTaskListViews.Remove(selectedTaskList.Name);
                    taskListView.Show();
                }
            }
        }

        public void LoadAllTaskLists(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(MainViewModel));
                using var reader = new StreamReader(filePath);

                if (serializer.Deserialize(reader) is MainViewModel taskLists
                    && ViewModel.ListOfLists != null)
                {
                    ViewModel.ListOfListsView = CollectionViewSource.GetDefaultView(ViewModel.ListOfLists);
                    ViewModel.UpdateHighPriorityTasks();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading task lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            try
            {
                await ViewModel.SaveAllTaskLists();
                Debug.WriteLine("TaskLists saved on close.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured while saving: {ex.Message}");
            }
        }
    }
}