using System.Windows;
using System.Windows.Controls;

namespace ToDoAppClient.Controls
{
    public partial class ToDoEntry : UserControl
    {
        public static readonly DependencyProperty EntryNameProperty = DependencyProperty.Register(nameof(EntryName), typeof(string), typeof(ToDoEntry), new PropertyMetadata("New ToDo entry"));
        public static readonly DependencyProperty GoalsCountProperty = DependencyProperty.Register(nameof(GoalsCount), typeof(int), typeof(ToDoEntry), new PropertyMetadata(3));
        public static readonly DependencyProperty CurrentGoalsCountProperty = DependencyProperty.Register(nameof(CurrentGoalsCount), typeof(int), typeof(ToDoEntry), new PropertyMetadata(0));
        public static readonly DependencyProperty IsFinishedProperty = DependencyProperty.Register(nameof(IsFinished), typeof(bool), typeof(ToDoEntry), new PropertyMetadata(false));

        public string EntryName
        {
            get => (string)GetValue(EntryNameProperty);
            set => SetValue(EntryNameProperty, value);
        }

        public int GoalsCount
        {
            get => (int)GetValue(GoalsCountProperty);
            set => SetValue(GoalsCountProperty, value);
        }

        public int CurrentGoalsCount
        {
            get => (int)GetValue(CurrentGoalsCountProperty);
            set => SetValue(CurrentGoalsCountProperty, value);
        }

        public bool IsFinished => CurrentGoalsCount == GoalsCount;

        public string GoalsCountString => $"{CurrentGoalsCount} / {GoalsCount}";

        public ToDoEntry()
        {
            InitializeComponent();
        }
    }
}
