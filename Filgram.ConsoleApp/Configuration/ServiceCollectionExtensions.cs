using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Filegram.ConsoleApp.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBotConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BotConfiguration>(configuration.GetSection("BotConfiguration"));
            services.AddSingleton<ITelegramBotClient>(provider =>
            {
                var botConfig = provider.GetRequiredService<IOptions<BotConfiguration>>().Value;
                return new TelegramBotClient(botConfig.BotToken);
            });

            return services;
        }
    }
}
