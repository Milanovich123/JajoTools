using Nice3point.Revit.Toolkit.External;
using Jajo.Utils.Core;

namespace Jajo.EntryPoint;

[UsedImplicitly]
public class Application : ExternalApplication
{
    /// <summary>
    /// The entry point for exporter and tools plugin
    /// </summary>
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