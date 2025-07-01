using Newtonsoft.Json;
using Server.Data;
using Server.Model;
using Server.ServerApp.Init;
using System.Net.Sockets;
using System.Text;

namespace Server.ServerApp
{
    static class Sending
    {
        public static void Send(Share packet, string ip, int port)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(packet));
            try
            {
                ServerInit.Server.Send(buffer, buffer.Length, ip, port);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error while sending: {ex.ToString()}");
                Console.ResetColor();
            }
        }

        public static void SendConfirmResult(string tag, string name, UdpReceiveResult result)
        {
            string sender = "Server";
            byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Share
            {
                SenderName = sender,
                ReciverName = name,
                Data = "Ok",
                Tag = tag
            }));
            try
            {
                ServerInit.Server.Send(buffer, buffer.Length, result.RemoteEndPoint.Address.ToString(), result.RemoteEndPoint.Port);
            }
            catch (Exception ex)
            {
            }
        }

        public static void SendClientList()
        {
            string sender = "Server";
            foreach (var o in ClientsList.Clients)
            {
                var avalableClinets = ClientsList.Clients.Where(c => !(c.Value.Ip == o.Value.Ip && c.Key == o.Key) && c.Value.IsAvalable == true)
                                                        .Select(c => c.Key).ToList();
                byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Share
                {
                    SenderName = sender,
                    ReciverName = o.Key,
                    Data = avalableClinets,
                    Tag = "Clients"
                }));
                try
                {
                    ServerInit.Server.Send(buffer, buffer.Length, o.Value.Ip.ToString(), o.Value.Port);
                }
                catch (Exception ex) {}
            }
        }
    }
}
