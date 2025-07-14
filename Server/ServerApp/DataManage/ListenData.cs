using Newtonsoft.Json;
using Server.Data;
using Server.Model;
using Server.ServerApp.ClientManage;
using System.Net.Sockets;
using System.Text;

namespace Server.ServerApp.DataManage
{
    static class ListenData
    {
        private static UdpReceiveResult _recivePacket;
        public static async Task StartListen()
        {
            //ClientListController.ListChanged += Clc_ListChanged;
            ConsoleOutput.Output($"{DateTime.Now} Start listening for clients...");
            while (true)
            {
                try
                {
                    _recivePacket = await Enviroments.Server.ReceiveAsync();
                    ConsoleOutput.Output($"{DateTime.Now} Recive packet from {_recivePacket.RemoteEndPoint}");
                    if (_recivePacket.RemoteEndPoint != null)
                        Task.Run(() => TranslatorHandler(_recivePacket));
                }
                catch (Exception ex)
                {
                    ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Error while recive packet {ex}");
                }
            }
        }

        //private static void Clc_ListChanged()
        //{
        //    Sending.SendClientList();
        //}

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
