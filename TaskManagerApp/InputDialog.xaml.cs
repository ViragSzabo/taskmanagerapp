using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskManagerApp
{
    public partial class InputDialog : Window
    {
        public string TaskName => TaskNameTextBox.Text;

        public InputDialog()
        {
            InitializeComponent();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}