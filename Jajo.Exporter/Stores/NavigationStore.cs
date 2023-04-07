using Jajo.Exporter.ViewModels;

namespace Jajo.Exporter.Stores;

public class NavigationStore
{
    // Is used to be subscribed on a propertychanged event for the CurrentViewModel property in MainViewModel
    public event Action CurrentViewModelChanged;

    private IViewModelBase _currentViewModel;

    public IViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}