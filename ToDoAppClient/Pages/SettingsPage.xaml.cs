using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoAppClient.Core.Themes;

namespace ToDoAppClient.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (MainPage.Current != null)
                MainPage.Current.Reopen();
        }

        private void ThemeRadioChecked(object sender, RoutedEventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            
            switch (btn.Name)
            {
                case "dark":
                    App.Instance.ThemesManager.ChangeTheme(Themes.Dark);
                    break;
                case "light":
                    App.Instance.ThemesManager.ChangeTheme(Themes.Light);
                    break;
                case "followSystem":
                default:
                    App.Instance.ThemesManager.ChangeTheme(ThemesManager.GetSystemTheme());
                    break;
            }
        }
    }
}
