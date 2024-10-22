using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TaskManagerApp
{
    public class Task : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDateTime { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }

        public Task(string Name, string Description, DateTime DueDateTime)
        {
            this.Name = Name;
            this.Description = Description;
            this.DueDateTime = DueDateTime;
            this.Priority = Priority.Low;
            this.Status = Status.InProgress;
        }

        public void MarkAsComplete()
        {
            this.Status = Status.Completed;
        }

        public void EditTask(string updatedName, 
            string updatedDescription, 
            DateTime? updatedDueDateTime,
            Priority updatedPriority,
            Status updatedStatus)
        {
            this.Name = updatedName;
            this.Description = updatedDescription;
            if (updatedDueDateTime.HasValue)
                this.DueDateTime = updatedDueDateTime.Value;
            this.Priority = updatedPriority;
            this.Status = updatedStatus;
        }

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

        public string GetDisplayName()
        {
            var displayName = typeof(Priority)
                .GetField(this.Priority.ToString())
                ?.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .FirstOrDefault() as DisplayNameAttribute;

            return displayName != null ? displayName.Name : this.Priority.ToString();
        }
    }
}
