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
        private readonly List<ToDoModel> allToDoModels = new List<ToDoModel>();

        public static MainPage? Current { get; private set; }

        public MainPage()
        {
            Current = this;
            InitializeComponent();
            currentUser = new User(1, "admin_1", "admin@gmail.com");
            usernameLabel.DataContext = currentUser;
            DownloadToDoLists();
            DrawToDoLists();
        }

        private void DownloadToDoLists()
        {
            ToDoModel model = new ToDoModel("Lista zakupów - 9.06.2022");
            allToDoModels.Add(model);
            model.ToDoEntries.Add(new ToDoEntry("Jabłka 3x", false));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", true));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", false));
            model.ToDoEntries.Add(new ToDoEntry("Chleb", true));
            model.ToDoEntries.Add(new ToDoEntry("Cebula 1kg", true));
            model.ToDoEntries.Add(new ToDoEntry("Papryka 3x", true));
            model.ToDoEntries.Add(new ToDoEntry("Jabłka 3x", false));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", true));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", false));
            model.ToDoEntries.Add(new ToDoEntry("Chleb", true));
            model.ToDoEntries.Add(new ToDoEntry("Cebula 1kg", true));
            model.ToDoEntries.Add(new ToDoEntry("Papryka 3x", true));
            model.ToDoEntries.Add(new ToDoEntry("Jabłka 3x", false));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", true));
            model.ToDoEntries.Add(new ToDoEntry("Mleko 2l", false));
            model.ToDoEntries.Add(new ToDoEntry("Chleb", true));
            model.ToDoEntries.Add(new ToDoEntry("Cebula 1kg", true));
            model.ToDoEntries.Add(new ToDoEntry("Papryka 3x", true));
            model.ToDoEntries.Add(new ToDoEntry("1", true));
            model.ToDoEntries.Add(new ToDoEntry("2", true));
            model.ToDoEntries.Add(new ToDoEntry("3", true));
            model.ToDoEntries.Add(new ToDoEntry("4", true));

            model = new ToDoModel("Zadania dla grafików");
            allToDoModels.Add(model);
            model.ToDoEntries.Add(new ToDoEntry("Zrobić jakieś tam przyciski", true));
            model.ToDoEntries.Add(new ToDoEntry("Zrobić coś tam jeszcze", false));
            model.ToDoEntries.Add(new ToDoEntry("I dodatkowo to", true));
        }

        public void DrawToDoLists()
        {
            todoList.Children.Clear();
            foreach (var model in allToDoModels)
            {
                ToDoEntryControl toDoEntryControl = new ToDoEntryControl("List zakupów", model);
                toDoEntryControl.Click += ListEntryClick;
                todoList.Children.Add(toDoEntryControl);
            }
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenPage(MainWindow.StartingPage);
            Current = null;
        }

        private void ListEntryClick(object sender, RoutedEventArgs e)
        {
            ToDoPage toDoPage = MainWindow.ToDoPage;
            toDoPage.SetDataContext(((ToDoEntryControl)sender).ToDoContent);
            MainWindow.Instance.OpenPage(toDoPage);
        }
    }
}
