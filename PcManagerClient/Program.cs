using PcManagerClient.PcManager;
using Terminal.Gui;

namespace PcManagerClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Application.Init();

            var top = Application.Top;
            var mainWindow = new Window("Multi-Stream Console")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var leftFrame = new FrameView("Stream A")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Fill()
            };
            leftFrame.Add(TextLables.LeftLabel);
            var rightFrame = new FrameView("Stream B")
            {
                X = Pos.Right(leftFrame),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            rightFrame.Add(TextLables.RightLabel);

            mainWindow.Add(leftFrame, rightFrame);
            top.Add(mainWindow);

            if (args.Length > 0)
                new PCManagerClient(args[0]);
            else
                new PCManagerClient(null);

            Application.Run();
        }
    }
}
