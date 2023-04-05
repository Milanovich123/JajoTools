using System.Windows.Input;
using Jajo.Exporter.Services;

namespace Jajo.Exporter.Commands;

public class NavigateCommand : ICommand
{
    private readonly NavigationService _navigationService;

    public NavigateCommand(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    
    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _navigationService.Navigate();
    }

    public event EventHandler CanExecuteChanged;
}