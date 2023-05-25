using Autodesk.Revit.DB;
using Jajo.Ui.Common;
using Jajo.Ui.MVVM.Services;
using Jajo.Utils.ViewModels;

namespace Jajo.Exporter.ViewModels.Pages;

public class ExportViewModel : PageBaseViewModel, IViewModelBase
{
    public SnackbarService SnackbarService { get; set; }

    /// <summary>
    ///     Override method from abstract class
    /// </summary>
    protected override void Export()
    {
        if (SnackbarService is null) return;

        List<View> viewExportList = new List<View>();
        foreach (ViewExample v in _exportViews)
        {
            if (v.IsSelected)
            {
                viewExportList.Add(v.RevitView);
            }
        }
        List<ViewSheet> sheetExportList = new List<ViewSheet>();
        foreach (SheetExample v in _exportSheets)
        {
            if (v.IsSelected)
            {
                sheetExportList.Add(v.RevitSheet);
            }
        }
        _exportEventHandler.viewList = viewExportList;
        _exportEventHandler.sheetList = sheetExportList;
        _exportEventHandler.dwg = IsExportToDwgSelected;
        _exportEventHandler.Raise();

        // Just an example how to use a snackbar
        if (IsExportToDwgSelected)
            SnackbarService.Show("Operation failed!", ControlAppearance.Failure);

        // logic when the dwg export check box was not selected
        else
            SnackbarService.Show("Export succeed!", ControlAppearance.Success);
    }
}