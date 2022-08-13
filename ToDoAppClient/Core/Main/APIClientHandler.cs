using RestSharp;
using System;
using System.Threading.Tasks;

namespace ToDoAppClient.Core.Main
{
    public class APIClientHandler : IAPIClient, IDisposable
    {
        public RestClient Client { get; private set; }

        public AccountAPIProvider AccountRequestsProvider { get; private set; }

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
        }

        public async Task<bool> HasConnectionWithServer()
        {
            RestRequest request = new RestRequest("user", Method.Get);
            RestResponse response = await Client.ExecuteGetAsync(request);
            return !string.IsNullOrEmpty(response.Server);
        }

        public static void AddTokenToRequest(RestRequest request)
        {
            string? token = App.Instance.SessionManager.Token;

            if (token != null)
                request.AddHeader("Authorization", $"Bearer {token}");
        }

        public static void AddTokenToRequest(RestRequest request, string? token)
        {
            if (token != null)
                request.AddHeader("Authorization", $"Bearer {token}");
        }

        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
