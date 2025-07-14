using Server.Model;

namespace Server.Data
{
    static class ClientListController
    {
        public delegate void ClientListChanged();
        public static event ClientListChanged? ListChanged;

        public static void AddClient(string name, DictModel model)
        {
            var isSuccuss = ClientsList.Clients.TryAdd(name, model);
            //if (isSuccuss)
            //    foreach (var o in ClientsList.Clients)
            //        Console.WriteLine(o);
            //if (isSuccuss)
            //    ListChanged?.Invoke();
        }

        public static (bool, DictModel) DeleteClient(string name)
        {
            var isSuccess = ClientsList.Clients.TryRemove(name, out var rc);
            if(isSuccess)
            {
                //foreach (var o in ClientsList.Clients)
                //    Console.WriteLine(o);
                //ListChanged?.Invoke();
                return (isSuccess, rc);
            }
            return (isSuccess, rc);
        }

        public static void UpdateClient() => ListChanged?.Invoke();
    }
}
