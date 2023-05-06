using SyntaxBibaScript;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class ExecuteThread
    {
        private readonly string _endString;
        private readonly string _beginCode;
        private readonly string _endCode;

        public ExecuteThread()
        {
            //Try get syntax instruction with library
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.EndInstruction, out _endString) == false)
                throw new System.Exception("No such end instruction syntax");
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.BeginCode, out _beginCode) == false)
                throw new System.Exception("No such begin code syntax");
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.EndCode, out _endCode) == false)
                throw new System.Exception("No such end code syntax");
        }

        //This cycle run separate code on block commands and run execute for one command
        public void PeformBlockCommand(string[] commands)
        {
            var execute = new ExecuteInstruction();
            for (int i = 0; i < commands.Length; )
            {
                var cmds = new List<string>();
                if (commands[i] == _beginCode)
                {
                    //Such block command and run new thread
                    var count = 0;
                    do
                    {
                        if (commands[i] == _beginCode)
                            count++;
                        if (commands[i] == _endCode)
                            count--;
                        if (count < 0)
                            throw new System.Exception("Invalid code separator " + i);
                        if (i >= commands.Length)
                            throw new System.Exception("Non such end command " + i);
                        cmds.Add(commands[i]);
                        i++;
                    }
                    while (count != 0);
                    cmds.RemoveAt(0);
                    cmds.RemoveAt(cmds.Count - 1);
                    var values = Memory.GetInstance().GetAllNames();
                    new ExecuteThread().PeformBlockCommand(cmds.ToArray());
                    Memory.GetInstance().RemoveWithout(values);
                    continue;
                }
                //Such and run command
                var mode = false;
                var countBaket = 0;
                    do
                    {
                        if (commands[i] == _beginCode && countBaket == 0)
                            mode = true;
                        if (commands[i] == _beginCode)
                            countBaket++;
                        if (commands[i] == _endCode)
                            countBaket--;
                        if (countBaket < 0)
                            throw new System.Exception("Invalid code separator " + i);
                        if (i >= commands.Length)
                            throw new System.Exception("Non such end command " + i);
                        cmds.Add(commands[i]);
                        i++;
                    }
                    while (mode ? countBaket != 0 : commands[i - 1] != _endString);
                execute.PeformCommand(cmds.ToArray());
            }
        }
    }
}
