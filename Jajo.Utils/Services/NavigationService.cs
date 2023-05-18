using Jajo.Utils.Stores;
using Jajo.Utils.ViewModels;

namespace Jajo.Utils.Services;

/// <summary>
///     A service that sets CurrentViewModel property in NavigationStore
///     see more https://www.youtube.com/watch?v=N26C_Cq-gAY&list=PLA8ZIAm2I03ggP55JbLOrXl6puKw4rEb2
/// </summary>
/// <typeparam name="TViewModel"></typeparam>
public class NavigationService<TViewModel> where TViewModel : IViewModelBase
{
    private readonly Func<TViewModel> _createViewModel;
    private readonly NavigationStore _navigationStore;

    public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;
    }

    public void Navigate()
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }
}