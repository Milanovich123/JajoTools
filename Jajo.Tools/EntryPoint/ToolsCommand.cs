using Autodesk.Revit.Attributes;
using Jajo.Utils.Core;
using Nice3point.Revit.Toolkit.External;

namespace Jajo.Tools.EntryPoint;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class ToolsCommand : ExternalCommand
{
    public override async void Execute()
    {
        RevitApi.UiApplication ??= ExternalCommandData.Application;

        // There i use Host class for implementing dependency injection principle
        await Host.StartHost();
    }
}