using System;
using System.Windows;
using System.Windows.Interop;
using ToDoAppClient.Core.Themes;
using ToDoAppClient.Core.Helpers;

namespace ToDoAppClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HandleHwndResize();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            app.ThemesManager.ChangeTheme(Themes.Dark);
        }

        private void HandleHwndResize()
        {
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(HwndResizeHandler.WindowProc));
            };
        }
    }
}
