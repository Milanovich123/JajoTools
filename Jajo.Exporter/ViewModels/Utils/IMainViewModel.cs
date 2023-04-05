namespace Jajo.Exporter.ViewModels.Utils;

public interface IMainViewModel
{
    public Action<string> ShowMessage { get; set; }
    public event EventHandler CloseRequested;

    public void OnApplicationClosing();
}