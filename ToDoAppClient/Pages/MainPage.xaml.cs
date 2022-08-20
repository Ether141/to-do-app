using RestSharp;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Controls;
using ToDoAppClient.Core.Main;
using ToDoAppClient.Models;
using ToDoAppClient.Resources.Strings;
using ToDoAppClient.Windows;
using ToDoAppSharedModels.Common;

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
            currentUser = App.Instance.SessionManager.CurrentUser!;
            usernameLabel.DataContext = currentUser;
            DownloadLists();
        }

        private async void DownloadLists()
        {
            ListsManager = new ToDoListsManager();

            HttpStatusCode downloadResult = await ListsManager.DownloadLists();
            downloadingListsUI.Visibility = Visibility.Collapsed;

            if (downloadResult == HttpStatusCode.OK)
            {
                EnableConnectionErrorUI(false);
                DrawToDoLists();
            }
            else if (downloadResult == HttpStatusCode.Unauthorized)
            {
                SessionExpiredPopup();
            }
            else
            {
                EnableConnectionErrorUI(true);
            }
        }

        public void DrawToDoLists()
        {
            todoList.Children.Clear();
            ToDoList[] allLists = ListsManager.AllLists;
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
        
        public void SessionExpiredPopup()
        {
            MessagePopup popup = new MessagePopup(Resource.error, Resource.sessionExpired, MessagePopupIcon.Error, MessagePopupButtons.Ok);
            popup.OnClosePopup += _ => LogOut();
            popup.ShowDialog();
        }

        public void EnableConnectionErrorUI(bool enabled) => connectionError.Visibility = enabled ? Visibility.Visible : Visibility.Collapsed;

        private void DrawToDoListEntry(ToDoList model)
        {
            ToDoEntryControl toDoEntryControl = new ToDoEntryControl(model);
            toDoEntryControl.Click += ListEntryClick;
            todoList.Children.Add(toDoEntryControl);
        }

        private void OpenToDoList(ToDoList list)
        {
            ToDoPage toDoPage = MainWindow.ToDoPage;
            toDoPage.SetDataContext(list);
            MainWindow.Instance.OpenPage(toDoPage);
        }

        private void LogOutClick(object sender, RoutedEventArgs e) => LogOut();

        private void LogOut()
        {
            App.Instance.SessionManager.Logout();
            MainWindow.Instance.OpenPage(MainWindow.StartingPage);
            Current = null;
        }

        private void SettingsButtonClick(object sender, RoutedEventArgs e) => MainWindow.Instance.OpenPage(MainWindow.SettingsPage);

        private void AddListClick(object sender, RoutedEventArgs e)
        {
            TextPopup popup = new TextPopup(Resource.listName)
            {
                MaxLength = ToDoList.MaxListNameLength
            };

            popup.OnAcceptPopup += async text =>
            {
                RestResponse<ToDoList> response = await ListsManager.AddToDoList(text);

                if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
                {
                    ToDoList newList = response.Data;
                    DrawToDoListEntry(newList);
                    OpenToDoList(newList);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    SessionExpiredPopup();
                }
                else
                {
                    EnableConnectionErrorUI(true);
                }
            };

            popup.ShowDialog();
        }

        private void ListEntryClick(object sender, RoutedEventArgs e) => OpenToDoList(((ToDoEntryControl)sender).ToDoContent);
    }
}
