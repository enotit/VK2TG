using Telegram.Bot;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using Telegram.Bot.Args;

namespace VK2TG
{
    class partTG
    {
        private static TelegramBotClient botClient = Program.tg_api;
        public static void Start()
        {
            sms("Start work");
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {   string text;
                try
                {
                    text = e.Message.ReplyToMessage.Text;
                }
                catch
                {
                    sms("Не реализовано");
                    return;
                }
                if (Regex.IsMatch(text, @"id(\S*)(\w+)\S(\w+)")){
                    var id = (long) int.Parse(text.Substring(2, text.IndexOf(" ") - 1));
                    partVK.sms(id, e.Message.Text);
                }
            }
        }
        public static async void sms(string text)
        {
            await Program.tg_api.SendTextMessageAsync(
                chatId: Users_information.telegram_ID,
                text: text
                );

        }
    }
}