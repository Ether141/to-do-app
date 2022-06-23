using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ToDoAppClient.Windows
{
    public partial class MessagePopup : Window
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(MessagePopup), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(MessagePopup), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty MessageIconProperty = DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(MessagePopup), new PropertyMetadata());

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
        public ImageSource MessageIcon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public event Action<MessagePopupResult> OnClosePopup;

        public MessagePopup(string label, string description, MessagePopupIcon icon, MessagePopupButtons buttons)
        {
            LabelText = label;
            Description = description;
            BitmapImage img = (BitmapImage)App.Current.Resources["infoIcon"];

            switch (icon)
            {
                case MessagePopupIcon.Error:
                    img = (BitmapImage)App.Current.Resources["errorIcon"];
                    break;
                case MessagePopupIcon.Warning:
                    img = (BitmapImage)App.Current.Resources["warningIcon"];
                    break;
                case MessagePopupIcon.Info:
                    img = (BitmapImage)App.Current.Resources["infoIcon"];
                    break;
            }

            MessageIcon = img;
            InitializeComponent();
            cancelButton.Visibility = buttons == MessagePopupButtons.OkAndCancel ? Visibility.Visible : Visibility.Collapsed;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            OnClosePopup?.Invoke(MessagePopupResult.Cancel);
            Close();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            OnClosePopup?.Invoke(MessagePopupResult.Ok);
            Close();
        }
    }

    public enum MessagePopupIcon
    {
        Info,
        Warning,
        Error
    }

    public enum MessagePopupButtons
    {
        Ok,
        OkAndCancel
    }

    public enum MessagePopupResult
    {
        Ok,
        Cancel
    }
}
