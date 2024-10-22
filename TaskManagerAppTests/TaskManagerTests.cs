namespace TaskManagerApp.Tests
{
    [TestClass()]
    public class TaskManagerTests
    {
        private TaskManager TaskManager = new TaskManager();
        private Task Task = new Task("Test Task", "Test Description", DateTime.Today);
        private Task ActiveTask = new Task("Active Task", "Test Description", DateTime.Today);

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
            TaskList list = TaskManager.CreateList("My Task");
            list.AddTask(Task);
            list.AddTask(ActiveTask);
            var result = TaskManager.AddActiveTasks();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Active Task", result[0].Name);
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