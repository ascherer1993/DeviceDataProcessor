//Todo remove
// See https://aka.ms/new-console-template for more information

using DeviceDataProcessor.ConsoleApp;
using DeviceDataProcessor.DataAccess;
using DeviceDataProcessor.Models;
using DeviceDataProcessor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;


// // create hosting object and DI layer
// using IHost host = CreateHostBuilder(args).Build();
//
// // create a service scope
// using var scope = host.Services.CreateScope();
//
// var services = scope.ServiceProvider;
//
//
// // add entry point or call service methods
// // more .....
//
//
// // implementatinon of 'CreateHostBuilder' static method and create host object
// IHostBuilder CreateHostBuilder(string[] strings)
// {
//     return Host.CreateDefaultBuilder()
//         .ConfigureServices((_, services) =>
//         {
//             //services.AddSingleton<ICustomerService, CustomerService>();
//             
//         });
// }




// var builder = Host.CreateDefaultBuilder(args)
//     .ConfigureServices((_, services) =>
//     {
//         services.AddScoped<IDeviceDataRepository, JsonDeviceDataRepository>();
//     });


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









// //Dependency injection
// DeviceDataConversionService deviceDataConversionService = new DeviceDataConversionService();
//
//
// if (args.Length < 1)
// {
//     Console.WriteLine("Please include a path to at least one json file");
// }
// Console.WriteLine("Args Length: " + args.Length);
//
// List<string> jsonFilesAsString = new List<string>();
//
// foreach (var filePath in args)
// {
//     string jsonContent = File.ReadAllText(filePath);
//     jsonFilesAsString.Add(jsonContent);
//     //Should all the object stuff happen in the service?
//     // JObject jsonObject = JObject.Parse(jsonContent);
//     //
//     // // Check if the property exists
//     // bool isFoo1 = jsonObject.ContainsKey("PartnerId");
//     // bool isFoo2 = jsonObject.ContainsKey("CompanyId");
//     // if (isFoo1)
//     // {
//     //     Foo1Dto? foo1Dto = jsonObject.ToObject<Foo1Dto>();
//     // }
//     // else if (isFoo2)
//     // {
//     //     Foo2Dto? foo2Dto = jsonObject.ToObject<Foo2Dto>();
//     // }
//     // else
//     // {
//     //     Console.WriteLine("Failed to parse one of the included files");
//     // }
//     
// }
//
// deviceDataConversionService.ConvertJsonToUnifiedDeviceData(jsonFilesAsString);
