using System;
using System.Windows;

namespace TaskManagerApp
{
    public partial class EditDialog : Window
    {
        public string TaskName => TaskNameTextBox.Text;
        public string TaskDescription => TaskDescriptionTextBox.Text;
        public DateTime DueDate => DueDatePicker.SelectedDate ?? DateTime.Now;
        public Priority TaskPriority => (Priority)PriorityComboBox.SelectedItem;

        public EditDialog(Task task)
        {
            InitializeComponent();

            // Set initial values from the passed task
            TaskNameTextBox.Text = task.Name;
            TaskDescriptionTextBox.Text = task.Description;
            DueDatePicker.SelectedDate = task.DueDateTime;

            // Populate the ComboBox with enum values
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            PriorityComboBox.SelectedItem = task.Priority;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate task name
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                MessageBox.Show("Task name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true; // Set DialogResult to true to indicate success
            Close(); // Close the dialog
        }
    }
}