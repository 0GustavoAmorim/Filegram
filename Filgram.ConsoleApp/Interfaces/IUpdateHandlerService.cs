using Telegram.Bot;
using Telegram.Bot.Types;

namespace Filegram.ConsoleApp.Interfaces;

public interface IUpdateHandlerService
{
    Task TratarAtualizacoesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    Task TratarErroDePollingAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
}
