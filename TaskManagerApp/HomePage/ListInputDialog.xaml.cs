using System.Windows;

namespace TaskManagerApp.HomePage
{
    public partial class ListInputDialog : Window
    {
        public string? ListName { get; private set; } // Property to hold the list name
        public string MessageOnListName = "Please enter a list name.";

        public ListInputDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListNameTextBox.Focus();
        }

        private void AddListButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                this.ListName = ListNameTextBox.Text.Trim(); // Set the list name
                DialogResult = true; // Close the dialog
                this.ListNameTextBox.Clear(); // Clear the input field
                Close(); // Close the dialog
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(ListNameTextBox.Text))
            {
                ShowErrorMessage(MessageOnListName);
                return false; // Invalid input
            }

            return true; // All inputs are valid
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}