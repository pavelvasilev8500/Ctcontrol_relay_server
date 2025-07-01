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
            ClientListController.ListChanged += Clc_ListChanged;
            new ServerInit();
            string start = $"[{DateTime.Now}] Server started at port: {ServerInit.ServerPort}";
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(start);
            Log.AddLog(start);
            Console.ResetColor();
            while (true)
            {
                try
                {
                    var RecivePacket = await ServerInit.Server.ReceiveAsync();
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
            Share recivedPacket = null;
            try
            {
                recivedPacket = JsonConvert.DeserializeObject<Share>(Encoding.UTF8.GetString(result.Buffer));
                Log.AddLog("Packet recived");
            }
            catch (Exception ex)
            {
                string errorMessage = $"[{DateTime.Now}] Packet error! Check data!\nError: {ex.ToString()}";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errorMessage);
                Log.AddLog(errorMessage);
            }
            if (recivedPacket != null)
            {
                ClientManager.Manage(recivedPacket, result);
            }
            else
            {
                string errorMessage = $"[{DateTime.Now}] Wrong sender!";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errorMessage);
                Console.ResetColor();
                Log.AddLog(errorMessage);
            }
        }
    }
}
