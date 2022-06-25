using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using ToDoAppClient.Core.Themes;

namespace ToDoAppClient.Core.Settings
{
    public class ApplicationSettingsManager
    {
        public static string LocalApplicationDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDoApp");
        public static string SettingsFilePath => Path.Combine(LocalApplicationDir, "settings.json");

        public static bool LocalApplicationDirExists => Directory.Exists(LocalApplicationDir);
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

            if (!CreateRequiredDirsFiles())
                return AppSettingsInitializationResult.CannotCreateDirOrFile;

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

            Settings = DefaultSettings;
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

            if (!SettingsFileExists)
            {
                if (!CreateRequiredDirsFiles())
                    return;
            }

            JsonSerializer serializer = new JsonSerializer();

            try
            {
                using StreamWriter sw = new StreamWriter(SettingsFilePath);
                using JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, Settings);
            }
            catch { }
        }

        private bool CreateRequiredDirsFiles()
        {
            try
            {
                if (!LocalApplicationDirExists)
                    Directory.CreateDirectory(LocalApplicationDir);
            }
            catch
            {
                return false;
            }

            try
            {
                if (!SettingsFileExists)
                {
                    using FileStream s = File.Open(SettingsFilePath, FileMode.Create);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
