using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Core.Settings;
using ToDoAppClient.Resources.Strings;
using ToDoAppClient.Windows;

namespace ToDoAppClient.Pages
{
    public partial class SettingsPage : Page
    {
        private ApplicationSettings CurrentSettings => (ApplicationSettings)DataContext;

        public SettingsPage()
        {
            if (!App.Instance.ApplicationSettings.IsInitialized)
                throw new SettingsManagerNotInitializedException("Cannot open SettingsPage because ApplicationSettingsManager is not initialized for this application.");

            DataContext = App.Instance.ApplicationSettings.Settings;
            InitializeComponent();

            CurrentSettings.PropertyChanged += CurrentSettingsPropertyChanged;
        }

        private void CurrentSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ApplyCurrentSettings();
            
            if (e.PropertyName == "Language")
                new MessagePopup(Resource.warning, Resource.restartToApplyChanges, MessagePopupIcon.Info, MessagePopupButtons.Ok).ShowDialog();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (MainPage.Current != null)
            {
                CurrentSettings.PropertyChanged -= CurrentSettingsPropertyChanged;
                MainPage.Current.Reopen();
            }
        }

        private void AccountSettingsBtnClick(object sender, RoutedEventArgs e)
        {
            accountSettingsView.Visibility = Visibility.Visible;
            appSettingsView.Visibility = Visibility.Collapsed;
        }

        private void AppSettingsBtnClick(object sender, RoutedEventArgs e)
        {
            accountSettingsView.Visibility = Visibility.Collapsed;
            appSettingsView.Visibility = Visibility.Visible;
        }

        private void ApplyCurrentSettings() => App.Instance.ApplicationSettings.ChangeSettings(CurrentSettings);
    }
}
