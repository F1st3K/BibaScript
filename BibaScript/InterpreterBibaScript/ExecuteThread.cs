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
            //Try get syntax instruction with library
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.EndInstruction, out _endString) == false)
                throw new System.Exception("No such end instruction syntax");
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.BeginCode, out _beginCode) == false)
                throw new System.Exception("No such begin code syntax");
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.EndCode, out _endCode) == false)
                throw new System.Exception("No such end code syntax");
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.ContinueCode, out _continueCode) == false)
                throw new System.Exception("No such continue code syntax");
        }

        //This cycle run separate code on block commands and run execute for one command
        public void PeformBlockCommand()
        {
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
                            throw new System.Exception("Invalid code separator " + i);
                        if (i >= _commands.Length)
                            throw new System.Exception("Non such end command " + i);
                        cmds.Add(_commands[i]);
                        i++;
                    }
                    while (count != 0);
                    cmds.RemoveAt(0);
                    cmds.RemoveAt(cmds.Count - 1);
                    var values = Memory.GetInstance().GetAllNames();
                    new ExecuteThread(cmds.ToArray()).PeformBlockCommand();
                    Memory.GetInstance().RemoveWithout(values.ToArray());
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
                            throw new System.Exception("Invalid code separator " + i);
                        if (i >= _commands.Length)
                            throw new System.Exception("Non such end command " + i);
                        cmds.Add(_commands[i]);
                        i++;
                    }
                    while (mode ? countBaket != 0 : _commands[i - 1] != _endString);
                    if (i >= _commands.Length)
                        break;
                }
                while (_commands[i] == _continueCode);
                new ExecuteInstruction(cmds.ToArray()).PeformCommand();
            }
        }
    }
}
