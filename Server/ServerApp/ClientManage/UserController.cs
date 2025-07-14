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
            ConsoleOutput.Output($"{DateTime.Now} Searching for existing clinet...");
            var existListClient = ClientsList.Clients.FirstOrDefault(c => c.Key.Equals(packet.SenderName));
            if (existListClient.Value == null)
            {
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Registering new client");
                ClientListController.AddClient(packet.SenderName, new DictModel
                {
                    IsAvalable = true,
                    Ip = result.RemoteEndPoint.Address,
                    Port = result.RemoteEndPoint.Port
                });
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Connected Name: {packet.SenderName} Ip: {result.RemoteEndPoint.Address} Port: {result.RemoteEndPoint.Port}");
                return (true, packet.SenderName, result);
            }
            else
            {
                ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Client with Name: {existListClient.Key} is exits at Ip: {existListClient.Value.Ip} Port: {existListClient.Value.Port}");
                return (false, packet.SenderName, result);
            }
        }

        public static bool Delete(Share packet, UdpReceiveResult result)
        {
            var client = ClientsList.Clients.FirstOrDefault(c => c.Key == packet.SenderName);
            var deleteClient = ClientListController.DeleteClient(client.Key);
            if (deleteClient.Item1)
            {
                ConsoleOutput.Output(ConsoleColor.Cyan, $"{DateTime.Now} Disconnected Name: {client.Key} Ip: {deleteClient.Item2.Ip} Port: {deleteClient.Item2.Port}");
            }
            return deleteClient.Item1;
        }

        public static void UnknownClient(Share packet, UdpReceiveResult result)
        {
            ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Unknown Client: {packet.SenderName}:{result.RemoteEndPoint}");
        }
    }
}
