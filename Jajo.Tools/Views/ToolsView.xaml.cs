using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Jajo.Tools.ViewModels.Utils;
using Application = Jajo.Ui.Common.Application;

namespace Jajo.Tools.Views;

public partial class ToolsView
{
    private readonly IViewModel _viewModel;

    public ToolsView(IViewModel viewModel) : this()
    {
        _viewModel = viewModel;
        viewModel.ShowMessage += ShowMessage;
        viewModel.CloseRequested += (_, _) => Close();
        Closing += Window_Closing;
        DataContext = viewModel;

        Application.Current = this;
    }

    public ToolsView()
    {
        InitializeComponent();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        _viewModel.OnApplicationClosing();
    }

    private void ShowMessage(string text)
    {
        MessageBox.Show(this, text, "Внимание");
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