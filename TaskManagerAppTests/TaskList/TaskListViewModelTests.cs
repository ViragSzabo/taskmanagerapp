using TaskManagerApp.TaskList;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Priority = TaskManagerApp.TasksBenefits.Priority;
using Status = TaskManagerApp.TasksBenefits.Status;
using Task = TaskManagerApp.TasksBenefits.Task;

namespace TaskManagerAppTests.TaskList
{
    [TestClass()]
    public class TaskListViewModelTests
    {
        private Task? _task;
        private Task? _task2;
        private TaskListViewModel? _viewModel;
        private TaskManagerApp.TaskList.TaskList? _taskList;

        [TestInitialize]
        public void SetUp()
        {
            _task = new Task("Test Task", "Test Description", DateTime.Now);
            _task2 = new Task("Blaa", "Blaa pronounced blahhh", DateTime.Now);
            _taskList = new TaskManagerApp.TaskList.TaskList("Test List");
            _viewModel = new TaskListViewModel(_taskList, _task);
        }

        [TestMethod()]
        public void Constructor_InitializeCorrectly()
        {
            Assert.IsNotNull(_viewModel.FilteredTasksView);
            Assert.AreEqual(0, _viewModel.FilteredTasksView.Cast<Task>().Count());
        }

        [TestMethod()]
        public void AddTaskTest()
        {
            _viewModel.AddTask(_task);
            _viewModel.AddTask(_task2);

            Assert.AreEqual(2, _viewModel._taskList.SizeOfTheList);
            Assert.IsTrue(_viewModel.FilteredTasksView.Cast<Task>().Any(t => t.Name == "Test Task"));
            Assert.IsTrue(_viewModel.FilteredTasksView.Cast<Task>().Any(t => t.Name == "Blaa"));
        }

        [TestMethod()]
        public void RemoveTaskTest()
        {
            _viewModel.AddTask(_task2);
            _viewModel.RemoveTask(_task2);

            Assert.AreEqual(0, _viewModel._taskList.SizeOfTheList);
            Assert.IsFalse(_viewModel.FilteredTasksView.Cast<Task>().Any(t => t.Name == "Blaa"));
        }

        [TestMethod()]
        public void FilterTasksByPriority_ShouldFilterCorrectly()
        {
            _viewModel.AddTask(_task);
            _viewModel.AddTask(_task2);

            _task.EditTask("Task 1", "Description", DateTime.Now, Priority.Low, Status.Pending);
            _task2.EditTask("Task 2", "Description", DateTime.Now, Priority.High, Status.Completed);

            _viewModel._taskList.UpdateTask(_task);
            _viewModel._taskList.UpdateTask(_task2);

            _viewModel.FilterTasksByPriority(Priority.High);
            _viewModel.FilteredTasksView.Refresh(); // 🔥 Force refresh manually

            var filteredTasks = _viewModel.FilteredTasksView.Cast<Task>().ToList();
            Assert.AreEqual(1, filteredTasks.Count());
            Assert.AreEqual(Priority.High, filteredTasks.First().Priority);
        }

        [TestMethod()]
        public void FilterTasksByStatus_ShouldFilterCorrectly()
        {
            _viewModel.AddTask(_task);

            _task.EditTask("Test", "Show Testing", DateTime.Now, Priority.Medium, Status.InProgress);

            _viewModel._taskList.UpdateTask(_task);

            _viewModel.FilterTasksByStatus(Status.InProgress);
            _viewModel.FilteredTasksView.Refresh();  // 🔥 Force refresh manually

            var filteredTasks = _viewModel.FilteredTasksView.Cast<Task>().ToList();
            Assert.AreEqual(1, filteredTasks.Count());
            Assert.AreEqual(Status.InProgress, filteredTasks.First().Status);
        }

        [TestMethod()]
        public void FilterTasksByPriority_All_ShouldShowAllTasks()
        {
            _viewModel.AddTask(_task);
            _viewModel.AddTask(_task2);

            _viewModel.FilterTasksByPriority(Priority.All);
            _viewModel.FilteredTasksView.Refresh();  // 🔥 Force refresh manually

            Assert.AreEqual(2, _viewModel._taskList.SizeOfTheList);
        }

        [TestMethod()]
        public void FilterTasksByStatus_All_ShouldShowAllTasks()
        {
            _viewModel.AddTask(_task);
            _viewModel.AddTask(_task2);
            _viewModel.FilterTasksByStatus(Status.All);
            _viewModel.FilteredTasksView.Refresh();  // 🔥 Force refresh manually

            Assert.AreEqual(2, _viewModel._taskList.SizeOfTheList);
        }

        [TestMethod()]
        public void FilterTasksByPriority_ShouldShowEmptyWhenNoMatch()
        {
            _viewModel.AddTask(_task);
            _viewModel.AddTask(_task2);

            _task.EditTask("Task 1", "Description", DateTime.Now, Priority.Low, Status.Pending);
            _task2.EditTask("Task 2", "Description", DateTime.Now, Priority.Low, Status.Completed);

            _viewModel._taskList.UpdateTask(_task);
            _viewModel._taskList.UpdateTask(_task2);

            _viewModel.FilterTasksByPriority(Priority.High);
            _viewModel.FilteredTasksView.Refresh();

            var filteredTasks = _viewModel.FilteredTasksView.Cast<Task>().ToList();
            Assert.AreEqual(0, filteredTasks.Count());
        }

        [TestMethod()]
        public void FilterTasksByStatus_ShouldShowEmptyWhenNoMatch()
        {
            _viewModel.AddTask(_task);

            _task.EditTask("Test", "Show Testing", DateTime.Now, Priority.Medium, Status.Pending);

            _viewModel._taskList.UpdateTask(_task);

            _viewModel.FilterTasksByStatus(Status.Completed);
            _viewModel.FilteredTasksView.Refresh();  // 🔥 Force refresh manually

            var filteredTasks = _viewModel.FilteredTasksView.Cast<Task>().ToList();
            Assert.AreEqual(0, filteredTasks.Count());
        }
    }
}