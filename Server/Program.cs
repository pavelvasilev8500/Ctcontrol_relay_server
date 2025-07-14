using Newtonsoft.Json;
using Server.Data;
using Server.Model;
using Server.ServerApp;
using Server.ServerApp.DataManage;
using Server.ServerApp.Init;
using System.Text;

namespace Server
{
    internal class Program
    {
        private static bool _serverInintStatus = false;
        private static ServerInit _init = new ServerInit();
        static async Task Main(string[] args)
        {
            ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Program strart");
            _serverInintStatus = _init.StartServerInit();
            if (!_serverInintStatus)
            {
                ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Program exit");
                Environment.Exit(0);
            }
            else
                await ListenData.StartListen();
            Thread ping = new Thread(() =>
            {
                while(ClientsList.Clients.Count > 0)
                {
                    foreach(var o in ClientsList.Clients)
                    {
                        var package = new Share
                        {
                            SenderName = "Server",
                            ReciverName = o.Key,
                            Tag = "ping",
                            Data = "ping"
                        };
                        byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(package));
                        try
                        {
                            Enviroments.Server.Send(buffer, buffer.Length, o.Value.Ip.ToString(), o.Value.Port);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    Thread.Sleep(25000);
                }
            });
            ping.Name = "Ping";
            ping.Start();
        }
    }
}