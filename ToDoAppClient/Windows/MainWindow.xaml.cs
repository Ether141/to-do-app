using System.Windows;
using ToDoAppClient.Core.Themes;

namespace ToDoAppClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.ThemesManager.ChangeTheme(Themes.Dark);
        }
    }
}
