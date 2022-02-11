using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ToDoAppClient.Models;

namespace ToDoAppClient.Pages
{
    public partial class MainPage : Page
    {
        private User currentUser;

        public MainPage()
        {
            InitializeComponent();
            currentUser = new User(1, "admin_1", "admin@gmail.com");
            usernameLabel.DataContext = currentUser;
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenPage(MainWindow.StartingPage);
        }
    }
}
