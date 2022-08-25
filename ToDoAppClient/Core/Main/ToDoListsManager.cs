using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ToDoAppSharedModels.Common;
using ToDoAppSharedModels.Requests;

namespace ToDoAppClient.Core.Main
{
    public class ToDoListsManager
    {
        private readonly List<ToDoList> allLists = new List<ToDoList>();

        public bool HasConnection { get; private set; } = false;
        public ToDoList[] AllLists => allLists.ToArray();

        public async Task<HttpStatusCode> DownloadLists()
        {
            RestResponse<IEnumerable<ToDoList>?> response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.GetToDoLists();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                IEnumerable<ToDoList>? lists = response.Data;
                HasConnection = true;

                if (lists != null)
                {
                    allLists.Clear();
                    allLists.AddRange(lists);
                }

                return HttpStatusCode.OK;
            }
            else
            {
                return response.StatusCode;
            }
        }

        public void ClearLists() => allLists.Clear();

        public async Task<RestResponse<ToDoList>> AddToDoList(string name)
        {
            RestResponse<ToDoList> response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.AddList(new AddToDoListDTO() { ListName = name });

            if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
            {
                ToDoList list = response.Data;
                allLists.Add(list);
            }

            return response;
        }

        public async Task<RestResponse<ToDoEntry>> AddToDoEntry(int listId, string entryName)
        {
            AddListEntryDTO dto = new AddListEntryDTO
            {
                ListId = listId,
                EntryName = entryName
            };

            RestResponse<ToDoEntry> response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.AddEntry(dto);

            if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
            {
                ToDoEntry entry = response.Data;
                allLists.First(l => l.Id == listId).ToDoEntries.Add(entry);
            }

            return response;
        }

        public async Task<RestResponse> ChangeToDoEntriesStates(IEnumerable<ToDoEntry> toDoEntries)
        {
            Dictionary<int, bool> newStates = new Dictionary<int, bool>();

            foreach (ToDoEntry entry in toDoEntries)
                newStates.Add(entry.Id, entry.IsDone);

            ChangeToDoEntryStateDTO dto = new ChangeToDoEntryStateDTO
            {
                EntriesIdsAndStates = newStates
            };

            RestResponse response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.ChangeEntriesStates(dto);
            return response;
        }

        public async Task<RestResponse> RemoveList(int listId)
        {
            RestResponse response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.RemoveList(new RemoveListDTO() { Id = listId });

            if (response.StatusCode == HttpStatusCode.OK)
                allLists.RemoveAll(l => l.Id == listId);

            return response;
        }

        public async Task<RestResponse> RenameList(int listId, string newName)
        {
            RenameListDTO dto = new RenameListDTO()
            {
                Id = listId,
                NewName = newName
            };

            RestResponse response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.RenameList(dto);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ToDoList list = allLists.First(l => l.Id == listId);
                list.Name = newName;
            }

            return response;
        }

        public async Task<RestResponse> RemoveEntry(int listId, int entryId)
        {
            RestResponse response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.RemoveEntry(new RemoveListEntryDTO() { Id = entryId });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ToDoList list = allLists.First(l => l.Id == listId);
                ToDoEntry entry = list.ToDoEntries.First(e => e.Id == entryId);
                list.ToDoEntries.Remove(entry);
            }

            return response;
        }

        public async Task<RestResponse> RenameEntry(int listId, int entryId, string newName)
        {
            RenameListEntryDTO dto = new RenameListEntryDTO()
            { 
                Id = entryId, 
                NewName = newName 
            };

            RestResponse response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.RenameEntry(dto);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                ToDoList list = allLists.First(l => l.Id == listId);
                ToDoEntry entry = list.ToDoEntries.First(e => e.Id == entryId);
                entry.Text = newName;
            }

            return response;
        }

        public bool ContainsListWithId(int id) => allLists.Any(list => list.Id == id);
    }
}
