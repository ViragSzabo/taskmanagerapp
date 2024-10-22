using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskManagerApp
{
    public partial class EditDialog : Window
    {
        // Properties to hold the task details
        public required string TaskName { get; set; } = string.Empty; // Initialize with an empty string
        public required string TaskDescription { get; set; } = string.Empty; // Initialize with an empty string
        public DateTime? DueDate { get; private set; }
        public Priority TaskPriority { get; private set; }

        // Constructor initializes the dialog
        public EditDialog()
        {
            InitializeComponent();

            // Populate PriorityComboBox with enum values and display names
            foreach (Priority priority in Enum.GetValues(typeof(Priority)))
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = priority.GetDisplayName()
                };
                PriorityComboBox.Items.Add(comboBoxItem);
            }
        }

        // Constructor that sets task details
        public EditDialog(string selectedTaskName, string selectedTaskDescription) : this()
        {
            InitializeComponent();
            this.TaskName = selectedTaskName;  // Set the task name
            this.TaskDescription = selectedTaskDescription;  // Set the task description
        }

        // Method to populate the dialog with existing task details
        public void LoadTaskDetails(Task task)
        {
            TaskNameTextBox.Text = task.Name;
            TaskDescriptionTextBox.Text = task.Description;
            DueDatePicker.SelectedDate = task.DueDateTime;

            // Set selected item based on task priority
            foreach (var item in PriorityComboBox.Items)
            {
                if (item is not ComboBoxItem comboBoxItem ||
                    comboBoxItem.Content.ToString() != task.Priority.GetDisplayName()) continue;
                PriorityComboBox.SelectedItem = comboBoxItem;
                break; // Exit once the item is found
            }
        }

        // Click event handler for the "Save" button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input before saving
            try
            {
                if (string.IsNullOrWhiteSpace(TaskNameTextBox.Text))
                {
                    throw new ArgumentException("Task name cannot be empty.");
                }

                if (DueDatePicker.SelectedDate == null)
                {
                    throw new ArgumentException("Please select a due date.");
                }

                TaskName = TaskNameTextBox.Text.Trim();
                TaskDescription = TaskDescriptionTextBox.Text.Trim();
                DueDate = DueDatePicker.SelectedDate;

                // Ensure the selected item is a ComboBoxItem and safely get its content
                if (PriorityComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    if (Enum.TryParse(typeof(Priority), selectedItem.Content.ToString(), out var priority))
                    {
                        TaskPriority = (Priority)priority; // Cast if parsing was successful
                    }
                    else
                    {
                        throw new ArgumentException("Selected priority is invalid.");
                    }
                }
                else
                {
                    throw new ArgumentException("Please select a valid priority.");
                }

                DialogResult = true; // Set DialogResult to true to indicate success
                Close(); // Close the dialog
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}