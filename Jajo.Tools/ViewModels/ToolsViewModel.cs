using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jajo.Tools.ViewModels.Utils;
using Jajo.Ui.Controls;
using Jajo.Utils.Commands;
using Jajo.Utils.Services;
using Jajo.Utils.Stores;
using Jajo.Utils.ViewModels;
using System.Windows.Input;
using Jajo.Tools.ViewModels.Pages;
using Jajo.Tools.Views.Pages;
using Jajo.Ui.MVVM.Services;

namespace Jajo.Tools.ViewModels;

public sealed partial class ToolsViewModel : ObservableValidator, IViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly HideTabsViewModel _hideTabsViewModel;
    private readonly WerkpakketViewModel _werkpakketViewModel;

    private ICommand _onWindowLoadedCommand;
    public ICommand SetHideTabsViewModelCommand { get; }
    public ICommand SetWerkpakketViewModelCommand { get; }

    public Action<string> ShowMessage { get; set; }
    public event EventHandler CloseRequested = delegate { }; // Invokes when the main window should be closed

    public ToolsViewModel(NavigationStore navigationStore, HideTabsViewModel hideTabsViewModel,
        WerkpakketViewModel werkpakketViewModel)
    {
        // To see how navigation works and is implemented step by step
        // https://www.youtube.com/watch?v=N26C_Cq-gAY&list=PLA8ZIAm2I03ggP55JbLOrXl6puKw4rEb2
    
        // Registering navigation store and setting startup page
        _navigationStore = navigationStore;
        _hideTabsViewModel = hideTabsViewModel;
        _werkpakketViewModel = werkpakketViewModel;
        _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));
    
        // Registering navigation commands, so after clicking a radiobutton
        // it will invoke one of this command
        SetHideTabsViewModelCommand = new NavigateCommand<HideTabsViewModel>(
            new NavigationService<HideTabsViewModel>(navigationStore, () => hideTabsViewModel));
        SetWerkpakketViewModelCommand =
            new NavigateCommand<WerkpakketViewModel>(
                new NavigationService<WerkpakketViewModel>(navigationStore, () => werkpakketViewModel));
    
        // Setting export view to be shown firstly on the startup of the application
        SetHideTabsViewModelCommand.Execute(null);
    }

    public IViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

    [RelayCommand]
    private void Close()
    {
        CloseRequested.Invoke(this, EventArgs.Empty);
    }

    // Here you can add code that will be executed before the window is shown
    public ICommand OnWindowLoadedCommand => _onWindowLoadedCommand ??= new RelayCommand<Snackbar>(snackBar =>
    {
        if (snackBar is null) return;
        var snackBarService = new SnackbarService();
        snackBarService.SetSnackbarControl(snackBar);
        _hideTabsViewModel.SnackbarService = snackBarService;
        _werkpakketViewModel.SnackbarService = snackBarService;
    });

    public void OnApplicationClosing()
    {
    }
}