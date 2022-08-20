namespace ToDoAppSharedModels.Requests
{
    public class ChangeToDoEntryStateDTO
    {
        public Dictionary<int, bool> EntriesIdsAndStates { get; set; }
    }
}
