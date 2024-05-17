using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace TelegramBotExample
{
    class Program
    {

        static async Task Main(string[] args)
        {

            var botClient = new TelegramBotClient("bot-token");

            using CancellationTokenSource cts = new();

            // StartReceiving não bloqueia a thread que o chamou. O recebimento é feito no ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                // recebe todos os tipos de atualizações, exceto as atualizações relacionadas a ChatMember
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: ProcessarAtualizacaoAsync,
                pollingErrorHandler: ProcessarErroPollingAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Começar a ouvir @{me.Username}");
            Console.ReadLine();

            cts.Cancel();

            async Task ProcessarAtualizacaoAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                //Processar apenas atualizações de mensagens
                if (update.Message is not { } message)
                    return;
                //Processa apenas mensagens de text
                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;

                Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Você disse:\n" + messageText,
                        cancellationToken: cancellationToken);
            }

            Task ProcessarErroPollingAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var MensagemErro = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(MensagemErro);
                return Task.CompletedTask;
            }
        }
    }
}
