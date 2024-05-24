using Filegram.ConsoleApp.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Filegram.ConsoleApp.Controllers
{
    public class BotController : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<BotController> _logger;
        private readonly IUpdateHandlerService _updateHandlerService;

        public BotController(
            ITelegramBotClient botClient,
            IUpdateHandlerService updateHandlerService,
            ILogger<BotController> logger)
        {
            _botClient = botClient;
            _updateHandlerService = updateHandlerService;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var me = await _botClient.GetMeAsync();
            _logger.LogInformation($"Olá, Mundo! Eu sou {me.Id} e meu nome é {me.FirstName}");

            _botClient.StartReceiving(
                _updateHandlerService.TratarAtualizacoesAsync,
                _updateHandlerService.TratarErroDePollingAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<Telegram.Bot.Types.Enums.UpdateType>()
                },
                cancellationToken: stoppingToken
            );

            _logger.LogInformation($"Iniciando escuta do @{me.Username}");
        }

    }
}
