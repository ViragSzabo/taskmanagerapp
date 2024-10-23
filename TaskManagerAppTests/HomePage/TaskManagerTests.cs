namespace TaskManagerApp.HomePage.Tests
{
    [TestClass()]
    public class TaskManagerTests
    {
        private TaskManager _taskManager;

        [TestInitialize]
        public void Setup()
        {
            // Initialize TaskManager before each test
            _taskManager = new TaskManager();
        }

        [TestMethod()]
        public void TaskManager_Initializes_With_Default_Task_List()
        {
            // Arrange & Act
            if (_taskManager.TaskLists != null)
            {
                var defaultTaskList = _taskManager.TaskLists.FirstOrDefault();

                // Assert
                Assert.IsNotNull(defaultTaskList, "Default task list should not be null");
                Assert.AreEqual("Default Task List", defaultTaskList.Name, "Default task list name should match");
            }

            if (_taskManager.TaskLists != null)
                Assert.AreEqual(1, _taskManager.TaskLists.Count, "Task manager should have one default task list");
        }

        [TestMethod()]
        public void AddTaskList_Increases_TaskLists_Count()
        {
            // Arrange
            var newTaskList = new TaskList.TaskList("New Task List");

            // Act
            _taskManager.AddTaskList(newTaskList);

            // Assert
            if (_taskManager.TaskLists != null)
            {
                Assert.AreEqual(2, _taskManager.TaskLists.Count,
                    "TaskLists count should increase by one after adding a new list");
                Assert.AreEqual(newTaskList, _taskManager.TaskLists.Last(),
                    "The last task list should be the one that was just added");
            }
        }

        [TestMethod()]
        public void RemoveTaskList_Decreases_TaskLists_Count()
        {
            // Arrange
            var newTaskList = new TaskList.TaskList("Task to Remove");
            _taskManager.AddTaskList(newTaskList);

            // Act
            _taskManager.RemoveTaskList(newTaskList);

            // Assert
            if (_taskManager.TaskLists != null)
            {
                Assert.AreEqual(1, _taskManager.TaskLists.Count,
                    "TaskLists count should decrease by one after removing a list");
                Assert.IsFalse(_taskManager.TaskLists.Contains(newTaskList),
                    "TaskLists should not contain the removed list");
            }
        }

        [TestMethod()]
        public void RemoveTaskList_NonExisting_List_Does_Nothing()
        {
            // Arrange
            var nonExistentTaskList = new TaskList.TaskList("Non-Existing List");

            // Act
            _taskManager.RemoveTaskList(nonExistentTaskList);

            // Assert
            if (_taskManager.TaskLists != null)
                Assert.AreEqual(1, _taskManager.TaskLists.Count,
                    "TaskLists count should remain the same when trying to remove a non-existing list");
        }
    }
}