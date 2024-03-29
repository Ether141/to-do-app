﻿using System.Windows;
using ToDoAppClient.Pages;
using System.Windows.Controls;
using ToDoAppClient.Resources.Strings;
using ToDoAppClient.Windows;
using ToDoAppClient.Core.Settings;

namespace ToDoAppClient
{
    public partial class MainWindow : Window
    {
        public bool FullScreenPage { get; private set; } = false;

        public static MainWindow Instance => (MainWindow)App.Instance.MainWindow;

        public static StartingPage StartingPage => new StartingPage();
        public static SignInPage SignInPage => new SignInPage();
        public static SignUpPage SignUpPage => new SignUpPage();
        public static MainPage MainPage => MainPage.Current ?? new MainPage();
        public static ToDoPage ToDoPage => new ToDoPage();
        public static SettingsPage SettingsPage => new SettingsPage();

        public Page CurrentlyOpenedPage { get; private set; }

        public const double CaptionSize = 28;
        public const double MaximizedBorderThickness = 8;
        public const double WindowedBorderThickness = 1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustWindowBorders();
            AdjustPageSize();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            InitializeSettings();
            OpenPage(StartingPage);
        }

        public void OpenPage(Page page) => OpenPage(page, true);

        public void OpenPage(Page page, bool isFullScreen)
        {
            CurrentlyOpenedPage = page;
            pageDisplay.Content = page;
            FullScreenPage = isFullScreen;

            if (FullScreenPage)
            {
                page.Width = ActualWidth - (WindowState == WindowState.Maximized ? MaximizedBorderThickness * 2 : 0);
                page.Height = ActualHeight - CaptionSize - (WindowState == WindowState.Maximized ? MaximizedBorderThickness * 2 : 0);
            }
        }

        private void AdjustPageSize()
        {
            if (FullScreenPage && pageDisplay.Content != null && pageDisplay.Content as Page != null)
            {
                Page page = (Page)pageDisplay.Content;
                page.Width = ActualWidth - (WindowState == WindowState.Maximized ? MaximizedBorderThickness * 2 : 0);
                page.Height = ActualHeight - CaptionSize - (WindowState == WindowState.Maximized ? MaximizedBorderThickness * 2 : 0);
            }
        }

        private void InitializeSettings()
        {
            AppSettingsInitializationResult result = App.Instance.ApplicationSettings.Initialize();

            if (result != AppSettingsInitializationResult.Success)
            {
                MessagePopup popup = new MessagePopup(Resource.error, Resource.cannotRunApp, MessagePopupIcon.Error, MessagePopupButtons.Ok);
                popup.ShowDialog();
                App.Instance.Shutdown(-1);
            }
        }
    }
}
