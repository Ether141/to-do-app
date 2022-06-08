using System.Linq;
using System.Collections.Generic;
using System;

namespace ToDoAppClient.Models
{
    public class ToDoModel
    {
        public string Name { get; set; }
        public List<ToDoEntry> ToDoEntries { get; private set; } = new List<ToDoEntry>();
        public int DoneEntriesCount => ToDoEntries.Count(entry => entry.IsDone);

        public ToDoModel(string name) => Name = name;
    }

    public class ToDoEntry
    {
        public int Id { get; private set; }
        public string Text { get; set; } = "New ToDo entry";
        public bool IsDone { get; set; } = false;

        public ToDoEntry(string text, bool isDone)
        {
            Id = new Random().Next();
            Text = text;
            IsDone = isDone;
        }
    }
}
