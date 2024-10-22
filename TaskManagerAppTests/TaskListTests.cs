namespace TaskManagerApp.Tests
{
    [TestClass()]
    public class TaskListTests
    {
        private TaskList taskList = new TaskList("My Tasks");
        private Task task = new Task("Test Task", "Test Description", DateTime.Today, Priority.HIGH, Status.InProgress);

        [TestMethod()]
        public void TaskListTest()
        {
            Assert.AreEqual("My Tasks", taskList.Name);
            Assert.AreEqual(0, taskList.GetTasks().Count);
        }

        [TestMethod()]
        public void AddTaskTest()
        {
            taskList.AddTask(task);
            Assert.AreEqual(1, taskList.GetTasks().Count);
            Assert.AreEqual(task, taskList.GetTasks()[0]);
        }

        [TestMethod()]
        public void RemoveTaskTest()
        {
            taskList.AddTask(task);
            taskList.RemoveTask(task);
            Assert.AreEqual(0, taskList.GetTasks().Count);
        }

        [TestMethod()]
        public void GetTasksTest()
        {
            taskList.AddTask(task);
            var tasks = taskList.GetTasks();
            Assert.AreEqual(1, tasks.Count);
            Assert.AreEqual(task, tasks[0]);
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            taskList.AddTask(task);
            int count = 0;
            foreach (var t in taskList)
            {
                count++;
            }
            Assert.AreEqual(1, count);
        }
    }
}