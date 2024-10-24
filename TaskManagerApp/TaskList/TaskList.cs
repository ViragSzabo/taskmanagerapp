using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    [XmlRoot("TaskList")]
    public class TaskList : IEnumerable<Task>
    {
        public string Name { get; set; }

        [XmlArray("Tasks")]
        [XmlArrayItem("Task")]
        public ObservableCollection<Task> Tasks { get; set; }
        public ICommand DownloadCommand { get; private set; }

        // Parameterless constructor for XML serialization
        public TaskList()
        {
            Tasks = new ObservableCollection<Task>();
        }

        // Constructor with parameters
        public TaskList(string name)
        {
            Name = name;
            Tasks = new ObservableCollection<Task>();
        }

        public void AddTask(Task task)
        {
            if (task == null)
            {
                LogError("Attempted to add a null task.");
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }

            if (!Tasks.Any(t => t.Name == task.Name && t.DueDateTime == task.DueDateTime))
            {
                Tasks.Add(task);
            }
            else
            {
                string message = $"{task.Name} already exists in the list with the same due date.";
                LogError(message);
                NotifyUser(message);
            }
        }

        public void RemoveTask(Task task)
        {
            if (!Tasks.Contains(task))
            {
                string message = "Task not found in the list.";
                LogError(message);
                throw new ArgumentException(message, nameof(task));
            }
            Tasks.Remove(task);
        }

        public void UpdateTask(Task existing, Task updated)
        {
            if (existing == null || updated == null)
            {
                LogError("Attempted to update a task with a null existing or updated task.");
                throw new ArgumentNullException(nameof(existing));
            }

            var index = Tasks.IndexOf(existing);
            if (index != -1)
            {
                Tasks[index]
                    .EditTask(updated.Name, updated.Description, updated.DueDateTime, updated.Priority, updated.Status)
                    .Wait();
            }
            else
            {
                string message = "Task not found in the list.";
                LogError(message);
                NotifyUser(message);
            }
        }

        public void Download(object parameter)
        {
            // Implement logic
        }

        public ObservableCollection<Task> GetTasks()
        {
            return Tasks;
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return Tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void LogError(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }

        private void NotifyUser(string message)
        {
            MessageBox.Show(message, "Task Management", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}