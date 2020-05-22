using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using VkNet;
using VkNet.Model;
using VkNet.Abstractions;
using VkNet.AudioBypassService.Extensions;
using VkNet.Model.RequestParams;

namespace VK2TG
{
    class partVK
    {
        public static /* async */ void Start()
        {
            while (true)
            {
                var Check = Program._api.Messages.GetConversations(new GetConversationsParams
                {
                    Count = 1,
                    Filter = VkNet.Enums.SafetyEnums.GetConversationFilter.Unread
                });
                Console.WriteLine($"{Program.LastCheck.Items[0].LastMessage.Text != Check.Items[0].LastMessage.Text}\n{Program.LastCheck.Items[0].LastMessage.Text} --- {Check.Items[0].LastMessage.Text}");
                if ((Program.LastCheck.Items[0].LastMessage.Text != Check.Items[0].LastMessage.Text))
                {   
                    var p = Program._api.Users.Get(new long[] {(long) Check.Items[0].LastMessage.FromId }).FirstOrDefault();
                    Program.sms($"id{p.Id} {p.FirstName} {p.LastName}:\n{Check.Items[0].LastMessage.Text}");
                    Program.LastCheck = Check;
                }
                Thread.Sleep(300);
            }

        }
    }
}