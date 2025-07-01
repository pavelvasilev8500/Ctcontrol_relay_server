using Newtonsoft.Json;
using PcManagerClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PcManagerClient.PcManager
{
    public static class Sending
    {
        public static void Send(UdpClient cleint, Share packet, string ip, int port)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(packet));
            try
            {
                TextLables.LeftLabel.Text = "Sendign...";
                cleint.Send(buffer, buffer.Length, ip, port);
                TextLables.LeftLabel.Text = "Done";
            }
            catch (Exception ex)
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                TextLables.LeftLabel.Text = $"Error while sending: {ex.ToString()}";
                //Console.ResetColor();
            }
        }
    }
}
