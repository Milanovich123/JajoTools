using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Jajo.Exporter.Services;
using Jajo.Exporter.Stores;
using Jajo.Exporter.ViewModels;
using Jajo.Exporter.ViewModels.Utils;
using Jajo.Exporter.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Jajo.Exporter;

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
            
                services.AddSingleton<MainView>();
                services.AddSingleton<NavigationStore>();
                
                services.AddScoped<IMainViewModel, MainViewModel>();
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