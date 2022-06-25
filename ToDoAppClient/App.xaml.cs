using System;
using System.IO;
using System.Windows;
using ToDoAppClient.Core.Main;
using ToDoAppClient.Core.Settings;
using ToDoAppClient.Core.Themes;
using ToDoAppClient.Resources.Strings;

namespace ToDoAppClient
{
    public partial class App : Application
    {
        public static string ApplicationLocalDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDoApp");
        public static bool ApplicationLocalDirExists => Directory.Exists(ApplicationLocalDir);

        public ThemesManager ThemesManager { get; }
        public SessionManager SessionManager { get; }
        public ApplicationSettingsManager ApplicationSettings { get; }

        public static App Instance => (App)Current;

        public App()
        {
            PrepareAppDir();

            ThemesManager = new ThemesManager(this);
            SessionManager = new SessionManager(this);
            ApplicationSettings = new ApplicationSettingsManager(this);
        }

        private static void PrepareAppDir()
        {
            try
            {
                if (!ApplicationLocalDirExists)
                    Directory.CreateDirectory(ApplicationLocalDir);
            }
            catch 
            {
                MessageBox.Show("Cannot create local data application directory.", Resource.error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
