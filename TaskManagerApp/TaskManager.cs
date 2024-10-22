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

        public ObservableCollection<TaskList> CreateList(string listName)
        {
            TaskLists.Add(new TaskList(listName));
            Console.WriteLine($"Task list '{listName}' created.");
            return TaskLists;
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
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<TaskList>));
                    serializer.Serialize(stream, new List<TaskList>(TaskLists));
                }
            }
            catch (Exception ex)
            {
                // Log the error or display a message to the user
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }

        public void LoadData(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<TaskList>));
                        var taskLists = (List<TaskList>)serializer.Deserialize(stream)!;
                        foreach (var taskList in taskLists)
                        {
                            TaskLists.Add(taskList);
                        }
                    }
                }
                else
                {
                    // Optionally create default structure or log message
                    TaskLists.Add(new TaskList("Default List")); // Create a default list if file doesn't exist
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