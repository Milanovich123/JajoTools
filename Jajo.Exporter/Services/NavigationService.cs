using Jajo.Exporter.Stores;
using Jajo.Exporter.ViewModels;

namespace Jajo.Exporter.Services;

/// <summary>
/// A service that sets CurrentViewModel property in NavigationStore
/// see more https://www.youtube.com/watch?v=N26C_Cq-gAY&list=PLA8ZIAm2I03ggP55JbLOrXl6puKw4rEb2
/// </summary>
/// <typeparam name="TViewModel"></typeparam>
public class NavigationService<TViewModel> where TViewModel : ViewModelBase
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;

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