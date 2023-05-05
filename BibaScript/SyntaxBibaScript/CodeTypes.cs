namespace SyntaxBibaScript
{
    public sealed class CodeTypes : CodeDictionary
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
            _dictionary.Add(SpecialWords.TypeInteger, "int");
            _dictionary.Add(SpecialWords.TypeString, "string");
            _dictionary.Add(SpecialWords.TypeFloat, "float");
            _dictionary.Add(SpecialWords.TypeBoolean, "bool");
        }
    }
}
