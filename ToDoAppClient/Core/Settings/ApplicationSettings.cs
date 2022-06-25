using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using ToDoAppClient.Core.Themes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoAppClient.Core.Settings
{
    public class ApplicationSettings : INotifyPropertyChanged
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Theme Theme
        {
            get => theme;
            set
            {
                theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Language Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        private Theme theme;
        private Language language;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
