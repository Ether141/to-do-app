using RestSharp;
using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using ToDoAppSharedModels.Responses;
using ToDoAppClient.Models;
using System.Diagnostics;
using System.Collections.Generic;

namespace ToDoAppClient.Core.Main
{
    public class APIClientHandler : IAPIClient, IDisposable
    {
        public RestClient Client { get; private set; }

        public AccountAPIProvider AccountRequestsProvider { get; private set; }
        public ToDoListsAPIProvider ToDoListsRequestsProvider { get; private set; }

        public const string ApiAddress = "http://localhost:5204";
        public const string ApiKeyHeader = "x-api-key";
        public const string ApiKey = "9SDHABN0DH902HH";

        public APIClientHandler() => ConfigureClient();

        private void ConfigureClient()
        {
            RestClientOptions options = new RestClientOptions(ApiAddress)
            {
                ThrowOnAnyError = false,
                MaxTimeout = 5000
            };
            Client = new RestClient(options);
            Client.AddDefaultHeader(ApiKeyHeader, ApiKey);

            AccountRequestsProvider = new AccountAPIProvider(this);
            ToDoListsRequestsProvider = new ToDoListsAPIProvider(this);
        }

        public async Task<bool> HasConnectionWithServer()
        {
            RestRequest request = new RestRequest("user", Method.Get);
            RestResponse response = await Client.ExecuteGetAsync(request);
            return !string.IsNullOrEmpty(response.Server);
        }

        public async Task<T?> HandleTokenRefreshing<T>(RestRequest request, Func<RestRequest, Task<T?>> sendingMethod) where T : class
        {
            T? result = null;

            try
            {
                result = await sendingMethod.Invoke(request);

                if ((typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(RestResponse<>)) || (typeof(T) == typeof(RestResponse)))
                {
                    RestResponse response = (result as RestResponse)!;
                    
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new HttpRequestException(null, null, HttpStatusCode.Unauthorized);
                }
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TokenResult? tokens = null;

                    try
                    {
                        tokens = await App.Instance.ClientHandler.AccountRequestsProvider.TryRefreshToken();
                    }
                    catch { }

                    if (tokens != null)
                    {
                        string newToken = tokens.Token;

                        RestRequest userInfoRequest = new RestRequest("user/getuser", Method.Post);
                        AddTokenToRequest(userInfoRequest, newToken);
                        UserInfoResult? userInfoResult = await Client.PostAsync<UserInfoResult?>(userInfoRequest);
                        User user = new User(userInfoResult!);
                        App.Instance.SessionManager.Login(user, tokens.Token, tokens.RefreshToken);

                        AddTokenToRequest(request, newToken);
                        result = await sendingMethod.Invoke(request);
                    }
                }
            }

            return result;
        }

        public static void AddTokenToRequest(RestRequest request)
        {
            string? token = App.Instance.SessionManager.Token;

            if (token != null)
                request.AddOrUpdateHeader("Authorization", $"Bearer {token}");
        }

        public static void AddTokenToRequest(RestRequest request, string? token)
        {
            if (token != null)
                request.AddOrUpdateHeader("Authorization", $"Bearer {token}");
        }

        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
