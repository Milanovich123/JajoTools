using Autodesk.Revit.Attributes;
using Jajo.Exporter.Commands.Handlers;
using Jajo.Exporter.Views;
using Jajo.Utils.Core;
using Nice3point.Revit.Toolkit.External;

namespace Jajo.Exporter.Commands;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class ExporterCommand : ExternalCommand
{
    public static readonly TestEventHandler ExternalEvent = new();
    public static readonly DelegateEventHandler DelegateEvent = new();
    
    public override async void Execute()
    {
        RevitApi.UiApplication ??= ExternalCommandData.Application;
        await Host.StartHost();

        var view = Host.GetService<MainView>();
        if (view is null) return;

        view.Focus();
        view.Show();
    }
}