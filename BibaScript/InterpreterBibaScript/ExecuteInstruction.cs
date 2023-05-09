using BeutifulConsole;
using SyntaxBibaScript;
using System;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class ExecuteInstruction
    {
        private string[] _command;

        public ExecuteInstruction(string[] command)
        {
            _command = command;
        }

        //This method run command
        public void PeformCommand()
        {
            View.ColorWriteLine(_command[0], System.ConsoleColor.Yellow);
            if (CodeTypes.GetInstance().ContainsValue(_command[0]))
            {
                DeclareVariable();
                return;
            }
        }

        private void DeclareVariable()
        {
            CodeTypes.GetInstance().TryGetKey(_command[0], out var type);
            Memory.GetInstance().DeclareVariable(type, _command[1]);
            if (_command.Length > 3)
            {
                var list = new List<string>(_command);
                list.RemoveAt(0);
                _command = list.ToArray();
                AssignVariable();
            }
        }

        private void AssignVariable()
        {
            string value = "0";
            Memory.GetInstance().AssignVariable(_command[0], value);
        }
    }
}
