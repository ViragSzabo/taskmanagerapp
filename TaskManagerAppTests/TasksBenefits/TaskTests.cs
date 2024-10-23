namespace TaskManagerApp.TasksBenefits.Tests
{
    [TestClass()]
    public class TaskTests
    {
        private Task _task;

        [TestInitialize]
        public void Setup()
        {
            // Arrange: Initialize a Task object before each test
            _task = new Task("Initial Task", "Initial Description", DateTime.Now);
        }

        [TestMethod()]
        public void TaskConstructorTest()
        {
            // Assert
            Assert.AreEqual("Initial Task", _task.Name);
            Assert.AreEqual("Initial Description", _task.Description);
            Assert.AreEqual(DateTime.Now.Date, _task.DueDateTime.Date); // Compare only the date part
            Assert.AreEqual(Priority.Medium, _task.Priority);
            Assert.AreEqual(Status.InProgress, _task.Status);
        }

        [TestMethod()]
        public void MarkAsCompleteTest()
        {
            // Act
            _task.MarkAsComplete();

            // Assert
            Assert.AreEqual(Status.Completed, _task.Status);
        }

        [TestMethod()]
        public async void EditTaskTest()
        {
            // Act
            await _task.EditTask("Updated Task", "Updated Description", DateTime.Now.AddDays(1), Priority.High, Status.Completed);

            // Assert
            Assert.AreEqual("Updated Task", _task.Name);
            Assert.AreEqual("Updated Description", _task.Description);
            Assert.AreEqual(DateTime.Now.AddDays(1).Date, _task.DueDateTime.Date); // Compare only the date part
            Assert.AreEqual(Priority.High, _task.Priority);
            Assert.AreEqual(Status.Completed, _task.Status);
        }

        [TestMethod()]
        public void GetDisplayNameTest()
        {
            // Arrange
            // Assuming you have a DisplayName attribute for your enum values
            string expectedDisplayName = "High"; // Update based on your attribute
            _task.Priority = Priority.High;

            // Act
            var displayName = _task.GetDisplayName();

            // Assert
            Assert.AreEqual(expectedDisplayName, displayName);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            // Act
            var result = _task.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Initial Task"));
            Assert.IsTrue(result.Contains("Initial Description"));
            Assert.IsTrue(result.Contains(_task.DueDateTime.ToString()));
            Assert.IsTrue(result.Contains(Priority.Medium.ToString()));
            Assert.IsTrue(result.Contains(Status.InProgress.ToString()));
        }
    }
}