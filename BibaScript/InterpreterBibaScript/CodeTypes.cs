using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class CodeTypes : Dictionary<SpecialWords, string>
    {
        private static CodeTypes _instance;

        public static CodeTypes GetInstance()
        {
            if (_instance == null)
                _instance = new CodeTypes();
            return _instance;
        }

        public CodeTypes()
        {
            Add(SpecialWords.TypeInteger, "int");
            Add(SpecialWords.TypeString, "string");
            Add(SpecialWords.TypeFloat, "float");
            Add(SpecialWords.TypeBoolean, "bool");
        }
    }
}
