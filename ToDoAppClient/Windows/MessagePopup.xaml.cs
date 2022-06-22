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

        public MessagePopup()
        {
            BitmapImage img = (BitmapImage)TryFindResource("addIcon");
            MessageIcon = img;
            InitializeComponent();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e) => Close();
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
}
