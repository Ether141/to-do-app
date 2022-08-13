using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Models;
using ToDoAppSharedModels.Responses;

namespace ToDoAppClient.Pages
{
    public partial class StartingPage : Page
    {
        public StartingPage()
        {
            InitializeComponent();
            CheckLoginStatus();
        }

        private async void CheckLoginStatus()
        {
            bool hasConnection = await App.Instance.ClientHandler.HasConnectionWithServer();

            if (!hasConnection)
            {
                DisableMenu();
                NormalStartingPage();
                return;
            }

            if (App.Instance.SessionManager.CookieExists("Token") && App.Instance.SessionManager.CookieExists("RefreshToken"))
            {
                UserInfoResult? userInfo = null;

                try
                {
                    userInfo = await App.Instance.ClientHandler.AccountRequestsProvider.GetUserInfo();
                }
                catch { }

                if (userInfo != null)
                {
                    User user = new User(userInfo);
                    ReloginUser(user, App.Instance.SessionManager.GetAndCastCookie<string>("Token")!, App.Instance.SessionManager.GetAndCastCookie<string>("RefreshToken")!);
                    return;
                }
                else
                {
                    TokenResult? tokens = null;

                    try
                    {
                        tokens = await App.Instance.ClientHandler.AccountRequestsProvider.TryRefreshToken();
                    }
                    catch { }

                    if (tokens != null)
                    {
                        userInfo = await App.Instance.ClientHandler.AccountRequestsProvider.GetUserInfo();
                        User user = new User(userInfo!);
                        ReloginUser(user, tokens.Token, tokens.RefreshToken);
                        return;
                    }
                }
            }

            NormalStartingPage();
        }

        private void ReloginUser(User user, string token, string refreshToken)
        {
            App.Instance.SessionManager.Login(user, token, refreshToken);
            MainWindow.Instance.OpenPage(MainWindow.MainPage);
        }

        private void DisableMenu()
        {
            signInBtn.IsEnabled = false;
            signUpBtn.IsEnabled = false;
            connectionError.Visibility = Visibility.Visible;
        }

        private void NormalStartingPage()
        {
            App.Instance.SessionManager.RemoveCookie("Token");
            App.Instance.SessionManager.RemoveCookie("RefreshToken");
            App.Instance.SessionManager.SaveSessionFile();
            loading.Visibility = Visibility.Collapsed;
            menu.Visibility = Visibility.Visible;
        }

        private void SignInButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.SignInPage);

        private void SignUpButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.SignUpPage);

        private void CloseButtonClick(object sender, RoutedEventArgs e) => App.Instance.Shutdown(0);
    }
}
