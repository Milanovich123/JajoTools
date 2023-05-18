using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Jajo.Exporter.ViewModels.Pages;

public abstract partial class PageBaseViewModel : ObservableObject
{
    private ObservableCollection<SheetExample> _exportSheets = new();
    private ObservableCollection<ViewExample> _exportViews = new();
    private bool _isExportToDwgSelected;
    private bool _isMainButtonAvailable;
    private ICollection<SheetExample> _selectedSheets;
    private ICollection<ViewExample> _selectedViews;
    private bool _switchPhaseBoolValue;

    protected PageBaseViewModel()
    {
        // Insert logic of initial collections
        PhaseSwitched();
    }

    // In real project you should change type
    public ObservableCollection<ViewExample> ExportViews
    {
        get => _exportViews;
        private set => SetProperty(ref _exportViews, value);
    }

    public ObservableCollection<SheetExample> ExportSheets
    {
        get => _exportSheets;
        private set => SetProperty(ref _exportSheets, value);
    }

    public ICollection<ViewExample> SelectedViews
    {
        get => _selectedViews;
        set => SetProperty(ref _selectedViews, value);
    }

    public ICollection<SheetExample> SelectedSheets
    {
        get => _selectedSheets;
        set => SetProperty(ref _selectedSheets, value);
    }

    public bool SwitchPhaseBoolValue
    {
        get => _switchPhaseBoolValue;
        set => SetProperty(ref _switchPhaseBoolValue, value);
    }

    public bool IsExportToDwgSelected
    {
        get => _isExportToDwgSelected;
        set => SetProperty(ref _isExportToDwgSelected, value);
    }

    public bool IsMainButtonAvailable
    {
        get => _isMainButtonAvailable;
        set => SetProperty(ref _isMainButtonAvailable, value);
    }

    [RelayCommand]
    protected virtual void Export()
    {
    }

    [RelayCommand]
    private void Help()
    {
        MessageBox.Show("Help button clicked");
    }

    [RelayCommand]
    private void UpdateChBoxesState()
    {
        SelectedViews = ExportViews.Where(i => i.IsSelected).ToList();
        SelectedSheets = ExportSheets.Where(i => i.IsSelected).ToList();

        if (SelectedSheets.Count > 0 || SelectedViews.Count > 0)
            IsMainButtonAvailable = true;
        else
            IsMainButtonAvailable = false;
    }

    /// <summary>
    ///     Happens when phase switcher changed it's value
    /// </summary>
    [RelayCommand]
    private void PhaseSwitched()
    {
        if (SwitchPhaseBoolValue)
        {
            // Insert there logic of finding TO/UO stage files
            ExportViews = new ObservableCollection<ViewExample>
            {
                new() { Name = "(TO/UO) first view" },
                new() { Name = "(TO/UO) second view" },
                new() { Name = "(TO/UO) third view" }
            };

            ExportSheets = new ObservableCollection<SheetExample>
            {
                new() { Name = "(TO/UO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(TO/UO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(TO/UO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(TO/UO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(TO/UO) 1234-TL - Tekeningenlijst" }
            };
        }
        else
        {
            // Insert there logic of finding VO/DO stage files
            ExportViews = new ObservableCollection<ViewExample>
            {
                new() { Name = "(VO/DO) first view" },
                new() { Name = "(VO/DO) second view" },
                new() { Name = "(VO/DO) third view" }
            };

            ExportSheets = new ObservableCollection<SheetExample>
            {
                new() { Name = "(VO/DO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(VO/DO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(VO/DO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(VO/DO) 1234-TL - Tekeningenlijst" },
                new() { Name = "(VO/DO) 1234-TL - Tekeningenlijst" }
            };
        }

        UpdateChBoxesState();
    }

    [RelayCommand]
    private void ChangeAllViewSelection(object o)
    {
        if (o is not bool boolValue) return;

        foreach (var view in ExportViews) view.IsSelected = boolValue;

        UpdateChBoxesState();
    }

    [RelayCommand]
    private void ChangeAllSheetsSelection(object o)
    {
        if (o is not bool boolValue) return;

        foreach (var sheet in ExportSheets) sheet.IsSelected = boolValue;

        UpdateChBoxesState();
    }
}

/// <summary>
///     Example class of a view
/// </summary>
public class ViewExample : ObservableObject
{
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Name { get; set; }
}

/// <summary>
///     Example class of a sheet
/// </summary>
public class SheetExample : ObservableObject
{
    private bool _isSelected;

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Name { get; set; }
}