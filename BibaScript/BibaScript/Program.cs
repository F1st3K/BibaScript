using System;
using BeutifulConsole;

namespace BibaScript
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var file in args)
            {
                try
                {
                    View.ColorWriteLine(file, ConsoleColor.Cyan);
                }
                catch (Exception ex)
                {
                    View.ColorWriteLine(ex.Message, ConsoleColor.Red);
                    continue;
                }
            }
        }

        
    }
}
