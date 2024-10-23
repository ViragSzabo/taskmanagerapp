using System.Collections.ObjectModel;

namespace TaskManagerApp.HomePage
{
    public class TaskManager
    {
        public ObservableCollection<TaskList.TaskList>? TaskLists { get; set; }

        public TaskManager()
        {
            TaskLists = new ObservableCollection<TaskList.TaskList>();
            // Add a default task list for testing
            TaskLists.Add(new TaskList.TaskList("Default Task List"));
        }

        public void AddTaskList(TaskList.TaskList newList)
        {
            TaskLists.Add(newList);
        }

        public void RemoveTaskList(TaskList.TaskList list)
        {
            TaskLists.Remove(list);
        }
    }
}