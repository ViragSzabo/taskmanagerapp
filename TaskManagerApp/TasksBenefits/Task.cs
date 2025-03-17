using System;
using System.Linq;
using System.Xml.Serialization;

namespace TaskManagerApp.TasksBenefits
{
    // To be compatible with XML serialization
    [Serializable]
    [XmlType("Task")]
    public class Task
    {

        [XmlElement("ID")]
        public string Id { get; set; }

        [XmlElement("Title")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("DueDate")]
        public DateTime DueDateTime { get; set; }

        [XmlElement("Priority")]
        public Priority Priority { get; set; }

        [XmlElement("Status")]
        public Status Status { get; set; }

        // Change the property to have a public setter
        [XmlElement("CreatedDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [XmlElement("LastUpdatedDateTime")]
        public DateTime? LastUpdatedDateTime { get; set; }

        public Task() { }

        // Constructor with parameters
        public Task(string name, string description, DateTime dueDateTime)
        {
            this.Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            DueDateTime = dueDateTime;
            Priority = Priority.Medium;
            Status = Status.InProgress;
            CreatedDateTime = DateTime.Now;
        }

        // Mark the task as complete
        public void MarkAsComplete()
        {
            Status = Status.Completed;
        }

        // Edit the task asynchronously
        public void EditTask(
            string updatedName,
            string updatedDescription,
            DateTime? updatedDueDateTime,
            Priority updatedPriority,
            Status updatedStatus)
        {
            Name = updatedName;
            Description = updatedDescription;

            if (updatedDueDateTime.HasValue)
            {
                DueDateTime = updatedDueDateTime.Value;
            }

            Priority = updatedPriority;
            Status = updatedStatus;
            LastUpdatedDateTime = DateTime.Now;
        }

        // Optional method: Get display name for priority enum
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