using Server.Logs;
using Server.Model;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Server.ServerApp.Init;
using Server.Data;

namespace Server.ServerApp
{
    static class Server
    {
        public static async Task StartServer(string[] args)
        {
            //ClientListController.ListChanged += Clc_ListChanged;
            new ServerInit();
            ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Server started at port: {ServerInit.ServerPort}");
            ConsoleOutput.Output($"{DateTime.Now} Start listening for clients...");
            while (true)
            {
                try
                {
                    var RecivePacket = await ServerInit.Server.ReceiveAsync();
                    ConsoleOutput.Output($"{DateTime.Now} Recive packet from {RecivePacket.RemoteEndPoint}");
                    if (RecivePacket.RemoteEndPoint != null)
                        Task.Run(() => TranslatorHandler(RecivePacket));
                }
                catch (Exception)
                {
                }
            }
        }

        private static void Clc_ListChanged()
        {
            Sending.SendClientList();
        }

        private static void TranslatorHandler(UdpReceiveResult result)
        {
            ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Recive packet for processing");
            Share recivedPacket = null;
            try
            {
                ConsoleOutput.Output($"{DateTime.Now} Trying read packet...");
                recivedPacket = JsonConvert.DeserializeObject<Share>(Encoding.UTF8.GetString(result.Buffer));
            }
            catch (Exception ex)
            {
                ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Packet error! Check data!\nError: {ex.ToString()}");
            }
            if (recivedPacket != null)
            {
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Packet recived");
                ClientManager.Manage(recivedPacket, result);
            }
            else
            {
                ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Wrong sender!");
            }
        }
    }
}
