using System.Windows;
using System.Windows.Controls;

namespace ToDoAppClient.Controls
{
    public partial class ToDoListEntryControl : UserControl
    {
        public ToDoListEntryControl()
        {
            InitializeComponent();

            MouseEnter += (_, _) =>
            {
                renameButton.Visibility = Visibility.Visible;
                removeButton.Visibility = Visibility.Visible;
            };

            MouseLeave += (_, _) =>
            {
                renameButton.Visibility = Visibility.Hidden;
                removeButton.Visibility = Visibility.Hidden;
            };
        }
    }
}
