namespace TaskManagerApp.Tests
{
    [TestClass()]
    public class TaskManagerTests
    {
        private TaskManager TaskManager = new TaskManager();
        private Task Task = new Task("Test Task", "Test Description", DateTime.Today);
        private Task ActiveTask = new Task("Active Task", "Test Description", DateTime.Today);
        private TaskList list = new TaskList("DefaultTest");

        [TestMethod()]
        public void TaskManagerTest()
        {
            Assert.IsNotNull(TaskManager);
        }

        [TestMethod()]
        public void CreateListTest()
        {
            TaskManager.CreateList("New Task List");
            Assert.AreEqual("New Task List", TaskManager.TaskLists[0].Name);
        }

        [TestMethod()]
        public void DeleteListTest()
        {
            TaskManager.CreateList("New Task List");
            TaskManager.DeleteList(TaskManager.TaskLists[0]);
            Assert.AreEqual(0, TaskManager.TaskLists.Count);
        }

        [TestMethod()]
        public void AddActiveTasksTest()
        {
            TaskManager.CreateList("list");  // Creates a new list, but you're using a local `list`
            TaskManager.TaskLists.Add(list);  // Make sure to add the list to TaskManager

            list.AddTask(ActiveTask);
            list.AddTask(Task);

            Task.EditTask("Task",
                "Test Description",
                DateTime.Today,
                Priority.Medium,
                Status.InProgress);
            ActiveTask.EditTask("Active Task",
                "Test Description",
                DateTime.Today,
                Priority.High,
                Status.InProgress);

            var result = TaskManager.AddActiveTasks();

            Assert.AreEqual(1, result.Count);  // Should now match
            Assert.AreEqual("Active Task", result[0].Name);  // Verifying the name
        }

        [TestMethod()]
        public void DisplayDashboardTest()
        {
            TaskManager.CreateList("New Task List");
            TaskManager.DisplayDashboard();
            Assert.AreEqual(1, TaskManager.TaskLists.Count);
            Assert.AreEqual("New Task List", TaskManager.TaskLists[0].Name);
        }
    }
}