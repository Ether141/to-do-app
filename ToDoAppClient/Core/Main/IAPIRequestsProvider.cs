using RestSharp;

namespace ToDoAppClient.Core.Main
{
    public interface IAPIRequestsProvider
    {
        public APIClientHandler ClientHandler { get; }
        public RestClient Client { get; }
    }
}
