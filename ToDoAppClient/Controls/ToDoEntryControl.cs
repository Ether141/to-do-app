using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ToDoAppClient.Models;

namespace ToDoAppClient.Controls
{
    public partial class ToDoEntryControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty EntryNameProperty = DependencyProperty.Register(nameof(EntryName), typeof(string), typeof(ToDoEntryControl), new PropertyMetadata("New ToDo entry"));
        public static readonly DependencyProperty GoalsCountProperty = DependencyProperty.Register(nameof(GoalsCount), typeof(int), typeof(ToDoEntryControl), new PropertyMetadata(3));
        public static readonly DependencyProperty CurrentGoalsCountProperty = DependencyProperty.Register(nameof(CurrentGoalsCount), typeof(int), typeof(ToDoEntryControl), new PropertyMetadata(0));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToDoEntryControl));

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
        public ToDoModel ToDoContent { get; private set; }

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool wasMouseDown = false;

        private void OnClick()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ClickEvent);
            RaiseEvent(newEventArgs);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            
            if (wasMouseDown)
            {
                OnClick();
                wasMouseDown = false;
            }
        }

        public ToDoEntryControl(string name, ToDoModel content)
        {
            EntryName = name;
            InitializeComponent();
            PrepareEvents();
            SetToDoContent(content);
        }

        public ToDoEntryControl()
        {
            InitializeComponent();
            PrepareEvents();
        }

        public void SetToDoContent(ToDoModel toDoModel)
        {
            ToDoContent = toDoModel;
            EntryName = toDoModel.Name;
            GoalsCount = ToDoContent.ToDoEntries.Count;
            CurrentGoalsCount = ToDoContent.DoneEntriesCount;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private void PrepareEvents()
        {
            PropertyChanged += (sender, e) => ControlFinishedIcon();
            Loaded += (sender, e) => ControlFinishedIcon();
            MouseDown += (sender, e) => wasMouseDown = true;
            MouseLeave += (sender, e) => wasMouseDown = false;
        }

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