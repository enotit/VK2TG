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
            if (e.Message.Chat.Id != int.Parse(Users_information.telegram_ID)) return;
            if (e.Message.Text != null)
            {
                string text = null; // текст где находится ID юзера
                try
                {
                    text = e.Message.ReplyToMessage.Text;

                }
                catch { }
                if (text == null)
                {
                    try { text = e.Message.ReplyToMessage.Caption; }
                    catch
                    {
                    }
                }

                if (text == null || e.Message.Text == null) return;
                try
                {
                    long a = (long)int.Parse(e.Message.ReplyToMessage.Caption);
                    partVK.sms(a, e.Message.Text);
                    return;
                }
                catch
                {
                    string reg = @"id(\S*)(\w+)\S(\w+)";
                    if (Regex.IsMatch(text, reg, RegexOptions.IgnoreCase))
                    {
                        var id = (long)int.Parse(text.Substring(2, text.IndexOf(" ") - 1));
                        partVK.sms(id, e.Message.Text);
                    }
                }
            }
        }
        public static async void sms(string text)
        {
            await Program.tg_api.SendTextMessageAsync(
                chatId: Users_information.telegram_ID,
                text: text
                );
            return;

        }

        public static async void sms_photo(string url, string who)
        {
            await botClient.SendPhotoAsync(
            chatId: Users_information.telegram_ID,
            photo: url,
            caption: who
            );
            return;
        }
    }
}