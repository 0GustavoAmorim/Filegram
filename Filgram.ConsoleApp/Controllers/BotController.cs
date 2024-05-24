using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Filegram.ConsoleApp.Controllers
{
    public class BotController : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<BotController> _logger;

        public BotController(ITelegramBotClient botClient, ILogger<BotController> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var me = await _botClient.GetMeAsync();
            _logger.LogInformation($"Olá, Mundo! Eu sou {me.Id} e meu nome é {me.FirstName}");

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                },
                cancellationToken: stoppingToken
            );

            _logger.LogInformation($"Iniciando escuta do @{me.Username}");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            _logger.LogInformation($"Recebento a '{messageText}' no chat {chatId}.");

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Você disse:\n" + messageText,
                cancellationToken: cancellationToken);
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(errorMessage);
            return Task.CompletedTask;
        }
    }
}
