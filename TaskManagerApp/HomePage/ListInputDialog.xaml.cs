using System.Windows;

namespace TaskManagerApp.HomePage
{
    public partial class ListInputDialog : Window
    {
        public string ListName { get; private set; } // Property to hold the list name
        public string messageOnListName = "Please enter a list name.";

        public ListInputDialog()
        {
            InitializeComponent();
        }

        // Click event handler for the "Add List" button
        private void AddListButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInputs())
            {
                ListName = ListNameTextBox.Text.Trim(); // Get the list name
                DialogResult = true; // Indicate that the dialog was successful
                Close(); // Close the dialog
            }
        }

        // Validates the input fields
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(ListNameTextBox.Text))
            {
                ShowErrorMessage(messageOnListName);
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