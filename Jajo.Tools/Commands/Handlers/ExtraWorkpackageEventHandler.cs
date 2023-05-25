using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jajo.Tools.Commands.Handlers
{
    public sealed class ExtraWorkpackageEventHandler : BaseEventHandler
    {
        public Document doc { get; set; }
        public string viewName { get; set; }
        public List<ElementId> categories { get; set; }
        public string prefix { get; set; }
        public List<string> views { get; set; }
        public List<string> filterValues { get; set; }
        public ElementId werkpakketParameterId { get; set; }

        public override void Execute(UIApplication app)
        {
            doc = app.ActiveUIDocument.Document;
            try
            {
                var direction = new XYZ(-1, 1, -1);
                var collector = new FilteredElementCollector(doc);
                var viewFamilyType = collector.OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>()
                  .FirstOrDefault(x => x.ViewFamily == ViewFamily.ThreeDimensional);

                using (Transaction ttNew = new Transaction(doc, "abc"))
                {
                    ttNew.Start();
                    var view3D = View3D.CreateIsometric(
                                      doc, viewFamilyType.Id);

                    view3D.SetOrientation(new ViewOrientation3D(
                      direction, new XYZ(0, 1, 1), new XYZ(0, 1, -1)));
                    view3D.Name = prefix + "_" + viewName;

                    View viewTemplate = (from v in new FilteredElementCollector(doc)
                        .OfClass(typeof(View))
                        .Cast<View>()
                                         where v.IsTemplate == true && v.Name == "JaJo_werkpakket_basis"
                                         select v)
                        .First();

                    view3D.ViewTemplateId = viewTemplate.Id;

                    ParameterFilterElement parameterFilterElement;
                    try
                    {
                        parameterFilterElement = ParameterFilterElement.Create(doc, viewName, categories);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    // Create the filter, the name is equal to the viewname - prefix

                    List<FilterRule> filterRules = new List<FilterRule>();

                    // Find JaJo_werkpakket shared parameter
                    FilteredElementCollector fec = new FilteredElementCollector(doc);
                    fec.OfClass(typeof(Wall));
                    Wall wall = fec.FirstElement() as Wall;
                    werkpakketParameterId = new ElementId(BuiltInParameter.FUNCTION_PARAM);
                    if (wall != null)
                    {
                        try
                        {
                            Parameter werkpakketParameter = wall.LookupParameter("JaJo_werkpakket");
                            werkpakketParameterId = werkpakketParameter.Id;
                        }
                        catch (Exception)
                        {
                            TaskDialog.Show("No werkpakket found", "The wall that was found does not have JaJo_werkpakket. WallName: " + wall.Name);
                            return;
                        }
                    }
                    else
                    {
                        TaskDialog.Show("No wall", "Please create at least one wall before adding werkpakketten.");
                        return;
                    }

                    try
                    {
                        // A rule that says JaJo_werkpakket does not equal coordinatie and a rule that says does not equal viewname
                        filterRules.Add(ParameterFilterRuleFactory.CreateNotEqualsRule(werkpakketParameterId, "coordinatie", false));
                        if (viewName == "EMPTY")
                        {
                            filterRules.Add(ParameterFilterRuleFactory.CreateNotEqualsRule(werkpakketParameterId, "", false));
                        }
                        else
                        {
                            //filterRules.Add(ParameterFilterRuleFactory.CreateNotEqualsRule(werkpakketParameterId, viewName, false));
                            filterRules.Add(ParameterFilterRuleFactory.CreateNotContainsRule(werkpakketParameterId, viewName, false));
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", "Failed to create rules " + ex.Message);
                    }

                    string msg = "Tracing:\n";
                    try
                    {
                        // Converts the filter rules to elementfilter
                        List<ElementFilter> elemFilters = new List<ElementFilter>();
                        foreach (FilterRule filterRule in filterRules)
                        {
                            ElementParameterFilter elemParamFilter = new ElementParameterFilter(filterRule);
                            elemFilters.Add(elemParamFilter);
                        }

                        LogicalAndFilter elemFilter = new LogicalAndFilter(elemFilters);

                        // Checks if the rules are valid for this filters categories ( Returns false )
                        if (parameterFilterElement.AllRuleParametersApplicable(elemFilter))
                        {
                            msg += "rules are applicable\n";
                        }

                        // Sets the elementfilters to the parameterfilterelement
                        parameterFilterElement.SetElementFilter(elemFilter);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", "Failed to set elementfilter\n" + msg + "\n" + ex.Message);
                        return;
                    }

                    try
                    {
                        // Apply filter to view
                        view3D.AddFilter(parameterFilterElement.Id);

                        // So the filters have grabbed everything except the things where werkpakket == viewname and coordination
                        // now with this code it makes them invisible, leaving you with only the things that belong in that view and coordination
                        view3D.SetFilterVisibility(parameterFilterElement.Id, false);
                    }
                    catch (Exception e)
                    {
                        TaskDialog.Show("Title", "Failed to add filter to view: " + e.Message);
                    }


                    ttNew.Commit();

                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("No", "Failed to create view: " + ex.Message);
                return;
            }
        }
    }
}
