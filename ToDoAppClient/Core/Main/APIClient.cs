using RestSharp;
using System;
using System.Threading.Tasks;
using ToDoAppSharedModels.Requests;

namespace ToDoAppClient.Core.Main
{
    public class APIClient : IDisposable
    {
        public RestClient Client { get; private set; }

        public const string ApiAddress = "http://localhost:5204";
        public const string ApiKeyHeader = "x-api-key";
        public const string ApiKey = "9SDHABN0DH902HH";

        public APIClient() => ConfigureClient();

        private void ConfigureClient()
        {
            RestClientOptions options = new RestClientOptions(ApiAddress)
            {
                ThrowOnAnyError = false,
                MaxTimeout = 5000
            };
            Client = new RestClient(options);
            Client.AddDefaultHeader(ApiKeyHeader, ApiKey);
        }

        public async Task<RestResponse> PostRegisterUser(UserRegisterDTO dto)
        {
            RestRequest request = new RestRequest("user/register", Method.Post);
            request.AddBody(dto);
            return await Client.ExecutePostAsync(request);
        }

        public async Task<RestResponse> PostLoginUser(UserLoginDTO dto)
        {
            RestRequest request = new RestRequest("user/login", Method.Post);
            request.AddBody(dto);
            return await Client.ExecutePostAsync(request);
        }

        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
