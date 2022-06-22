using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Controls;
using ToDoAppClient.Core;
using ToDoAppClient.Models;
using ToDoAppClient.Windows;
using ToDoAppClient.Resources.Strings;

namespace ToDoAppClient.Pages
{
    public partial class MainPage : Page
    {
        private readonly User currentUser;

        public static MainPage? Current { get; private set; }
        public static ToDoListsManager ListsManager { get; private set; }

        public MainPage()
        {
            Current = this;
            InitializeComponent();
            currentUser = new User(1, "admin_1", "admin@gmail.com");
            usernameLabel.DataContext = currentUser;
            ListsManager = new ToDoListsManager();
            ListsManager.DownloadLists();
            DrawToDoLists();
        }

        public void DrawToDoLists()
        {
            todoList.Children.Clear();
            ToDoModel[] allLists = ListsManager.AllLists;
            foreach (var model in allLists)
                DrawToDoListEntry(model);
        }

        public void Reopen()
        {
            if (Current == null && MainWindow.Instance.CurrentlyOpenedPage != Current)
                return;

            MainWindow.Instance.OpenPage(Current);
            DrawToDoLists();
        }

        private void DrawToDoListEntry(ToDoModel model)
        {
            ToDoEntryControl toDoEntryControl = new ToDoEntryControl(model);
            toDoEntryControl.Click += ListEntryClick;
            todoList.Children.Add(toDoEntryControl);
        }

        private void OpenToDoList(ToDoModel list)
        {
            ToDoPage toDoPage = MainWindow.ToDoPage;
            toDoPage.SetDataContext(list);
            MainWindow.Instance.OpenPage(toDoPage);
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenPage(MainWindow.StartingPage);
            Current = null;
        }

        private void AddListClick(object sender, RoutedEventArgs e)
        {
            TextPopup popup = new TextPopup(Resource.listName);

            popup.OnAcceptPopup += text =>
            {
                ToDoModel newList = new ToDoModel(10, text);
                ListsManager.AddToDoList(newList);
                DrawToDoListEntry(newList);
                OpenToDoList(newList);
            };

            popup.ShowDialog();
        }

        private void ListEntryClick(object sender, RoutedEventArgs e) => OpenToDoList(((ToDoEntryControl)sender).ToDoContent);
    }
}
