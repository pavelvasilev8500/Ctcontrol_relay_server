using Server.Data;
using Server.Logs;
using Server.Model;
using System.Net.Sockets;

namespace Server.ServerApp.ClientManage
{
    public static class UserController
    {
        public static (bool, string, UdpReceiveResult) Add(Share packet, UdpReceiveResult result)
        {
            var existListClient = ClientsList.Clients.FirstOrDefault(c => c.Key.Equals(packet.SenderName));
            if (existListClient.Value == null)
            {
                ClientListController.AddClient(packet.SenderName, new DictModel
                {
                    IsAvalable = true,
                    Ip = result.RemoteEndPoint.Address,
                    Port = result.RemoteEndPoint.Port
                });
                string connectedClientLog = $"[{DateTime.Now}] Connected Name: {packet.SenderName} Ip: {result.RemoteEndPoint.Address} Port: {result.RemoteEndPoint.Port}\n";
                string connectedClientView = $"[{DateTime.Now}] Connected:\n" + @"\" + $"\n | Name: {packet.SenderName}\n | Ip: {result.RemoteEndPoint.Address}\n | Port: {result.RemoteEndPoint.Port}";
                Log.AddLog($"{connectedClientLog}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(connectedClientView);
                Console.ResetColor();
                return (true, packet.SenderName, result);
            }
            else
            {
                string existClient = $"[{DateTime.Now}] Client with Name: {existListClient.Key} is exits at Ip: {existListClient.Value.Ip} Port: {existListClient.Value.Port}\n";
                Log.AddLog(existClient);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(existClient);
                Console.ResetColor();
                return (false, packet.SenderName, result);
            }
        }

        public static bool Delete(Share packet, UdpReceiveResult result)
        {
            string disconnectedClientLog = "";
            string disconnectedClientView = "";
            var client = ClientsList.Clients.FirstOrDefault(c => c.Key == packet.SenderName);
            var deleteClient = ClientListController.DeleteClient(client.Key);
            if (deleteClient.Item1)
            {
                disconnectedClientLog = $"[{DateTime.Now}] Disconnected Name: {client.Key} Ip: {deleteClient.Item2.Ip} Port: {deleteClient.Item2.Port}\n";
                disconnectedClientView = $"[{DateTime.Now}] Disconnected:\n" + @"\" + $"\n | Name: {client.Key}\n | Ip: {deleteClient.Item2.Ip}\n | Port: {deleteClient.Item2.Port}";
                Log.AddLog($"{disconnectedClientLog}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(disconnectedClientView);
                Console.ResetColor();
            }
            return deleteClient.Item1;
        }

        public static void UnknownClient(Share packet, UdpReceiveResult result)
        {
            string client = $"[{DateTime.Now}] Unknown Client: {packet.SenderName}:{result.RemoteEndPoint}";
            Log.AddLog($"Warning! {client}\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(client);
            Console.ResetColor();
        }
    }
}
