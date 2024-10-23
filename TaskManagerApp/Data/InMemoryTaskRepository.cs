using System.Collections.Generic;

namespace TaskManagerApp.Data
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskList.TaskList> taskLists = new();

        public IEnumerable<TaskList.TaskList> GetAllTaskLists()
        {
            return taskLists.AsReadOnly();
        }

        public void AddTaskList(TaskList.TaskList taskList)
        {
            taskLists.Add(taskList);
        }

        public void RemoveTaskList(TaskList.TaskList taskList)
        {
            taskLists.Remove(taskList);
        }
    }
}