using System.Windows;
using Jajo.Tools.Views;
using Microsoft.Extensions.Hosting;

namespace Jajo.Tools.Services;

/// <summary>
/// This class is used to determine what should be done 
/// </summary>
public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_serviceProvider.GetService(typeof(ToolsView)) is Window mainView) mainView.Show();
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}