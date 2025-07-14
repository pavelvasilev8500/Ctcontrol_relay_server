using Server.ServerApp;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleOutput.Output(ConsoleColor.Green, $"{DateTime.Now} Program strart");
            await ServerApp.Server.StartServer(args);
        }
    }
}