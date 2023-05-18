using Autodesk.Revit.Attributes;
using Jajo.Exporter.Utils;
using Jajo.Exporter.ViewModels;
using Jajo.Exporter.ViewModels.Pages;
using Jajo.Exporter.ViewModels.Utils;
using Jajo.Exporter.Views;
using Jajo.Exporter.Views.Pages;
using Jajo.Utils.Core;
using Jajo.Utils.Stores;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;

namespace Jajo.Exporter.EntryPoint;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class ExporterCommand : ExternalCommand
{
    public override void Execute()
    {
        RevitApi.UiApplication ??= ExternalCommandData.Application;

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MainView>();
        serviceCollection.AddSingleton<NavigationStore>();
        serviceCollection.AddSingleton<IMainViewModel, MainViewModel>();
        serviceCollection.AddSingleton<ExportView>();
        serviceCollection.AddSingleton<ExportViewModel>();
        serviceCollection.AddSingleton<SchedulerView>();
        serviceCollection.AddSingleton<SchedulerViewModel>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var exporterView = serviceProvider.GetService<MainView>();
        exporterView.Show(UiApplication);
    }
}