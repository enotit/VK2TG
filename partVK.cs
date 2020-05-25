using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net;
using System.IO;
using VkNet.Model.RequestParams;

namespace VK2TG
{
    class partVK
    {
        public static Random rnd = new Random();
        public static void Start()
        {
            long big = 2000000000;
            while (true)
            {
                var Check = Program._api.Messages.GetConversations(new GetConversationsParams
                {
                    Count = 1,
                    Filter = VkNet.Enums.SafetyEnums.GetConversationFilter.Unread
                });
                var now = Program._api.Utils.GetServerTime(); var last_mes_date = (DateTime)Check.Items[0].LastMessage.Date;
                if ((Program.LastCheck.Items[0].LastMessage.Text != Check.Items[0].LastMessage.Text) && ((now > last_mes_date) && (now.AddSeconds(-3) < last_mes_date)))
                {
                    var text = Check.Items[0].LastMessage.Text;
                    if (Check.Items[0].LastMessage.Action != null) text = "* Произошли в беседе изменения.";
                    var id = Check.Items[0].LastMessage.PeerId;
                    if (id < 0)
                    {
                        partTG.sms($"id{id} {Program._api.Groups.GetById(null, (id * -1).ToString(), null).FirstOrDefault().Name}:\n{text}");
                    }
                    else
                    {
                        var p = Program._api.Users.Get(new long[] { (long)Check.Items[0].LastMessage.FromId }).FirstOrDefault();
                        if (id > big)
                        {
                            partTG.sms($"id{id} '{Program._api.Messages.GetChat(new long[] { (long)(id - big) }).FirstOrDefault().Title}' - id{p.Id} {p.FirstName} {p.LastName}:\n{text}");
                        }
                        else
                        {
                            partTG.sms($"id{p.Id} {p.FirstName} {p.LastName}:\n{text}");
                        }
                    }
                    Program.LastCheck = Check;
                }
            }



        }

        public static void sms(long? id, string message)
        {
            if (id > 2000000000 || id < 0)
                Program._api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                {
                    RandomId = rnd.Next(1, 1000 * 255), // уникальный
                    PeerId = id,
                    Message = message
                });
            else
                Program._api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                {
                    RandomId = rnd.Next(1, 1000 * 255), // уникальный
                    UserId = id,
                    Message = message
                });
        }
    }
}