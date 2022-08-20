using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoAppSharedModels.Common
{
    [Table("todoentry")]
    public class ToDoEntry
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
        public int ToDoListId { get; set; }

        public const int MaxEntryNameLength = 128;
    }
}
