using Task = TaskManagerApp.TasksBenefits.Task;

namespace TaskManagerApp.TaskList.Tests
{
    [TestClass()]
    public class TaskListTests
    {
        private TaskList _taskList;
        private Task task; // Moved to class level for accessibility
        private Task updatedTask; // Moved to class level for accessibility

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            _taskList = new TaskList("Test Task List");

            task = new Task("Test", "Testing", DateTime.Now);
            _taskList.AddTask(task);

            Task task1 = new Task("Test Task 1", "Testing", DateTime.Now);
            Task task2 = new Task("Test Task 2", "Testing", DateTime.Now.AddDays(1));
            _taskList.AddTask(task1);
            _taskList.AddTask(task2);

            updatedTask = new Task("Updated Task", "Testing Updated", DateTime.Now.AddDays(1));
        }

        [TestMethod()]
        public void TaskListTest()
        {
            Assert.AreEqual("Test Task List", _taskList.Name);
            Assert.IsNotNull(_taskList.Tasks);
            Assert.AreEqual(3, _taskList.Tasks.Count); // Changed to 3 since we added 3 tasks
        }

        [TestMethod()]
        public void AddTaskTest()
        {
            // Arrange
            Task newTask = new Task("New Task", "Testing", DateTime.Now);

            // Act
            _taskList.AddTask(newTask);

            // Assert
            Assert.AreEqual(4, _taskList.Tasks.Count); // Should be 4 now
            Assert.AreEqual("New Task", _taskList.Tasks[3].Name); // Check the newly added task
        }

        [TestMethod()]
        public void RemoveTaskTest()
        {
            // Act
            _taskList.RemoveTask(task);

            // Assert
            Assert.AreEqual(2, _taskList.Tasks.Count); // Should be 2 now
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveTask_NotFound_ThrowsException()
        {
            // Arrange
            Task nonExistentTask = new Task("Non-existent", "Testing", DateTime.Now); // This task was never added

            // Act
            _taskList.RemoveTask(nonExistentTask); // Should throw exception
        }

        [TestMethod()]
        public void UpdateTaskTest()
        {
            // Act
            _taskList.UpdateTask(task, updatedTask);

            // Assert
            Assert.AreEqual("Updated Task", _taskList.Tasks[0].Name);
            Assert.AreEqual(DateTime.Now.AddDays(1).Date, _taskList.Tasks[0].DueDateTime.Date); // Check updated due date
        }

        [TestMethod()]
        public void GetTasksTest()
        {
            // Act
            var tasks = _taskList.GetTasks();

            // Assert
            Assert.AreEqual(3, tasks.Count); // We expect 3 tasks
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            // Act
            var enumerator = _taskList.GetEnumerator();
            enumerator.MoveNext();

            // Assert
            Assert.AreEqual("Test", enumerator.Current.Name); // Check the name of the first task
        }
    }
}