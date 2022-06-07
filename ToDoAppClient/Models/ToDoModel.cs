using System.Linq;
using System.Collections.Generic;

namespace ToDoAppClient.Models
{
    public class ToDoModel
    {
        public List<ToDoEntry> ToDoEntries { get; private set; } = new List<ToDoEntry>();
        public int DoneEntriesCount => ToDoEntries.Count(entry => entry.IsDone);
    }

    public struct ToDoEntry
    {
        public int Id { get; set; } = 0;
        public string Text { get; set; } = "New ToDo entry";
        public bool IsDone { get; set; } = false;

        public ToDoEntry(int id, string text, bool isDone)
        {
            Id = id;
            Text = text;
            IsDone = isDone;
        }
    }
}
