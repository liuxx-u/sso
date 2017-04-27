using System.Collections.Generic;
using sso.Client;

namespace sso.server
{
    public class SampleUserClientStore: IUserClientStore
    {
        public ICollection<ClientInfo> GetUserClients(string userName)
        {
            return new List<ClientInfo>
            {
                new ClientInfo
                {
                    ClientName = "用户中心",
                    Host = "localhost:58806",
                    LoginNotifyUrl = "http://localhost:58806/Account/LoginNotify",
                    LogoutNotifyUrl = "http://localhost:58806/Account/LogoutNotify"
                },
                new ClientInfo
                {
                    ClientName = "业务系统1",
                    Host = "localhost:58812",
                    LoginNotifyUrl = "http://localhost:58812/Account/LoginNotify",
                    LogoutNotifyUrl = "http://localhost:58812/Account/LogoutNotify"
                },
                new ClientInfo
                {
                    ClientName = "业务系统2",
                    Host = "localhost:58816",
                    LoginNotifyUrl = "http://localhost:58816/Account/LoginNotify",
                    LogoutNotifyUrl = "http://localhost:58816/Account/LogoutNotify"
                }
            };
        }
    }
}