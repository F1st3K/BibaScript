using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class CodeSeparators : Dictionary<SpecialWords, string>
    {
        private static CodeSeparators _instance;

        public static CodeSeparators GetInstance()
        {
            if (_instance == null)
                _instance = new CodeSeparators();
            return _instance;
        }

        public CodeSeparators()
        {
            Add(SpecialWords.BeginCode, "{");
            Add(SpecialWords.EndCode, "}");
            Add(SpecialWords.BeginParameters, "(");
            Add(SpecialWords.EndParameters, ")");
            Add(SpecialWords.EndInstruction, ";");
        }
    }
}
