using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Task(string name, string description, DateTime dueDateTime, Priority priority, Status status)
        {
            this.Name = name;
            this.Description = description;
            this.DueDateTime = dueDateTime;
            this.Priority = priority;
            this.Status = status;
        }

        public void markAsComplete()
        {
            this.Status = Status.Completed;
            Console.WriteLine($"Task '{Name}' marked as completed.");
        }

        public void EditTask(string updatedName, string updatedDescription, DateTime updatedDueDateTime, Priority updatedPriority)
        {
            this.Name = updatedName;
            this.Description = updatedDescription;
            this.DueDateTime = updatedDueDateTime;
            this.Priority = updatedPriority;
            Console.WriteLine($"Task '{Name}' updated.");
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
    }
}
