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
            var sender = ClientsList.Clients.FirstOrDefault(c => c.Key.Equals(packet.SenderName));
            if(sender.Value != null)
            {
                var client = ClientsList.Clients.FirstOrDefault(c => c.Key.Equals(packet.ReciverName));
                if (client.Value != null)
                {
                    if (!sender.Key.Equals(client.Key))
                        try
                        {
                            string resend = $"[{DateTime.Now}] Sending from [ip: {result.RemoteEndPoint.Address} Name: {packet.SenderName}] to [ip: {client.Value.Ip} Name: {packet.ReciverName}]";
                            Log.AddLog(resend);
                            Console.WriteLine(resend);
                            Sending.Send(packet, client.Value.Ip.ToString(), client.Value.Port);
                            sender.Value.IsAvalable = false;
                            client.Value.IsAvalable = false;
                            //ClientListController.UpdateClient();
                        }
                        catch (Exception)
                        { }
                }
            }
            else
            {
                string senderName = "Server";
                Sending.Send(new Share
                {
                    SenderName = senderName,
                    ReciverName = packet.SenderName,
                    Data = "Not Register",
                    Tag = packet.Tag
                }, result.RemoteEndPoint.Address.ToString(), result.RemoteEndPoint.Port);
            }
        }
    }
}
