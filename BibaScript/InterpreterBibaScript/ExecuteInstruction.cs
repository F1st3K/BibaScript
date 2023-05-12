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
        private string _constFunc;
        private string _paramSeparator;

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
            _paramSeparator = CodeSeparators.GetInstance().GetValue(SpecialWords.SeparatorParameters);
            _constFunc = CodeConstructions.GetInstance().GetValue(SpecialWords.DeclarationFunction);
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
            else if (_command[0] == _constFunc)
                DeclareFunction();
            else throw new Exception("No such instruction or name: " + _command[0]);
        }

        private void DeclareFunction()
        {
            CodeTypeWords.GetInstance().TryGetKey(_command[1], out var wordType);
            var name = _command[2];
            var parameters = Parameter.ConvertToParameters(Code.FindBlock(3, _command, _beginParam, _endParam, out var i), _paramSeparator);
            var blockCommand = Code.FindBlock(i, _command, _beginCode, _endCode, out i);
            if (wordType == SpecialWords.ValueVoid)
            {
                Memory.GetInstance().DeclareFunction(name, blockCommand, parameters);
                return;
            }
        }

        private void CallProcedure()
        {
            if (Memory.GetInstance().Procedures.TryGetValue(_command[0], out var proc) == false)
                throw new Exception("No such procedure: " + _command[0]);
            var values = new List<string>();
            var blockParam = Code.FindBlock(1, _command, _beginParam, _endParam, out var i);
            int k = 0;
            foreach (var p in proc.Parameters)
            {
                var parameter = new List<string>(Code.FindInstruction(k, blockParam, _paramSeparator, out k));
                if (parameter[parameter.Count - 1] == _paramSeparator)
                    parameter.RemoveAt(parameter.Count - 1);
                values.Add(Comber.Calculate(p.Type, parameter.ToArray()));
            }
            Memory.GetInstance().RunFunction(_command[0], values.ToArray());
        }

        private string CallFunction()
        {
            if (Memory.GetInstance().Functions.TryGetValue(_command[0], out var func) == false)
                throw new Exception("No such function: " + _command[0]);
            var values = new List<string>();
            var blockParam = Code.FindBlock(1, _command, _beginParam, _endParam, out var i);
            int k = 1;
            foreach (var p in func.Parameters)
            {
                var parameter = new List<string>(Code.FindInstruction(k, blockParam, _paramSeparator, out k));
                if (parameter[parameter.Count - 1] == _paramSeparator)
                    parameter.RemoveAt(parameter.Count - 1);
                values.Add(Comber.Calculate(p.Type, parameter.ToArray()));
            }
            Memory.GetInstance().RunFunction(_command[0], out var result, values.ToArray());
            return result;
        }

        private void RunWhileOperator()
        {
            while (true)
            {
                int i = 1;
                var value = Comber.Calculate(Types.Boolean, Code.FindBlock(i, _command, _beginParam, _endParam, out i));
                if (value == CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue))
                {
                    if (_command[i] == _beginCode)
                        new ExecuteThread(Code.FindBlock(i, _command, _beginCode, _endCode, out i)).PeformBlockCommand();
                    else new ExecuteInstruction(Code.FindInstruction(i, _command, _endInstruction, out i)).PeformCommand();
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
                    var value = Comber.Calculate(Types.Boolean , Code.FindBlock(i, _command, _beginParam, _endParam, out i));
                    if (value == CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue))
                    {
                        if (_command[i] == _beginCode)
                            new ExecuteThread(Code.FindBlock(i, _command, _beginCode, _endCode, out i)).PeformBlockCommand();
                        else new ExecuteInstruction(Code.FindInstruction(i, _command, _endInstruction, out i)).PeformCommand();
                        break;
                    }
                    else
                    {
                        if (_command[i] == _beginCode)
                            Code.FindBlock(i, _command, _beginCode, _endCode, out i);
                        else Code.FindInstruction(i, _command, _endInstruction, out i);
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
                                new ExecuteThread(Code.FindBlock(i, _command, _beginCode, _endCode, out i)).PeformBlockCommand();
                            else new ExecuteInstruction(Code.FindInstruction(i, _command, _endInstruction, out i)).PeformCommand();
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
            var t = Code.ConvertWordTypeToTypes(type);
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
