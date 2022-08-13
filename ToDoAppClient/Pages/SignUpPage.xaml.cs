using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToDoAppClient.Core.ValidationRules;
using ToDoAppClient.Resources.Strings;
using ToDoAppSharedModels.Requests;
using ToDoAppSharedModels.Results;

namespace ToDoAppClient.Pages
{
    public partial class SignUpPage : Page
    {
        private readonly LoginRule loginRule = new LoginRule();
        private readonly EmailRule emailRule = new EmailRule();
        private readonly PasswordRule passwordRule = new PasswordRule();

        private readonly SolidColorBrush errorColor = (SolidColorBrush)App.Current.Resources["errorColor"];
        private readonly SolidColorBrush borderColor = (SolidColorBrush)App.Current.Resources["thirdBackgroundBrush"];

        public SignUpPage() => InitializeComponent();

        private async void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateForm();

            if (isValid)
            {
                form.Visibility = Visibility.Collapsed;
                loading.Visibility = Visibility.Visible;

                await SendRegisterRequest();
            }
        }

        private async Task SendRegisterRequest()
        {
            UserRegisterDTO user = new UserRegisterDTO
            {
                Nickname = loginBox.Text,
                Email = emailBox.Text,
                Password = passwordBox.Password,
                PasswordConfirm = repeatPasswordBox.Password
            };

            RestResponse response = await App.Instance.ClientHandler.AccountRequestsProvider.PostRegisterUser(user);
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

            RegisterResult result = (RegisterResult)int.Parse(content["Code"]);

            if (statusCode == HttpStatusCode.OK && result == RegisterResult.Success)
            {
                Success();
            }
            else
            {
                Error(ResponseCodeMapper.MapRegisterResult(result));
            }
        }

        private void Success()
        {
            loading.Visibility = Visibility.Collapsed;
            success.Visibility = Visibility.Visible;
        }

        private void Error(string msg)
        {
            loading.Visibility = Visibility.Collapsed;
            SetErrorLabelText(msg);
            form.Visibility = Visibility.Visible;
        }

        private void OpenSignInForm(object sender, RoutedEventArgs e)
        {
            success.Visibility = Visibility.Collapsed;
            MainWindow.Instance.OpenPage(MainWindow.SignInPage);
        }

        private bool ValidateForm()
        {
            ValidationResult loginValidationResult = loginRule.Validate(loginBox.Text);
            ValidationResult emailValidationResult = emailRule.Validate(emailBox.Text);
            ValidationResult passwordValidationResult = passwordRule.Validate(passwordBox.Password);

            SetErrorLabelText(string.Empty);

            if (passwordBox.Password != repeatPasswordBox.Password)
            {
                SetErrorLabelText(Resource.passwordsAreNotSame);
                repeatPasswordBox.BorderBrush = errorColor;
            }
            else
            {
                repeatPasswordBox.BorderBrush = borderColor;
            }

            if (!passwordValidationResult.IsValid)
            {
                SetErrorLabelText((string)passwordValidationResult.ErrorContent);
                passwordBox.BorderBrush = errorColor;
            }
            else
            {
                passwordBox.BorderBrush = borderColor;
            }

            if (!emailValidationResult.IsValid)
            {
                SetErrorLabelText((string)emailValidationResult.ErrorContent);
                emailBox.BorderBrush = errorColor;
            }
            else
            {
                emailBox.BorderBrush = borderColor;
            }

            if (!loginValidationResult.IsValid)
            {
                SetErrorLabelText((string)loginValidationResult.ErrorContent);
                loginBox.BorderBrush = errorColor;
            }
            else
            {
                loginBox.BorderBrush = borderColor;
            }

            return loginValidationResult.IsValid
                && emailValidationResult.IsValid
                && passwordValidationResult.IsValid
                && passwordBox.Password == repeatPasswordBox.Password;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.StartingPage);

        private void SetErrorLabelText(string text) => errorLabel.Content = text;
    }
}
