using SyntaxBibaScript;

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
    }
}
