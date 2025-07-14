using Server.Data;
using Server.Logs;
using Server.Model;
using System.Net.Sockets;

namespace Server.ServerApp.DataManage
{
    public class ResendData
    {
        public static void Resend(Share packet, UdpReceiveResult result)
        {
            ConsoleOutput.Output(new string[] { $"{DateTime.Now} Preparing for resend data", $"{DateTime.Now} Searching for {packet.SenderName} registration..." });
            var sender = ClientsList.Clients.FirstOrDefault(c => c.Key.Equals(packet.SenderName));
            if(sender.Value != null)
            {
                ConsoleOutput.Output(new string[] { $"{DateTime.Now} {packet.SenderName} registered", $"{DateTime.Now} Searching for {packet.ReciverName} registration..." });
                var client = ClientsList.Clients.FirstOrDefault(c => c.Key.Equals(packet.ReciverName));
                if (client.Value != null)
                {
                    if (!sender.Key.Equals(client.Key))
                    {
                        try
                        {
                            ConsoleOutput.Output(ConsoleColor.DarkCyan, $"{DateTime.Now} Sending from ip: {result.RemoteEndPoint.Address} Name: {packet.SenderName} to ip: {client.Value.Ip} Name: {packet.ReciverName}");
                            Sending.Send(packet, client.Value.Ip.ToString(), client.Value.Port);
                            ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Send from ip: {result.RemoteEndPoint.Address} Name: {packet.SenderName} to ip: {client.Value.Ip} Name: {packet.ReciverName}");
                            ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} {sender.Value.Ip} now is unavalable");
                            sender.Value.IsAvalable = false;
                            ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} {client.Value.Ip} now is unavalable");
                            client.Value.IsAvalable = false;
                            //ClientListController.UpdateClient();
                        }
                        catch (Exception)
                        { }
                    }
                    else
                    {
                        ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} {packet.SenderName} trying send package to  {packet.SenderName}");
                    }
                }
                else
                {
                    ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} There are no {packet.ReciverName} in registration list");
                }
            }
            else
            {
                ConsoleOutput.Output(ConsoleColor.DarkCyan, $"{DateTime.Now} Preparing answer that client not registered...");
                string senderName = "Server";
                Sending.Send(new Share
                {
                    SenderName = senderName,
                    ReciverName = packet.SenderName,
                    Data = "Not Register",
                    Tag = packet.Tag
                }, result.RemoteEndPoint.Address.ToString(), result.RemoteEndPoint.Port);
                ConsoleOutput.Output(ConsoleColor.DarkCyan, $"{DateTime.Now} Send answer that client not registered");
            }
        }
    }
}
