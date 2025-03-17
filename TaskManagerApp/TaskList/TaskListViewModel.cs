using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Windows.Data;
using System.Xml.Serialization;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    public class TaskListViewModel
    {
        public TaskList _taskList { get; private set; }
        public ICollectionView FilteredTasksView { get; }
        public Task? _selectedTask { get; set; }

        private Priority? _selectedPriority = Priority.All;
        private Status? _selectedStatus = Status.All;


        public TaskListViewModel(TaskList list, Task? selected)
        {
            _taskList = list ?? throw new ArgumentNullException(nameof(list));
            _selectedTask = selected;

            FilteredTasksView = CollectionViewSource.GetDefaultView(_taskList.tasks);
            FilteredTasksView.Filter = FilterTasks;

            _taskList.tasks.CollectionChanged += (s, e) => FilteredTasksView?.Refresh();
            FilteredTasksView.Refresh();
        }

        public void AddTask(Task task)
        {
            if (task == null) return;
            _taskList.AddTask(task);
            FilteredTasksView?.Refresh();  // 🔥 Force a refresh
        }

        public void RemoveTask(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _taskList.RemoveTask(task);
            FilteredTasksView?.Refresh(); // 🔥 Force a refresh
        }

        public void FilterTasksByPriority(Priority? selectedPriority)
        {
            _selectedPriority = selectedPriority;
            FilteredTasksView.Refresh(); // 🔥 Force a refresh
        }

        public void FilterTasksByStatus(Status? selectedStatus)
        {
            _selectedStatus = selectedStatus;
            FilteredTasksView.Refresh(); // 🔥 Force a refresh
        }

        private bool FilterTasks(object obj)
        {
            if (obj is not Task task) return false;

            bool matchesPriority = _selectedPriority == Priority.All || task.Priority == _selectedPriority;
            bool matchesStatus = _selectedStatus == Status.All || task.Status == _selectedStatus;

            return matchesPriority && matchesStatus;
        }
    }
}