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
            
            LastCheck = _api.Messages.GetConversations(new GetConversationsParams
            {
                Count = 1,
                Filter = VkNet.Enums.SafetyEnums.GetConversationFilter.Unread
            });
            var a = LastCheck.Items.Count;
            // partVK.Start();
            Thread thread1 = new Thread(partVK.Start);
            Thread thread2 = new Thread(partTG.Start);
            thread1.Start();
            thread2.Start();
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
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
        }
    }
}
