using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Core.Themes;
using ToDoAppClient.Pages;

namespace ToDoAppClient
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance => (MainWindow)App.Instance.MainWindow;

        public static StartingPage StartingPage => new StartingPage();
        public static SignInPage SignInPage => new SignInPage();
        public static SignUpPage SignUpPage => new SignUpPage();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.ThemesManager.ChangeTheme(Themes.Dark);
            OpenPage(StartingPage);
        }

        public void OpenPage(Page page) => pageDisplay.Content = page;
    }
}
