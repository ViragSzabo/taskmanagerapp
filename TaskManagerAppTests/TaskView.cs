using TaskManagerApp;
using Task = TaskManagerApp.Task;

namespace TaskManagerAppTests
{
    public class TaskView
    {
        public Task SelectedTask { get; set; }

        public TaskView(Task task)
        {
            SelectedTask = task;
        }

        public void MarkAsComplete()
        {
            SelectedTask.Status = Status.Completed;
        }
    }
}
