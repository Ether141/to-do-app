using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoAppSharedModels.Common
{
    [Table("todolist")]
    public class ToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ToDoEntry> ToDoEntries { get; set; }
    }
}
