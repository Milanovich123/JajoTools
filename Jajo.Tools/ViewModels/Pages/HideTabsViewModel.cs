using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jajo.Ui.Common;

namespace Jajo.Tools.ViewModels.Pages;

public partial class HideTabsViewModel : PageBaseViewModel
{
    private bool _isMainButtonAvailable;

    private ICollection<TabExample> _selectedTabs;
    private ObservableCollection<TabExample> _tabs = new();

    public HideTabsViewModel()
    {
        // Insert logic to set tabs collection
        CreateTabCollection();
    }

    // In real project you should change type
    public ObservableCollection<TabExample> Tabs
    {
        get => _tabs;
        private set => SetProperty(ref _tabs, value);
    }

    public ICollection<TabExample> SelectedTabs
    {
        get => _selectedTabs;
        set => SetProperty(ref _selectedTabs, value);
    }

    public bool IsMainButtonAvailable
    {
        get => _isMainButtonAvailable;
        set => SetProperty(ref _isMainButtonAvailable, value);
    }

    public void CreateTabCollection()
    {
        Tabs = new ObservableCollection<TabExample>
        {
            new() { Name = "Architecture" },
            new() { Name = "Structure" },
            new() { Name = "Steel" },
            new() { Name = "Precast" },
            new() { Name = "Systems" },
            new() { Name = "Annotate" },
            new() { Name = "View" },
            new() { Name = "DiRoots" },
            new() { Name = "Modify" },
            new() { Name = "Insert" }
        };
    }

    [RelayCommand]
    private void ShowTabs()
    {
        try
        {
            SnackbarService.Show($"{SelectedTabs.Count} tabs were shown", ControlAppearance.Success);
        }
        catch (Exception)
        {
            SnackbarService.Show("Failure!", ControlAppearance.Failure);
        }
    }

    [RelayCommand]
    private void HideTabs()
    {
        try
        {
            SnackbarService.Show($"{SelectedTabs.Count} tabs were hidden", ControlAppearance.Success);
        }
        catch (Exception)
        {
            SnackbarService.Show("Failure!", ControlAppearance.Failure);
        }
    }

    [RelayCommand]
    private void Help()
    {
        MessageBox.Show("Help button clicked");
    }

    [RelayCommand]
    private void UpdateChBoxesState()
    {
        SelectedTabs = Tabs.Where(i => i.IsSelected).ToList();

        IsMainButtonAvailable = SelectedTabs.Count > 0;
    }

    [RelayCommand]
    private void ChangeAllTabsSelection(object o)
    {
        if (o is not bool boolValue) return;

        foreach (var view in Tabs) view.IsSelected = boolValue;

        UpdateChBoxesState();
    }
}

/// <summary>
///     Example class of a tab
/// </summary>
public class TabExample : ObservableObject
{
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Name { get; set; }
}