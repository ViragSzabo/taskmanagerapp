using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManagerApp
{
    // priority of a task
    public enum Priority
    {
        High,
        Medium,
        Low
    }

    // status os a task
    public enum Status
    {
        Completed,
        InProgress,
        NotStarted
    }

    // present of a task
    public class Task
    {
        // attributes of a task
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        // constructor of a task
        public Task(string name)
        {
            this.Name = name;
            this.DueDate = DateTime.Now;
            this.Priority = Priority.Medium;
            this.Status = Status.NotStarted;
        }
    }

    // present a task list of the app
    public class TaskList
    {
        // collect a list of the tasks that are currently available
        // encapsulation: to give a good practice for data integrity and modularity
        // ObservableCollection is suitable for updating UI elements automatically when the underlying data changes
        public ObservableCollection<Task> Tasks { get; set; }
        public Task SelectedTask { get; set; }

        // constructor of a tasklist
        public TaskList()
        {
            Tasks = new ObservableCollection<Task>();
        }

        // add a new task
        public void AddTaskToList(Task newTask)
        {
            Tasks.Add(newTask);
        }

        // edit a task
        public void EditTask(string name, DateTime? date, Priority priority, Status status)
        {
            // update the attributes of the given task
            this.SelectedTask.Name = name;
            this.SelectedTask.DueDate = date;
            this.SelectedTask.Priority = priority;
            this.SelectedTask.Status = status;
        }

        // remove a task
        public void RemoveTask(Task task)
        {
            Tasks.Remove(task);
        }

        // sort tasks by the name
        public void SortTasksByName()
        {
            Tasks = new ObservableCollection<Task>(Tasks.OrderBy(task => task.Name));
        }

        // sort tasks by the due date
        public void SortTasksByDueDate()
        {
            Tasks = new ObservableCollection<Task>(Tasks.OrderBy(task => task.DueDate));
        }

        // filter tasks by the status
        public void FilterTasksByStatus(Status status)
        {
            Tasks = new ObservableCollection<Task>(Tasks.Where(task => task.Status == status));
        }

        // filter tasks by the priority
        public void FilterTasksByPriority(Priority priority)
        {
            Tasks = new ObservableCollection<Task>(Tasks.Where(task => task.Priority == priority));
        }
    }

    // preset a task manager
    public class TaskManager
    {
        // attributes of the taskmanager: collect tasklists of the app
        // ObservableCollection is suitable for updating UI elements automatically when the underlying data changes
        public ObservableCollection<TaskList> TaskLists { get; set; }

        public TaskManager()
        {
            TaskLists = new ObservableCollection<TaskList>();
        }

        public void AddTaskList(TaskList taskList)
        {
            TaskLists.Add(taskList);
        }

        public void RemoveTaskList(TaskList taskList)
        {
            TaskLists.Remove(taskList);
        }
    }

    public partial class MainWindow : Window
    {
        private int CHARACTERLIMIT = 15;
        private TaskManager taskManager;
        private TaskList defaultTaskList;
        private Task defaultTask;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            taskManager = new TaskManager();
            defaultTaskList = new TaskList();
            defaultTask = new Task("test");

            TriggerButtonVisibility(true, true, false, true); // set initial button visibility
            //SetUpSortingAndFiltering(); // set up sorting and filtering
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TriggerButtonVisibility(false, true, true, true); // Show the necessary buttons
            string newName = taskInput.Text.Trim();

            if(!string.IsNullOrEmpty(newName) && newName.Length <= CHARACTERLIMIT)
            {
                Task newTask = new Task(newName);
                defaultTaskList.AddTaskToList(newTask);
                taskListBox.ItemsSource = defaultTaskList.Tasks;
            }
            else
            {
                MessageBox.Show("Invalid task name!");
            }
            taskInput.Clear();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            TriggerButtonVisibility(false, false, true, true); // Show the necessary buttons

            if(taskListBox.SelectedIndex != -1)
            {
                // Retrieve the selected task
                defaultTaskList.SelectedTask = defaultTaskList.Tasks[taskListBox.SelectedIndex];
                taskInput.Text = defaultTaskList.SelectedTask.Name;
            }
            taskListBox.ItemsSource = defaultTaskList.Tasks;
            defaultTaskList.Tasks.RemoveAt(taskListBox.SelectedIndex);
        }

        private void SaveEditedTask_Click(object sender, RoutedEventArgs e)
        {
            TriggerButtonVisibility(true, true, false, true); // Show the necessary buttons
            string edited = taskInput.Text.Trim();

            if (defaultTaskList.SelectedTask != null && edited.Length <= CHARACTERLIMIT)
            {
                defaultTaskList.SelectedTask.Name = edited; // Update the name of the selected task
                //defaultTaskList.SelectedTask.DueDate = changeDueDate.SelectedDate;
                //Priority selectedPriority = ((ComboBoxItem)changePriority.SelectedItem).Content;
                //defaultTaskList.SelectedTask.Priority = selectedPriority;
                //Status selectedStatus = ((ComboBoxItem)changeStatus.SelectedItem).Content;
                //defaultTaskList.SelectedTask.Status = selectedStatus;
            }
            else
            {
                MessageBox.Show("Invalid task name!");
            }

            if (!defaultTaskList.Tasks.Contains(defaultTaskList.SelectedTask))
            {
                defaultTaskList.Tasks.Add(defaultTaskList.SelectedTask);
                taskListBox.ItemsSource = defaultTaskList.Tasks;
            }
            taskInput.Clear();
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            TriggerButtonVisibility(true, true, true, false); // Show the necessary buttons
            defaultTaskList.RemoveTask(defaultTaskList.SelectedTask);
            taskListBox.ItemsSource = defaultTaskList.Tasks;
        }

        private void TriggerButtonVisibility(bool add, bool edit, bool remove, bool save)
        {
            addButton.Visibility = add ? Visibility.Visible : Visibility.Collapsed;
            editButton.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
            removeButton.Visibility = remove ? Visibility.Visible : Visibility.Collapsed;
            saveButton.Visibility = save ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SortByDueDate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Priority_Select(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Status_Select(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}