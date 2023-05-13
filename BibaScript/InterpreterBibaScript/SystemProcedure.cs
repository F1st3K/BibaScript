using System;

namespace InterpreterBibaScript
{
    internal class SystemProcedure : Procedure
    {
        public readonly Func<string[], string> Procedure;
        public SystemProcedure(Func<string[], string> p, params Parameter[] parameters)
            : base(parameters)
        {
            Procedure = p;
        }

        public override void Run(params string[] values)
        {
            Procedure.Invoke(values);   
        }
    }
}
