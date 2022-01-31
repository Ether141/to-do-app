using System.Windows;
using ToDoAppClient.Core.Themes;

namespace ToDoAppClient
{
    public partial class App : Application
    {
        /// <summary>
        /// <see cref="Core.Themes.ThemesManager"/> connected with this application.
        /// </summary>
        public ThemesManager ThemesManager { get; }

        public static App Instance => (App)Current;

        public App() => ThemesManager = new ThemesManager(this);
    }
}
