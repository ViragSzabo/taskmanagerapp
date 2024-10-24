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
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority)); 

            // Populate the Status ComboBox
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(Status)); 
        }

        public void LoadTaskDetails(Task task)
        {
            Task = task;

            // Load task details into the UI controls
            TaskNameTextBox.Text = Task.Name;
            TaskDescriptionTextBox.Text = Task.Description;
            TaskDueDatePicker.SelectedDate = Task.DueDateTime;
            PriorityComboBox.SelectedItem = Task.Priority; 
            StatusComboBox.SelectedItem = Task.Status; 
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs here if needed

            // Update Task properties from UI controls
            Task.Name = TaskNameTextBox.Text;
            Task.Description = TaskDescriptionTextBox.Text;
            Task.DueDateTime = TaskDueDatePicker.SelectedDate ?? DateTime.Now;

            // Map selected priority
            Task.Priority = (Priority)PriorityComboBox.SelectedItem; 

            // Map selected status
            Task.Status = (Status)StatusComboBox.SelectedItem; 

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Just close without saving
        }
    }
}