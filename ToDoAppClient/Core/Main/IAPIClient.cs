using RestSharp;

namespace ToDoAppClient.Core.Main
{
    public interface IAPIClient
    {
        RestClient Client { get; }
    }
}
