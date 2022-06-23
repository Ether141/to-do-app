using System;
using System.Windows;
using ToDoAppClient.Utilities;

namespace ToDoAppClient.Core.Themes
{
    public class ThemesManager
    {
        public readonly Application Parent;

        public const string LightThemeSource = "/Resources/Styles/LightTheme.xaml";
        public const string DarkThemeSource = "/Resources/Styles/DarkTheme.xaml";

        public const int ThemeDictionaryIndex = 0;

        public ThemesManager(Application parent) => Parent = parent;

        public void ChangeTheme(Themes theme)
        {
            Uri themeSource = new (GetThemeSource(theme), UriKind.Relative);
            Parent.Resources.MergedDictionaries[ThemeDictionaryIndex].Source = themeSource;
        }

        public static Themes GetSystemTheme() => SystemUtilities.IsDarkTheme() ? Themes.Dark : Themes.Light;

        private static string GetThemeSource(Themes theme)
        {
            return theme switch
            {
                Themes.Dark => DarkThemeSource,
                Themes.Light => LightThemeSource,
                _ => DarkThemeSource
            };
        }
    }
}
