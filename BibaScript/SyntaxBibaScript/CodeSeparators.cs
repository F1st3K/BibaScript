namespace SyntaxBibaScript
{
    public sealed class CodeSeparators : CodeDictionary
    {
        private static CodeSeparators _instance;

        public static CodeSeparators GetInstance()
        {
            if (_instance == null)
                _instance = new CodeSeparators();
            return _instance;
        }

        public CodeSeparators() : base()
        {
            _dictionary.Add(SpecialWords.BeginCode, "{");
            _dictionary.Add(SpecialWords.EndCode, "}");
            _dictionary.Add(SpecialWords.BeginParameters, "(");
            _dictionary.Add(SpecialWords.EndParameters, ")");
            _dictionary.Add(SpecialWords.EndInstruction, ";");
            _dictionary.Add(SpecialWords.ContinueCode, ":");
        }
    }
}
