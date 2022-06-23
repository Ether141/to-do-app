using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Controls;
using ToDoAppClient.Models;
using ToDoAppClient.Resources.Strings;
using ToDoAppClient.Windows;

namespace ToDoAppClient.Pages
{
    public partial class ToDoPage : Page
    {
        public ToDoModel? CurrentlyHandledList => DataContext as ToDoModel;

        public ToDoPage() => InitializeComponent();

        public void SetDataContext(ToDoModel dataContext)
        {
            DataContext = dataContext;
            ReloadToDoEntries();
        }

        public void ReloadToDoEntries()
        {
            if (CurrentlyHandledList == null)
                return;

            todoEntriesList.Children.Clear();

            foreach (ToDoEntry? todoEntry in CurrentlyHandledList.ToDoEntries)
            {
                ToDoListEntryControl newControl = new ToDoListEntryControl
                {
                    DataContext = todoEntry
                };

                newControl.renameButton.Click += (_, _) => RenameToDoEntry(todoEntry);
                newControl.removeButton.Click += (_, _) => RemoveToDoEntry(todoEntry);

                todoEntriesList.Children.Add(newControl);
            }
        }

        private void RemoveListButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentlyHandledList == null)
                return;

            MessagePopup popup = new MessagePopup(Resource.warning, Resource.sureRemoveList, MessagePopupIcon.Warning, MessagePopupButtons.OkAndCancel);

            popup.OnClosePopup += result =>
            {
                if (result == MessagePopupResult.Ok)
                {
                    MainPage.ListsManager.RemoveList(CurrentlyHandledList.Id);
                    BackToMainPage();
                }
            };

            popup.ShowDialog();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e) => BackToMainPage();

        private void BackToMainPage()
        {
            if (MainPage.Current != null)
                MainPage.Current.Reopen();
        }

        private void RenameListButtonClick(object sender, RoutedEventArgs e)
        {
            if (CurrentlyHandledList == null)
                return;

            TextPopup popup = new TextPopup(Resource.newName, ((ToDoModel)DataContext).Name);
            popup.MaxLength = ToDoModel.MaxListNameLength;
            popup.OnAcceptPopup += text =>
            {
                if (CurrentlyHandledList.Name == text)
                    return;

                CurrentlyHandledList.Name = text;
                MainPage.ListsManager.UpdateList(CurrentlyHandledList.Id, CurrentlyHandledList);
                listNameLabel.GetBindingExpression(ContentProperty).UpdateTarget();
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
            TextPopup popup = new TextPopup(Resource.newName, toDoEntry.Text);
            popup.MaxLength = ToDoEntry.MaxEntryNameLength;
            popup.OnAcceptPopup += text =>
            {
                if (text != toDoEntry.Text)
                {
                    toDoEntry.Text = text;
                    UpdateCurrentList();
                    ReloadToDoEntries();
                }
            };
            popup.ShowDialog();
        }

        private void RemoveToDoEntry(ToDoEntry toDoEntry)
        {
            if (CurrentlyHandledList == null)
                return;

            if (CurrentlyHandledList.ToDoEntries.RemoveAll(entry => entry.Id == toDoEntry.Id) > 0)
            {
                UpdateCurrentList();
                ReloadToDoEntries();
            }
        }

        private void AddToDoEntry(string content)
        {
            if (CurrentlyHandledList == null || DataContext.GetType() != typeof(ToDoModel))
                return;

            CurrentlyHandledList.ToDoEntries.Add(new ToDoEntry(content, false));
            UpdateCurrentList();
            ReloadToDoEntries();
        }

        private void UpdateCurrentList()
        {
            if (CurrentlyHandledList != null)
                MainPage.ListsManager.UpdateList(CurrentlyHandledList.Id, CurrentlyHandledList);
        }
    }
}
