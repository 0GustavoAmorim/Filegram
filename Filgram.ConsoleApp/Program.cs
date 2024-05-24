using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Filegram.ConsoleApp.Configuration;
using Filegram.ConsoleApp.Controllers;
using Microsoft.Extensions.Configuration;
using Filegram.ConsoleApp.Interfaces;
using Filegram.ConsoleApp.Handlers;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddBotConfiguration(context.Configuration);
        services.AddTransient<IUpdateHandlerService, UpdateHandlerService>();



        services.AddHostedService<BotController>();
    })
    .Build()
    .Run();
