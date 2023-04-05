using Autodesk.Revit.UI;
using Jajo.Tools.Commands;

namespace Jajo.Tools.EntryPoint;

public static class Buttons
{
    public static void CreateButton(RibbonPanel panel)
    {
        var showButton = panel.AddPushButton<ToolsCommand>("Tools");
        
        showButton.SetImage("/Jajo.Tools;component/Resources/Icons/JajoLogo32.png");
        showButton.SetLargeImage("/Jajo.Tools;component/Resources/Icons/JajoLogo32.png");
        showButton.ToolTip = "Jajo Tools";
        showButton.LongDescription = "This application is used to...";
    }
}