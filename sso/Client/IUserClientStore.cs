using System.Collections.Generic;

namespace sso.Client
{
    public interface IUserClientStore
    {
        ICollection<ClientInfo> GetUserClients(string userName);
    }
}
