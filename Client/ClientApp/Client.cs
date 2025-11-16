using Client.Model;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Client.ClientApp
{
    class Client
    {
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        private static IPEndPoint _remotePoint;
        private static UdpClient _out = new UdpClient(0);
        private static Share _share;
        private static byte[] _data;
        private static bool _enable = true;
        private static string Name = "";

        public Client(string name)
        {
            var config = JsonConvert.DeserializeObject<ConnectionModel>(File.ReadAllText("Settings/config.json"));
            var random = new Random();
            if (string.IsNullOrEmpty(name))
                Name = $"Client #{random.NextInt64(10)}";
            else
                Name = name;
            _remotePoint = new IPEndPoint(IPAddress.Parse(config.IPAddress), config.Port);
            Thread controlThread = new Thread(Control);
            controlThread.Name = "Control";
            controlThread.Start();
            Task.Run(Listen);
        }

        private static void Control()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            while (!_cts.IsCancellationRequested)
            {
                Console.WriteLine("1 - register");
                Console.WriteLine("2 - send data");
                Console.WriteLine("q - exit");
                var key = Console.ReadKey();
                Console.WriteLine();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        CreateDataToSend(Name, "", "Connect", null);
                        break;
                    case ConsoleKey.D2:
                        Console.Write("Reciver Name: ");
                        var resName = Console.ReadLine();
                        Console.WriteLine("1 - Shutdown\n2 - Reboot\n3 - Sleep");
                        var action = Console.ReadLine();
                        CreateDataToSend(Name, resName, "Data", action);
                        break;
                    case ConsoleKey.Q:
                        CreateDataToSend(Name, "", "Disconnect", null);
                        _cts.Cancel();
                        break;
                }
            }
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
                Console.WriteLine(d.Data);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Packet error! Check data!\nError: {ex.ToString()}");
            }
        }

        private static async void Send(object? state)
        {
            _data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(state));
            await _out.SendAsync(_data, _remotePoint);
        }

        private static void CreateDataToSend(string name, string reciverName, string tag, object sendData)
        {
            var data = new Share
            {
                SenderName = name,
                ReciverName = reciverName,
                Data = sendData,
                Tag = tag
            };
            Send(data);
        }

        //static async Task Main(string[] args)
        //{
        //    _remotePoint =  new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
        //    while(!_cts.IsCancellationRequested)
        //    {
        //        Console.WriteLine("1 - register\n2 - send data\nq - exit");
        //        var key = Console.ReadKey();
        //        switch(key.Key)
        //        {
        //            case ConsoleKey.D1:
        //                Register();
        //                break;
        //            case ConsoleKey.D2:
        //                Console.WriteLine("World");
        //                break;
        //            case ConsoleKey.Q:
        //                Disconnect();
        //                _cts.Cancel();
        //                break;
        //        }
        //        Console.WriteLine();
        //        var recivePacket = await _out.ReceiveAsync();
        //        Task.Run(() => MessageHandler(recivePacket));
        //    }
        //TimerCallback tm = new TimerCallback(Send);
        //var timer = new System.Threading.Timer(tm, args, 0, 1000);
        //_data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Share
        //{
        //    SenderName = args[1].ToString(),
        //    ReciverName = null,
        //    Data = null
        //}));
        //await _out.SendAsync(_data, _remotePoint);
        //Task.Run(async () =>
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            var receivedResult = await _out.ReceiveAsync();
        //            string jsonData = Encoding.UTF8.GetString(receivedResult.Buffer);
        //            var data = JsonConvert.DeserializeObject<ControlDataModel>(jsonData);
        //            Console.WriteLine($"{data.WorkTime} - {data.Control}");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine();
        //            Console.WriteLine(ex.Message);
        //        }
        //    }
        //});
        //while(true)
        //{
        //    Console.Clear();
        //    _data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(CreateData(0, args)));
        //    var c = Console.ReadKey();
        //    switch(c.Key)
        //    {
        //        case ConsoleKey.S:
        //            _data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(CreateData(1, args)));
        //            break;
        //        case ConsoleKey.R:
        //            _data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(CreateData(2, args)));
        //            break;
        //        case ConsoleKey.L:
        //            _data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(CreateData(3, args)));
        //            break;
        //        case ConsoleKey.Q:
        //            Environment.Exit(0);
        //            break;
        //    }
        //    await _out.SendAsync(_data, _remotePoint);
        //} 
        //}

        //private static Share CreateData(int control, string[] args)
        //{
        //    return _share = new Share
        //    {
        //        SenderName = args[1].ToString(),
        //        ReciverName = args[2].ToString(),
        //        Data = new ControlDataModel
        //        {
        //            WorkTime = DateTime.Now.ToString("HH:mm:ss"),
        //            Control = control
        //        }
        //    };
        //}


    }
}
