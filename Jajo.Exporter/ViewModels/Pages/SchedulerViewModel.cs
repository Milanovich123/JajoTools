using System.Windows;

namespace Jajo.Exporter.ViewModels.Pages;

public class SchedulerViewModel : PageBaseViewModel, IViewModelBase
{
    /// <summary>
    /// Override method from abstract class
    /// </summary>
    protected override void Export()
    {
        MessageBox.Show("Submit button was clicked");
    }
}