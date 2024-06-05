using Filegram.ConsoleApp.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Filegram.ConsoleApp.Handlers
{
    public class MensagemHandler : IMensagemHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateHandlerService> _logger;

        public MensagemHandler(
            ITelegramBotClient botClient,
            ILogger<UpdateHandlerService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }
        public static async Task RespostaAnimada(ITelegramBotClient botClient, long chatId, string? messageText, CancellationToken cancellationToken)
        {
            if (messageText.EndsWith("Figurinha"))
            {
                await botClient.SendStickerAsync(
                    chatId: chatId,
                    sticker: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"),
                    cancellationToken: cancellationToken);
            }
            else if (messageText.EndsWith("Soninho"))
            {
                await botClient.SendVideoAsync(
                        chatId: chatId,
                        video: InputFile.FromUri("https://sample-videos.com/video321/mp4/720/big_buck_bunny_720p_1mb.mp4"),
                     cancellationToken: cancellationToken);
            }
            else if (messageText.EndsWith("Mundo"))
            {
                await botClient.SendVideoAsync(
                    chatId: chatId,
                    video: InputFile.FromUri("https://file-examples.com/storage/fe4e1227086659fa1a24064/2017/04/file_example_MP4_480_1_5MG.mp4"),
                    cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Não existe essa opção.",
                cancellationToken: cancellationToken);
            }

        }
    }
}
