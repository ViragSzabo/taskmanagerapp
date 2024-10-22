using System;
using System.Collections.ObjectModel;

namespace TaskManagerApp
{
    public class TaskManager
    {
        public ObservableCollection<TaskList> TaskLists { get; set; }
        public ObservableCollection<Task> CurrentTasks { get; set; }

        public TaskManager()
        {
            TaskLists = new ObservableCollection<TaskList>();
            CurrentTasks = new ObservableCollection<Task>();
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

        public ObservableCollection<Task> FilterTasks(Priority priority, Status status)
        {
            var filteredTasks = new ObservableCollection<Task>();
            foreach (var list in TaskLists)
            {
                foreach (var task in list.GetTasks())
                {
                    if (task.Priority == priority && task.Status == status)
                    {
                        filteredTasks.Add(task);
                    }
                }
            }
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
    }
}
