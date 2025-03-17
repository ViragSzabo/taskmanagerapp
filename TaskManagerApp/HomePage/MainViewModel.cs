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
        public ObservableCollection<TaskList.TaskList> ListOfLists {  get; set; }
        public ICollectionView ListOfListsView { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static readonly string DownloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        private static readonly string FileName = "TaskLists.xml";
        private readonly string FilePath = Path.Combine(DownloadsPath, FileName);

        public MainViewModel()
        {
            ListOfLists = new ObservableCollection<TaskList.TaskList>()
            {
                new TaskList.TaskList("Item1"), new TaskList.TaskList("Item2"), new TaskList.TaskList("Item3")
            };
            ListOfListsView = CollectionViewSource.GetDefaultView(ListOfLists);
            LoadDataAsync().ConfigureAwait(false);
        }

        private async Task LoadDataAsync()
        {
            if (File.Exists(FilePath))
            {
                var loadedLists = await LoadFromFileAsync<List<TaskList.TaskList>>(FilePath);
                if (loadedLists != null)
                {
                    foreach (var list in loadedLists)
                    {
                        ListOfLists.Add(list);
                    }
                    ListOfListsView.Refresh(); // Refresh the collection view
                }
            }
            else
            {
                InitializeDefaultTaskLists();
                await SaveToFileAsync(ListOfLists, FilePath);
            }

            Debug.WriteLine($"Loaded task lists: {ListOfLists.Count}"); // Check if lists are loaded correctly
        }

        public void AddTaskList(string listName)
        {
            if (!string.IsNullOrWhiteSpace(listName))
            {
                var newList = new TaskList.TaskList(listName);
                ListOfLists.Add(newList);
                OnPropertyChanged(nameof(ListOfLists)); 
                OnPropertyChanged(nameof(ListOfLists.Count));

                ListOfListsView.Refresh();

                Console.WriteLine($"Task list added: {newList.Name}"); // Debug line
            }
        }

        public void InitializeDefaultTaskLists()
        {
                ListOfLists.Add(CreateTaskList("Personal", new[]
            {
                new TasksBenefits.Task("Grocery Shopping", "Buy groceries for the week.", DateTime.Today),
                new TasksBenefits.Task("Exercise", "Go for a run in the evening.", DateTime.Today)
            }));

            ListOfLists.Add(CreateTaskList("Work", new[]
            {
                new TasksBenefits.Task("Project Meeting", "Prepare for the project meeting on Friday.", DateTime.Today),
                new TasksBenefits.Task("Submit Report", "Finalize and submit the monthly report.", DateTime.Today)
            }));

            ListOfLists.Add(CreateTaskList("Study", new[]
            {
                new TasksBenefits.Task("Finish Assignment", "Complete the programming assignment by Wednesday.", DateTime.Today),
                new TasksBenefits.Task("Review Lecture Notes", "Go through notes from the last lecture.", DateTime.Today)
            }));
        }

        public static TaskList.TaskList CreateTaskList(string listName, TasksBenefits.Task[] tasks)
        {
            var taskList = new TaskList.TaskList(listName);
            foreach (var task in tasks)
            {
                taskList.AddTask(task);
            }
            return taskList;
        }

        public static async Task SaveToFileAsync<T>(T data, string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StreamWriter(filePath);
                await Task.Run(() => serializer.Serialize(writer, data));
                Debug.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public static async Task<T?> LoadFromFileAsync<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath)) return null;

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StreamReader(filePath);
                return await Task.Run(() => serializer.Deserialize(reader) as T);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"Error deserializing file: {ex.Message} ",filePath);
                MessageBox.Show($"Error deserializing file: {ex.Message}", "Deserialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"General error loading file: {ex.Message}");
                MessageBox.Show($"Error loading file: {ex.Message}", "File Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        // Saving all task lists to XML file
        public async Task SaveAllTaskLists(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(MainViewModel)); // List of TaskList
                using (var writer = new StreamWriter(filePath))
                {
                    await Task.Run(() => serializer.Serialize(writer, this)); // Serialize and write to file
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving task lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}