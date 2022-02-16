using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;

namespace ToDoAppClient.Controls
{
    public partial class ToDoEntry : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public static readonly DependencyProperty EntryNameProperty = DependencyProperty.Register(nameof(EntryName), typeof(string), typeof(ToDoEntry), new PropertyMetadata("New ToDo entry"));
        public static readonly DependencyProperty GoalsCountProperty = DependencyProperty.Register(nameof(GoalsCount), typeof(int), typeof(ToDoEntry), new PropertyMetadata(3));
        public static readonly DependencyProperty CurrentGoalsCountProperty = DependencyProperty.Register(nameof(CurrentGoalsCount), typeof(int), typeof(ToDoEntry), new PropertyMetadata(0));

        public string EntryName
        {
            get => (string)GetValue(EntryNameProperty);
            set => SetValue(EntryNameProperty, value);
        }

        public int GoalsCount
        {
            get => (int)GetValue(GoalsCountProperty);
            set
            {
                SetValue(GoalsCountProperty, value);
                OnPropertyChanged();
            }
        }

        public int CurrentGoalsCount
        {
            get => (int)GetValue(CurrentGoalsCountProperty);
            set
            {
                SetValue(CurrentGoalsCountProperty, value);
                OnPropertyChanged();
            }
        }

        public bool IsFinished => GoalsCount == CurrentGoalsCount;

        public ToDoEntry()
        {
            InitializeComponent();

            PropertyChanged += (sender, e) => ControlFinishedIcon();
            Loaded += (sender, e) => ControlFinishedIcon();
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void ControlFinishedIcon()
        {
            if (IsFinished)
            {
                finishedIcon.Source = (BitmapImage)App.Current.Resources["toDoFinished"];
                finishedIcon.Opacity = 1;
            }
            else
            {
                finishedIcon.Source = (BitmapImage)App.Current.Resources["toDoNotFinished"];
                finishedIcon.Opacity = 0.33;
            }
        }
    }
}