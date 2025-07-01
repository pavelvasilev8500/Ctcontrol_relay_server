using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace PcManagerClient.PcManager
{
    public static class TextLables
    {
        public static TextView LeftLabel = new TextView
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ReadOnly = true
        };

        public static TextView RightLabel = new TextView
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ReadOnly = true
        };
    }
}
