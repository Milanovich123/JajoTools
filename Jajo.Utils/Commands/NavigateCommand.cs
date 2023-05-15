using Jajo.Utils.Services;
using Jajo.Utils.ViewModels;

namespace Jajo.Utils.Commands;

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