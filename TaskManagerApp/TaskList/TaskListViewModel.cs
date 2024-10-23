using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    public class TaskListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Task> Tasks { get; set; }
        private readonly ObservableCollection<Task> _allTasks;

        private Task _selectedTask;
        public Task SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        public TaskListViewModel(TaskList taskList)
        {
            _allTasks = new ObservableCollection<Task>(taskList.GetTasks());
            Tasks = new ObservableCollection<Task>(_allTasks); // Start with all tasks
        }

        public void AddTask(Task newTask)
        {
            ValidateTask(newTask);
            Tasks.Add(newTask);
        }

        public void RemoveTask(Task taskToRemove)
        {
            Tasks.Remove(taskToRemove);
        }

        public void UpdateTask(Task oldTask, Task newTask)
        {
            ValidateTask(newTask);
            var index = Tasks.IndexOf(oldTask);
            if (index != -1)
            {
                Tasks[index] = newTask; // Update the task in the collection
                SelectedTask = newTask; // Update the SelectedTask if needed
            }
        }

        private void ValidateTask(Task task)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new ValidationException("Task name cannot be empty.");
            }
        }

        public void FilterTasksByPriority(Priority priority)
        {
            // Filter tasks based on the specified priority
            var filteredTasks = _allTasks.Where(t => t.Priority == priority).ToList();

            // Clear the current task list and add the filtered tasks
            Tasks.Clear();
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }

        public void FilterTasksByStatus(Status status)
        {
            // Filter tasks based on the specified status
            var filteredTasks = _allTasks.Where(t => t.Status == status).ToList();

            // Clear the current task list and add the filtered tasks
            Tasks.Clear();
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}