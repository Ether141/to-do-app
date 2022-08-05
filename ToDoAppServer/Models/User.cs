using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoAppServer.Models
{
    [Table("User")]
    public class User
    {
        [Index]
        public int Id { get; set; }

        public string Nickname { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryDate { get; set; }

        public User() { }

        public User(int id, string nickname, string email, string password)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
            Password = password;
        }

        public static User Map(UserRegisterDTO dto, string password)
        {
            User user = new User
            {
                Nickname = dto.Nickname,
                Email = dto.Email,
                Password = password,
                RefreshToken = string.Empty,
                RefreshTokenExpiryDate = DateTime.MinValue
            };
            return user;
        }

        public override string ToString() => $"Id: {Id} Nickname: {Nickname} Email: {Email}";
    }
}
