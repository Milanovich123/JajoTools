using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Jajo.Ui.Common;
using Jajo.Ui.Controls;
using Jajo.Ui.MVVM.Services;

namespace Jajo.Exporter.ViewModels.Pages;

public partial class ExportViewModel : PageBaseViewModel, IViewModelBase
{
    public SnackbarService SnackbarService { get; set; }

    /// <summary>
    /// Override method from abstract class
    /// </summary>
    protected override void Export()
    {
        if (SnackbarService is null) return;

        if (IsExportToDwgSelected)
        {
            SnackbarService.Show("Export succeed!", ControlAppearance.Success);
        }
        // logic when the dwg export check box was not selected
        else
        {
            SnackbarService.Show("Export failed!", ControlAppearance.Failure);
        }
    }
}