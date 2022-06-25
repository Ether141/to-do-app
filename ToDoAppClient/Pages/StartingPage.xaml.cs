using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoAppClient.Pages
{
    public partial class StartingPage : Page
    {
        public StartingPage() => InitializeComponent();

        private void SignInButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.SignInPage);

        private void SignUpButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.SignUpPage);

        private void CloseButtonClick(object sender, RoutedEventArgs e) => App.Instance.Shutdown(0);
    }
}
