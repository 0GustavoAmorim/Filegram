using Filegram.ConsoleApp.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Filegram.ConsoleApp.Handlers;

public class UpdateHandlerService : IUpdateHandlerService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandlerService> _logger;

    public UpdateHandlerService(
        ITelegramBotClient botClient,
        ILogger<UpdateHandlerService> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task TratarAtualizacoesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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

    public Task TratarErroDePollingAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
