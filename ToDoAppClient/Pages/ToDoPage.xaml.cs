using RestSharp;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Controls;
using ToDoAppClient.Resources.Strings;
using ToDoAppClient.Windows;
using ToDoAppSharedModels.Common;

namespace ToDoAppClient.Pages
{
    public partial class ToDoPage : Page
    {
        public ToDoList? CurrentlyHandledList => DataContext as ToDoList;
        private bool changedAnyIsDoneState = false;
        private bool[] startIsDoneStates;

        public ToDoPage() => InitializeComponent();

        public void SetDataContext(ToDoList dataContext)
        {
            DataContext = dataContext;
            ReloadToDoEntries();
        }

        public void ReloadToDoEntries()
        {
            if (CurrentlyHandledList == null)
                return;

            todoEntriesList.Children.Clear();

            if (CurrentlyHandledList.ToDoEntries == null)
                return;

            startIsDoneStates = new bool[CurrentlyHandledList.ToDoEntries.Count];
            int i = 0;

            foreach (ToDoEntry? todoEntry in CurrentlyHandledList.ToDoEntries)
            {
                ToDoListEntryControl newControl = new ToDoListEntryControl
                {
                    DataContext = todoEntry
                };

                newControl.renameButton.Click += (_, _) => RenameToDoEntry(todoEntry);
                newControl.removeButton.Click += (_, _) => RemoveToDoEntry(todoEntry);
                newControl.IsDoneStatusChanged += _ =>
                {
                    System.Diagnostics.Debug.WriteLine("\nchanged\n");
                    changedAnyIsDoneState = true;
                };

                todoEntriesList.Children.Add(newControl);
                startIsDoneStates[i++] = todoEntry?.IsDone ?? false;
            }
        }

        private void RemoveListButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentlyHandledList == null)
                return;

            MessagePopup popup = new MessagePopup(Resource.warning, Resource.sureRemoveList, MessagePopupIcon.Warning, MessagePopupButtons.OkAndCancel);

            popup.OnClosePopup += async result =>
            {
                if (result == MessagePopupResult.Ok)
                {
                    RestResponse response = await MainPage.ListsManager.RemoveList(CurrentlyHandledList.Id);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        BackToMainPage();
                    }
                    else
                    {
                        HandlePotenialResponseErrors(response);
                    }
                }
            };

            popup.ShowDialog();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e) => BackToMainPage();

        private void BackToMainPage()
        {
            if (changedAnyIsDoneState)
                ChangeEntriesStatus();

            if (MainPage.Current != null)
                MainPage.Current.Reopen();
        }

        private void BackToMainPageDueToError()
        {
            MainPage.Current!.EnableConnectionErrorUI(true);
            BackToMainPage();
        }

        private async void ChangeEntriesStatus()
        {
            if (CurrentlyHandledList == null)
                return;

            RestResponse response = await MainPage.ListsManager.ChangeToDoEntriesStates(CurrentlyHandledList.ToDoEntries);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MainPage.Current!.EnableConnectionErrorUI(true);

                int i = 0;

                foreach (ToDoEntry? todoEntry in CurrentlyHandledList.ToDoEntries)
                {
                    if (todoEntry != null)
                        todoEntry.IsDone = startIsDoneStates[i];
                    i++;
                }
            }
            else
            {
                HandlePotenialResponseErrors(response);
            }
        }

        private void RenameListButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentlyHandledList == null)
                return;

            TextPopup popup = new TextPopup(Resource.newName, ((ToDoList)DataContext).Name);
            popup.MaxLength = ToDoList.MaxListNameLength;
            popup.OnAcceptPopup += async text =>
            {
                if (CurrentlyHandledList.Name == text)
                    return;

                RestResponse response = await MainPage.ListsManager.RenameList(CurrentlyHandledList.Id, text);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    listNameLabel.GetBindingExpression(ContentProperty).UpdateTarget();
                }
                else
                {
                    HandlePotenialResponseErrors(response);
                }
            };
            popup.ShowDialog();
        }

        private void AddEntryButtonClick(object sender, RoutedEventArgs e)
        {
            TextPopup popup = new TextPopup(Resource.addTask);
            popup.MaxLength = ToDoEntry.MaxEntryNameLength;
            popup.OnAcceptPopup += AddToDoEntry;
            popup.ShowDialog();
        }

        private void RenameToDoEntry(ToDoEntry toDoEntry)
        {
            if (CurrentlyHandledList == null)
                return;

            TextPopup popup = new TextPopup(Resource.newName, toDoEntry.Text);
            popup.MaxLength = ToDoEntry.MaxEntryNameLength;
            popup.OnAcceptPopup += async text =>
            {
                if (text == toDoEntry.Text)
                    return;

                RestResponse response = await MainPage.ListsManager.RenameEntry(CurrentlyHandledList.Id, toDoEntry.Id, text);
                HandleEntriesOperationResponse(response);
            };
            popup.ShowDialog();
        }

        private async void RemoveToDoEntry(ToDoEntry toDoEntry)
        {
            if (CurrentlyHandledList == null)
                return;

            RestResponse response = await MainPage.ListsManager.RemoveEntry(CurrentlyHandledList.Id, toDoEntry.Id);
            HandleEntriesOperationResponse(response);
        }

        private async void AddToDoEntry(string content)
        {
            if (CurrentlyHandledList == null)
                return;

            RestResponse<ToDoEntry> response = await MainPage.ListsManager.AddToDoEntry(CurrentlyHandledList.Id, content);
            HandleEntriesOperationResponse(response);
        }

        private void HandleEntriesOperationResponse(RestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                ReloadToDoEntries();
            }
            else
            {
                HandlePotenialResponseErrors(response);
            }
        }

        private void HandlePotenialResponseErrors(RestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
                return;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                MainPage.Current!.SessionExpiredPopup();
            }
            else
            {
                BackToMainPageDueToError();
            }
        }
    }
}
