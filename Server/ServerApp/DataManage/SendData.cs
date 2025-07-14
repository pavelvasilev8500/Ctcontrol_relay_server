using Newtonsoft.Json;
using Server.Data;
using Server.Model;
using System.Net.Sockets;
using System.Text;

namespace Server.ServerApp.DataManage
{
    static class SendData
    {
        public static void SendConfirmResult(string tag, string name, UdpReceiveResult result)
        {
            ConsoleOutput.Output($"{DateTime.Now} Creating confirm connct result");
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
                ConsoleOutput.Output($"{DateTime.Now} Sending confirm connct result");
                Enviroments.Server.Send(buffer, buffer.Length, result.RemoteEndPoint.Address.ToString(), result.RemoteEndPoint.Port);
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Confirm connct result send to ip: {result.RemoteEndPoint.Address} port: {result.RemoteEndPoint.Port}");
            }
            catch (Exception ex)
            {
                ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Error while sending: {ex.Message}");
            }
        }

        //public static void SendClientList()
        //{
        //    string sender = "Server";
        //    foreach (var o in ClientsList.Clients)
        //    {
        //        var avalableClinets = ClientsList.Clients.Where(c => !(c.Value.Ip == o.Value.Ip && c.Key == o.Key) && c.Value.IsAvalable == true)
        //                                                .Select(c => c.Key).ToList();
        //        byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Share
        //        {
        //            SenderName = sender,
        //            ReciverName = o.Key,
        //            Data = avalableClinets,
        //            Tag = "Clients"
        //        }));
        //        try
        //        {
        //            ServerInit.Server.Send(buffer, buffer.Length, o.Value.Ip.ToString(), o.Value.Port);
        //        }
        //        catch (Exception ex) {}
        //    }
        //}
    }
}
