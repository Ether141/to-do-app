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
        private readonly FirstNameRule firstNameRule = new FirstNameRule();
        private readonly LastNameRule lastNameRule = new LastNameRule();
        private readonly EmailRule emailRule = new EmailRule();
        private readonly PasswordRule passwordRule = new PasswordRule();

        public SignUpPage()
        {
            InitializeComponent();
        }

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
            ValidationResult firstNameValidationResult = firstNameRule.Validate(firstNameBox.Text);
            ValidationResult lastNameValidationResult = lastNameRule.Validate(lastNameBox.Text);
            ValidationResult emailValidationResult = emailRule.Validate(emailBox.Text);
            ValidationResult passwordValidationResult = passwordRule.Validate(passwordBox.Password);

            SetErrorLabelText(string.Empty);

            if (!loginValidationResult.IsValid)
            {
                SetErrorLabelText((string)loginValidationResult.ErrorContent);
                loginBox.BorderBrush = (SolidColorBrush)App.Current.Resources["errorColor"];
            }
            else
            {
                loginBox.BorderBrush = (SolidColorBrush)App.Current.Resources["thirdBackgroundBrush"];
            }

            if (!firstNameValidationResult.IsValid)
            {
                SetErrorLabelText((string)firstNameValidationResult.ErrorContent);
                firstNameBox.BorderBrush = (SolidColorBrush)App.Current.Resources["errorColor"];
            }
            else
            {
                firstNameBox.BorderBrush = (SolidColorBrush)App.Current.Resources["thirdBackgroundBrush"];
            }

            if (!lastNameValidationResult.IsValid)
            {
                SetErrorLabelText((string)lastNameValidationResult.ErrorContent);
                lastNameBox.BorderBrush = (SolidColorBrush)App.Current.Resources["errorColor"];
            }
            else
            {
                lastNameBox.BorderBrush = (SolidColorBrush)App.Current.Resources["thirdBackgroundBrush"];
            }

            if (!emailValidationResult.IsValid)
            {
                SetErrorLabelText((string)emailValidationResult.ErrorContent);
                emailBox.BorderBrush = (SolidColorBrush)App.Current.Resources["errorColor"];
            }
            else
            {
                emailBox.BorderBrush = (SolidColorBrush)App.Current.Resources["thirdBackgroundBrush"];
            }

            if (!passwordValidationResult.IsValid)
            {
                SetErrorLabelText((string)passwordValidationResult.ErrorContent);
                passwordBox.BorderBrush = (SolidColorBrush)App.Current.Resources["errorColor"];
            }
            else
            {
                passwordBox.BorderBrush = (SolidColorBrush)App.Current.Resources["thirdBackgroundBrush"];
            }

            if (passwordBox.Password != repeatPasswordBox.Password)
            {
                SetErrorLabelText(Resource.passwordsAreNotSame);
                repeatPasswordBox.BorderBrush = (SolidColorBrush)App.Current.Resources["errorColor"];
            }
            else
            {
                repeatPasswordBox.BorderBrush = (SolidColorBrush)App.Current.Resources["thirdBackgroundBrush"];
            }

            return loginValidationResult.IsValid
                && firstNameValidationResult.IsValid
                && lastNameValidationResult.IsValid
                && emailValidationResult.IsValid
                && passwordValidationResult.IsValid
                && passwordBox.Password == repeatPasswordBox.Password;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.StartingPage);

        private void SetErrorLabelText(string text) => errorLabel.Content = text;
    }
}
