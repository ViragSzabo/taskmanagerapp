using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml.Serialization;

namespace TaskManagerApp.HomePage
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TaskList.TaskList> ListOfLists { get; set; }
        public ICollectionView ListOfListsView { get; }
        private const string FilePath = "TaskLists.xml";

        public MainViewModel()
        {
            // Initialize ListOfLists to avoid null reference
            ListOfLists = new ObservableCollection<TaskList.TaskList>
            {
                new TaskList.TaskList("Default Task List 1"),
                new TaskList.TaskList("Default Task List 2")
            };

            // Load task lists from file at startup
            //LoadTaskListsFromFile(FilePath).ConfigureAwait(false); // Load asynchronously

            if (!ListOfLists.Any())
            {
                InitializeDefaultTaskLists();
            }

            ListOfListsView = CollectionViewSource.GetDefaultView(ListOfLists);
            Debug.WriteLine("Task Manager's list count: " + ListOfLists.Count);
            Debug.WriteLine("First element: " + ListOfLists[0]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddTaskList(string listName)
        {
                ListOfLists.Add(new TaskList.TaskList(listName));
                OnPropertyChanged(nameof(ListOfLists)); // Notify the UI about the update
                ListOfListsView.Refresh(); // Force refresh
        }


        private void InitializeDefaultTaskLists()
        {
            if (!ListOfLists.Any()) // Only add if there are no lists
            {
                // Create default task lists
                var personalList = new TaskList.TaskList("Personal");
                personalList.Tasks.Add(new TasksBenefits.Task("Grocery Shopping", "Buy groceries for the week.", new DateTime(2024, 10, 30)));
                personalList.Tasks.Add(new TasksBenefits.Task("Exercise", "Go for a run in the evening.", new DateTime(2024, 10, 30)));

                var workList = new TaskList.TaskList("Work");
                workList.Tasks.Add(new TasksBenefits.Task("Project Meeting", "Prepare for the project meeting on Friday.", new DateTime(2024, 10, 30)));
                workList.Tasks.Add(new TasksBenefits.Task("Submit Report", "Finalize and submit the monthly report.", new DateTime(2024, 10, 30)));

                var studyList = new TaskList.TaskList("Study");
                studyList.Tasks.Add(new TasksBenefits.Task("Finish Assignment", "Complete the programming assignment by Wednesday.", new DateTime(2024, 10, 30)));
                studyList.Tasks.Add
                (new TasksBenefits.Task("Review Lecture Notes",
                    "Go through notes from the last lecture.",
                    new DateTime(2024, 10, 30)));

                // Add lists to TaskManager
                ListOfLists.Add(personalList);
                ListOfLists.Add(workList);
                ListOfLists.Add(studyList);
            }
        }

        public void RemoveTaskList(TaskList.TaskList taskList)
        {
            if (ListOfLists.Contains(taskList))
            {
                ListOfLists.Remove(taskList);
            }
        }

        public void AddTaskList(TaskList.TaskList taskList)
        {
            ListOfLists.Add(taskList); // Add the new task list to the collection
            Debug.WriteLine("Add to Task Manager: " + taskList);
        }

        public List<TaskList.TaskList> GetSerializableTaskLists()
        {
            return new List<TaskList.TaskList>(ListOfLists); // Return a list for serialization
        }

        public async Task SaveAllTaskLists()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TaskList.TaskList>));
                using StreamWriter writer = new StreamWriter(FilePath);
                var taskLists = GetSerializableTaskLists();
                await Task.Run(() => serializer.Serialize(writer, taskLists));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving task lists: {ex.Message}");
                MessageBox.Show("Failed to save task lists. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task LoadTaskListsFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<TaskList.TaskList>));
                    using StreamReader reader = new StreamReader(filePath);
                    var taskLists = (List<TaskList.TaskList>)serializer.Deserialize(reader)!;

                    if (taskLists.Any())
                    {
                        ListOfLists.Clear(); // Clear existing lists
                        LoadFromSerializableTaskLists(taskLists); // Load data into ListOfLists
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading task lists: {ex.Message}");
                MessageBox.Show("Failed to load task lists. Please check the file and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFromSerializableTaskLists(List<TaskList.TaskList> taskLists)
        {
            // Clear existing items in ListOfLists
            //ListOfLists.Clear();
            foreach (var taskList in taskLists)
            {
                // Add each loaded task list to the ObservableCollection
                ListOfLists.Add(taskList); 
            }
        }
    }
}