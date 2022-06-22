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
        public ToDoPage()
        {
            InitializeComponent();
        }

        public void SetDataContext(ToDoModel dataContext)
        {
            DataContext = dataContext;
            ReloadToDoEntries();
        }

        public void ReloadToDoEntries()
        {
            if (DataContext == null || DataContext.GetType() != typeof(ToDoModel))
                return;

            todoEntriesList.Children.Clear();

            ToDoModel model = (ToDoModel)DataContext;

            foreach (var todoEntry in model.ToDoEntries)
            {
                ToDoListEntryControl newControl = new ToDoListEntryControl
                {
                    DataContext = todoEntry
                };
                todoEntriesList.Children.Add(newControl);
            }
        }

        private void RemoveListButtonClick(object sender, RoutedEventArgs e)
        {
            MessagePopup popup = new MessagePopup();
            popup.ShowDialog();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (MainPage.Current != null)
                MainPage.Current.Reopen();
        }

        private void RenameListButtonClick(object sender, RoutedEventArgs e)
        {
            TextPopup popup = new TextPopup(Resource.newName, ((ToDoModel)DataContext).Name);
            popup.OnAcceptPopup += text =>
            {
                ToDoModel model = (ToDoModel)DataContext;

                if (model.Name == text)
                    return;

                model.Name = text;
                MainPage.ListsManager.UpdateList(model.Id, model);
                listNameLabel.GetBindingExpression(ContentProperty).UpdateTarget();
            };
            popup.ShowDialog();
        }

        private void AddEntryButtonClick(object sender, RoutedEventArgs e)
        {
            TextPopup popup = new TextPopup(Resource.addTask);
            popup.OnAcceptPopup += AddTodoEntry;
            popup.ShowDialog();
        }

        private void AddTodoEntry(string content)
        {
            if (DataContext == null || DataContext.GetType() != typeof(ToDoModel))
                return;

            ToDoModel model = (ToDoModel)DataContext;
            model.ToDoEntries.Add(new ToDoEntry(content, false));
            MainPage.ListsManager.UpdateList(model.Id, model);
            ReloadToDoEntries();
        }
    }
}
