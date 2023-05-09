namespace SyntaxBibaScript
{
    public sealed class CodeTypeWords : CodeDictionary
    {
        private static CodeTypeWords _instance;

        public static CodeTypeWords GetInstance()
        {
            if (_instance == null)
                _instance = new CodeTypeWords();
            return _instance;
        }

        public CodeTypeWords()
        {
            _dictionary.Add(SpecialWords.ValueVoid, "void");
            _dictionary.Add(SpecialWords.FunctionReturn, "return");
            _dictionary.Add(SpecialWords.SeparatorString, "\"");
            _dictionary.Add(SpecialWords.ValueTrue, "true");
            _dictionary.Add(SpecialWords.ValueFalse, "false");
        }
    }
}
