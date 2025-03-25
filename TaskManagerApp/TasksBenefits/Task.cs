using System;
using System.Xml.Serialization;

namespace TaskManagerApp.TasksBenefits
{
    [Serializable]
    [XmlRoot("Task")]
    public class Task
    {
        [XmlElement("ID")]
        public Guid Id { get; set; }

        [XmlElement("Title")]
        public string? Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("DueDate")]
        public DateTime DueDateTime { get; set; }

        [XmlElement("Priority")]
        public Priority Priority { get; set; }

        [XmlElement("Status")]
        public Status Status { get; set; }

        [XmlElement("CreatedDate")]
        public DateTime CreatedDateTime { get; set; }

        [XmlElement("LastUpdatedDate")]
        public DateTime? LastUpdatedDateTime { get; set; }

        public Task()
        {
            Name = "Untitled";
            Description = "Undefined";
        }

        public Task(string name, string description, DateTime dueDateTime)
        {
            this.Id = Guid.NewGuid();
            Name = name;
            Description = description;
            DueDateTime = dueDateTime;
            Priority = Priority.Medium;
            Status = Status.InProgress;
            CreatedDateTime = DateTime.Now;
        }

        public void EditTask(string updatedName, string updatedDescription, DateTime? updatedDueDateTime, Priority updatedPriority, Status updatedStatus)
        {
            Name = updatedName ?? "Untitled";
            Description = updatedDescription;

            if (updatedDueDateTime.HasValue)
            {
                DueDateTime = updatedDueDateTime.Value;
            }

            Priority = updatedPriority;
            Status = updatedStatus;
            LastUpdatedDateTime = DateTime.Now;
        }

        public void MarkAsComplete() => Status = Status.Completed;

        public override string ToString() => $"{Name} - {Description} (Due: {DueDateTime:yyyy-MM-dd HH:mm}, Priority: {Priority}, Status: {Status})";
    }
}