using Server.Model;
using Server.ServerApp.ClientManage;
using Server.ServerApp.DataManage;
using System.Net.Sockets;

namespace Server.ServerApp
{
    static class ClientManager
    {
        public static void Manage(Share packet, UdpReceiveResult result)
        {

            switch(packet.Tag)
            {
                case "Connect":
                    var client = UserController.Add(packet, result);
                    if (client.Item1)
                        Sending.SendConfirmResult(packet.Tag, client.Item2, client.Item3);
                    break;
                case "Disconnect":
                    UserController.Delete(packet, result);
                    break;
                case "Data":
                    ResendData.Resend(packet, result);
                    break;
                case "":
                    UserController.UnknownClient(packet, result);
                    break;
            }
        }
    }
}
