// See https://aka.ms/new-console-template for more information

using App;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment.EnvironmentName ?? "Production";
            config.AddJsonFile("appsettings.json", false);
            config.AddJsonFile(
                $"appsettings.{env}.json",
                optional: true, reloadOnChange: true);

        })
        .ConfigureLogging((context, logging) =>
        {
            logging.AddConfiguration(context.Configuration.GetSection("Logging"));
            logging.AddConsole();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(context.Configuration).CreateLogger();
        })
        .UseSerilog()
        .ConfigureContainer<ContainerBuilder>((context, builder) =>
        {
            
        })
        .ConfigureServices(collection =>
        {
            collection.AddHostedService<SampleService>();
        })
        .UseConsoleLifetime()
        .Build()
        .RunAsync();