using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using ToDoAppClient.Core.ValidationRules;
using ToDoAppSharedModels.Requests;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using ToDoAppSharedModels.Results;
using ToDoAppClient.Models;

namespace ToDoAppClient.Pages
{
    public partial class SignInPage : Page
    {
        private readonly SignInLoginRule loginRule = new SignInLoginRule();
        private readonly SignInPasswordRule passwordRule = new SignInPasswordRule();

        public SignInPage()
        {
            InitializeComponent();
        }

        private async void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateForm();

            if (isValid)
            {
                form.Visibility = Visibility.Collapsed;
                loading.Visibility = Visibility.Visible;
                await SendLoginRequest();
            }
        }

        private async Task SendLoginRequest()
        {
            UserLoginDTO dto = new UserLoginDTO
            {
                Nickname = loginBox.Text,
                Password = passwordBox.Password
            };

            RestResponse response = await App.Instance.ClientHandler.AccountRequestsProvider.PostLoginUser(dto);
            HandleResponse(response);
        }

        private void HandleResponse(RestResponse response)
        {
            if (response.ResponseStatus == ResponseStatus.TimedOut || response.Content == null)
            {
                Error(ResponseCodeMapper.MapResponseStatus(response.ResponseStatus));
                return;
            }

            HttpStatusCode statusCode = response.StatusCode;
            Dictionary<string, string> content = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content)!;

            if (!content.ContainsKey("Code") || !content.ContainsKey("Message"))
            {
                Error($"{(int)statusCode}: {statusCode}");
                return;
            }

            LoginResult result = (LoginResult)int.Parse(content["Code"]);

            if (statusCode == HttpStatusCode.OK && (result == LoginResult.Success || result == LoginResult.SuccessShouldUpdatePassword))
            {
                Success(content);
            }
            else
            {
                Error(ResponseCodeMapper.MapLoginResult(result));
            }
        }

        private void Success(Dictionary<string, string> content)
        {
            string token = content["Token"];
            string refreshToken = content["RefreshToken"];
            User user = new User(int.Parse(content["Id"]), content["Nickname"], content["Email"]);

            App.Instance.SessionManager.Login(user, token, refreshToken);

            loading.Visibility = Visibility.Collapsed;
            MainWindow.Instance.OpenPage(MainWindow.MainPage, true);
        }

        private void Error(string msg)
        {
            loading.Visibility = Visibility.Collapsed;
            SetErrorLabelText(msg);
            form.Visibility = Visibility.Visible;
        }

        private bool ValidateForm()
        {
            ValidationResult loginNotEmptyResult = loginRule.Validate(loginBox.Text);
            ValidationResult passwordNotEmptyResult = passwordRule.Validate(passwordBox.Password);
            SetErrorLabelText(string.Empty);

            if (!loginNotEmptyResult.IsValid)
            {
                SetErrorLabelText((string)loginNotEmptyResult.ErrorContent);
                loginBox.BorderBrush = (SolidColorBrush)App.Instance.Resources["errorColor"];
            }
            else
            {
                loginBox.BorderBrush = (SolidColorBrush)App.Instance.Resources["thirdBackgroundBrush"];
            }

            if (!passwordNotEmptyResult.IsValid)
            {
                SetErrorLabelText((string)passwordNotEmptyResult.ErrorContent);
                passwordBox.BorderBrush = (SolidColorBrush)App.Instance.Resources["errorColor"];
            }
            else
            {
                passwordBox.BorderBrush = (SolidColorBrush)App.Instance.Resources["thirdBackgroundBrush"];
            }

            return loginNotEmptyResult.IsValid && passwordNotEmptyResult.IsValid;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.StartingPage);

        private void SetErrorLabelText(string text) => errorLabel.Content = text;
    }
}
