namespace TaskManagerApp.Tests
{
    [TestClass()]
    public class TaskListTests
    {
        private TaskList taskList = new TaskList("My Tasks");
        private Task task = new Task("Test Task", "Test Description", DateTime.Today);

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
        public void AddDuplicateTaskTest()
        {
            taskList.AddTask(task);
            taskList.AddTask(task); // Adding the same task again
            Assert.AreEqual(1, taskList.GetTasks().Count); // Assuming you don't allow duplicates
        }

        [TestMethod()]
        public void RemoveTaskTest()
        {
            taskList.AddTask(task);
            taskList.RemoveTask(task);
            Assert.AreEqual(0, taskList.GetTasks().Count);
        }

        [TestMethod()]
        public void RemoveFromEmptyListTest()
        {
            taskList.RemoveTask(task); // Attempting to remove a task that isn't in the list
            Assert.AreEqual(0, taskList.GetTasks().Count); // Should still be 0
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

        [TestMethod()]
        public void EnumeratorReturnsCorrectTasksTest()
        {
            taskList.AddTask(task);
            var tasks = taskList.ToList(); // Assuming you can convert to a list
            Assert.AreEqual(task, tasks[0]); // Verify the enumerator returns the correct task
        }

        [TestMethod()]
        public void TaskPropertiesTest()
        {
            taskList.AddTask(task);
            var addedTask = taskList.GetTasks()[0];
            Assert.AreEqual("Test Task", addedTask.Name);
            Assert.AreEqual("Test Description", addedTask.Description);
            Assert.AreEqual(DateTime.Today, addedTask.DueDateTime);
        }
    }
}