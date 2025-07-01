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
        public static string ServerPort = "";
        private static readonly string _path = Path.Combine(AppContext.BaseDirectory, "config.json");
        public ServerInit()
        {
            if(!File.Exists(_path))
            {
                var setings = JsonConvert.SerializeObject(new SettingsModel
                {
                    IPAddress = "0.0.0.0",
                    Port = 5555
                });
                File.AppendAllText(_path, setings);
            }
            var port = Environment.GetEnvironmentVariable("SERVER_PORT");
            if (port == null)
            {
                var config = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(_path));
                ServerPort = config.Port.ToString();
            }
            else
                ServerPort = port; 
            Log.AddLog("Start settings initialization");
            Log.AddLog("Initialization done");
            Server = new UdpClient(new IPEndPoint(
                IPAddress.Any, int.Parse(ServerPort)));
            Log.AddLog($"Settings port: {ServerPort}");
        }
    }
}
