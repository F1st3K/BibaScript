using SyntaxBibaScript;
using System;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class Parameter
    {
        public readonly Types Type;

        public readonly string Name;

        public Parameter(Types type, string name)
        {
            Type = type;
            Name = name;
        }

        public static Parameter[] ConvertToParameters(string[] values, string separator)
        {
            var parameters = new List<Parameter>();
            for (int p = 0; p < values.Length; p++)
            {
                var param = new List<string>(Code.FindInstruction(p, values, separator, out p));
                if (param[param.Count - 1] == separator)
                    param.RemoveAt(param.Count - 1);
                if (param.Count != 2)
                    throw new Exception("Invalid declare parameter: " + Code.SubStringMass(param.ToArray()));
                p--;
                CodeTypes.GetInstance().TryGetKey(param[0], out var t);
                var type = Code.ConvertWordTypeToTypes(t);
                parameters.Add(new Parameter(type, param[1]));
            }
            return parameters.ToArray();
        }
    }
}
