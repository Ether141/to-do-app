namespace ToDoAppSharedModels.Requests
{
    public class UserRegisterDTO
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
