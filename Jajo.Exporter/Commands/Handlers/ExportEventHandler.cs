using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Jajo.Utils.Core;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace Jajo.Exporter.Commands.Handlers
{
    public sealed class ExportEventHandler : BaseEventHandler
    {
        public bool dwg { get; set; }
        public List<View> viewList { get; set; }
        public List<ViewSheet> sheetList { get; set; }
        public Document doc { get; set; }

        private int printedSheets = 0;
        private string _projectNumber;
        public string ProjectNumber
        {
            get
            {
                if (_projectNumber != null)
                {
                    return _projectNumber;
                }
                else
                {
                    _projectNumber = doc.ProjectInformation.GetParameters("BHV_projectnummer")[0].AsString();
                    return _projectNumber;
                }
            }
        }

        public override void Execute(UIApplication app)
        {
            doc = app.ActiveUIDocument.Document;
            if (viewList.Count > 0)
            {
                ExportViews();
            }
            if (sheetList.Count > 0)
            {
                ExportSheets();
            }
        }

        private void ExportViews()
        {
            try
            {
                using (Transaction t = new Transaction(doc, "Export Views"))
                {
                    t.Start();
                    //Create an Instance of the IFC Export Class
                    IFCExportOptions IFCExportOptions = new IFCExportOptions();

                    //Create an instance of the IFC Export Configuration Class
                    BIM.IFC.Export.UI.IFCExportConfiguration myIFCExportConfiguration = BIM.IFC.Export.UI.IFCExportConfiguration.CreateDefaultConfiguration();

                    //add settings:
                    myIFCExportConfiguration.StoreIFCGUID = true;
                    myIFCExportConfiguration.VisibleElementsOfCurrentView = true;
                    myIFCExportConfiguration.TessellationLevelOfDetail = 0; //(standaard op low)
                    myIFCExportConfiguration.ExportPartsAsBuildingElements = true;
                    myIFCExportConfiguration.ExportSolidModelRep = true;
                    myIFCExportConfiguration.UseActiveViewGeometry = true;
                    myIFCExportConfiguration.ExportBoundingBox = true;
                    myIFCExportConfiguration.IncludeSteelElements = false;
                    myIFCExportConfiguration.ExportIFCCommonPropertySets = false;
                    myIFCExportConfiguration.Export2DElements = true;
                    myIFCExportConfiguration.ExportRoomsInView = false;
                    myIFCExportConfiguration.ExportUserDefinedParameterMapping = true;
                    myIFCExportConfiguration.ExportUserDefinedPsets = true;
                    myIFCExportConfiguration.ExportUserDefinedParameterMappingFileName = @"V:\Revit\01 Configuratie bestanden\03 IFC\NLRS3.0.1_IFC Export Mapping Table.txt";
                    myIFCExportConfiguration.ExportUserDefinedPsetsFileName = @"V:\Revit\01 Configuratie bestanden\03 IFC\JaJo_ParameterSets.txt";


                    //Define the output Directory for the IFC Export
                    var projectNumber = doc.ProjectInformation.GetParameters("BHV_projectnummer")[0].AsString();
                    if (projectNumber == "" || projectNumber == null)
                    {
                        projectNumber = "404 projectnumber not found";
                    }

                    // Exports go to S schijf.
                    string dir = "S:\\Exports";
                    dir += "\\" + projectNumber;
                    DirectoryInfo di = Directory.CreateDirectory(dir);

                    string version = string.Empty;
                    int number = 0;
                    try
                    {
                        version = doc.ProjectInformation.LookupParameter("JaJo_template_version").AsString();
                        string numberstring = new string(version.Where(char.IsDigit).Take(2).ToArray());
                        int.TryParse(numberstring, out number);
                    }
                    catch
                    {
                    }

                    //Iterate through each of the selected views
                    foreach (var v in viewList)
                    {
                        myIFCExportConfiguration.ExportRoomsInView = false;

                        if (string.IsNullOrEmpty(version) || number < 26)
                        {
                            //If it's bouwkundig (or in new versions "Ruimtes") it should export Rooms
                            if (v.Name.Contains("bouwkundig") || v.Name.Contains("Bouwkundig"))
                            {
                                myIFCExportConfiguration.ExportRoomsInView = true;
                            }
                        }
                        else
                        {
                            if (v.Name.Contains("Ruimtes") || v.Name.Contains("ruimtes"))
                            {
                                myIFCExportConfiguration.ExportRoomsInView = true;
                            }
                        }

                        //Pass the setting of the myIFCExportConfiguration to the IFCExportOptions
                        myIFCExportConfiguration.UpdateOptions(IFCExportOptions, v.Id);
                        string nu = DateTime.Now.ToString("dd-MM-yyyy");
                        string name = v.Name;
                        string viewName = projectNumber + "_" + name;
                        string path = dir + "\\" + viewName;
                        try
                        {
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                                doc.Export(dir, viewName, IFCExportOptions);
                            }
                            else
                            {
                                doc.Export(dir, viewName, IFCExportOptions);
                            }
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Export Error", "Export failed: " + ex.Message);
                        }

                        try
                        {
                            string text = doc.ProjectInformation.LookupParameter("NLRS_C_IFC_files").AsString();
                            string phrase = text;
                            string[] words = phrase.Split('|');
                            List<string> wordslist = words.ToList();

                            foreach (var word in words)
                            {
                                if (word.Contains(v.Name))
                                {
                                    wordslist.Remove(word);
                                }
                            }
                            text = "| ";
                            foreach (var word in wordslist)
                            {
                                if (word.Length > 1)
                                {
                                    text += word.Trim() + " | ";
                                }
                            }
                            string newtext = text + projectNumber + "_" + v.Name + ".ifc (" + nu + ") | ";
                            doc.ProjectInformation.LookupParameter("NLRS_C_IFC_files").Set(newtext);

                        }
                        catch (Exception ex)
                        {
                            Autodesk.Revit.UI.TaskDialog.Show("Error with NLRS parameter", ex.Message);
                        }
                        //Delete txt file
                        string[] files = System.IO.Directory.GetFiles(dir);
                        foreach (string s in files)
                        {
                            if (s.Contains(v.Name) && Path.GetExtension(s) == ".log")
                            {
                                File.Delete(s);
                            }
                        }

                    }
                    if (sheetList.Count == 0)
                    {
                        TaskDialog td = new TaskDialog("Succes");
                        td.MainInstruction = "IFC('s) have been succesfully saved on this location: ";
                        td.MainContent = dir;
                        td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Open file location.");
                        td.CommonButtons = TaskDialogCommonButtons.Ok;
                        td.DefaultButton = TaskDialogResult.Ok;
                        TaskDialogResult tdRes = td.Show();
                        if (tdRes == TaskDialogResult.CommandLink1)
                        {
                            Process.Start(dir);
                        }
                    }
                    t.Commit();
                }//end transaction
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error while exporting manually", ex.Message);
            }
        }

        private void ExportSheets()
        {
            printedSheets = sheetList.Count;
            string msg = "De volgende sheet(s) heb ik niet kunnen printen:\n";
            bool unprintable = false;
            using (Transaction t = new Transaction(doc, "PDF Printing"))
            {
                t.Start();
                foreach (var sheet in sheetList)
                {
                    ElementId e = sheet.Id;
                    FamilyInstance sheet_instance = new FilteredElementCollector(doc, e).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_TitleBlocks).FirstOrDefault() as FamilyInstance;
                    if (sheet_instance == null)//view chosen
                    {
                        msg += "    -" + sheet.Name + ": heeft geen titleblock.\n";
                        printedSheets--;
                        unprintable = true;
                        continue;
                    }
                    if (sheet_instance != null)
                    {
                        try
                        {
                            var printmgr = doc.PrintManager;
                            printmgr.PrintSetup.CurrentPrintSetting = printmgr.PrintSetup.InSession;
                            printmgr.SelectNewPrintDriver("PDF24");

                            // Printsetting should be the same as sheet name.
                            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(PrintSetting));
                            PrintSetting set1 = null;
                            foreach (PrintSetting printSetting in col)
                            {
                                if (printSetting.Name == sheet_instance.Name)
                                {
                                    set1 = printSetting;
                                    break;
                                }
                            }
                            // No printsetting found that matches, stop printing this sheet.
                            if (set1 == null)
                            {
                                printedSheets--;
                                msg += "    -" + sheet.Name + ": Can't find printsetting that matches: " + sheet_instance.Name + "\n";
                                unprintable = true;
                                continue;
                            }
                            try
                            {
                                printmgr.PrintSetup.CurrentPrintSetting = set1;

                            }
                            catch (Exception ex)
                            {
                                printedSheets--;
                                msg += "    -" + sheet.Name + ": Failed to apply print settings " + ex.Message + "\n";
                                unprintable = true;
                                continue;
                            }
                            if (dwg)
                            {
                                exportDwg(sheet);
                            }

                            string finalName = "";
                            string path = @"S:\Exports\";
                            string vname = removeSpecialCharacters(sheet.Name);
                            try
                            {
                                finalName = path + ProjectNumber + " " + sheet.SheetNumber + " " + vname + ".pdf";
                                printmgr.PrintToFile = true;
                                printmgr.PrintToFileName = finalName;
                            }
                            catch (Exception ex)
                            {
                                printedSheets--;
                                msg += "    -" + sheet.Name + ": pdfExport error with name. " + ex.Message + "\n";
                                unprintable = true;
                                continue;
                            }
                            try
                            {
                                printmgr.Apply();
                            }
                            catch (Exception ex)
                            {
                                printedSheets--;
                                msg += "    -" + sheet.Name + ": Failed to apply print settings" + ex.Message + "\n";
                                unprintable = true;
                                continue;
                            }
                            try
                            {
                                printmgr.SubmitPrint(sheet as View);
                            }
                            catch (Exception ex)
                            {
                                printedSheets--;
                                msg += "    -" + sheet.Name + ": pdfExport failed to submit print. " + ex.Message + "\n";
                                unprintable = true;
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            printedSheets--;
                            msg += "    -" + sheet.Name + ": Unable to print sheet: " + ex.Message + "\n";
                            unprintable = true;
                        }
                    }
                }
                bool waitingForFiles = true;
                int waitingtime = 60;
                while (waitingForFiles)
                {
                    Thread.Sleep(500);
                    if (detectFiles() || waitingtime == 0)
                    {
                        waitingForFiles = false;
                    }
                    waitingtime--;
                }
                moveFiles();
                if (unprintable)
                {
                    TaskDialog.Show("Unprintable", msg + "Controleer of er een printinstelling is met dezelfde naam als de typename van het titleblock van deze sheet(s).");
                }
                string nmbr = doc.ProjectInformation.LookupParameter("BHV_projectnummer").AsString();


                string dir = "S:\\Exports\\";
                TaskDialog td = new TaskDialog("Succes");
                td.MainInstruction = "Je bestanden zijn succesvol opgeslagen op deze locatie: ";
                td.MainContent = dir + nmbr;
                td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Open file location.");
                td.CommonButtons = TaskDialogCommonButtons.Ok;
                td.DefaultButton = TaskDialogResult.Ok;
                TaskDialogResult tdRes = td.Show();
                if (tdRes == TaskDialogResult.CommandLink1)
                {
                    Process.Start(dir + nmbr);
                }
                t.Commit();
            }

        }

        private void moveFiles()
        {
            try
            {
                string nmbr = ProjectNumber;
                string sourcePath = @"S:\Exports";
                string targetPath = sourcePath + "\\" + nmbr;
                System.IO.Directory.CreateDirectory(targetPath);
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        if (s.Contains(nmbr))
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string fileName = System.IO.Path.GetFileName(s);
                            string destFile = System.IO.Path.Combine(targetPath, fileName);
                            System.IO.File.Copy(s, destFile, true);
                            File.Delete(s);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private bool detectFiles()
        {
            int count = 0;
            int targetcount = printedSheets;
            string nmbr = ProjectNumber;
            string sourcePath = @"S:\Exports";

            string[] files = System.IO.Directory.GetFiles(sourcePath);
            foreach (string s in files)
            {
                if (s.Contains(nmbr))
                {
                    count++;
                }
            }
            if (count == targetcount)
            {
                return true;
            }
            return false;
        }

        private void exportDwg(ViewSheet sheet)
        {
            try
            {
                string dir = "S:\\Exports";
                dir += "\\" + ProjectNumber;
                DirectoryInfo di = Directory.CreateDirectory(dir);

                DWGExportOptions DWGExportOptions = new DWGExportOptions();
                ExportDWGSettings dwgSettings = ExportDWGSettings.GetActivePredefinedSettings(doc);
                if (dwgSettings != null)
                {
                    DWGExportOptions = dwgSettings.GetDWGExportOptions();
                }
                else
                {
                    DWGExportOptions.MergedViews = true;
                    DWGExportOptions.FileVersion = ACADVersion.R2013;

                }

                ICollection<ElementId> col = new List<ElementId>
            {
                sheet.Id
            };
                string vname = removeSpecialCharacters(sheet.Name);
                string viewName = ProjectNumber + " " + sheet.SheetNumber + " " + vname;
                string path = dir + "\\" + viewName;
                if (File.Exists(path))
                {
                    File.Delete(path);
                    doc.Export(dir, viewName, col, DWGExportOptions);
                }
                else
                {
                    doc.Export(dir, viewName, col, DWGExportOptions);
                }

                try
                {
                    string[] files = System.IO.Directory.GetFiles(dir);
                    foreach (string s in files)
                    {
                        if (s.Contains(ProjectNumber) && Path.GetExtension(s) == ".pcp" || Path.GetExtension(s) == ".jpg" || Path.GetExtension(s) == ".JPG" || Path.GetExtension(s) == ".png")
                        {
                            File.Delete(s);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("error pcp", "Failed to delete pcp file: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error with DWG", "Something went wrong while trying to export DWG: " + ex.Message);
            }
        }

        private string removeSpecialCharacters(string name)
        {
            string allowedChars = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-+,.!@#$%&^()_=";

            StringBuilder sb = new StringBuilder();
            foreach (char c in name)
            {
                if (allowedChars.Contains(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
