using System;
using System.IO;
using BeutifulConsole;
using InterpreterBibaScript;

namespace BibaScript
{
    class Program
    {
        static void Main(string[] args)
        {
            //foreach (var file in args)
            //{
                try
                {
                    BSExecutor.GetInstance().Run(File.ReadAllText("prog.bs"));
                }
                catch (Exception ex)
                {
                    View.ColorWriteLine(ex.Message, ConsoleColor.Red);
                    //continue;
                }
            //}
        }

        
    }
}
