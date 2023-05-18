using System.Windows;
using System.Windows.Interop;
using Autodesk.Revit.UI;

namespace Jajo.Exporter.Utils;

/// <summary>
///     Helper class for retaining modeless dialog on top of the Revit
/// </summary>
public static class WindowExecution
{
    /// <summary>
    ///     Shows plugin window and sets Revit window as window owner.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="window">The window.</param>
    public static void Show(this Window window, UIApplication application)
    {
        var hwndSource = HwndSource.FromHwnd(application.MainWindowHandle);
        if (hwndSource != null)
        {
            var currentWindow = hwndSource.RootVisual as Window;
            window.Owner = currentWindow;
        }

        window.Show();
    }
}