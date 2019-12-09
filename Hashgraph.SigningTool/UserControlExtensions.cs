using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public static class UserControlExtensions
    {
        public static void MoveToControl<TControl>(this UserControl existing) where TControl : UserControl, new()
        {
            Window.GetWindow(existing).Content = new TControl();
        }
    }
}
