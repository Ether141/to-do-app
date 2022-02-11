using System.Windows;
using ToDoAppClient.Controls;

namespace ToDoAppClient
{
    public partial class MainWindow : Window
    {
        private void AdjustWindowBorders()
        {
            if (WindowState == WindowState.Maximized)
            {
                windowedButton.ButtonType = TitlebarNavButtonType.Windowed;
                BorderThickness = new Thickness(MaximizedBorderThickness);
            }
            else
            {
                windowedButton.ButtonType = TitlebarNavButtonType.Maximize;
                BorderThickness = new Thickness(WindowedBorderThickness);
            }
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void WindowedButtonClick(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;

        private void CloseButtonClick(object sender, RoutedEventArgs e) => Close();
    }
}
