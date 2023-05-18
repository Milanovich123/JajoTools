using Autodesk.Revit.UI;

namespace Jajo.Exporter.EntryPoint;

public static class Buttons
{
    /// <summary>
    ///     Creates a button in a Revit tabpanel
    /// </summary>
    /// <param name="panel"></param>
    public static void CreateButton(RibbonPanel panel)
    {
        var showButton = panel.AddPushButton<ExporterCommand>("Exporter");

        showButton.SetLargeImage("/Jajo.Exporter;component/Resources/Icons/ExportExtended32.png");
        showButton.ToolTip = "Jajo Exporter";
        showButton.LongDescription = "This application is used to...";
    }
}