﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoAppSharedModels.Common
{
    [Table("todolist")]
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<ToDoEntry> ToDoEntries { get; set; }

        public int DoneEntriesCount => ToDoEntries != null ? ToDoEntries.Count(entry => entry.IsDone) : 0;
        public static int MaxListNameLength = 46;
    }
}
