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
            RestResponse<ToDoList> response = await App.Instance.ClientHandler.ToDoListsRequestsProvider.AddList(name);

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

        public void RemoveList(int listId)
        {
            if (!ContainsListWithId(listId))
                return;
            allLists.RemoveAll(x => x.Id == listId);
        }

        public bool ContainsListWithId(int id) => allLists.Any(list => list.Id == id);
    }
}
