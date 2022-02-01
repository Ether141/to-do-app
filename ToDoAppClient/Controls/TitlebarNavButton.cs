using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;

namespace ToDoAppClient.Controls
{
    public enum TitlebarNavButtonType
    {
        Close,
        Minimize,
        Maximize,
        Windowed
    }

    public class TitlebarNavButton : Button
    {
        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(nameof(ButtonType), typeof(TitlebarNavButtonType), typeof(TitlebarNavButton), new PropertyMetadata(TitlebarNavButtonType.Close));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(TitlebarNavButton), new PropertyMetadata(default(ImageSource)));

        public TitlebarNavButtonType ButtonType
        {
            get { return (TitlebarNavButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        [Bindable(true)]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}
