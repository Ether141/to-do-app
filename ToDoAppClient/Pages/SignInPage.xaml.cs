using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using ToDoAppClient.Core.ValidationRules;

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

        private void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateForm();

            if (isValid)
            {
                // TODO: Sign in
            }
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
