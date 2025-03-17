using System.Collections.Generic;

namespace TaskManagerApp.Data
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskList.TaskList> _taskLists = new();

        public IEnumerable<TaskList.TaskList> GetAllTaskLists()
        {
            return _taskLists.AsReadOnly();
        }

        public void AddTaskList(TaskList.TaskList taskList)
        {
            _taskLists.Add(taskList);
        }

        public void RemoveTaskList(TaskList.TaskList taskList)
        {
            _taskLists.Remove(taskList);
        }
    }
}