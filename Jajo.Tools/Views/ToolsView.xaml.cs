using Jajo.Tools.ViewModels.Utils;
using System.Windows;
using System.Windows.Input;

namespace Jajo.Tools.Views;

public partial class ToolsView
{
    private readonly IViewModel _viewModel;

    public ToolsView(IViewModel viewModel) : this()
    {
        _viewModel = viewModel;
        viewModel.ShowMessage += ShowMessage;
        Closing += Window_Closing;
        DataContext = viewModel;

        Ui.Common.Application.Current = this;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _viewModel.OnApplicationClosing();
    }

    private void ShowMessage(string text)
    {
        MessageBox.Show(this, text, "Внимание");
    }

    public ToolsView()
    {
        InitializeComponent();
    }

    private void Window_TopPart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void Hide_BTN_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}