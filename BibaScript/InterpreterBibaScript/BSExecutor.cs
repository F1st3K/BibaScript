using BeutifulConsole;
using System.Collections.Generic;

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
            programm = "{\n" + programm + "\n}";
            var parser = new Parser();
            var commands = parser.Parse(programm);
            
        }

    }
}
