using Newtonsoft.Json;
using PcManagerClient.Model;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PcManagerClient.PcManager
{
    class PCManagerClient
    {
        private static UdpClient _out = new UdpClient(0);
        private static IPEndPoint _remotePoint;
        private static string Name = "";

        public PCManagerClient(string name)
        {
            ConnectionModel config;
            try
            {
                config = JsonConvert.DeserializeObject<ConnectionModel>(File.ReadAllText("Settings/config.json"));
                _remotePoint = new IPEndPoint(IPAddress.Parse(config.IPAddress), config.Port);
            }
            catch (Exception)
            {
                _remotePoint = new IPEndPoint(IPAddress.Any, 5555);
            }
            var random = new Random();
            if (string.IsNullOrEmpty(name))
                Name = $"Client #{random.NextInt64(10)}";
            Thread sender = new Thread(() =>
            {
                while(true)
                {
                    Data("127.0.0.1", 5555);
                    Thread.Sleep(1000);
                }
            });
            sender.Start();
            Task.Run(Listen);
        }

        private static async Task Listen()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            while (!cts.IsCancellationRequested)
            {
                UdpReceiveResult result = await _out.ReceiveAsync();
                MessageHandler(result);
            }
        }

        private static void MessageHandler(UdpReceiveResult result)
        {
            try
            {
                var d = JsonConvert.DeserializeObject<Share>(Encoding.UTF8.GetString(result.Buffer));
                TextLables.RightLabel.Text = d.Data.ToString();
            }
            catch (Exception ex)
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                TextLables.RightLabel.Text = $"Packet error! Check data!\nError: {ex.ToString()}";
            }
        }

        private static void Data(string ip, int port)
        {
            Console.WriteLine("Start sending");
            Sending.Send(_out, new Share
            {
                SenderName = Name,
                ReciverName = "",
                Data = DateTime.Now,
                Tag = "Data"
            }, ip, port);
        }

    }
}
