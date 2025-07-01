using Server.Logs;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.StartLog();
            await ServerApp.Server.StartServer(args);
        }
    }
}
