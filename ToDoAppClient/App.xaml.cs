using System;
using System.Windows;
using ToDoAppClient.Core.Settings;
using ToDoAppClient.Core.Themes;

namespace ToDoAppClient
{
    public partial class App : Application
    {
        public ThemesManager ThemesManager { get; }
        public ApplicationSettingsManager ApplicationSettings { get; }

        public static App Instance => (App)Current;

        public App()
        {
            ThemesManager = new ThemesManager(this);
            ApplicationSettings = new ApplicationSettingsManager(this);
        }
    }
}
