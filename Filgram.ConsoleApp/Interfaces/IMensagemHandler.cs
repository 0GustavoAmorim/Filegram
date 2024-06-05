using Telegram.Bot;

namespace Filegram.ConsoleApp.Interfaces
{
    public interface IMensagemHandler
    {
        Task RespostaAnimada(ITelegramBotClient botClient, long chatId, string? messageText, CancellationToken cancellationToken);
    }
}
