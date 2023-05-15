using CommunityToolkit.Mvvm.ComponentModel;
using Jajo.Ui.MVVM.Services;
using Jajo.Utils.ViewModels;

namespace Jajo.Tools.ViewModels.Pages;

public abstract class PageBaseViewModel : ObservableObject, IViewModelBase
{
    public SnackbarService SnackbarService { get; set; }
}