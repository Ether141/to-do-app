using System;
using System.Windows;
using System.Windows.Input;
using ToDoAppClient.Utilities;

namespace ToDoAppClient.Windows
{
    public partial class TextPopup : Window
    {
        public static readonly DependencyProperty PopupLabelProperty = DependencyProperty.Register(nameof(PopupLabel), typeof(string), typeof(TextPopup), new PropertyMetadata(string.Empty));

        public string PopupLabel
        {
            get => (string)GetValue(PopupLabelProperty);
            set => SetValue(PopupLabelProperty, value);
        }

        public event Action<string> OnAcceptPopup;

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
            SetPositionToMousePosition();
            inputBox.Focus();

            KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (e.Key == Key.Enter)
                    AcceptButtonClick(null, null);
            };
        }

        public void SetPositionToMousePosition()
        {
            Point mousePos = PositionsUtilities.GetMousePosition();
            Left = mousePos.X + 5;
            Top = mousePos.Y - (Height / 2);

            int screenSide = PositionsUtilities.ScreenSide(MainWindow.Instance, Left);

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
