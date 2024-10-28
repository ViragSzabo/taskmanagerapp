using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    [XmlRoot("TaskList")]
    public class TaskList : ObservableCollection<Task>
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        public ObservableCollection<Task> Tasks { get; set; }

        [XmlIgnore] // ICommand cannot be serialized directly
        public ICommand? DownloadCommand { get; private set; }

        // Parameterless constructor for XML serialization
        public TaskList() { }

        public TaskList(string listName)
        {
            Name = listName;
            Tasks = new ObservableCollection<Task>
            {
                new Task("Sample Task 1", "Sample", DateTime.Now),
                new Task("Sample Task 2", "Sample", DateTime.Now)
            };
            Debug.WriteLine("Show name: " + Name);
        }

        public void AddTask(Task task)
        {
            this.Add(task);
        }

        public void RemoveTask(Task task)
        {
            this.Remove(task);
        }

        public void UpdateTask(Task existing, Task updated)
        {
            var index = this.IndexOf(existing);
            if (index != -1)
            {
                this[index].EditTask(updated.Name, updated.Description, updated.DueDateTime, updated.Priority, updated.Status).Wait();
            }
        }

        private void LogError(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }

        private void NotifyUser(string message)
        {
            MessageBox.Show(message, "Task Management", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}