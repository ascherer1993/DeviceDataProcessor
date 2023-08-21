using DeviceDataProcessor.ConsoleApp;
using DeviceDataProcessor.DataAccess;
using DeviceDataProcessor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

using IHost host = CreateHostBuilder().Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

IHostBuilder CreateHostBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<IDeviceDataConversionService, DeviceDataConversionService>();
            services.AddSingleton<IDeviceDataRepository, JsonDeviceDataRepository>();
            services.AddSingleton<App>();
            services.AddLogging(builder =>
            {
                builder
                    .AddConsole();
            });
        });
}