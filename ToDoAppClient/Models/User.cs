using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToDoAppSharedModels.Responses;

namespace ToDoAppClient.Models
{
    public class User : INotifyPropertyChanged
    {
        private int id;
        private string nickname;
        private string email;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                NotifyPropertyChanged();
            }
        }
        public string Email
        {
            get => email;
            set
            {
                email = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public User(int id, string nickname, string email)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
        }

        public User(UserInfoResult userInfo)
        {
            Id = userInfo.Id;
            Nickname = userInfo.Nickname;
            Email = userInfo.Email;
        }

        private void NotifyPropertyChanged([CallerMemberName] string? name = default) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
