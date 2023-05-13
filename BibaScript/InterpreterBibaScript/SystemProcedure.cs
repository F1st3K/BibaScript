using System;

namespace InterpreterBibaScript
{
    internal class SystemProcedure : Procedure
    {
        public readonly Func<string[], string> Procedure;
        public SystemProcedure(Func<string[], string> p, params Parameter[] parameters)
            : base(string.Empty, new string[] { }, new Memory(), parameters)
        {
            Procedure = p;
        }

        public override void Run(params string[] values)
        {
            Procedure.Invoke(values);   
        }
    }
}
