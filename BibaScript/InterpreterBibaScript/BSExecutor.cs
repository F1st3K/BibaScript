using BeutifulConsole;

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

        public void Run(string programm)
        {
            View.ColorWrite(programm, System.ConsoleColor.Cyan);
        }

    }
}
