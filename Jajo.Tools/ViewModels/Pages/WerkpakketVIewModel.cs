using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jajo.Ui.Common;

namespace Jajo.Tools.ViewModels.Pages;

public partial class WerkpakketViewModel : PageBaseViewModel
{
    private ObservableCollection<CompanyExample> _companies = new();
    private string _workpackageName;

    public WerkpakketViewModel()
    {
        // Insert logic of initial collections
        CreateCompaniesCollection();
    }

    public CompanyExample SelectedCompany { get; set; }

    /// <summary>
    /// Name of the custom workpackage
    /// </summary>
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
        SnackbarService.Show("Update completed", ControlAppearance.Success);
    }

    [RelayCommand]
    private void CreateStandardWorkpackage()
    {
        SnackbarService.Show("Workpackage created", ControlAppearance.Success);
    }

    [RelayCommand]
    private void CreateCustomWorkpackage()
    {
        SnackbarService.Show("Workpackage created", ControlAppearance.Success);
    }

    [RelayCommand]
    private void Help()
    {
        MessageBox.Show("Help button clicked");
    }

    private void CreateCompaniesCollection()
    {
        Companies = new ObservableCollection<CompanyExample>
        {
            new() { Name = "Hercuton" },
            new() { Name = "SBP" },
            new() { Name = "Remco" },
            new() { Name = "Pvvt" }
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

    public string Name { get; set; }
}