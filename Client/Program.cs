namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
                new ClientApp.Client(args[0]);
            else
                new ClientApp.Client(null);
        }
    }
}
