using RestSharp;
using System.Threading.Tasks;
using ToDoAppSharedModels.Requests;
using ToDoAppSharedModels.Responses;

namespace ToDoAppClient.Core.Main
{
    public class AccountAPIProvider : IAPIRequestsProvider
    {
        public APIClientHandler ClientHandler { get; init; }
        public RestClient Client => ClientHandler.Client;

        public AccountAPIProvider(APIClientHandler clientHandler) => ClientHandler = clientHandler;

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

        public async Task<UserInfoResult?> GetUserInfo()
        {
            RestRequest request = new RestRequest("user/getuser", Method.Post);
            APIClientHandler.AddTokenToRequest(request, App.Instance.SessionManager.GetAndCastCookie<string>("Token"));
            UserInfoResult? result = await ClientHandler.HandleTokenRefreshing(request, async request => await Client.PostAsync<UserInfoResult?>(request));
            return result;
        }

        public async Task<RestResponse> ValidateToken(string token)
        {
            RestRequest request = new RestRequest("user/validateToken", Method.Post);
            APIClientHandler.AddTokenToRequest(request, token);
            return await Client.ExecutePostAsync(request);
        }

        public async Task<TokenResult?> TryRefreshToken()
        {
            RestRequest request = new RestRequest("user/refresh", Method.Post);
            TokenRefreshRequestDTO dto = new TokenRefreshRequestDTO()
            {
                Token = App.Instance.SessionManager.Token,
                RefreshToken = App.Instance.SessionManager.RefreshToken
            };
            request.AddBody(dto);
            return await Client.PostAsync<TokenResult?>(request);
        }

    }
}
