using Autodesk.Revit.Attributes;
using Jajo.Tools.Utils;
using Jajo.Tools.ViewModels;
using Jajo.Tools.ViewModels.Pages;
using Jajo.Tools.ViewModels.Utils;
using Jajo.Tools.Views;
using Jajo.Tools.Views.Pages;
using Jajo.Utils.Core;
using Jajo.Utils.Stores;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Toolkit.External;

namespace Jajo.Tools.EntryPoint;

[UsedImplicitly]
[Transaction(TransactionMode.Manual)]
public class ToolsCommand : ExternalCommand
{
    public override void Execute()
    {
        RevitApi.UiApplication ??= ExternalCommandData.Application;

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ToolsView>();
        serviceCollection.AddSingleton<NavigationStore>();
        serviceCollection.AddSingleton<IViewModel, ToolsViewModel>();
        serviceCollection.AddSingleton<HideTabsView>();
        serviceCollection.AddSingleton<HideTabsViewModel>();
        serviceCollection.AddSingleton<WerkpakketView>();
        serviceCollection.AddSingleton<WerkpakketViewModel>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var toolsView = serviceProvider.GetService<ToolsView>();
        toolsView.Show(UiApplication);
    }
}