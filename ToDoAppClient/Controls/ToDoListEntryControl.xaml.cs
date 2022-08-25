using System;
using System.Windows;
using System.Windows.Controls;

namespace ToDoAppClient.Controls
{
    public partial class ToDoListEntryControl : UserControl
    {
        public event Action<bool> IsDoneStatusChanged;

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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
                return;

            CheckBox checkBox = (CheckBox)sender;
            IsDoneStatusChanged?.Invoke(checkBox.IsChecked ?? false);
        }
    }
}
