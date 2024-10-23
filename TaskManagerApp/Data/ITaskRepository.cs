using System.Collections.Generic;

namespace TaskManagerApp.Data
{
    public interface ITaskRepository
    {
        IEnumerable<TaskList.TaskList> GetAllTaskLists();
        void AddTaskList(TaskList.TaskList taskList);
        void RemoveTaskList(TaskList.TaskList taskList);
    }
}
