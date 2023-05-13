using SyntaxBibaScript;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class ExecuteThread
    {
        private readonly string _endString;
        private readonly string _beginCode;
        private readonly string _continueCode;
        private readonly string _endCode;

        private string[] _commands;

        public ExecuteThread(string[] commands)
        {
            _commands = commands;
            _endString = CodeSeparators.GetInstance().GetValue(SpecialWords.EndInstruction);
            _beginCode = CodeSeparators.GetInstance().GetValue(SpecialWords.BeginCode);
            _continueCode = CodeSeparators.GetInstance().GetValue(SpecialWords.ContinueCode);
            _endCode = CodeSeparators.GetInstance().GetValue(SpecialWords.EndCode);
        }

        public void PeformBlockCommand()
        {
            var values = Memory.GetInstance().GetAllNames();

            for (int i = 0; i < _commands.Length;)
            {
                if (_commands[i] == _beginCode)
                {
                    new ExecuteThread(Code.FindBlock(i, _commands, _beginCode, _endCode, out i)).PeformBlockCommand();
                    continue;
                }
                var cmds = FindCommand(i, out i);
                if (cmds[0] == CodeTypeWords.GetInstance().GetValue(SpecialWords.FunctionReturn))
                    break;
                new ExecuteInstruction(cmds).PeformCommand();
            }
            Memory.GetInstance().RemoveWithout(values.ToArray());
        }

        public string PeformBlockCommand(Types returnType)
        {
            var values = Memory.GetInstance().GetAllNames();
            string result = string.Empty;

            for (int i = 0; i < _commands.Length;)
            {
                if (_commands[i] == _beginCode)
                {
                    new ExecuteThread(Code.FindBlock(i, _commands, _beginCode, _endCode, out i)).PeformBlockCommand();
                    continue;
                }
                var cmds = FindCommand(i, out i);
                if (cmds[0] == CodeTypeWords.GetInstance().GetValue(SpecialWords.FunctionReturn))
                {
                    var list = new List<string>(Code.FindInstruction(1, cmds, _endString, out _));
                    list.RemoveAt(list.Count - 1);
                    result =  Comber.Calculate(returnType, list.ToArray());
                    break;
                }
                new ExecuteInstruction(cmds).PeformCommand();
            }
            Memory.GetInstance().RemoveWithout(values.ToArray());
            return result;
        }

        private string[] FindCommand(int i, out int end)
        {
            var cmds = new List<string>();
            do
            {
                var mode = false;
                var countBaket = 0;
                if (_commands[i] == _continueCode)
                    i++;
                do
                {
                    if (_commands[i] == _beginCode && countBaket == 0)
                        mode = true;
                    if (_commands[i] == _beginCode)
                        countBaket++;
                    if (_commands[i] == _endCode)
                        countBaket--;
                    if (countBaket < 0)
                        throw new System.Exception("Invalid code separator " + _commands[i]);
                    cmds.Add(_commands[i]);
                    i++;
                    if (i > _commands.Length)
                        throw new System.Exception("Non such end command " + _beginCode + " ... " + _commands[_commands.Length - 1]);
                }
                while (mode ? countBaket != 0 : _commands[i - 1] != _endString);
                if (i >= _commands.Length)
                    break;
            }
            while (_commands[i] == _continueCode);
            end = i;
            return cmds.ToArray();
        }
    }
}
