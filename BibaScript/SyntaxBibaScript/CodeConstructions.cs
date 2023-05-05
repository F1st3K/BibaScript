namespace SyntaxBibaScript
{
    public sealed class CodeConstructions : CodeDictionary
    {
        private static CodeConstructions _instance;

        public static CodeConstructions GetInstance()
        {
            if (_instance == null)
                _instance = new CodeConstructions();
            return _instance;
        }

        public CodeConstructions() : base()
        {
            _dictionary.Add(SpecialWords.DeclarationFunction, "func");
            _dictionary.Add(SpecialWords.ConstructionIf, "if");
            _dictionary.Add(SpecialWords.ConstructionElse, "else");
            _dictionary.Add(SpecialWords.ConstructionWhile, "while");
        }
    }
}
