using System.Globalization;
using System.Runtime.InteropServices;

namespace ToDoAppClient.Utilities
{
    public static class SystemUtilities
    {
        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        private static extern bool ShouldSystemUseDarkMode();

        /// <summary>
        /// Returns true if the applications use a dark theme.
        /// </summary>
        public static bool IsDarkTheme()
        {
            bool isDarkMode = true;

            try
            {
                isDarkMode = ShouldSystemUseDarkMode();
            }
            catch { }

            return isDarkMode;
        }

        /// <summary>
        /// Returns the language that is currently used on the system.
        /// </summary>
        public static string LanguageBasedOnSystemLanguage()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;

            if (ci.Name.ToLower() == "pl-pl")
                return "pl";
            else
                return "en";
        }
    }
}
