using BeutifulConsole;
using SyntaxBibaScript;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal class ExecuteThread
    {
        private string _endString;
        private string _beginCode;
        private string _endCode;

        public ExecuteThread()
        {
            CodeSeparators.GetInstance().TryGetValue(SpecialWords.EndInstruction, out _endString);
            CodeSeparators.GetInstance().TryGetValue(SpecialWords.BeginCode, out _beginCode);
            CodeSeparators.GetInstance().TryGetValue(SpecialWords.EndCode, out _endCode);
        }

        public void PeformBlockCommand(string[] commands)
        {
            View.ColorWriteLine(_beginCode, System.ConsoleColor.Cyan);
            for (int i = 0; i < commands.Length; i++)
            {
                var cmds = new List<string>();
                if (commands[i] == _beginCode)
                {
                    i++;
                    var count = 1;
                    for (; commands[i] != _endCode && count != 0; i++)
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
                    }
                    new ExecuteThread().PeformBlockCommand(cmds.ToArray());
                    continue;
                }
                var countBraket = 0;
                var codeMode = false;
                for (; codeMode ? countBraket != 0 : commands[i] != _endString; i++)
                {
                    if (commands[i] == _beginCode && !codeMode)
                        codeMode = true;
                    if (commands[i] == _beginCode)
                        countBraket++;
                    if (commands[i] == _endCode)
                        countBraket--;
                    if (countBraket < 0)
                        throw new System.Exception("Invalid code separator " + i);
                    if (i >= commands.Length)
                        throw new System.Exception("Non such end command " + i);
                    cmds.Add(commands[i]);
                }
                PeformCommand(cmds.ToArray());
            }
            View.ColorWriteLine(_endCode, System.ConsoleColor.Cyan);
        }

        public void PeformCommand(string[] command)
        {
            View.ColorWriteLine(_endString, System.ConsoleColor.Yellow);
        }
    }
}
