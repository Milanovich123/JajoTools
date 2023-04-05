using Autodesk.Revit.Attributes;
using Jajo.Utils.Core;
using Nice3point.Revit.Toolkit.External;

namespace Jajo.Exporter.EntryPoint;

/// <summary>
/// The entrypoint for jajo export plugin
/// </summary>
[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class ExporterCommand : ExternalCommand
{
    public override async void Execute()
    {
        RevitApi.UiApplication ??= ExternalCommandData.Application;
        
        // There i use Host class for implementing dependency injection principle
        await Host.StartHost();
    }
}