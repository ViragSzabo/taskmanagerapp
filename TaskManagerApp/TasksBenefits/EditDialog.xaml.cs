using System;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerApp.TasksBenefits;
using Task = TaskManagerApp.TasksBenefits.Task;

namespace TaskManagerApp
{
    public partial class EditDialog : Window
    {
        public Task Task { get; set; }

        public EditDialog()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        private void LoadComboBoxes()
        {
            // Populate the Priority ComboBox
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority)); // Assuming Priority is an enum

            // Populate the Status ComboBox
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(TaskStatus)); // Assuming TaskStatus is an enum
        }

        public void LoadTaskDetails(Task task)
        {
            Task = task;

            // Load task details into the UI controls
            TaskNameTextBox.Text = Task.Name;
            TaskDescriptionTextBox.Text = Task.Description;
            TaskDueDatePicker.SelectedDate = Task.DueDateTime;
            PriorityComboBox.SelectedItem = Task.Priority; // Set the selected item directly
            StatusComboBox.SelectedItem = Task.Status; // Set the selected item directly
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs here if needed

            // Update Task properties from UI controls
            Task.Name = TaskNameTextBox.Text;
            Task.Description = TaskDescriptionTextBox.Text;
            Task.DueDateTime = TaskDueDatePicker.SelectedDate ?? DateTime.Now; // Default to now if no date is selected

            // Map selected priority
            Task.Priority = (Priority)PriorityComboBox.SelectedItem; // Directly cast the selected item

            // Map selected status
            Task.Status = (Status)StatusComboBox.SelectedItem; // Directly cast the selected item

            DialogResult = true; // Indicate that the dialog was successful
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Just close without saving
        }
    }
}