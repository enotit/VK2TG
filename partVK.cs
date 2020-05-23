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
            long big = 2000000000;
            while (true)
            {
                var Check = Program._api.Messages.GetConversations(new GetConversationsParams
                {
                    Count = 1,
                    Filter = VkNet.Enums.SafetyEnums.GetConversationFilter.Unread
                });
                var now = Program._api.Utils.GetServerTime().AddMilliseconds(250); var last_mes_date = Check.Items[0].LastMessage.Date;
                if ((Program.LastCheck.Items[0].LastMessage.Text != Check.Items[0].LastMessage.Text) & (now > last_mes_date))
                {
                    var text = Check.Items[0].LastMessage.Text;
                    var id = Check.Items[0].LastMessage.PeerId;
                    if (id < 0)
                    {
                        Program.sms($"id{id} {Program._api.Groups.GetById(null, (id * -1).ToString(), null).FirstOrDefault().Name}\n{text}");
                    }
                    else
                    {
                        var p = Program._api.Users.Get(new long[] { (long)Check.Items[0].LastMessage.FromId }).FirstOrDefault();
                        Program.sms($"id{p.Id} {(id > big ? $"'{Program._api.Messages.GetChat(new long[] { (long)(id - big) }).FirstOrDefault().Title}'" : "")} {p.FirstName} {p.LastName}:\n{text}");
                    }
                    Program.LastCheck = Check;
                }
                Thread.Sleep(300);
            }

        }
    }
}