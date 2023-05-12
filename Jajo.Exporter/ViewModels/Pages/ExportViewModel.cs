using System.Windows;

namespace Jajo.Exporter.ViewModels.Pages;

public class ExportViewModel : PageBaseViewModel, IViewModelBase
{
    /// <summary>
    /// Override method from abstract class
    /// </summary>
    protected override void Export()
    {
        // logic when you need to export sheets to dwg format
        if (IsExportToDwgSelected)
        {
            MessageBox.Show("Elements were successfully exported");
            MessageBox.Show("Sheets were successfully exported as DWG");
        }
        // logic when the dwg export check box was not selected
        else
        {
            MessageBox.Show("Elements were successfully exported");
        }
    }
}