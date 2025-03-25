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
        public ObservableCollection<TasksBenefits.Task> HighPriorityTasks { get; set; }

        public TasksBenefits.Task? _selectedHighPriorityTask;
        public TasksBenefits.Task? SelectedHighPriorityTask
        {
            get => _selectedHighPriorityTask;
            set
            {
                _selectedHighPriorityTask = value;
                OnPropertyChanged(nameof(SelectedHighPriorityTask));
            }
        }

        public ICollectionView ListOfListsView { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static readonly string DownloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        private static readonly string FileName = "TaskLists.xml";
        private readonly string FilePath = Path.Combine(DownloadsPath, FileName);

        public MainViewModel()
        {
            ListOfLists = new ObservableCollection<TaskList.TaskList>();
            HighPriorityTasks = new ObservableCollection<TasksBenefits.Task>();

            ListOfListsView = CollectionViewSource.GetDefaultView(ListOfLists);
            InitializeDefaultTaskLists();

            _ = Task.Run(() => LoadDataAsync());
        }

        public void UpdateHighPriorityTasks()
        {
            HighPriorityTasks.Clear();

            foreach (var taskList in ListOfLists)
            {
                foreach (var task in taskList.Tasks.Where(t => t.Priority == TasksBenefits.Priority.High))
                {
                    HighPriorityTasks.Add(task);

                    if (task.Status == TasksBenefits.Status.Completed)
                    {
                        HighPriorityTasks.Remove(task);
                    }
                }
            }
            OnPropertyChanged(nameof(HighPriorityTasks));
        }

        public async void LoadDataAsync()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var loadedLists = LoadTaskListsFromFile(FilePath);
                    if (loadedLists != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            UpdateListView(loadedLists);
                            UpdateHighPriorityTasks();
                        });
                    }
                }
                else
                {
                    InitializeDefaultTaskLists();
                    await SaveToFileAsync(ListOfLists, FilePath);
                    Debug.WriteLine("Initialized default task lists and saved to file.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading task lists: {ex.Message}");
                MessageBox.Show($"An error occurred while loading task lists: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Debug.WriteLine($"Loaded task lists: {ListOfLists.Count}");
        }

        public static List<TaskList.TaskList>? LoadTaskListsFromFile(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<TaskList.TaskList>));
                using var reader = new StreamReader(filePath);
                return (List<TaskList.TaskList>?)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading task lists from file: {ex.Message}");
                MessageBox.Show($"An error occurred while loading task lists from file: {ex.Message}", "File Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public void UpdateListView(List<TaskList.TaskList> loadedLists)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ListOfLists.Clear();
                foreach (var list in loadedLists)
                {
                    ListOfLists.Add(list);
                    UpdateHighPriorityTasks();
                    Debug.WriteLine($"Successfully loaded {list.Tasks.Count} tasks.");
                }
                ListOfListsView.Refresh();
            });
        }

        public void AddTaskList(string listName)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var newList = new TaskList.TaskList(listName);
                ListOfLists.Add(newList);
                UpdateHighPriorityTasks();
                ListOfListsView.Refresh();
                _ = SaveOnExist();

                Console.WriteLine($"Task list added: {newList.Name}");
            });
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

        public static async Task<T?> LoadFromFileAsync<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath)) return null;

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StreamReader(filePath);
                var xmlContent = await reader.ReadToEndAsync();
                Debug.WriteLine($"XML Content:\n{xmlContent}");
                return await Task.Run(() => serializer.Deserialize(reader) as T);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"Error deserializing file: {ex.Message} ", filePath);
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

        public static async Task SaveToFileAsync<T>(T data, string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StreamWriter(filePath);
                await Task.Run(() => serializer.Serialize(writer, data));
                Debug.WriteLine("Tasklists and tasks saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public async Task SaveAllTaskLists()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<TaskList.TaskList>));
                using (var writer = new StreamWriter(FilePath))
                {
                    await Task.Run(() => serializer.Serialize(writer, ListOfLists));
                }
                Debug.WriteLine("Task lists saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving task lists: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task? SaveOnExist()
        {
            await SaveToFileAsync(ListOfLists, FilePath);
        }
    }
}