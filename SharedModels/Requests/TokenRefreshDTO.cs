namespace ToDoAppSharedModels.Requests
{
    public class TokenRefreshRequestDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
