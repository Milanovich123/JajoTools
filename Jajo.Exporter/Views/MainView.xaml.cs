using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Jajo.Exporter.ViewModels.Utils;
using Application = Jajo.Ui.Common.Application;

namespace Jajo.Exporter.Views;

public partial class MainView
{
    private readonly IMainViewModel _mainViewModel;

    public MainView(IMainViewModel mainViewModel) : this()
    {
        _mainViewModel = mainViewModel;
        _mainViewModel.CloseRequested += (_, _) => Close();
        mainViewModel.ShowMessage += ShowMessage;
        Closing += Window_Closing;
        DataContext = mainViewModel;

        Application.Current = this;
    }

    public MainView()
    {
        InitializeComponent();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        _mainViewModel.OnApplicationClosing();
    }

    private void Window_TopPart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    // Shows MessageBox with text using MVVM
    private void ShowMessage(string text)
    {
        MessageBox.Show(this, text, "Attention");
    }

    private void Hide_BTN_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}