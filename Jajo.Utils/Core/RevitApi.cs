using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Jajo.Utils.Core;

/// <summary>
///     The class contains wrapping methods for working with the Revit API.
/// </summary>
public static class RevitApi
{
    public static UIControlledApplication UiControlledApplication { get; set; }
    public static UIApplication UiApplication { get; set; }
    public static UIDocument UiDocument => UiApplication.ActiveUIDocument;
    public static Document Document => UiApplication.ActiveUIDocument.Document;
    public static Application Application => UiApplication.Application;

    public static List<ViewSheet> GetSheets(string phase)
    {
        List<ViewSheet> _sheets = new List<Autodesk.Revit.DB.ViewSheet>();
        foreach (Autodesk.Revit.DB.ViewSheet v in (new FilteredElementCollector(Document).OfClass(typeof(Autodesk.Revit.DB.ViewSheet))))
        {
            if (v.LookupParameter("Sheet Category").AsString() == null)
            {
                continue;
            }
            else if (!v.Name.Contains("Tekeningenlijst") && v.LookupParameter("Sheet Category").AsString() == "Documentatie")
            {
                continue;
            }
            _sheets.Add(v);
        }
        _sheets = _sheets.OrderBy(o => o.Title).ToList();
        return _sheets;
    }

    public static List<View> GetViews(string phase)
    {
        List<View> _views = new List<View>();
        foreach (Autodesk.Revit.DB.View v in (new FilteredElementCollector(Document).OfClass(typeof(Autodesk.Revit.DB.View))))
        {
            foreach (var p in v.GetParameters("Project Views"))
            {
                if (p.AsString() == "JaJo_IFC export" && !v.IsTemplate)
                {
                    _views.Add(v);
                }
            }
        }
        return _views;
    }
}