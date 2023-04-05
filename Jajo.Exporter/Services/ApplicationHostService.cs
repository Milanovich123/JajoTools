using System.Windows;
using Jajo.Exporter.Views;
using Microsoft.Extensions.Hosting;

namespace Jajo.Exporter.Services;

public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_serviceProvider.GetService(typeof(MainView)) is Window mainView) mainView.Show();
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}