using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TaskManagerApp.TasksBenefits
{
    [Serializable] // To be compatible with XML serialization
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
            Priority = Priority.Medium;
            Status = Status.InProgress;
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
                // Simulate some delay or heavy work
                System.Threading.Tasks.Task.Delay(1000).Wait();

                // Update fields within the task
                Name = updatedName;
                Description = updatedDescription;

                if (updatedDueDateTime.HasValue)
                {
                    DueDateTime = updatedDueDateTime.Value;
                }

                Priority = updatedPriority;
                Status = updatedStatus;
            });

            // Notify property change after the task edit completes
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(DueDateTime));
            OnPropertyChanged(nameof(Priority));
            OnPropertyChanged(nameof(Status));
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