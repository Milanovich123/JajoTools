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
        if (SnackbarService is not null)
        {
            SnackbarService.Show("Hello world", ControlAppearance.Success);
        }
        // // logic when you need to export sheets to dwg format
        // if (IsExportToDwgSelected)
        // {
        //     MessageBox.Show("Elements were successfully exported");
        //     MessageBox.Show("Sheets were successfully exported as DWG");
        // }
        // // logic when the dwg export check box was not selected
        // else
        // {
        //     MessageBox.Show("Elements were successfully exported");
        // }
    }
}