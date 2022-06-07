using System;
using System.Collections.Generic;
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
using ToDoAppClient.Models;

namespace ToDoAppClient.Pages
{
    public partial class ToDoPage : Page
    {
        public new ToDoModel DataContext { get; private set; }

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
            if (DataContext == null)
                return;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.OpenPage(MainWindow.MainPage);
        }
    }
}
