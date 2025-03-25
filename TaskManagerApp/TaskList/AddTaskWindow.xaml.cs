using System;
using System.Windows;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    public partial class AddTaskWindow : Window
    {
        public Task? NewTask { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();

            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(Priority));
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(Status));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskNameTextBox.Text))
            {
                MessageBox.Show("Task name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PriorityComboBox.SelectedItem == null || StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a priority and status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewTask = new Task(
                TaskNameTextBox.Text,
                TaskDescriptionTextBox.Text,
                DateTime.Now)
            {
                Priority = (Priority)PriorityComboBox.SelectedItem,
                Status = (Status)StatusComboBox.SelectedItem
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}