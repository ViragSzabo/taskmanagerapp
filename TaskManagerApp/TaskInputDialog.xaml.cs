using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskManagerApp
{
    public partial class TaskInputDialog : Window
    {
        // Property to retrieve the created Task object
        public Task Task { get; private set; }

        // Constructor initializes the dialog
        public TaskInputDialog()
        {
            InitializeComponent();
        }

        // Click event handler for the "Add Task" button
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Task = new Task("", "", DateTime.Now);
            // Validate input before closing
            if (ValidateInputs())
            {
                Task.Name = TaskNameTextBox.Text.Trim();
                Task.Description = TaskDescriptionTextBox.Text.Trim();
                Task.DueDateTime = DueDatePicker.SelectedDate ?? DateTime.Now;

                DialogResult = true;
                Close();
            }
        }

        // Validates the input fields
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(TaskNameTextBox.Text))
            {
                MessageBox.Show("Please enter a task name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (DueDatePicker.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("The due date cannot be in the past.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // All inputs are valid
        }
    }
}