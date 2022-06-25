using System;
using System.Windows;
using ToDoAppClient.Utilities;

namespace ToDoAppClient.Core.Themes
{
    public class ThemesManager
    {
        public Application Parent { get; }

        public const string LightThemeSource = "/Resources/Styles/LightTheme.xaml";
        public const string DarkThemeSource = "/Resources/Styles/DarkTheme.xaml";

        public const int ThemeDictionaryIndex = 0;

        public ThemesManager(Application parent) => Parent = parent;

        public void ChangeTheme(Theme theme)
        {
            Uri themeSource = new (GetThemeSource(theme), UriKind.Relative);
            Parent.Resources.MergedDictionaries[ThemeDictionaryIndex].Source = themeSource;
        }

        private static string GetThemeSource(Theme theme)
        {
            return theme switch
            {
                Theme.Dark => DarkThemeSource,
                Theme.Light => LightThemeSource,
                Theme.FollowSystem => SystemUtilities.IsDarkTheme() ? DarkThemeSource : LightThemeSource,
                _ => DarkThemeSource
            };
        }
    }
}
