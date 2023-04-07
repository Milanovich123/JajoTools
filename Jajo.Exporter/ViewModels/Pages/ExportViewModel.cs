using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Jajo.Exporter.ViewModels.Pages;

public class ExportViewModel : ObservableObject, IViewModelBase
{
	private ObservableCollection<ViewExample> _exportViews = new()
    {
        new ViewExample {Name="first view"},
        new ViewExample {Name="second view"},
        new ViewExample {Name="third view"},
    };

	// In real project you should change type
	public ObservableCollection<ViewExample> ExportViews
	{
		get => _exportViews;
        set => SetProperty(ref _exportViews, value);
    }
}

public class ViewExample
{
    public string Name { get; set; }
}