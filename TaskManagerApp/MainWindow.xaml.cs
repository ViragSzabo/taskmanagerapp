using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManagerApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Tasks { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<string>();
            taskListBox.ItemsSource = Tasks;
        }
        
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string newTask = taskInput.Text.Trim();
            if(!string.IsNullOrEmpty(newTask) )
            {
                Tasks.Add(newTask);
                taskInput.Clear();
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if(taskListBox.SelectedIndex != -1)
            {
                // Hide the save button
                saveButton.Visibility = Visibility.Collapsed;

                // Show the edit button
                editButton.Visibility = Visibility.Visible;

                // Retrieve the selected task
                string selectedTask = Tasks[taskListBox.SelectedIndex];

                //Show the selected task in the taskInput Textbox for editing
                taskInput.Text = selectedTask;

                // Remove the selected task from the Tasks collection
                Tasks.RemoveAt(taskListBox.SelectedIndex);
            }
        }

        public void SaveEditedTask_Click(object sender, RoutedEventArgs e)
        {
            // Hide the edit button before
            editButton.Visibility = Visibility.Collapsed;

            // Show the save button
            saveButton.Visibility = Visibility.Visible;

            // Get the edited tasks from the task input textbox
            string editedTask = taskInput.Text.Trim();

            // Add the edited task back to the Tasks collection
            Tasks.Add(editedTask);

            // Clear the taskInput Textbox
            taskInput.Clear();

            // Hide the save button again
            saveButton.Visibility = Visibility.Collapsed;
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if(taskListBox.SelectedIndex >= 0)
            {
                Tasks.RemoveAt(taskListBox.SelectedIndex);
                taskInput.Clear();
            }
        }
    }
}
