using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Abstractions;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model.RequestParams;
using Telegram.Bot;

namespace VK2TG
{
    class Program
    {   
        private static IVkApi _api;
        private static VkApi vk_api;
        private static TelegramBotClient tg_api;
        static void Main(string[] args)
        {   
            AuthorizeVK();
            AuthorizeTelegram();
            _api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
            {
                RandomId = 1, // уникальный
                UserId = _api.UserId,
                Message = "Start..."
            });
            sms();
        }
        private static async void sms(){
            await tg_api.SendTextMessageAsync(
                chatId: Users_information.telegram_ID,
                text:   "Work successfully"
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
