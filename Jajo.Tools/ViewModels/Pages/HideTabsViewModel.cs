using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Autodesk.Windows;
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

    private ICollection<TabExample> SelectedTabs
    {
        get => _selectedTabs;
        set => SetProperty(ref _selectedTabs, value);
    }

    public bool IsMainButtonAvailable
    {
        get => _isMainButtonAvailable;
        private set => SetProperty(ref _isMainButtonAvailable, value);
    }

    private void CreateTabCollection()
    {
        Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
        string[] input = { "BIM", "Create", "In-Place Model", "In-Place Mass", "Zone", "Family Editor", "Jajo" };
        List<string> tabTitles = new List<string>();
        tabTitles.AddRange(input);
        Tabs = new ObservableCollection<TabExample>();

        foreach (Autodesk.Windows.RibbonTab tab in ribbon.Tabs)
        {
            if (tabTitles.Contains(tab.Title))
            {
                continue;
            }
            Tabs.Add(new TabExample { Name = tab.Title, IsSelected = tab.IsVisible, RevitTab = tab});
            tabTitles.Add(tab.Title);
        }

    }

    private void SaveSettings(bool hide)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        path = path + "\\hideTabsSettings.txt";
        File.WriteAllText(path, "Revit Hide Tabs Settings\n");
        File.AppendAllText(path, hide.ToString());
        foreach (var item in SelectedTabs)
        {
            File.AppendAllText(path, "\n" + item.Name);
        }
    }

    [RelayCommand]
    private void ShowTabs()
    {
        try
        {
            SaveSettings(false);
            foreach (var tab in Tabs)
            {
                tab.RevitTab.IsVisible = false;
                if (tab.IsSelected) tab.RevitTab.IsVisible = true;
            }
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
            SaveSettings(true);
            foreach (var tab in Tabs)
            {
                tab.RevitTab.IsVisible = true;
                if (tab.IsSelected) tab.RevitTab.IsVisible = false;
            }
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
        MessageBox.Show("Kies welke Revit Tabs je wilt laten zien of verstoppen.");
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

    public Autodesk.Windows.RibbonTab RevitTab { get; set; }

    public string Name { get; set; }
}