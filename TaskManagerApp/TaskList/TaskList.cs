using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    /// <summary>
    /// Represents a list of tasks with a name and allows management of those tasks.
    /// </summary>
    public class TaskList : IEnumerable<Task>
    {
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
        public ICommand DownloadCommand { get; private set; }

        public TaskList(string name)
        {
            Name = name;
            Tasks = new List<Task>();
            DownloadCommand = new RelayCommand(Download);
        }

        /// <summary>
        /// Adds a task to the list if it does not already exist with the same name and due date.
        /// </summary>
        /// <param name="task">The task to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if the task is null.</exception>
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

        /// <summary>
        /// Removes a task from the list.
        /// </summary>
        /// <param name="task">The task to be removed.</param>
        /// <exception cref="ArgumentException">Thrown if the task is not found in the list.</exception>
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

        /// <summary>
        /// Updates an existing task's properties with the new values provided in the updated task.
        /// </summary>
        /// <param name="existing">The existing task to be updated.</param>
        /// <param name="updated">The updated task with new values.</param>
        /// <exception cref="ArgumentNullException">Thrown if either existing or updated task is null.</exception>
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

        /// <summary>
        /// Retrieves all tasks in the list.
        /// </summary>
        /// <returns>An observable collection of tasks.</returns>
        public List<Task> GetTasks()
        {
            return Tasks;
        }

        // Implement IEnumerable<Task>
        public IEnumerator<Task> GetEnumerator()
        {
            return Tasks.GetEnumerator();
        }

        // Explicit non-generic IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Log error messages for debugging or auditing purposes
        private void LogError(string message)
        {
            // Implement logging logic (e.g., write to a log file, send to a monitoring service)
            Console.WriteLine($"ERROR: {message}"); // Placeholder for actual logging
        }

        // Notify the user of issues
        private void NotifyUser(string message)
        {
            // Implement user notification logic (e.g., show a message box, display in UI)
            MessageBox.Show(message, "Task Management", MessageBoxButton.OK, MessageBoxImage.Warning); // Example of a user notification
        }
    }
}
