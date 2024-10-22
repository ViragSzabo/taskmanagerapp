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
            if(!Tasks.Contains(task))
            {
                this.Tasks.Add(task);
            }
            else
            {
                Console.WriteLine($"{task} is already in the list.");
            }
        }

        public void RemoveTask(Task task)
        {
            this.Tasks.Remove(task);
        }

        public void UpdateTask(Task existing, Task updated)
        {
            // Find and update the task in the Tasks collection
            var index = Tasks.IndexOf(existing);
            if (index != -1)
            {
                Tasks[index] = updated;
            }
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
    }
}