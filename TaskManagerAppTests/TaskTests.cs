namespace TaskManagerApp.Tests
{
    [TestClass()]
    public class TaskTests
    {
        Task newTask = new Task("Default", "Default", DateTime.Today);

        [TestMethod()]
        public void TaskTest()
        {
            Assert.AreEqual("Default", newTask.Name);
        }

        [TestMethod()]
        public void markAsCompleteTest()
        {
            Task newTask = new Task("Default", "Default", DateTime.Today);
            newTask.Status = Status.InProgress;
            Assert.AreEqual(Status.InProgress, newTask.Status);
            newTask.Status = Status.Completed;
            Assert.AreEqual(Status.Completed, newTask.Status);
        }

        [TestMethod()]
        public void EditTaskTest()
        {
            newTask.EditTask("Clean up", "Vacuum the floor", DateTime.Now, Priority.High, Status.InProgress);
            Assert.AreEqual("Clean up", newTask.Name);
        }
    }
}