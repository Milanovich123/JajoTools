using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Jajo.Exporter.ViewModels.Pages;

public partial class ExportViewModel : ObservableObject, IViewModelBase
{
    private ObservableCollection<ViewExample> _exportViews = new ();
    private ObservableCollection<SheetExample> _exportSheets = new ();
    private bool _isExportToDwgSelected;

    // In real project you should change type
	public ObservableCollection<ViewExample> ExportViews
    {
        get
        {
            if (_exportViews.Any()) return _exportViews;
            
            ExportViews = new ObservableCollection<ViewExample>
            {
                new() {Name="first view"},
                new() {Name="second view"},
                new() {Name="third view"},
            };
            return _exportViews;
        }
        set => SetProperty(ref _exportViews, value);
    }

    public ObservableCollection<SheetExample> ExportSheets
    {
        get
        {
            if (_exportSheets.Any()) return _exportSheets;
            
            ExportSheets = new ObservableCollection<SheetExample>
            {
                new() {Name = "1234-TL - Tekeningenlijst"},
                new() {Name = "1234-TL - Tekeningenlijst"},
                new() {Name = "1234-TL - Tekeningenlijst"},
                new() {Name = "1234-TL - Tekeningenlijst"},
                new() {Name = "1234-TL - Tekeningenlijst"}
            };
            return _exportSheets;
        }
        set => SetProperty(ref _exportSheets, value);
    }

    public bool IsExportToDwgSelected
    {
        get => _isExportToDwgSelected;
        set => SetProperty(ref _isExportToDwgSelected, value);
    }

    [RelayCommand]
    private void Export()
    {
        // logic when you need to export sheets to dwg format
        if (IsExportToDwgSelected)
        {
            MessageBox.Show("Elements were successfully exported");
            MessageBox.Show("Sheets were successfully exported as DWG");
        }
        // logic when the dwg export check box was not selected
        else
        {
            MessageBox.Show("Elements were successfully exported");
        }
    }

    [RelayCommand]
    private void Help()
    {
        MessageBox.Show("Help button clicked");
    }
}

/// <summary>
/// Example class of a view
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
/// Example class of a sheet
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