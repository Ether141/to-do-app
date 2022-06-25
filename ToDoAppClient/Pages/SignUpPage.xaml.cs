using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using ToDoAppClient.Core.ValidationRules;
using ToDoAppClient.Resources.Strings;

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

        private void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateForm();

            if (isValid)
            {
                // TODO: Sign up
            }
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
