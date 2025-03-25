using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using TaskManagerApp.HomePage;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    [Serializable]
    [XmlRoot("TaskList")]
    [XmlType("TaskList")]
    public class TaskList : INotifyPropertyChanged
    {
        [XmlArray("Tasks"), XmlArrayItem("Task")]
        public ObservableCollection<Task> Tasks { get; set; }

        private string? _name;

        [XmlElement("Name")]
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [XmlElement("SizeOfTheList")]
        public int SizeOfTheList => Tasks.Count;

        public event PropertyChangedEventHandler? PropertyChanged;

        public TaskList(string listName)
        {
            Name = listName;
            Tasks = new ObservableCollection<Task>();
        }

        public TaskList()
        {
            Name = "Unknown";
            Tasks = new ObservableCollection<Task>();
        }

        public void AddTask(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            Tasks.Add(task);
            OnPropertyChanged(nameof(SizeOfTheList));
            (Application.Current.MainWindow.DataContext as MainViewModel)?.UpdateHighPriorityTasks();
        }

        public void RemoveTask(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            Tasks.Remove(task);
            OnPropertyChanged(nameof(SizeOfTheList));
            (Application.Current.MainWindow.DataContext as MainViewModel)?.UpdateHighPriorityTasks();
        }

        public void UpdateTask(Task specificTask)
        {
            if (specificTask == null) throw new ArgumentNullException(nameof(specificTask));

            var existingTask = this.Tasks.FirstOrDefault(t => t.Id == specificTask.Id);
            if (existingTask != null)
            {
                existingTask.EditTask(
                    specificTask.Name ?? "Untitled",
                    specificTask.Description,
                    specificTask.DueDateTime,
                    specificTask.Priority,
                    specificTask.Status);

                OnPropertyChanged(nameof(Tasks));
            }
        }

        public override string? ToString() => this.Name;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}