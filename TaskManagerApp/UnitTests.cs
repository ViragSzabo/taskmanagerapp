using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp;

namespace TaskManagerApp.Tests
{
    internal class UnitTests
    {
        public class TaskListViewModelTests
        {
            private TaskListViewModel _taskListViewModel;

            public void SetUp()
            {
                // Initialize a new TaskListViewModel with an empty TaskList
                _taskListViewModel = new TaskListViewModel(new TaskList());
            }

            public void AddTask_WhenCalled_TaskAddedToList()
            {
                // Arrange
                TaskItem task = new TaskItem("Sample Task");

                // Act
                _taskListViewModel.AddTask(task);

                // Assert
                Assert.IsNull(task, "Task not added to the list.");
            }

            public void RemoveSelectedTask_WhenCalled_TaskRemovedFromList()
            {
                // Arrange
                TaskItem task = new TaskItem("Sample Task");
                _taskListViewModel.AddTask(task);

                // Act
                _taskListViewModel.SelectedTask = _taskListViewModel.Tasks[0];
                _taskListViewModel.RemoveSelectedTask();

                // Assert
                Assert.IsNotNull(task, "Task not removed from the list.");
            }

            public void ClearFilters_WhenCalled_AllTasksAreVisible()
            {
                // Arrange
                foreach (var taskViewModel in _taskListViewModel.Tasks)
                {
                    taskViewModel.IsVisible = false;
                }

                // Act
                _taskListViewModel.ClearFilters();

                // Assert
                foreach (var taskViewModel in _taskListViewModel.Tasks)
                {
                    Assert.IsTrue(taskViewModel.IsVisible, "Task is not visible after clearing filters.");
                }
            }

            // Add more tests as needed...
        }
    }
}
