using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TaskManagerApp
{
    public class TaskList : IEnumerable<Task>
    {
        public string Name { get; private set; }
        public ObservableCollection<Task> Tasks { get; set; }

        public TaskList(string name)
        {
            Name = name;
            this.Tasks = new ObservableCollection<Task>();
        }

        public void AddTask(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            this.Tasks.Add(task);
            Console.WriteLine($"Task '{task.Name}' added to '{Name}'.");
        }

        public void RemoveTask(Task task)
        {
            if (this.Tasks.Contains(task))
            {
                this.Tasks.Remove(task);
                Console.WriteLine($"Task '{task.Name}' removed from '{Name}'.");
            }
            else
            {
                Console.WriteLine($"Task '{task.Name}' not found in the list of '{Name}'.");
            }
        }

        public ObservableCollection<Task> GetTasks()
        {
            return this.Tasks;
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return Tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}