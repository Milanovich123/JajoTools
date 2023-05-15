using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Jajo.Tools.Services;
using Jajo.Tools.ViewModels;
using Jajo.Tools.ViewModels.Pages;
using Jajo.Tools.ViewModels.Utils;
using Jajo.Tools.Views;
using Jajo.Tools.Views.Pages;
using Jajo.Utils.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Jajo.Tools;

/// <summary>
/// This class adds dependency injection principle to a wpf application
/// </summary>
public static class Host
{
    private static IHost _host;
    
    public static async Task StartHost()
    {
        _host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(builder =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyLocation = assembly.Location;
                var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
                builder.SetBasePath(assemblyDirectory);

                var targetFrameworkAttributes = assembly.GetCustomAttributes(typeof(TargetFrameworkAttribute), true);
                var targetFrameworkAttribute = (TargetFrameworkAttribute) targetFrameworkAttributes.First();
                var targetFramework = targetFrameworkAttribute.FrameworkDisplayName;

                builder.AddInMemoryCollection(new KeyValuePair<string, string>[]
                {
                    new("Assembly", assemblyLocation),
                    new("Framework", targetFramework),
                    new("ConfigFolder", Path.Combine(assemblyDirectory, "Configurations")),
                    new("DownloadFolder", Path.Combine(assemblyDirectory, "Downloads"))
                });
            })
            .ConfigureServices((_, services) =>
            {
                services.AddHostedService<ApplicationHostService>();
            
                services.AddSingleton<ToolsView>();
                services.AddSingleton<NavigationStore>();
                services.AddSingleton<IViewModel, ToolsViewModel>();
                services.AddSingleton<HideTabsView>();
                services.AddSingleton<HideTabsViewModel>();
                services.AddSingleton<WerkpakketView>();
                services.AddSingleton<WerkpakketViewModel>();
            })
            .Build();

        await _host.StartAsync();
    }

    [UsedImplicitly]
    public static async Task StopHost()
    {
        await _host.StopAsync();
        _host.Dispose();
    }

    [UsedImplicitly]
    public static T GetService<T>() where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }
}