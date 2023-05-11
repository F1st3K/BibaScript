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

        //This cycle run separate code on block commands and run execute for one command
        public void PeformBlockCommand()
        {
            var values = Memory.GetInstance().GetAllNames();
            for (int i = 0; i < _commands.Length; )
            {
                var cmds = new List<string>();
                if (_commands[i] == _beginCode)
                {
                    //Such block command and run new thread
                    var count = 0;
                    do
                    {
                        if (_commands[i] == _beginCode)
                            count++;
                        if (_commands[i] == _endCode)
                            count--;
                        if (count < 0)
                            throw new System.Exception("Invalid code separator " + _commands[i]);
                        cmds.Add(_commands[i]);
                        i++;
                        if (i >= _commands.Length)
                            throw new System.Exception("Non such end command " + _beginCode + " ... " + _commands[_commands.Length - 1]);
                    }
                    while (count != 0);
                    cmds.RemoveAt(0);
                    cmds.RemoveAt(cmds.Count - 1);
                    new ExecuteThread(cmds.ToArray()).PeformBlockCommand();
                    continue;
                }
                //Such and run command
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
                new ExecuteInstruction(cmds.ToArray()).PeformCommand();
            }
            var m = Memory.GetInstance();
            Memory.GetInstance().RemoveWithout(values.ToArray());
        }
    }
}
