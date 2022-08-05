namespace ToDoAppServer.Models
{
    public class TokenRefreshRequestDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
