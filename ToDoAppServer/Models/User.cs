namespace ToDoAppServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }

        public User() { }

        public User(int id, string nickname, string email)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
        }

        public override string ToString()
        {
            return $"Id: {Id} Nickname: {Nickname} Email: {Email}";
        }
    }
}
