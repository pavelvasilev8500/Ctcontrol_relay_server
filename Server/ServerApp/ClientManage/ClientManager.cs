using Server.Model;
using Server.ServerApp.DataManage;
using System.Net.Sockets;

namespace Server.ServerApp.ClientManage
{
    static class ClientManager
    {
        public static void Manage(Share packet, UdpReceiveResult result)
        {
            ConsoleOutput.Output($"{DateTime.Now} Checking tag...");
            switch(packet.Tag)
            {
                case "Connect":
                    ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Tag [Connect] detected");
                    var client = UserController.Add(packet, result);
                    if (client.Item1)
                        SendData.SendConfirmResult(packet.Tag, client.Item2, client.Item3);
                    break;
                case "Disconnect":
                    ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Tag [Disconnect] detected");
                    UserController.Delete(packet, result);
                    break;
                case "Data":
                    ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Tag [Data] detected");
                    ResendData.Resend(packet, result);
                    break;
                case "ping":
                    ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Tag [ping] detected");

                    break;
                case "":
                    ConsoleOutput.Output(ConsoleColor.Red, $"{DateTime.Now} Empty tag");
                    UserController.UnknownClient(packet, result);
                    break;
            }
        }
    }
}
