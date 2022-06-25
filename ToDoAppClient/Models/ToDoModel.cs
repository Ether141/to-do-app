using System.Linq;
using System.Collections.Generic;
using System;

namespace ToDoAppClient.Models
{
    public class ToDoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDoEntry> ToDoEntries { get; set; } = new List<ToDoEntry>();
        public int DoneEntriesCount => ToDoEntries.Count(entry => entry.IsDone);

        public const int MaxListNameLength = 46;

        public ToDoModel(int id, string name)
        {
            Name = name;
            Id = id;
        }
    }

    public class ToDoEntry
    {
        public int Id { get; private set; }
        public string Text { get; set; } = "New ToDo entry";
        public bool IsDone { get; set; } = false;

        public const int MaxEntryNameLength = 128;

        public ToDoEntry(string text, bool isDone)
        {
            Id = new Random().Next();
            Text = text;
            IsDone = isDone;
        }
    }
}
