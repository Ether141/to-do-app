using ToDoAppSharedModels.Common;

namespace ToDoAppSharedModels.Requests
{
    public class UpdateToDoListDTO
    {
        public ToDoList ToDoList { get; set; }
        public IEnumerable<ToDoEntry> ToDoEntries { get; set; }
    }
}
