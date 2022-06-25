using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ToDoAppClient.Core.Main
{
    public class SessionManager
    {
        public static string SessionFilePath => Path.Combine(App.ApplicationLocalDir, "session");
        public static bool SessionFileExists => File.Exists(SessionFilePath);

        public App Parent { get; }

        private readonly Dictionary<string, object> allCookies = new Dictionary<string, object>();

        public SessionManager(App parent) => Parent = parent;

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
    }
}
