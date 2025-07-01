using Server.Model;
using System.Collections.Concurrent;

namespace Server.Data
{
    static class ClientsList
    {
        public static ConcurrentDictionary<string, DictModel> Clients = new ConcurrentDictionary<string, DictModel>();
    }
}
