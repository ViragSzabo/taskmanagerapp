using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace TaskManagerApp
{
    public class TaskManager : INotifyPropertyChanged
    {
        public ObservableCollection<TaskList> TaskLists { get; set; } = new();
        public ObservableCollection<Task> CurrentTasks { get; set; } = new();

        public ObservableCollection<Task> AddActiveTasks()
        {
            // Clear current active tasks before adding new ones
            CurrentTasks.Clear();

            // Loop through each task list
            foreach (var list in TaskLists)
            {
                // Loop through each task in the current list
                foreach (var task in list.GetTasks())
                {
                    // Check if the task is high priority and in progress
                    if (task is { Priority: Priority.High, Status: Status.InProgress })
                    {
                        // Add to CurrentTasks collection
                        CurrentTasks.Add(task);
                    }
                }
            }
            Console.WriteLine("Active tasks updated.");
            return CurrentTasks;
        }

        public TaskList CreateList(string listName)
        {
            TaskList newList = new TaskList(listName);
            TaskLists.Add(newList);
            Console.WriteLine($"Task list '{listName}' created.");
            return newList;
        }

        public void DeleteList(TaskList list)
        {
            if (TaskLists.Contains(list))
            {
                TaskLists.Remove(list);
                Console.WriteLine($"Task list '{list.Name}' removed.");
            }
            else
            {
                Console.WriteLine($"Task list '{list.Name}' not found.");
            }
        }

        public ObservableCollection<Task> SortTasksByDueDate()
        {
            var sortedTasks = new ObservableCollection<Task>(CurrentTasks.OrderBy(t => t.DueDateTime));
            CurrentTasks.Clear();
            foreach (var task in sortedTasks)
            {
                CurrentTasks.Add(task);
            }
            OnPropertyChanged(nameof(CurrentTasks));
            return CurrentTasks;
        }

        public ObservableCollection<Task> FilterTasks(Priority priority, Status status, DateTime? dueDate = null)
        {
            var filteredTasks = new ObservableCollection<Task>(
                TaskLists.SelectMany(list => list.GetTasks())
                    .Where(task => (dueDate == null || task.DueDateTime.Date == dueDate.Value.Date) &&
                                   task.Priority == priority && task.Status == status));

            return filteredTasks;
        }

        public void DisplayDashboard()
        {
            Console.WriteLine("Task Lists");
            foreach (TaskList list in TaskLists)
            {
                Console.WriteLine($"{list.Name}");
            }

            Console.WriteLine("\n Active High-Priority Tasks:");
            foreach (Task task in CurrentTasks)
            {
                Console.WriteLine($"{task.Name} (Due: {task.DueDateTime.ToShortDateString()})");
            }
        }

        public void SaveData(string filePath)
        {
            try
            {
                // Create the serializer for the ObservableCollection<TaskList> type
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<TaskList>));

                // Save data to the XML file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, TaskLists);
                }

                Console.WriteLine("Data saved successfully to XML file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public void LoadData(string filePath)
        {
            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Create the serializer for the ObservableCollection<TaskList> type
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<TaskList>));

                    // Load data from the XML file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        TaskLists = (ObservableCollection<TaskList>)serializer.Deserialize(reader)!;
                    }

                    Console.WriteLine("Data loaded successfully from XML file.");
                }
                else
                {
                    // If file doesn't exist, initialize an empty collection
                    TaskLists = new ObservableCollection<TaskList>();
                    Console.WriteLine("No XML file found, initialized with an empty task list.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        public void SortTaskLists(bool ascending)
        {
            if (ascending)
            {
                TaskLists = new ObservableCollection<TaskList>(TaskLists.OrderBy(t => t.Name));
                Console.WriteLine("Task lists sorted in ascending order by name.");
            }
            else
            {
                TaskLists = new ObservableCollection<TaskList>(TaskLists.OrderByDescending(t => t.Name));
                Console.WriteLine("Task lists sorted in descending order by name.");
            }
            OnPropertyChanged(nameof(TaskLists));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}