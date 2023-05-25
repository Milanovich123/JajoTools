using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Jajo.Utils.Core;
using System.IO;
using System.Xml.Serialization;

namespace Jajo.Tools.Commands.Handlers
{
    public sealed class GenerateWorkpackagesEventHandler : BaseEventHandler
    {
        public Document doc { get; set; }
        public List<ElementId> categories { get; set; }
        public string prefix { get; set; }
        public List<string> views { get; set; }
        public ElementId werkpakketParameterId { get; set; }
        public override void Execute(UIApplication app)
        {
            doc = app.ActiveUIDocument.Document;
            string path = @"W:\03 Scripting\Library\werkpakketten";
            try
            {
                string tmsg = "";
                views = new List<string>();
                // Get View Names From File
                string[] files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    //trim the directory and .txt to get the name of werkpakket
                    string filename = file.Substring(file.LastIndexOf('\\') + 1);
                    string viewName = filename.Substring(0, filename.Length - 4);
                    //string werkpaketName = prefix + "_" + viewName;
                    tmsg += viewName + "\n";
                    views.Add(viewName);
                }
                //TaskDialog.Show("Title", "Created views:\n" + tmsg);
            }
            catch (Exception)
            {
                TaskDialog.Show("Error", "Failed to find werkpakketten in: " + path);
                return;
            }

            // Create views and filters
            createView(doc);

            // Find werkpakket parameter id
            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(ParameterElement));
            string parameterName = "";
            ElementId parameterId = null;
            foreach (Element f in col)
            {
                string name = f.Name;
                if (name == "JaJo_werkpakket")
                {
                    parameterName = name;
                    parameterId = f.Id;
                    break;
                }
            }

            // Filter only objects with parameter JaJo_werkpakket
            SharedParameterApplicableRule fRule = new SharedParameterApplicableRule(parameterName);
            ElementParameterFilter filter = new ElementParameterFilter(fRule);

            // Filter only objects where the parameter is empty
            ParameterValueProvider pvp = new ParameterValueProvider(parameterId);
            FilterStringEquals fsc = new FilterStringEquals();
            string ruleValue = "";
            FilterStringRule f2Rule = new FilterStringRule(pvp, fsc, ruleValue, false);
            ElementParameterFilter filter2 = new ElementParameterFilter(f2Rule);

            // List of elements with empty werkpakket parameter
            List<Element> elements = new FilteredElementCollector(doc).WherePasses(filter).WherePasses(filter2).WhereElementIsNotElementType().ToElements().ToList();

