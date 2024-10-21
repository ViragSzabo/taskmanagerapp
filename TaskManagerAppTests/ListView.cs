using TaskManagerApp;

namespace TaskManagerAppTests
{
    public class ListView
    {
        public List<TaskList> TaskLists { get; set; } = new();

        public void LoadTaskLists(TaskManager taskManager)
        {
            TaskLists.Clear();
            foreach (TaskList list in taskManager.TaskLists)
            {
                this.TaskLists.Add(list);
            }
        }
    }
}