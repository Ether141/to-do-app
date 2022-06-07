using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using ToDoAppClient.Controls;
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

            ToDoModel model = new ToDoModel();
            model.ToDoEntries.Add(new ToDoEntry(1, "Jabłka 3x", false));
            model.ToDoEntries.Add(new ToDoEntry(1, "Mleko 2l", false));
            ToDoEntryControl toDoEntryControl = new ToDoEntryControl("List zakupów", model);
            toDoEntryControl.Click += ListEntryClick;
            todoList.Children.Add(toDoEntryControl);
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenPage(MainWindow.StartingPage);
        }

        private void ListEntryClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenPage(MainWindow.ToDoPage);
        }
    }
}
