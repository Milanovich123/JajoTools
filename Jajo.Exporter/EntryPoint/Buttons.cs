using Autodesk.Revit.UI;
using Jajo.Exporter.Commands;

namespace Jajo.Exporter.EntryPoint;

public static class Buttons
{
    public static void CreateButton(RibbonPanel panel)
    {
        var showButton = panel.AddPushButton<ExporterCommand>("Exporter");
        
        showButton.SetImage("/Jajo.Exporter;component/Resources/Icons/JajoLogo32.png");
        showButton.SetLargeImage("/Jajo.Exporter;component/Resources/Icons/JajoLogo32.png");
        showButton.ToolTip = "Jajo Exporter";
        showButton.LongDescription = "This application is used to...";
    }
}