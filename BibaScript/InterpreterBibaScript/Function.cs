using SyntaxBibaScript;
using System;

namespace InterpreterBibaScript
{
    internal sealed class Function
    {
        public readonly Types ReturnType;
        public readonly Types[] ParamTypes;
        public readonly string[] Commands;

        public Function(Types returnType, string[] commands, params Types[] paramTypes)
        {
            ReturnType = returnType;
            Commands = commands;
            ParamTypes = paramTypes;
        }

        public string Run(params string[] parameters)
        {
            if (parameters.Length != ParamTypes.Length)
                throw new Exception("Invalid count parameters");
            var thread = new ExecuteThread(ConvertParameters(Commands, parameters));
            return Comber.Calculate(ReturnType, new string[] { thread.PeformBlockCommand() });
        }

        private string[] ConvertParameters(string[] commands, string[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
