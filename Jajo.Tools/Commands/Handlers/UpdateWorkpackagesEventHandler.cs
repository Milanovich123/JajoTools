using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
using System.Xml.Serialization;

namespace Jajo.Tools.Commands.Handlers
{
    public sealed class UpdateWorkpackagesEventHandler : BaseEventHandler
    {
        public Document doc { get; set; }
        public override void Execute(UIApplication app)
        {
            doc = app.ActiveUIDocument.Document;
            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(ParameterElement));
            List<string> names = new List<string>();

            string parameterName = "";
            ElementId parameterId = null;

            // Find werkpakket parameter id
            foreach (Element f in col)
            {
                string name = f.Name;
                names.Add(name);
                if (name == "JaJo_werkpakket")
                {
                    parameterName = name;
                    parameterId = f.Id;
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
            string path = @"W:\03 Scripting\Library\xmlPaketten";
            string debug = "Remaining elements will be set to unclassified:\n";
            string errormessage = "Log to check how far the script got: \n\n";
            try
            {
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

                        errormessage += "File: " + filename + "\n";

                        // Check if this pakket still exists, skip if it doesnt
                        if (!CheckIfExists(pakketName))
                        {
                            continue;
                        }

                        List<element> xmlValues = ResultFromXML(pakketName);
                        //debugValues(xmlValues, pakketName, elements);
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
                                    errormessage += "jajo_name: " + elem.name + "\n";
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
                                    // JaJo name was not found
                                    elem.name = "";
                                }
                                try
                                {
                                    // set the nlsfb
                                    elem.nlsfb = type.LookupParameter("Assembly Code").AsString();
                                }
                                catch (Exception)
                                {
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
                    try
                    {
                        foreach (Element item in elements)
                        {
                            debug += item.Name + "\n";
                            item.get_Parameter(guid).Set("UNCLASSIFIED");
                        }
                    }
                    catch (Exception e)
                    {
                        TaskDialog.Show("Error", "Failed to set unclassified value: " + e.Message);
                    }

                    t.Commit();
                }
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error", "Failed to update werkpakketten value: " + e.Message);
                return;
            }

        }

        private bool CheckIfExists(string pakketName)
        {
            bool truth = false;
            foreach (Autodesk.Revit.DB.View v in (new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.View))))
            {
                foreach (var p in v.GetParameters("Project Views"))
                {
                    if (p.AsString() == "JaJo_IFC export" && !v.IsTemplate)
                    {
                        if (v.Name.Contains("_" + pakketName))
                        {
                            //TaskDialog.Show("Found in Project Views", v.Name + " contains _" + pakketName);
                            truth = true;
                        }
                    }
                }
                foreach (var p in v.GetParameters("Views_submap"))
                {
                    if (p.AsString() == "00. Alle fases" && !v.IsTemplate)
                    {
                        if (v.Name.Contains("_" + pakketName))
                        {
                            //TaskDialog.Show("Found in Views_submap", v.Name + " contains _" + pakketName);
                            truth = true;
                        }
                    }
                }

            }
            return truth;
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
}
