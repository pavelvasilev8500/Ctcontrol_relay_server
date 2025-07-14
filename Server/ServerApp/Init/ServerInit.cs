using Newtonsoft.Json;
using Server.Data;
using Server.Model;
using System.Net;
using System.Net.Sockets;

namespace Server.ServerApp.Init
{
    class ServerInit
    {
        public static readonly string _path = Path.Combine(AppContext.BaseDirectory, "config.json");
        public bool StartServerInit()
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
            else
            {
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Load settings from file");
                var config = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText(_path));
                Enviroments.ServerPort = config.Port.ToString();
            }
            //var port = Environment.GetEnvironmentVariable("SERVER_PORT");
            //if (port == null)
            //{
            //}
            //else
            //    ServerPort = port; 
            try
            {
                Enviroments.Server = new UdpClient(new IPEndPoint(IPAddress.Any, int.Parse(Enviroments.ServerPort)));
                ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Server started at port: {Enviroments.ServerPort}");
                return true;

            }
            catch (Exception ex)
            {
                ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Server start failed at port: {Enviroments.ServerPort}");
                return false;
            }
        }
    }
}
