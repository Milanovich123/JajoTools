using System.Windows.Input;
using Jajo.Exporter.Services;
using Jajo.Exporter.ViewModels.Pages;

namespace Jajo.Exporter.Commands;

public class NavigateToExportCommand : ICommand
{
    private readonly NavigationService _navigationService;
    private readonly ExportViewModel _exportViewModel;

    public NavigateToExportCommand(NavigationService navigationService, ExportViewModel exportViewModel)
    {
        _navigationService = navigationService;
        _exportViewModel = exportViewModel;
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