using TaskManagerApp.TasksBenefits;
using Task = TaskManagerApp.TasksBenefits.Task;

namespace TaskManagerAppTests.TasksBenefits
{
    [TestClass()]
    public class TaskTests
    {
        private Task? _task;

        [TestInitialize]
        public void Setup()
        {
            _task = new Task("Test Task", "Test Description", DateTime.Now);
        }

        [TestMethod()]
        public void Constructor_InitializesCorrectly()
        {
            Assert.IsNotNull(_task);
            Assert.IsNotNull(_task.CreatedDateTime);
            Assert.AreEqual("Test Task", _task.Name);
            Assert.AreEqual("Test Description", _task.Description);
            Assert.AreEqual(Priority.Medium, _task.Priority);
            Assert.AreEqual(Status.InProgress, _task.Status);
        }

        [TestMethod()]
        public void Constructor_GeneratesUniqueId()
        {
            Task task1 = new Task("Task 1", "Description 1", DateTime.Now);

            Assert.IsNotNull(task1);
            Assert.AreNotEqual(task1.Id, _task.Id);
        }

        [TestMethod()]
        public void MarkAsCompleteTest()
        {
            _task.MarkAsComplete();
            Assert.AreEqual(Status.Completed, _task.Status);
        }

        [TestMethod()]
        public void EditTask_UpdatesProperties()
        {
            DateTime newDueDate = DateTime.Now.AddDays(5);
            _task.EditTask("Updated Task", "Updated Description", newDueDate, Priority.High, Status.Pending);

            Assert.IsNotNull(_task);
            Assert.IsNotNull(newDueDate);
            Assert.AreEqual("Updated Task", _task.Name);
            Assert.AreEqual("Updated Description", _task.Description);
            Assert.AreEqual(Priority.High, _task.Priority);
            Assert.AreEqual(Status.Pending, _task.Status);
            Assert.IsNotNull(_task.LastUpdatedDateTime);
        }

        [TestMethod()]
        public void GetDisplayName_ReturnsCorrectDisplayName()
        {
            string displayName = _task.GetDisplayName();
            Assert.AreEqual(Priority.Medium.ToString(), displayName);
        }

        [TestMethod()]
        public void ToString_ReturnsCorrectFormat()
        {
            string expected = $"{_task.Name} - {_task.Description} (Due: {_task.DueDateTime}, Priority: {_task.Priority}, Status: {_task.Status})";
            Assert.AreEqual(expected, _task.ToString());
        }
    }
}