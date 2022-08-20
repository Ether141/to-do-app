﻿using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoAppSharedModels.Common;
using ToDoAppSharedModels.Requests;

namespace ToDoAppClient.Core.Main
{
    public class ToDoListsAPIProvider : IAPIRequestsProvider
    {
        public APIClientHandler ClientHandler { get; private set; }
        public RestClient Client => ClientHandler.Client;

        public ToDoListsAPIProvider(APIClientHandler clientHandler) => ClientHandler = clientHandler;

        public async Task<RestResponse<IEnumerable<ToDoList>?>> GetToDoLists()
        {
            RestRequest request = new RestRequest("todolist/all", Method.Get);
            APIClientHandler.AddTokenToRequest(request);
            RestResponse<IEnumerable<ToDoList>?> result = (await ClientHandler.HandleTokenRefreshing(request, async request => await Client.ExecuteGetAsync<IEnumerable<ToDoList>?>(request)))!;
            return result;
        }

        public async Task<RestResponse<ToDoList>> AddList(string listName)
        {
            RestRequest request = new RestRequest("todolist/add", Method.Post);
            request.AddBody(new AddToDoListDTO { ListName = listName });
            APIClientHandler.AddTokenToRequest(request);
            RestResponse<ToDoList> result = (await ClientHandler.HandleTokenRefreshing(request, async request => await Client.ExecutePostAsync<ToDoList>(request)))!;
            return result;
        }

        public async Task<RestResponse<ToDoEntry>> AddEntry(AddListEntryDTO dto)
        {
            RestRequest request = new RestRequest("todolist/addentry", Method.Post);
            request.AddBody(dto);
            APIClientHandler.AddTokenToRequest(request);
            RestResponse<ToDoEntry> result = (await ClientHandler.HandleTokenRefreshing(request, async request => await Client.ExecutePostAsync<ToDoEntry>(request)))!;
            return result;
        }

        public async Task<RestResponse> ChangeEntriesStates(ChangeToDoEntryStateDTO dto)
        {
            RestRequest request = new RestRequest("todolist/changeentriesstate", Method.Post);
            request.AddBody(dto);
            APIClientHandler.AddTokenToRequest(request);
            RestResponse result = (await ClientHandler.HandleTokenRefreshing(request, async request => await Client.ExecutePostAsync(request)))!;
            return result;
        }
    }
}
