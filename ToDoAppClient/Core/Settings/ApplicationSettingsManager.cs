using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Threading;
using ToDoAppClient.Core.Themes;

namespace ToDoAppClient.Core.Settings
{
    public class ApplicationSettingsManager
    {
        public static string SettingsFilePath => Path.Combine(App.ApplicationLocalDir, "settings");
        public static bool SettingsFileExists => File.Exists(SettingsFilePath);

        public static ApplicationSettings DefaultSettings => new ApplicationSettings { Language = Language.System, Theme = Theme.FollowSystem };

        public App Parent { get; }
        public bool IsInitialized { get; private set; } = false;
        public ApplicationSettings Settings { get; private set; }

        public ApplicationSettingsManager(App app) => Parent = app;

        public AppSettingsInitializationResult Initialize()
        {
            if (IsInitialized)
                return AppSettingsInitializationResult.AlreadyInitialized;

            if (!App.ApplicationLocalDirExists)
                return AppSettingsInitializationResult.NotFoundDataDir;

            Settings = DefaultSettings;
            IsInitialized = true;

            LoadSettingsFromFile();
            ApplyCurrentSettings(true);
            SaveSettingsToFile();

            return AppSettingsInitializationResult.Success;
        }
        
        private bool LoadSettingsFromFile()
        {
            if (!SettingsFileExists || !IsInitialized)
                return false;

            string fileContent = File.ReadAllText(SettingsFilePath);
            ApplicationSettings? readSettings = null;

            try
            {
                readSettings = JsonConvert.DeserializeObject<ApplicationSettings>(fileContent);
            }
            catch 
            {
                SaveSettingsToFile();
            }

            if (readSettings != null)
                Settings = readSettings;

            return true;
        }

        public void ApplyCurrentSettings(bool changeLang = false)
        {
            if (!IsInitialized)
                return;

            Parent.ThemesManager.ChangeTheme(Settings.Theme);   

            if (changeLang)
            {
                string lang = Thread.CurrentThread.CurrentCulture.Name;

                if (Settings.Language == Language.System)
                {
                    if (lang == "pl-PL")
                        Settings.Language = Language.Polish;
                    else
                        Settings.Language = Language.English;
                }
                else
                {
                    lang = Settings.Language == Language.English ? "en-US" : "pl-PL";
                    CultureInfo culture = new CultureInfo(lang);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
        }

        public void ChangeSettings(ApplicationSettings newSettings)
        {
            Settings = newSettings;
            ApplyCurrentSettings();
            SaveSettingsToFile();
        }

        private void SaveSettingsToFile()
        {
            if (!IsInitialized)
                return;

            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using StreamWriter sw = new StreamWriter(SettingsFilePath, false);
                using JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, Settings);
            }
            catch { }
        }

    }
}
