using SyntaxBibaScript;
using System;

namespace InterpreterBibaScript
{
    internal sealed class SystemFunction : Function
    {
        public readonly Func<string[], string> Function;

        public SystemFunction(Func<string[], string> f, Types returnType, params Parameter[] parameters)
            : base(returnType, parameters)
        {
            Function = f;
        }

        public override string Run(params string[] values)
        {
            return Function.Invoke(values);    
        }
    }
}