            // For each file in the following path, check if element is in the file and if so give it that werkpakket
            path = @"W:\03 Scripting\Library\xmlPaketten";
            try
            {
                DateTime nu = DateTime.Now;
                // Guid for JaJo_Werkpakket shared parameter
                var guid = new Guid("42de1111-6399-48d3-809a-6cc49afe982a");
                using (Transaction t = new Transaction(doc, "set werkpakket"))
                {
                    t.Start();
                    // Get werkpakket Names From File
                    string[] files = Directory.GetFiles(path);
                    foreach (var file in files)
                    {

                        // Trim the directory and .txt to get the name of werkpakket
                        string filename = file.Substring(file.LastIndexOf('\\') + 1);
                        string pakketName = filename.Substring(0, filename.Length - 4);

                        List<element> xmlValues = null;
                        try
                        {
                            xmlValues = ResultFromXML(pakketName);
                            if (xmlValues == null)
                            {
                                continue;
                            }

                        }
                        catch (Exception e)
                        {
                            TaskDialog.Show("Error", "Error retrieving result from XML for " + pakketName + ": " + e.Message);
                        }

                        // For all the elements we start at the last one and when a werkpakket has been found we remove it from the list
                        for (int i = elements.Count - 1; i >= 0; i--)
                        {
                            // We create a new custom class element which only contains jajo_name and nlsfb
                            element elem = new element();
                            Element type = null;
                            try
                            {
                                // get the type of the instance
                                type = doc.GetElement(elements[i].GetTypeId());
                                try
                                {
                                    // set the newly created class's jajoname
                                    elem.name = type.LookupParameter("JaJo_Name").AsString();

                                    // if we happen to find the coordinatie object we can set the werkpakket parameter to coordinatie
                                    if (elem.name == "Coordinatie object")
                                    {
                                        elements[i].get_Parameter(guid).Set("coordinatie");
                                        elements.RemoveAt(i);
                                        continue;
                                    }
                                }
                                catch
                                {
                                    // JaJo name was not found, is there any reason why we need to search for empty jajo_name?
                                    elem.name = "";
                                    //continue; Maybe skip element?
                                }
                                try
                                {
                                    // set the nlsfb
                                    elem.nlsfb = type.LookupParameter("Assembly Code").AsString();
                                }
                                catch (Exception e)
                                {
                                    elem.nlsfb = "";
                                    //TaskDialog.Show("Unable to set nslfb", elem.name + " unable to find assembly code.");
                                    continue;
                                }


                            }
                            catch (Exception)
                            {
                                // Element type was not found, going to next element
                                continue;
                            }

                            // Now we iterate through the preset values defined in xml
                            foreach (var xmlElement in xmlValues)
                            {
                                // if we have a match we set its jajo_werkpakket parameter and break out of the loop to go to next element
                                if (xmlElement.name == elem.name && xmlElement.nlsfb == elem.nlsfb)
                                {
                                    try
                                    {
                                        elements[i].get_Parameter(guid).Set(pakketName);
                                        elements.RemoveAt(i);
                                        break;
                                    }
                                    catch (Exception)
                                    {
                                        continue;
                                    }
                                }
                            }

                        }
                    }

                    // We have now iterated through all the files and elements, everything left in elements is UNCLASSIFIED
                    string vertexName = "MyPropertySetBimforceGUID";
                    string msg = "";
                    try
                    {
                        // Log voor beheer

                        //string msg = "Date and user:," + nu.ToString() + "," + app.Application.Username + "\n" + app.ActiveUIDocument.Document.Title + "\n";
                        msg += "Typename,Familyname,NLSFB,JaJo_Name,JaJo_TypeName,Vertex\n";
                        foreach (Element instance in elements)
                        {
                            // Now we would like to check if for these elements the subcomponent has a value for werkpakket
                            Element item = null;
                            Element x = null;

                            // The HashSet can only contain unique values, so we add the subcomponents werkpakket values to it
                            HashSet<string> uniqueWerkpakketValues = new HashSet<string>();
                            try
                            {
                                item = doc.GetElement(instance.GetTypeId());
                                try
                                {
                                    if (instance is FamilyInstance)
                                    {
                                        FamilyInstance aFamilyInst = instance as FamilyInstance;
                                        var subcomps = aFamilyInst.GetSubComponentIds();
                                        foreach (ElementId subele in subcomps)
                                        {
                                            x = doc.GetElement(subele);
                                            string werkpakket = null;
                                            try
                                            {
                                                werkpakket = x.get_Parameter(guid).AsString();
                                            }
                                            catch
                                            {
                                            }
                                            if (!string.IsNullOrEmpty(werkpakket))
                                            {
                                                uniqueWerkpakketValues.Add(werkpakket);
                                            }
                                        }
                                    }

                                    // If the subcomponents did have values for werkpakket it will give the main component those values aswell
                                    if (uniqueWerkpakketValues.Count > 0)
                                    {
                                        instance.get_Parameter(guid).Set(string.Join("_", uniqueWerkpakketValues));
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    TaskDialog.Show("title", "Error subcomponents. " + x.Name + " does not have a werkpakket. Instance: " + instance.Name + ex.Message);
                                }
                                instance.get_Parameter(guid).Set("UNCLASSIFIED");
                            }
                            catch (Exception e)
                            {
                                TaskDialog.Show("Error", "Error getting type of " + instance.Name + " unable to set werkpakket: " + e.Message);
                            }

                            var famName = "";
                            var j_Name = "";
                            var j_type = "";
                            var nlsfb = "";
                            var vertex = "nee";
                            try
                            {
                                famName = item.LookupParameter("Family Name").AsString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                j_Name = item.LookupParameter("JaJo_Name").AsString();
                                j_type = item.LookupParameter("JaJo_TypeName").AsString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                nlsfb = item.LookupParameter("Assembly Code").AsString();
                            }
                            catch
                            {
                            }
                            try
                            {
                                var schguids = item.GetEntitySchemaGuids();
                                foreach (Guid schG in schguids)
                                {
                                    var schema = Schema.Lookup(schG);
                                    var schname = schema.SchemaName;
                                    if (schname == vertexName)
                                    {
                                        vertex = "ja";
                                        //foreach (var field in schema.ListFields())
                                        //{
                                        //    Entity retrievedEntity = item.GetEntity(schema);
                                        //    var data = retrievedEntity.Get<string>(field.FieldName);
                                        //}
                                    }
                                }
                            }
                            catch
                            {
                            }
                            try
                            {
                                msg += item.Name.Replace(',', '.') + ",";
                                msg += famName.Replace(',', '.') + ",";
                                msg += nlsfb + ",";
                                msg += j_Name.Replace(',', '.') + ",";
                                msg += j_type.Replace(',', '.') + ",";
                                msg += vertex + "\n";
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //TaskDialog.Show("Unable to make log", "Error with making log file: " + e.Message);
                        //TaskDialog.Show("Message", msg);
                    }

                    msg += "\n";

                    path = @"W:\03 Scripting\Library";
                    string logfile = path + "\\log_werkpakketten.csv";

                    File.AppendAllText(logfile, msg);

                    t.Commit();
                }

            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", "Failed to set werkpakketten value: " + e.Message);
                return;
            }
        }

        // From CreateViewClass:
        private void createView(Document doc)
        {
            var existingViews = new FilteredElementCollector(doc).OfClass(typeof(View3D)).ToElements();

            // Convert to strings
            var existingList = new List<string>();
            foreach (var item in existingViews)
            {
                existingList.Add(item.Name);
            }
            // Create views 
            foreach (string name in views)
            {
                // If view already exists skip it
                if (existingList.Contains(prefix + "_" + name))
                {
                    //TaskDialog.Show("Title", prefix + "_" + name + " exists.");
                    continue;
                }

                try
                {
                    var direction = new XYZ(-1, 1, -1);
                    var collector = new FilteredElementCollector(doc);
                    var viewFamilyType = collector.OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>()
                      .FirstOrDefault(x => x.ViewFamily == ViewFamily.ThreeDimensional);

                    using (Transaction ttNew = new Transaction(doc, "create_view"))
                    {
                        ttNew.Start();
                        var view3D = View3D.CreateIsometric(
                                          doc, viewFamilyType.Id);

                        view3D.SetOrientation(new ViewOrientation3D(
                          direction, new XYZ(0, 1, 1), new XYZ(0, 1, -1)));
                        view3D.Name = prefix + "_" + name;

                        View viewTemplate = (from v in new FilteredElementCollector(doc)
                            .OfClass(typeof(View))
                            .Cast<View>()
                                             where v.IsTemplate == true && v.Name == "JaJo_werkpakket_basis"
                                             select v)
                            .First();

                        view3D.ViewTemplateId = viewTemplate.Id;

                        // Try creating the filter, if it already exists it will fail, in that case we grab the existing filter and continue.
                        ParameterFilterElement parameterFilterElement;
                        try
                        {
                            parameterFilterElement = ParameterFilterElement.Create(doc, name, categories);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                parameterFilterElement = (from p in new FilteredElementCollector(doc)
                            .OfClass(typeof(ParameterFilterElement))
                            .Cast<ParameterFilterElement>()
                                                          where p.Name == name
                                                          select p)
                            .First();
                                view3D.AddFilter(parameterFilterElement.Id);
                                view3D.SetFilterVisibility(parameterFilterElement.Id, false);
                            }
                            catch (Exception ex)
                            {
                                TaskDialog.Show("parameter error", "Stopping execution, parameter filter unable to be added to view. \n" + ex.Message);

                                return;
                            }
                            ttNew.Commit();
                            continue;
                        }
                        // Set rules for the filter, the name is equal to the viewname - prefix
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
                            // A rule that says JaJo_werkpakket does not equal coordinatie and a rule that says does not contain viewname
                            filterRules.Add(ParameterFilterRuleFactory.CreateNotEqualsRule(werkpakketParameterId, "coordinatie", false));
                            if (name == "EMPTY")
                            {
                                filterRules.Add(ParameterFilterRuleFactory.CreateNotEqualsRule(werkpakketParameterId, "", false));
                            }
                            else
                            {
                                filterRules.Add(ParameterFilterRuleFactory.CreateNotContainsRule(werkpakketParameterId, name, false));
                                //filterRules.Add(ParameterFilterRuleFactory.CreateNotEqualsRule(werkpakketParameterId, name, false));
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

                            // Checks if the rules are valid for this filters categories
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

        private void debugValues(List<element> xmlValues, string pakket, List<Element> elements)
        {
            string msg = "Values found in " + pakket + ".xml: \n";
            foreach (var item in xmlValues)
            {
                msg += item.name + "\n";
            }
            msg += "elements to check: \n";
            foreach (var item in elements)
            {
                msg += item.Name + "\n";
            }
            TaskDialog.Show("dubug", msg);
        }

        // Method to retrieve list of our custom class 'element' from xml
        private static List<element> ResultFromXML(string werkpakketName)
        {
            List<element> result = new List<element>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<element>));
            string path = @"W:\03 Scripting\Library\xmlPaketten";
            path += "\\" + werkpakketName + ".xml";
            if (File.Exists(path))
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    result = (List<element>)serializer.Deserialize(fs);
                }
                return result;
            }
            return null;
        }

    }
    public class element
    {
        public string name { get; set; }
        public string nlsfb { get; set; }
    }
}
