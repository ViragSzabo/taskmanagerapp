using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace TaskManagerApp.TasksBenefits
{
    [Serializable] // To be compatible with XML serialization
    public class Task : INotifyPropertyChanged
    {
        [XmlElement("Title")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("DueDate")]
        public DateTime DueDateTime { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        // Additional properties
        public DateTime CreatedDateTime { get; private set; }
        public DateTime? LastUpdatedDateTime { get; private set; }
        public List<string> Comments { get; set; } = new List<string>();


        public Task(string Name, string Description, DateTime DueDateTime)
        {
            this.Name = Name;
            this.Description = Description;
            this.DueDateTime = DueDateTime;
            Priority = Priority.Medium;
            Status = Status.InProgress;
            CreatedDateTime = DateTime.Now;
        }

        // Mark the task as complete
        public void MarkAsComplete()
        {
            Status = Status.Completed;
        }

        // Edit the task asynchronously using Task.Run()
        public async System.Threading.Tasks.Task EditTask(
            string updatedName,
            string updatedDescription,
            DateTime? updatedDueDateTime,
            Priority updatedPriority,
            Status updatedStatus)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                // Simulate work
                System.Threading.Tasks.Task.Delay(1000).Wait();
                Name = updatedName;
                Description = updatedDescription;

                if (updatedDueDateTime.HasValue)
                {
                    DueDateTime = updatedDueDateTime.Value;
                }

                Priority = updatedPriority;
                Status = updatedStatus;
                LastUpdatedDateTime = DateTime.Now; // Update last modified date
            });

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(DueDateTime));
            OnPropertyChanged(nameof(Priority));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(LastUpdatedDateTime));
        }

        // Property change event
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Optional method: Get display name for priority enum, for example
        public string GetDisplayName()
        {
            return typeof(Priority)
                .GetField(Priority.ToString())
                ?.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .FirstOrDefault() is DisplayNameAttribute displayName
                ? displayName.Name
                : Priority.ToString();
        }

        public override string ToString()
        {
            return $"{Name} - {Description} (Due: {DueDateTime}, Priority: {Priority}, Status: {Status})";
        }
    }
}