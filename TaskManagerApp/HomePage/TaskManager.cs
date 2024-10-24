using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace TaskManagerApp.HomePage
{
    public class TaskManager
    {
        private const string FilePath = "tasks.xml";
        public ObservableCollection<TaskList.TaskList>? TaskLists { get; set; } = new();

        public static void SaveTasks(TaskList.TaskList? taskList)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TaskList.TaskList));
            using StreamWriter writer = new StreamWriter(FilePath);
            serializer.Serialize(writer, taskList);
        }

        public static TaskList.TaskList? LoadTasks()
        {
            if (!File.Exists(FilePath))
            {
                return new TaskList.TaskList(); 
            }

            XmlSerializer serializer = new XmlSerializer(typeof(TaskList.TaskList));
            using (StreamReader reader = new StreamReader(FilePath))
            {
                return (TaskList.TaskList)serializer.Deserialize(reader)!;
            }
        }

        public void AddTaskList(TaskList.TaskList newList)
        {
            TaskLists?.Add(newList);
        }

        public void RemoveTaskList(TaskList.TaskList list)
        {
            TaskLists?.Remove(list);
        }
    }
}