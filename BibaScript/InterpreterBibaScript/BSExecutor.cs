using BeutifulConsole;
using System;
using System.Runtime.InteropServices;

namespace InterpreterBibaScript
{
    public sealed class BSExecutor
    {
        private static BSExecutor _instance;

        public static BSExecutor GetInstance()
        {
            if (_instance == null)
                _instance = new BSExecutor();
            return _instance;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        //Run programm
        public void Run(string programm)
        {
            var parser = new Parser(new string[] { " ", "\n", "\t", "\r" });
            var commands = parser.Parse(programm);
            ShowConsoleWindow();
            try
            {
                new ExecuteThread(commands).PeformBlockCommand();
            }
            catch (Exception ex)
            {
                View.ColorWriteLine(ex.Message, ConsoleColor.Red);
            }
            Console.ReadKey();
            HideConsoleWindow();
            Console.Clear();
        }

    }
}
