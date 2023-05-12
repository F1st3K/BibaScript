using SyntaxBibaScript;
using System;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class Procedure
    {
        public readonly Parameter[] Parameters;
        public readonly string[] Commands;

        public Procedure(string[] commands, params Parameter[] parameters)
        {
            Commands = commands;
            Parameters = parameters;
        }

        public void Run(params string[] values)
        {
            if (values.Length != Parameters.Length)
                throw new Exception("Invalid count parameters");
            new ExecuteThread(ConvertParameters(Commands, values)).PeformBlockCommand();
        }

        private string[] ConvertParameters(string[] commands, string[] values)
        {
            var list = new List<string>();
            var assign = CodeSeparators.GetInstance().GetValue(SpecialWords.Assign);
            var end = CodeSeparators.GetInstance().GetValue(SpecialWords.EndInstruction);
            for (int i = 0; i < values.Length; i++)
            {
                string nameType;
                switch (Parameters[i].Type)
                {
                    case Types.Integer: nameType = CodeTypes.GetInstance().GetValue(SpecialWords.TypeInteger); break;
                    case Types.String: nameType = CodeTypes.GetInstance().GetValue(SpecialWords.TypeString); break;
                    case Types.Float: nameType = CodeTypes.GetInstance().GetValue(SpecialWords.TypeFloat); break;
                    case Types.Boolean: nameType = CodeTypes.GetInstance().GetValue(SpecialWords.TypeBoolean); break;
                    default:
                        throw new Exception("Invalid type: " + Parameters[i].Type.ToString());
                }
                list.AddRange(new string[] { nameType, Parameters[i].Name, assign, values[i], end });
            }
            list.AddRange(commands);
            return list.ToArray();
        }
    }
}
