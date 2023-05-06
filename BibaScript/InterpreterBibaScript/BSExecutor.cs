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

        //Run programm
        public void Run(string programm)
        {
            var parser = new Parser(new string[] { " ", "\n", "\t", "\r" });
            var commands = parser.Parse(programm);
            var thread = new ExecuteThread();
            try
            {
                thread.PeformBlockCommand(commands);
            }
            catch (System.Exception ex)
            {
                View.ColorWriteLine(ex.Message, System.ConsoleColor.Red);
            }
        }

    }
}
