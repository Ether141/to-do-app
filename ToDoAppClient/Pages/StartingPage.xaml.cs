using RestSharp;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using ToDoAppClient.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            if (App.Instance.SessionManager.CookieExists("Token") && App.Instance.SessionManager.CookieExists("RefreshToken"))
            {
                RestResponse response = await App.Instance.APIClient.ValidateToken();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Dictionary<string, string> content = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content!)!;
                    User user = new User(int.Parse(content["Id"]), content["Nickname"], content["Email"]);
                    ReloginUser(user);
                }
                else
                {
                    App.Instance.SessionManager.RemoveCookie("Token");
                    App.Instance.SessionManager.RemoveCookie("RefreshToken");
                    NormalStartingPage();
                }
            }
            else
            {
                NormalStartingPage();
            }
        }

        private void ReloginUser(User user)
        {
            string token = App.Instance.SessionManager.GetAndCastCookie<string>("Token")!;
            string refreshToken = App.Instance.SessionManager.GetAndCastCookie<string>("RefreshToken")!;
            App.Instance.SessionManager.Login(user, token, refreshToken);
            MainWindow.Instance.OpenPage(MainWindow.MainPage);
        }

        private void NormalStartingPage()
        {
            loading.Visibility = Visibility.Collapsed;
            menu.Visibility = Visibility.Visible;
        }

        private void SignInButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.SignInPage);

        private void SignUpButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.SignUpPage);

        private void CloseButtonClick(object sender, RoutedEventArgs e) => App.Instance.Shutdown(0);
    }
}
