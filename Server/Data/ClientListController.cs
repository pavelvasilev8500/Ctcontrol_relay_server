using Server.Model;
using Server.ServerApp;

namespace Server.Data
{
    static class ClientListController
    {
        public delegate void ClientListChanged();
        public static event ClientListChanged? ListChanged;

        public static void AddClient(string name, DictModel model)
        {
            var isSuccuss = ClientsList.Clients.TryAdd(name, model);
            if (isSuccuss)
            {
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} {name} added");
                foreach (var o in ClientsList.Clients)
                    ConsoleOutput.Output(ConsoleColor.DarkMagenta, $"  |{o.Key}");
            }
            //if (isSuccuss)
            //    ListChanged?.Invoke();
        }

        public static (bool, DictModel) DeleteClient(string name)
        {
            var isSuccess = ClientsList.Clients.TryRemove(name, out var rc);
            if(isSuccess)
            {
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} {name} deleted");
                foreach (var o in ClientsList.Clients)
                    ConsoleOutput.Output(ConsoleColor.DarkMagenta, $"  |{o.Key}");
                //ListChanged?.Invoke();
                return (isSuccess, rc);
            }
            return (isSuccess, rc);
        }

        public static void UpdateClient() => ListChanged?.Invoke();
    }
}
