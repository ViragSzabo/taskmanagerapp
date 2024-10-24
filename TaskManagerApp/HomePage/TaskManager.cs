using System.Collections.ObjectModel;

namespace TaskManagerApp.HomePage
{
    public class TaskManager
    {
        public ObservableCollection<TaskList.TaskList>? TaskLists { get; set; } = new();

        public void AddTaskList(TaskList.TaskList newList)
        {
            TaskLists?.Add(newList);
        }

        public void RemoveTaskList(TaskList.TaskList list)
        {
            TaskLists?.Remove(list);
        }
    }
}