using System.Windows;
using System.Windows.Controls;
using ToDoAppClient.Controls;
using ToDoAppClient.Models;

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

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (MainPage.Current != null)
                MainPage.Current.Reopen();
        }
    }
}
