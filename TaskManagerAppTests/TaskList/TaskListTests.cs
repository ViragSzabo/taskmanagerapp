using TaskManagerApp.TasksBenefits;
using Task = TaskManagerApp.TasksBenefits.Task;

namespace TaskManagerAppTests.TaskList
{
    [TestClass()]
    public class TaskListTests
    {
        private TaskManagerApp.TaskList.TaskList? _list;
        private Task? _task;

        [TestInitialize]
        public void Setup()
        {
            _task = new Task("Test Task", "Test Description", DateTime.Now);
            _list = new TaskManagerApp.TaskList.TaskList("List of Tests");
        }

        [TestMethod()]
        public void Constructor_InitializesCorrectly()
        {
            Assert.IsNotNull(_list?.Name);
            Assert.IsTrue(_list.SizeOfTheList == 0);
        }

        [TestMethod()]
        public void AddTaskTest()
        {
            _list?.AddTask(_task);
            Assert.AreEqual(1, _list?.SizeOfTheList);
        }

        [TestMethod()]
        public void RemoveTaskTest()
        {
            if (_task != null)
            {
                _list?.AddTask(_task);
                _list?.RemoveTask(_task);
            }
            Assert.IsTrue(_list?.SizeOfTheList == 0);
        }

        [TestMethod()]
        public void UpdateTaskTest()
        {
            if (_task != null)
            {
                _list?.AddTask(_task);

                // Verify the task is added before proceeding
                Assert.AreEqual(1, _list?.SizeOfTheList);

                _task?.EditTask("Updated Test", "Updated", DateTime.Now, Priority.High, Status.Completed);
                _list?.UpdateTask(_task);
            }

            // Verify the task is updated after UpdateTask
            Assert.AreEqual("Updated Test", _list?.Tasks[0].Name);
            Assert.AreEqual("Updated", _list?.Tasks[0].Description);
            Assert.AreEqual(Priority.High, _list?.Tasks[0].Priority);
            Assert.AreEqual(Status.Completed, _list?.Tasks[0].Status);
        }

        [TestMethod()]
        public void ToString_ReturnsCorrectFormat()
        {
            string expected = $"{_list?.Name}";
            Assert.AreEqual(expected, _list?.ToString());
        }
    }
}