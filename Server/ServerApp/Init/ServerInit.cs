using Newtonsoft.Json;
using Server.Logs;
using Server.Model;
using System.Net;
using System.Net.Sockets;

namespace Server.ServerApp.Init
{
    class ServerInit
    {
        public static UdpClient Server;
        public static string ServerPort = "5555";
        private static readonly string _path = Path.Combine(AppContext.BaseDirectory, "config.json");
        public ServerInit()
        {
            ConsoleOutput.Output(new string[] { $"{DateTime.Now} Server Init", $"{DateTime.Now} Load settings..." });
            if(!File.Exists(_path))
            {
                ConsoleOutput.Output(ConsoleColor.Red, new string[] { $"{DateTime.Now} No settings file in {_path}!", $"{DateTime.Now} Creating settings file with IP: 0.0.0.0 Port: 5555" });
                var setings = JsonConvert.SerializeObject(new SettingsModel
                {
                    IPAddress = "0.0.0.0",
                    Port = 5555
                });
                File.AppendAllText(_path, setings);
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Settings file created, settings loaded");
            }
            //var port = Environment.GetEnvironmentVariable("SERVER_PORT");
            //if (port == null)
            //{
            //    var config = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(_path));
            //    ServerPort = config.Port.ToString();
            //}
            //else
            //    ServerPort = port; 
            Server = new UdpClient(new IPEndPoint(IPAddress.Any, int.Parse(ServerPort)));
        }
    }
}
