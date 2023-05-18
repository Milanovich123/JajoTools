using Autodesk.Revit.UI;

namespace Jajo.Tools.EntryPoint;

public static class Buttons
{
    public static void CreateButton(RibbonPanel panel)
    {
        var showButton = panel.AddPushButton<ToolsCommand>("Tools");
        
        showButton.SetLargeImage("/Jajo.Tools;component/Resources/Icons/ToolsExtended32.png");
        showButton.ToolTip = "Jajo Tools";
        showButton.LongDescription = "This application is used to...";
    }
}