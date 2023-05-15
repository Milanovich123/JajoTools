using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jajo.Exporter.ViewModels.Pages;
using Jajo.Exporter.ViewModels.Utils;
using Jajo.Ui.Controls;
using Jajo.Ui.MVVM.Services;
using Jajo.Utils.Commands;
using Jajo.Utils.Services;
using Jajo.Utils.Stores;
using Jajo.Utils.ViewModels;

namespace Jajo.Exporter.ViewModels;

public sealed partial class MainViewModel : ObservableValidator, IMainViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ExportViewModel _exportViewModel;
    private readonly SchedulerViewModel _schedulerViewModel;

    private ICommand _onWindowLoadedCommand;
    public ICommand SetExportViewModelCommand { get; }
    public ICommand SetSchedulerViewModelCommand { get; }

    public Action<string> ShowMessage { get; set; }
    public event EventHandler CloseRequested = delegate { }; // Invokes when the main window should be closed

    public MainViewModel(NavigationStore navigationStore, ExportViewModel exportViewModel, SchedulerViewModel schedulerViewModel)
    {
        // To see how navigation works and is implemented step by step
        // https://www.youtube.com/watch?v=N26C_Cq-gAY&list=PLA8ZIAm2I03ggP55JbLOrXl6puKw4rEb2

        // Registering navigation store and setting startup page
        _navigationStore = navigationStore;
        _exportViewModel = exportViewModel;
        _schedulerViewModel = schedulerViewModel;
        _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));

        // Registering navigation commands, so after clicking a radiobutton
        // it will invoke one of this command
        SetExportViewModelCommand = new NavigateCommand<ExportViewModel>(
            new NavigationService<ExportViewModel>(navigationStore, () => exportViewModel));
        SetSchedulerViewModelCommand =
            new NavigateCommand<SchedulerViewModel>(
                new NavigationService<SchedulerViewModel>(navigationStore, () => schedulerViewModel));


        // Setting export view to be shown firstly on the startup of the application
        SetExportViewModelCommand.Execute(null);
    }

    public IViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;
    
    [RelayCommand]
    private void Close()
    {
        CloseRequested.Invoke(this,EventArgs.Empty);
    }

    // Here you can add code that will be executed before the window is shown
    public ICommand OnWindowLoadedCommand => _onWindowLoadedCommand ??= new RelayCommand<Snackbar>( snackBar =>
    {
        if (snackBar is null) return;
        var snackBarService = new SnackbarService();
        snackBarService.SetSnackbarControl(snackBar);
        _exportViewModel.SnackbarService = snackBarService;
        _schedulerViewModel.SnackbarService = snackBarService;
    });

    public void OnApplicationClosing()
    {
    }
}