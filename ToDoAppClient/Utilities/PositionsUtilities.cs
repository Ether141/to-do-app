using System.Windows.Input;
using System.Windows;

namespace ToDoAppClient.Utilities
{
    public static class PositionsUtilities
    {
        public static Point GetMousePosition() => MainWindow.Instance.PointToScreen(Mouse.GetPosition(MainWindow.Instance));
    }
}
