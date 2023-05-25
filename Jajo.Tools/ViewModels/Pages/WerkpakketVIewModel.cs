using System.Collections.ObjectModel;
using System.Windows;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jajo.Tools.Commands.Handlers;
using Jajo.Ui.Common;

namespace Jajo.Tools.ViewModels.Pages;

public partial class WerkpakketViewModel : PageBaseViewModel
{
    private ObservableCollection<CompanyExample> _companies = new();
    private string _workpackageName;
    public List<ElementId> categories;

    private GenerateWorkpackagesEventHandler _generateWorkpackagesEventHandler = new();
    private ExtraWorkpackageEventHandler _extraWorkpackageEventHandler = new();
    private UpdateWorkpackagesEventHandler _updateWorkpackagesEventHandler = new();

    public WerkpakketViewModel()
    {
        // Insert logic of initial collections
        CreateCompaniesCollection();
        CreateCategoryList();
    }

    public CompanyExample SelectedCompany { get; set; }

    public string WorkpackageName
    {
        get => _workpackageName;
        set => SetProperty(ref _workpackageName, value);
    }

    // In real project you should change type
    public ObservableCollection<CompanyExample> Companies
    {
        get => _companies;
        private set => SetProperty(ref _companies, value);
    }


    [RelayCommand]
    private void UpdateSelectedCompany()
    {
        SelectedCompany = Companies.FirstOrDefault(c => c.IsSelected);
    }

    [RelayCommand]
    private void Update()
    {
        _updateWorkpackagesEventHandler.Raise();
        SnackbarService.Show("Update completed", ControlAppearance.Success);
    }

    [RelayCommand]
    private void CreateStandardWorkpackage()
    {
        try
        {
            _generateWorkpackagesEventHandler.prefix = SelectedCompany.Prefix;
            _generateWorkpackagesEventHandler.categories = categories;
            _generateWorkpackagesEventHandler.Raise();
            SnackbarService.Show("Workpackages created", ControlAppearance.Success);
        }
        catch (Exception)
        {
            SnackbarService.Show("Failure!", ControlAppearance.Failure);
        }
        
    }

    [RelayCommand]
    private void CreateCustomWorkpackage()
    {
        _extraWorkpackageEventHandler.prefix = SelectedCompany.Prefix; 
        _extraWorkpackageEventHandler.categories = categories;
        _extraWorkpackageEventHandler.viewName = WorkpackageName;
        _extraWorkpackageEventHandler.Raise();
        SnackbarService.Show($"{SelectedCompany.Prefix}_{WorkpackageName} created", ControlAppearance.Success);
    }

    [RelayCommand]
    private void Help()
    {
        MessageBox.Show(SelectedCompany.Prefix);
    }

    private void CreateCompaniesCollection()
    {
        Companies = new ObservableCollection<CompanyExample>
        {
            new() { Name = "Hercuton", Prefix = "HER" },
            new() { Name = "SBP", Prefix = "SBP" },
            new() { Name = "Remco", Prefix = "REM" },
            new() { Name = "Pvvt", Prefix = "PVVT" }
        };
    }

    private void CreateCategoryList()
    {
        categories = new List<ElementId>{
            new ElementId(BuiltInCategory.OST_Walls),
            new ElementId(BuiltInCategory.OST_Ceilings),
            new ElementId(BuiltInCategory.OST_Roofs),
            new ElementId(BuiltInCategory.OST_Floors),
            new ElementId(BuiltInCategory.OST_Stairs),
            new ElementId(BuiltInCategory.OST_StairsRailing),
            new ElementId(BuiltInCategory.OST_DuctTerminal),
            new ElementId(BuiltInCategory.OST_ElectricalEquipment),
            new ElementId(BuiltInCategory.OST_Casework),
            new ElementId(BuiltInCategory.OST_LightingFixtures),
            new ElementId(BuiltInCategory.OST_PlumbingFixtures),
            new ElementId(BuiltInCategory.OST_SpecialityEquipment),
            new ElementId(BuiltInCategory.OST_GenericModel),
            new ElementId(BuiltInCategory.OST_StructuralColumns),
            new ElementId(BuiltInCategory.OST_StructuralFraming),
            new ElementId(BuiltInCategory.OST_CurtainWallMullions),
            new ElementId(BuiltInCategory.OST_Parking),
            new ElementId(BuiltInCategory.OST_CurtainWallPanels),
            new ElementId(BuiltInCategory.OST_Doors),
            new ElementId(BuiltInCategory.OST_Windows),
            new ElementId(BuiltInCategory.OST_Ramps),
            new ElementId(BuiltInCategory.OST_EdgeSlab),
            new ElementId(BuiltInCategory.OST_PipeFitting),
            new ElementId(BuiltInCategory.OST_PipeCurves),
            new ElementId(BuiltInCategory.OST_Entourage),
            new ElementId(BuiltInCategory.OST_Columns),
            new ElementId(BuiltInCategory.OST_ElectricalFixtures),
            new ElementId(BuiltInCategory.OST_LightingDevices),
            new ElementId(BuiltInCategory.OST_MechanicalEquipment)
            };
    }
}

/// <summary>
///     Example class of a tab
/// </summary>
public class CompanyExample : ObservableObject
{
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Prefix { get; set; }
    public string Name { get; set; }
}