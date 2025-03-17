using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;
using TaskManagerApp.TasksBenefits;

namespace TaskManagerApp.TaskList
{
    [Serializable]
    [XmlType("TaskList")]
    public class TaskList : INotifyPropertyChanged
    {

        [XmlArray("Tasks")]
        [XmlArrayItem("Task")]
        public List<Task> TasksList { get; set; }


        public ObservableCollection<Task> tasks { get; set; }

        private string _name;

        [XmlElement("Name")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [XmlElement("SizeOfTheList")]
        public int SizeOfTheList => tasks.Count;

        // Make PropertyChanged public
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskList(string listName)
        {
            Name = listName;
            tasks = new ObservableCollection<Task>();
        }

        public TaskList()
        {
            Name = "Unknown";
            tasks = new ObservableCollection<Task>();
        }

        public void AddTask(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            tasks.Add(task);
            OnPropertyChanged(nameof(SizeOfTheList)); // Notify that SizeOfTheList has changed
        }

        public void RemoveTask(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            tasks.Remove(task);
            OnPropertyChanged(nameof(SizeOfTheList)); // Notify that SizeOfTheList has changed
        }

        public void UpdateTask(Task specificTask)
        {
            var existingTask = this.tasks.FirstOrDefault(t => t.Id == specificTask.Id);
            if (existingTask != null)
            {
                existingTask.EditTask(
                    specificTask.Name,
                    specificTask.Description,
                    specificTask.DueDateTime,
                    specificTask.Priority,
                    specificTask.Status);

                OnPropertyChanged(nameof(SizeOfTheList));
            }
        }

        public override string? ToString()
        {
            return this.Name;
        }
    }
}