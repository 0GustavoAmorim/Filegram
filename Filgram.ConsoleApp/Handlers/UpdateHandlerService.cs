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

        await botClient.SendStickerAsync(
            chatId: chatId, 
            sticker: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"),
            cancellationToken: cancellationToken
            );

        if (messageText.StartsWith("Soninho"))
        {
            await botClient.SendVideoAsync(
                chatId: chatId,
                video: InputFile.FromUri("https://sample-videos.com/video321/mp4/720/big_buck_bunny_720p_1mb.mp4"),
                cancellationToken: cancellationToken);
        } 
        else if (messageText == "Mundo")
        {
            await botClient.SendVideoAsync(
                chatId: chatId,
                video: InputFile.FromUri("https://file-examples.com/storage/fe4e1227086659fa1a24064/2017/04/file_example_MP4_480_1_5MG.mp4"),
                cancellationToken: cancellationToken);
        }
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
