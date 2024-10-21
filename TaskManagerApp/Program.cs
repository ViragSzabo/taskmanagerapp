using System;

namespace TaskManagerApp
{
    public class Program
    {
        public static void main(string[] args)
        {
            TaskManager manager = new TaskManager();

            manager.CreateList("Personal");
            manager.CreateList("Work");

            TaskList personalList = manager.TaskLists[0];
            personalList.AddTask(new Task("Buy groceries", "Milk, Eggs, Bread", DateTime.Now.AddDays(2), Priority.HIGH, Status.NotStarted));
            personalList.AddTask(new Task("Read a book", "Finish reading '1984'", DateTime.Now.AddDays(7), Priority.MEDIUM, Status.NotStarted));

            TaskList workList = manager.TaskLists[1];
            workList.AddTask(new Task("Finish report", "Complete the annual report", DateTime.Now.AddDays(5), Priority.HIGH, Status.NotStarted));

            manager.DisplayDashboard();
        }
    }
}
