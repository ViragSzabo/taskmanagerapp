using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TaskManagerApp
{
    public partial class MainWindow : Window
    {
        private readonly int characterLimit = 15;
        private TaskManager taskManager;
        private TaskList defaultTaskList;
        private object priorityComboBox;
        private object datePicker;
        private object? datePickerControl;
        private object priorityComboBoxControl;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTaskManager();
            defaultTaskList = new TaskList();
            SetUpDefaultTask();
            SetInitialButtonVisibility();
        }

        private void SetInitialButtonVisibility()
        {
            addButton.Visibility = Visibility.Visible;
            editButton.Visibility = Visibility.Visible;
            removeButton.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Collapsed;
        }

        private void SetUpDefaultTask()
        {
            taskListBox.Items.Clear();
            //defaultTaskList.AddTask(new Task("default"));
            taskListBox.ItemsSource = defaultTaskList.Tasks;
        }

        private void InitializeTaskManager()
        {
            taskManager = new TaskManager();
            defaultTaskList = new TaskList();
            taskManager.AddTaskList(defaultTaskList);
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newName = taskInput.Text.Trim();

            if (!string.IsNullOrEmpty(newName) && newName.Length <= characterLimit)
            {
                Task newTask = new Task(newName);
                defaultTaskList.AddTask(newTask);
            }
            else
            {
                MessageBox.Show("Invalid task name!");
            }
            taskInput.Clear();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedIndex != -1)
            {
                // Get the selected task
                defaultTaskList.SelectedTask = defaultTaskList.Tasks[taskListBox.SelectedIndex];

                // Populate UI elements with task details
                taskInput.Text = defaultTaskList.SelectedTask.Name;
                datePicker = defaultTaskList.SelectedTask.DueDate;
                priorityComboBox = defaultTaskList.SelectedTask.Priority.ToString();
                statusComboBox.SelectedItem = defaultTaskList.SelectedTask.Status.ToString();

                // Show the appropriate buttons
                addButton.Visibility = Visibility.Collapsed;
                editButton.Visibility = Visibility.Collapsed;
                removeButton.Visibility = Visibility.Visible;
                saveButton.Visibility = Visibility.Visible;

                // Update datePicker and priorityComboBox fields with the current values of UI controls
                datePicker = datePickerControl;
                priorityComboBox = priorityComboBoxControl;
            }
        }

        private void SaveEditedTask_Click(object sender, RoutedEventArgs e)
        {
            // Check if a task is selected
            if (defaultTaskList.SelectedTask != null)
            {
                // Get the selected task
                Task selectedTask = defaultTaskList.SelectedTask;
                Task previous = selectedTask;

                // Update task properties with values from UI controls
                string editedName = taskInput.Text.Trim();
                selectedTask.Name = editedName;

                // Update DueDate if DatePicker has a selected date
                if (datePicker != null && datePicker is DatePicker datePickerControl)
                {
                    if (datePickerControl.SelectedDate.HasValue)
                    {
                        selectedTask.DueDate = datePickerControl.SelectedDate;
                    }
                    else
                    {
                        selectedTask.DueDate = DateTime.Today;
                    }
                }

                // Update Priority if ComboBox has a selected item
                if (priorityComboBox != null && priorityComboBox is ComboBox priorityComboBoxControl)
                {
                    if (priorityComboBoxControl.SelectedItem != null)
                    {
                        string selectedPriorityString = ((ComboBoxItem)priorityComboBoxControl.SelectedItem).Content.ToString();

                        // Parse the selected priority string to Priority enum
                        if (Enum.TryParse(selectedPriorityString, out Priority selectedPriority))
                        {
                            selectedTask.Priority = selectedPriority;
                        }
                        else
                        {
                            MessageBox.Show("Invalid priority selection!");
                        }
                    }
                    else
                    {
                        selectedTask.Priority = Priority.Medium;
                    }
                }

                // Update Status if ComboBox has a selected item
                if (statusComboBox.SelectedItem != null && statusComboBox is ComboBox statusComboBoxControl)
                {
                    if (statusComboBoxControl.SelectedItem != null)
                    {
                        string selectedStatusString = ((ComboBoxItem)statusComboBoxControl.SelectedItem).Content.ToString();

                        // Parse the selected status string to Status enum
                        if (Enum.TryParse(selectedStatusString, out Status selectedStatus))
                        {
                            selectedTask.Status = selectedStatus;
                        }
                        else
                        {
                            MessageBox.Show("Invalid status selection!");
                        }
                    }
                    else
                    {
                        selectedTask.Status = Status.NotStarted;
                    }
                }
                else
                {
                    selectedTask.Status = Status.NotStarted; // Set a default status if none is selected
                }

                // Update the task input field with the new task name
                taskInput.Text = selectedTask.Name;

                // Trim any leading or trailing whitespace from the task name in the input field
                taskInput.Text = taskInput.Text.Trim();

                // Update task in the tasklist
                defaultTaskList.EditTask(selectedTask.Name, selectedTask.DueDate, selectedTask.Priority, selectedTask.Status);
                defaultTaskList.AddTask(selectedTask);
                defaultTaskList.RemoveTask(previous);

                // Force the taskInput control to refresh its display
                taskInput.InvalidateVisual();

                // Clear the task input field to prepare for a new task name input
                taskInput.Clear();

                // Reset UI elements and button visibility
                SetInitialButtonVisibility();
            }
            else
            {
                MessageBox.Show("No task selected.");
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (defaultTaskList.SelectedTask != null)
            {
                defaultTaskList.RemoveTask(defaultTaskList.SelectedTask);
            }
        }

        private void SortByDueDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = dueDatePicker.SelectedDate;

            if (selectedDate.HasValue)
            {
                // Filter tasks based on the selected due date
                defaultTaskList.FilterTasksByDueDate(selectedDate.Value);
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            defaultTaskList.SortTasksByName();
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            ComboBoxItem selectedStatus = (ComboBoxItem)box.SelectedItem;

            if (selectedStatus != null)
            {
                string status = selectedStatus.Content.ToString();
                // Filter tasks based on the selected status
                if (!string.IsNullOrEmpty(status))
                {
                    defaultTaskList.FilterTasksByStatus((Status)Enum.Parse(typeof(Status), status));
                }
            }
        }

        private void Priority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            ComboBoxItem selectedPriority = (ComboBoxItem)box.SelectedItem;

            if (selectedPriority != null)
            {
                string priority = selectedPriority.Content.ToString();
                // Filter tasks based on the selected priority
                if (!string.IsNullOrEmpty(priority))
                {
                    defaultTaskList.FilterTasksByPriority((Priority)Enum.Parse(typeof(Priority), priority));
                }
            }
        }
    }

    // priority of a task
    public enum Priority
    {
        High,
        Medium,
        Low
    }

    // status of a task
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
            this.Tasks = new ObservableCollection<Task>();
        }

        // add a new task
        public void AddTask(Task newTask)
        {
            //this.Tasks.Add(newTask);
            // insert new task at the beginning of the list
            this.Tasks.Insert(0, newTask);
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
            this.Tasks.Remove(task);
        }

        // sort tasks by the name
        public void SortTasksByName() //Clear filters
        {
            List<Task> sortedTasks = Tasks.OrderBy(t => t.Name).ToList();
            Tasks.Clear();
            foreach(var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }

        // sort tasks by the due date
        public void SortTasksByDueDate()
        {
            List<Task> sortedTasks = Tasks.OrderBy(t => t.DueDate).ToList();
            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }

        // filter tasks by the status
        public void FilterTasksByStatus(Status status)
        {
            List<Task> filteredTasks = Tasks.Where(t => t.Status == status).ToList();
            Tasks.Clear();
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }

        // filter tasks by the priority
        public void FilterTasksByPriority(Priority priority)
        {
            List<Task> filteredTasks = Tasks.Where(t => t.Priority == priority).ToList();
            Tasks.Clear();
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }

        //Filter by the due date
        public void FilterTasksByDueDate(DateTime? selectedDate)
        {
            List<Task> filteredTasks = Tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == selectedDate.Value).ToList();
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }
    }

    // preset a task manager
    public class TaskManager
    {
        public ObservableCollection<TaskList> TaskLists { get; set; }

        public TaskManager()
        {
            this.TaskLists = new ObservableCollection<TaskList>();
        }

        public void AddTaskList(TaskList taskList)
        {
            this.TaskLists.Add(taskList);
        }

        public void RemoveTaskList(TaskList taskList)
        {
            this.TaskLists.Remove(taskList);
        }
    }
}