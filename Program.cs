using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Abstractions;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model.RequestParams;
using VkNet.Utils.JsonConverter;

using Telegram.Bot;

namespace VK2TG
{
    class Program
    {
        public static GetConversationsResult LastCheck;
        public static IVkApi _api;
        public static TelegramBotClient tg_api;
        static void Main(string[] args)
        {
            Random rnd = new Random();
            AuthorizeVK();
            AuthorizeTelegram();
            _api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
            {
                RandomId = rnd.Next(1, 1000 * 255), // уникальный
                UserId = _api.UserId,
                Message = "Start..."
            });
            LastCheck = _api.Messages.GetConversations(new GetConversationsParams
            {
                Count = 1,
                Filter = VkNet.Enums.SafetyEnums.GetConversationFilter.Unread
            });
            var a = LastCheck.Items.Count;
            sms("Start work");
            partVK.Start();
        }
        public static async void sms(string text)
        {
            await tg_api.SendTextMessageAsync(
                chatId: Users_information.telegram_ID,
                text: text
                );

        }
        private static void AuthorizeVK()
        {
            // Псевдо ВК апи, для обхода блокировки ВК
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAudioBypass();
            _api = new VkApi(serviceCollection);
            _api.Authorize(new ApiAuthParams
            {
                Login = Users_information.login,
                Password = Users_information.password
            });
            Console.WriteLine($"Token AudioBypass: {_api.Token}");
        }

        private static void AuthorizeTelegram()
        {
            tg_api = new TelegramBotClient(Users_information.API_telegram);
            var me = tg_api.GetMeAsync().Result;
            Console.WriteLine(
            $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
        }
    }
}
