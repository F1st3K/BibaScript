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
            Types t;
            switch (type)
            {
                case SpecialWords.TypeInteger:
                    t = Types.Integer;
                    break;
                case SpecialWords.TypeString:
                    t = Types.String;
                    break;
                case SpecialWords.TypeFloat:
                    t = Types.Float;
                    break;
                case SpecialWords.TypeBoolean:
                    t = Types.Boolean;
                    break;
                default:
                    throw new Exception("Invalid type " + type.ToString());
            }
            Memory.GetInstance().DeclareVariable(t, _command[1]);
            if (CodeSeparators.GetInstance().TryGetValue(SpecialWords.Assign, out var assign) && _command[2] == assign)
            {
                var list = new List<string>(_command);
                list.RemoveAt(0);
                _command = list.ToArray();
                AssignVariable();
            }
        }

        private void AssignVariable()
        {
            var list = new List<string>(_command);
            list.RemoveRange(0, 2);
            list.RemoveAt(list.Count - 1);
            var m = Memory.GetInstance();
            string value = Comber.Calculate(Memory.GetInstance().GetVariableType(_command[0]), list.ToArray());
            Memory.GetInstance().SetVariable(_command[0], value);
        }

    }
}
