using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ToDoAppClient.Utilities;
using System.Globalization;

namespace ToDoAppClient.Windows
{
    public partial class TextPopup : Window
    {
        public static readonly DependencyProperty PopupLabelProperty = DependencyProperty.Register(nameof(PopupLabel), typeof(string), typeof(TextPopup), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(TextPopup), new PropertyMetadata(1024));
        public const double MaxWindowWidth = 600d;

        public string PopupLabel
        {
            get => (string)GetValue(PopupLabelProperty);
            set => SetValue(PopupLabelProperty, value);
        }
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public event Action<string> OnAcceptPopup;

        private double inputBoxStartWidth;
        private double windowStartWidth;
        private int screenSide;

        private Typeface typeface;

        public TextPopup() => Initialize();

        public TextPopup(string label)
        {
            PopupLabel = label;
            Initialize();
        }

        public TextPopup(string label, string startText)
        {
            PopupLabel = label;
            Initialize();
            inputBox.Text = startText;
            inputBox.CaretIndex = startText.Length;
        }

        private void Initialize()
        {
            InitializeComponent();
            inputBox.Focus();
            SetPositionToMousePosition();

            typeface = new Typeface(inputBox.FontFamily, inputBox.FontStyle, inputBox.FontWeight, inputBox.FontStretch);

            KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (e.Key == Key.Enter)
                    AcceptButtonClick(null, null);
            };

            Loaded += (_, _) =>
            {
                inputBoxStartWidth = inputBox.ActualWidth;
                windowStartWidth = ActualWidth;
                SetWindowWidthBasedOnInput();
                SetPositionToMousePosition();
            };

            inputBox.TextChanged += (_, _) => SetWindowWidthBasedOnInput();
        }

        private void SetWindowWidthBasedOnInput()
        {
            double textWidth = new FormattedText(inputBox.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, inputBox.FontSize, Brushes.Black, VisualTreeHelper.GetDpi(inputBox).PixelsPerDip).Width;
            double targetWidth = MaxWindowWidth;

            if ((textWidth + 20 > inputBoxStartWidth && ActualWidth < MaxWindowWidth) || (textWidth < inputBox.ActualWidth && textWidth > inputBoxStartWidth))
                targetWidth = windowStartWidth - inputBoxStartWidth + textWidth + 20;
            else if (textWidth <= inputBoxStartWidth)
                targetWidth = windowStartWidth;

            if (screenSide == 1)
                Left += ActualWidth - targetWidth;

            Width = targetWidth <= MaxWindowWidth ? targetWidth : MaxWindowWidth;
        }

        public void SetPositionToMousePosition()
        {
            Point mousePos = PositionsUtilities.GetMousePosition();
            Left = mousePos.X + 5;
            Top = mousePos.Y - (Height / 2);

            screenSide = PositionsUtilities.ScreenSide(MainWindow.Instance, Left);

            if (screenSide == 1)
                Left -= Width + 5;
        }

        private void AcceptButtonClick(object? sender, RoutedEventArgs? e)
        {
            if (inputBox.Text.Length > 0 && inputBox.Text.Length < 256)
            {
                OnAcceptPopup?.Invoke(inputBox.Text);
                Close();
            }
        }
    }
}
