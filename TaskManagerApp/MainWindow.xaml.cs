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
        public ObservableCollection<string> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<string>();
            taskListBox.ItemsSource = Tasks;

            // Set initial button visibility
            TriggerButtonVisibility(true, true, true, false);
        }
        
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newTask = taskInput.Text.Trim();
            if(!string.IsNullOrEmpty(newTask) )
            {
                Tasks.Add(newTask);
                taskInput.Clear();
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if(taskListBox.SelectedIndex != -1)
            {
                // Show the necessary buttons
                TriggerButtonVisibility(true, false, true, true);

                // Retrieve the selected task
                string selectedTask = Tasks[taskListBox.SelectedIndex];

                //Show the selected task in the taskInput Textbox for editing
                taskInput.Text = selectedTask;

                // Remove the selected task from the Tasks collection
                Tasks.RemoveAt(taskListBox.SelectedIndex);
            }
        }

        public void SaveEditedTask_Click(object sender, RoutedEventArgs e)
        {
            // Show the necessary buttons
            TriggerButtonVisibility(true, true, true, false);

            // Get the edited tasks from the task input textbox
            string editedTask = taskInput.Text.Trim();

            // Add the edited task back to the Tasks collection
            Tasks.Add(editedTask);

            // Clear the taskInput Textbox
            taskInput.Clear();
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
