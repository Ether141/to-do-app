using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using ToDoAppClient.Models;

namespace ToDoAppClient.Core.Main
{
    public class SessionManager
    {
        public static string SessionFilePath => Path.Combine(App.ApplicationLocalDir, "session");
        public static bool SessionFileExists => File.Exists(SessionFilePath);

        public App Parent { get; }

        private string? token;
        private string? refreshToken;

        public string? Token
        {
            get => token;
            private set
            {
                token = value;

                if (token != null)
                    AddOrOverwriteCookie("Token", token);
                else if (CookieExists("Token"))
                    RemoveCookie("Token");
            }
        }
        public string? RefreshToken
        {
            get => refreshToken;
            private set
            {
                refreshToken = value;

                if (refreshToken != null)
                    AddOrOverwriteCookie("RefreshToken", refreshToken);
                else if (CookieExists("RefreshToken"))
                    RemoveCookie("RefreshToken");
            }
        }
        public User? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        private Dictionary<string, object> allCookies = new Dictionary<string, object>();

        public SessionManager(App parent)
        {
            Parent = parent;
            LoadSessionFile();
        }

        public void Login(User user, string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
            CurrentUser = user;
            SaveSessionFile();
        }

        public void Logout()
        {
            Token = null;
            RefreshToken = null;
            CurrentUser = null;
            SaveSessionFile();
        }

        public void AddOrOverwriteCookie(string key, object value) => allCookies[key] = value;

        public object? GetCookie(string key)
        {
            if (CookieExists(key))
                return allCookies[key];
            return null;
        }

        public T? GetAndCastCookie<T>(string key)
        {
            object? cookie = GetCookie(key);
            if (cookie == null)
                return default;
            return (T)cookie;
        }

        public void RemoveCookie(string key)
        {
            if (CookieExists(key))
                allCookies.Remove(key);
        }

        public bool CookieExists(string key) => allCookies.ContainsKey(key);

        public void SaveSessionFile()
        {
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using StreamWriter sw = new StreamWriter(SessionFilePath, false);
                using JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, allCookies);
            }
            catch { }
        }

        public void LoadSessionFile()
        {
            if (!SessionFileExists)
                return;

            try
            {
                using StreamReader file = File.OpenText(SessionFilePath);
                JsonSerializer serializer = new JsonSerializer();
                Dictionary<string, object> backup = new Dictionary<string, object>();
                allCookies = (Dictionary<string, object>)serializer.Deserialize(file, typeof(Dictionary<string, object>))! ?? backup;

                if (CookieExists("Token"))
                    Token = GetAndCastCookie<string>("Token");

                if (CookieExists("RefreshToken"))
                    RefreshToken = GetAndCastCookie<string>("RefreshToken");
            }
            catch { }
        }
    }
}
