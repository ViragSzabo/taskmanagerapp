using System.ComponentModel;

namespace TaskManagerApp
{
    public enum Priority
    {
        [DisplayName("Low Priority")]
        Low,
        [DisplayName("Medium Priority")]
        Medium,
        [DisplayName("High Priority")]
        High
    }
}
