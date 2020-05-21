using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VK12TG
{
    class Program
    {
        static void Main(string[] args)
        {
            var api = new VkApi();
            
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = 123456,
                Login = Users_information.login,
                Password = Users_information.password,
                Settings = Settings.All
            });
            Console.WriteLine(api.Token);
            //var res = api.Groups.Get(new GroupsGetParams());

            //Console.WriteLine(res.TotalCount);

            Console.ReadLine();
    }
    }
}
