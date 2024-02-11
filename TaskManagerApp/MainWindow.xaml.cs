using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManagerApp
{
    public enum Priority
    {
        High,
        Medium,
        Low
    }

    public enum Status
    {
        Completed,
        InProgress,
        NotStarted
    }

    public class Task
    {
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        public Task(string givenName)
        {
            this.Name = givenName;
            this.DueDate = DateTime.Now;
            this.Priority = Priority.Medium;
            this.Status = Status.NotStarted;

        }
    }

    public class TaskList
    {
        public ObservableCollection<Task> Tasks { get; set; }

        public TaskList()
        {
            Tasks = new ObservableCollection<Task>();
        }

        public void AddTask(Task task)
        {
            Tasks.Add(task);
        }

        public void EditTask(int index, Task editedTask)
        {
            if(index >= 0 && index < Tasks.Count)
            {
                Tasks[index] = editedTask;
            }
        }

        public void RemoveTask(int index)
        {
            if (index >= 0 && index < Tasks.Count)
            {
                Tasks.RemoveAt(index);
            }
        }

        public void SortTasksByName()
        {
            Tasks = new ObservableCollection<Task>(Tasks.OrderBy(task => task.Name));
        }

        public void SortTasksByDueDate()
        {
            Tasks = new ObservableCollection<Task>(Tasks.OrderBy(task => task.DueDate));
        }

        public void FilterTasksByStatus(Status status)
        {
            Tasks = new ObservableCollection<Task>(Tasks.Where(task => task.Status == status));
        }
    }

    public class TaskManager
    {
        public List<TaskList> TaskLists { get; private set; }
        public TaskList CurrentTaskList { get; set; }

        public TaskManager()
        {
            TaskLists = new List<TaskList>();
            CurrentTaskList = new TaskList();
        }

        public void CreateTaskList()
        {
            TaskList newList = new TaskList();
            TaskLists.Add(newList);
        }

        public void RemoveTaskList(TaskList taskList)
        {
            TaskLists.Remove(taskList);
        }

        public void SortTasksByName()
        {
            CurrentTaskList.SortTasksByName();
        }

        public void SortTasksByDueDate()
        {
            CurrentTaskList.SortTasksByDueDate();
        }

        public void FilterTasksByStatus(Status status)
        {
            CurrentTaskList.FilterTasksByStatus(status);
        }
    }

    public partial class MainWindow : Window
    {
        public ObservableCollection<Task> Tasks { get; set; }
        private Task selectedTask { get; set; }
        private int CHARACTERLIMIT = 15;

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<Task>();
            taskListBox.ItemsSource = Tasks;

            // Set initial button visibility
            TriggerButtonVisibility(true, true, true, false);
        }
        
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newTaskName = taskInput.Text.Trim();

            if (!string.IsNullOrEmpty(newTaskName)) { 
                if (newTaskName.Length <= CHARACTERLIMIT)
                {
                    Task newTask = new Task(newTaskName);
                    Tasks.Add(newTask);
                    taskInput.Clear();
                } else
                  {
                    MessageBox.Show("Too long task name!");
                  }
            } else
                {
                    MessageBox.Show("Invalid task name!");
                }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            // Show the necessary buttons
            TriggerButtonVisibility(false, false, true, true);

            if (taskListBox.SelectedIndex != -1)
            {
                // Retrieve the selected task
                selectedTask = Tasks[taskListBox.SelectedIndex];

                //Show the selected task in the taskInput Textbox for editing
                taskInput.Text = selectedTask.Name;

                // Remove the selected task from the Tasks collection
                Tasks.RemoveAt(taskListBox.SelectedIndex);
            }
        }

        public void SaveEditedTask_Click(object sender, RoutedEventArgs e)
        {
            // Get the edited tasks from the task input textbox
            string editedTaskName = taskInput.Text.Trim();

            if (selectedTask != null)
            {
                if (editedTaskName.Length <= CHARACTERLIMIT)
                {

                    // Update the selected task
                    selectedTask.Name = editedTaskName;

                    // Add the edited task to the Task collection
                    Tasks.Add(selectedTask);

                    // Show the necessary buttons
                    TriggerButtonVisibility(true, true, true, false);

                    // Clear the taskInput
                    taskInput.Clear();

                    // Reset selectedTask to null
                    selectedTask = null;
                } else
                  {
                    MessageBox.Show("Too long task name!");
                  }
            }
            else
            {
                MessageBox.Show("Invalid task name!");
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {

            if (taskListBox.SelectedIndex >= 0)
            {
                Tasks.RemoveAt(taskListBox.SelectedIndex);
                taskInput.Clear();
            }
        }

        private void TriggerButtonVisibility(bool add, bool edit, bool remove, bool save)
        {
            addButton.Visibility = add ? Visibility.Visible : Visibility.Collapsed;
            editButton.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
            removeButton.Visibility = remove ? Visibility.Visible : Visibility.Collapsed;
            saveButton.Visibility = save ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
