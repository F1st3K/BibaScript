using SyntaxBibaScript;
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
                var param = Code.FindInstruction(p, values, separator, out p);
                p--;
                CodeTypes.GetInstance().TryGetKey(param[0], out var t);
                var type = Code.ConvertWordTypeToTypes(t);
                parameters.Add(new Parameter(type, param[1]));
            }
            return parameters.ToArray();
        }
    }
}
