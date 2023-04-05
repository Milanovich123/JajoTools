using System.Windows;
using System.Windows.Input;
using Jajo.Exporter.ViewModels;
using Jajo.Exporter.ViewModels.Utils;

namespace Jajo.Exporter.Views;

public partial class MainView
{
    readonly IMainViewModel _mainViewModel;
    
    public MainView(IMainViewModel mainViewModel) : this()
    {
        _mainViewModel = mainViewModel;
        _mainViewModel.CloseRequested += (_, _) => Close();
        mainViewModel.ShowMessage += ShowMessage;
        Closing += Window_Closing;
        DataContext = mainViewModel;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

    public MainView()
    {
        InitializeComponent();
    }

    private void Hide_BTN_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
}