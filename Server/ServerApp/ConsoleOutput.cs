namespace Server.ServerApp
{
    static class ConsoleOutput
    {
        public static void Output(ConsoleColor color, string[] message)
        {
            Console.ForegroundColor = color;
            foreach (var o in message)
                Console.WriteLine(o);
            Console.ResetColor();
        }

        public static void Output(string[] message)
        {
            foreach (var o in message)
                Console.WriteLine(o);
            Console.ResetColor();
        }

        public static void Output(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Output(string message)
        {
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
