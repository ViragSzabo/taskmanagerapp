using System;
using System.Windows;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public Task TaskToEdit { get; private set; } // To hold the task being edited

        // Constructor for adding a new task
        public AddTaskWindow()
        {
            InitializeComponent();
            // Populate Priority and Status ComboBoxes
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(Status));
        }

        // Constructor for editing an existing task
        public AddTaskWindow(Task task) : this() // Call the parameterless constructor
        {
            TaskToEdit = task;
            // Populate fields with task data
            TaskNameTextBox.Text = task.Name;
            TaskDescriptionTextBox.Text = task.Description;
            PriorityComboBox.SelectedItem = task.Priority;
            StatusComboBox.SelectedItem = task.Status;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(TaskNameTextBox.Text))
            {
                MessageBox.Show("Task name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PriorityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a priority.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TaskToEdit != null) // Editing an existing task
            {
                // Update the existing task
                TaskToEdit.Name = TaskNameTextBox.Text;
                TaskToEdit.Description = TaskDescriptionTextBox.Text;
                TaskToEdit.Priority = (Priority)PriorityComboBox.SelectedItem;
                TaskToEdit.Status = (Status)StatusComboBox.SelectedItem;
            }
            else // Creating a new task
            {
                // Create a new task using the constructor
                TaskToEdit = new Task(
                    TaskNameTextBox.Text,
                    TaskDescriptionTextBox.Text,
                    DateTime.Now
                )
                {
                    Priority = (Priority)PriorityComboBox.SelectedItem,
                    Status = (Status)StatusComboBox.SelectedItem
                };
            }

            DialogResult = true; // Indicate that the dialog was successful
            Close(); // Close the window
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Close the window without adding a task
        }
    }
}