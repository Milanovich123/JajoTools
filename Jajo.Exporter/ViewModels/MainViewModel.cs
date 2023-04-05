using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jajo.Exporter.Stores;
using Jajo.Exporter.ViewModels.Pages;
using Jajo.Exporter.ViewModels.Utils;

namespace Jajo.Exporter.ViewModels;

public sealed partial class MainViewModel : ObservableValidator, IMainViewModel
{
    private readonly NavigationStore _navigationStore;
    private ICommand _simpleRelayCommand;
    private ICommand _onWindowLoadedCommand;

    public Action<string> ShowMessage { get; set; }
    public event EventHandler CloseRequested = delegate { };

    public MainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModel = new ExportViewModel();
    }

    public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
    
    [RelayCommand]
    private void Close()
    {
        CloseRequested.Invoke(this,EventArgs.Empty);
    }

    [RelayCommand]
    private void SetExportViewModel()
    {
        _navigationStore.CurrentViewModel = new ExportViewModel();
        OnPropertyChanged(nameof(CurrentViewModel));
    }
    
    [RelayCommand]
    private void SetSchedularViewModel()
    {
        _navigationStore.CurrentViewModel = new SchedularViewModel();
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public ICommand SimpleRelayCommand => _simpleRelayCommand ??= new RelayCommand(() =>
    {
        ShowMessage?.Invoke("Simple Command");
    });

    public ICommand OnWindowLoadedCommand => _onWindowLoadedCommand ??= new RelayCommand(() =>
    {
    });

    public void OnApplicationClosing()
    {
    }
}