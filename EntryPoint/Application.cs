using Nice3point.Revit.Toolkit.External;
using Jajo.Utils.Core;

namespace Jajo.EntryPoint;

[UsedImplicitly]
public class Application : ExternalApplication
{
    public override void OnStartup()
    {
        var panel = Application.CreatePanel("Utils", "Jajo");

        RevitApi.UiControlledApplication = Application;

        // Exporter project
        Exporter.EntryPoint.Buttons.CreateButton(panel);

        // Tools project
        Tools.EntryPoint.Buttons.CreateButton(panel);
    }
}