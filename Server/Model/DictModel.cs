using System.Net;

namespace Server.Model
{
    class DictModel
    {
        public IPAddress Ip { get; set; }
        public int Port { get; set; }
        public bool IsAvalable { get; set; }
    }
}
