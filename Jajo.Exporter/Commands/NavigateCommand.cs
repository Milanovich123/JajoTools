using Jajo.Exporter.Services;
using Jajo.Exporter.ViewModels;

namespace Jajo.Exporter.Commands;

public class NavigateCommand<TViewModel> : CommandBase
where TViewModel : IViewModelBase
{
    private readonly NavigationService<TViewModel> _navigationService;

    public NavigateCommand(NavigationService<TViewModel> navigationService)
    {
        _navigationService = navigationService;
    }

    public override void Execute(object parameter)
    {
        _navigationService.Navigate();
    }
}