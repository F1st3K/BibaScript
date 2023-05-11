using BeutifulConsole;
using SyntaxBibaScript;
using System;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class ExecuteInstruction
    {
        private string[] _command;
        private string _assign;
        private string _beginParam;
        private string _endParam;
        private string _beginCode;
        private string _endCode;
        private string _endInstruction;
        private string _constIf;
        private string _constElse;
        private string _constWhile;

        public static string[] FindBlock(int i, string[] mass, string begin, string end, out int endParam)
        {
            var expr = new List<string>();
            int count = 0;
            do
            {
                if (mass[i] == begin)
                    count++;
                else if (mass[i] == end)
                    count--;
                expr.Add(mass[i]);
                i++;
            } while (i < mass.Length && count != 0);
            expr.RemoveAt(0);
            expr.RemoveAt(expr.Count - 1);
            endParam = i;
            return expr.ToArray();
        }

        public static string[] FindInstruction(int i, string[] mass, string end, out int endParam)
        {
            var expr = new List<string>();
            do
            {

                expr.Add(mass[i]);
                if (mass[i] == end)
                {
                    i++;
                    break;
                }
                i++;
            } while (i < mass.Length);
            endParam = i;
            return expr.ToArray();
        }

        public ExecuteInstruction(string[] command)
        {
            _command = command;
            _assign = CodeSeparators.GetInstance().GetValue(SpecialWords.Assign);
            _beginParam = CodeSeparators.GetInstance().GetValue(SpecialWords.BeginParameters);
            _endParam = CodeSeparators.GetInstance().GetValue(SpecialWords.EndParameters);
            _beginCode = CodeSeparators.GetInstance().GetValue(SpecialWords.BeginCode);
            _endCode = CodeSeparators.GetInstance().GetValue(SpecialWords.EndCode);
            _endInstruction = CodeSeparators.GetInstance().GetValue(SpecialWords.EndInstruction);
            _constIf = CodeConstructions.GetInstance().GetValue(SpecialWords.ConstructionIf);
            _constElse = CodeConstructions.GetInstance().GetValue(SpecialWords.ConstructionElse);
            _constWhile = CodeConstructions.GetInstance().GetValue(SpecialWords.ConstructionWhile);
        }

        //This method run command
        public void PeformCommand()
        {
            View.ColorWriteLine(_command[0], System.ConsoleColor.Yellow);
            if (CodeTypes.GetInstance().ContainsValue(_command[0]))
                DeclareVariable();
            else if (Memory.GetInstance().GetAllNames().Contains(_command[0]))
                if (Memory.GetInstance().Functions.ContainsKey(_command[0]))
                    CallFunction();
                else if (Memory.GetInstance().Procedures.ContainsKey(_command[0]))
                    CallProcedure();
                else AssignVariable();
            else if (_command[0] == _constIf)
                RunIfOperator();
            else if (_command[0] == _constWhile)
                RunWhileOperator();
            else throw new Exception("No such instruction or name: " + _command[0]);
        }

        private void CallProcedure()
        {
            Memory.GetInstance().RunFunction(_command[0]);
        }

        private void CallFunction()
        {
            Memory.GetInstance().RunFunction(_command[0], out var result);
        }

        private void RunWhileOperator()
        {
            
            while (true)
            {
                int i = 1;
                var value = Comber.Calculate(Types.Boolean, FindBlock(i, _command, _beginParam, _endParam, out i));
                if (value == CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue))
                {
                    if (_command[i] == _beginCode)
                        new ExecuteThread(FindBlock(i, _command, _beginCode, _endCode, out i)).PeformBlockCommand();
                    else new ExecuteInstruction(FindInstruction(i, _command, _endInstruction, out i)).PeformCommand();
                    continue;
                }
                else break;
            }
        }


        private void RunIfOperator()
        {
            int i = 1;
            do
                try
                {
                    var value = Comber.Calculate(Types.Boolean ,FindBlock(i, _command, _beginParam, _endParam, out i));
                    if (value == CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue))
                    {
                        if (_command[i] == _beginCode)
                            new ExecuteThread(FindBlock(i, _command, _beginCode, _endCode, out i)).PeformBlockCommand();
                        else new ExecuteInstruction(FindInstruction(i, _command, _endInstruction, out i)).PeformCommand();
                        break;
                    }
                    else
                    {
                        if (_command[i] == _beginCode)
                            FindBlock(i, _command, _beginCode, _endCode, out i);
                        else FindInstruction(i, _command, _endInstruction, out i);
                    }
                    if (_command[i] == _constElse)
                    {
                        i++;
                        if (_command[i] == _constIf)
                        {
                            i++;
                            continue;
                        }
                        else
                        {
                            if (_command[i] == _beginCode)
                                new ExecuteThread(FindBlock(i, _command, _beginCode, _endCode, out i)).PeformBlockCommand();
                            else new ExecuteInstruction(FindInstruction(i, _command, _endInstruction, out i)).PeformCommand();
                            break;
                        }
                    }
                    else throw new Exception("no such else block");
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid syntax: " + _command[i-1] + " :" + ex.Message);
                }
                while (i < _command.Length);
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
            if (_command[2] == _assign)
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
            string value = Comber.Calculate(Memory.GetInstance().GetVariableType(_command[0]), list.ToArray());
            Memory.GetInstance().SetVariable(_command[0], value);
        }

    }
}
