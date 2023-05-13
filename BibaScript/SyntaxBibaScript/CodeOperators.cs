namespace SyntaxBibaScript
{
    public sealed class CodeOperators : CodeDictionary
    {
        private static CodeOperators _instance;

        public static CodeOperators GetInstance()
        {
            if (_instance == null)
                _instance = new CodeOperators();
            return _instance;
        }

        public CodeOperators() : base()
        {
            _dictionary.Add(SpecialWords.OperationAdd, "+");
            _dictionary.Add(SpecialWords.OperationSub, "-");
            _dictionary.Add(SpecialWords.OperationMul, "*");
            _dictionary.Add(SpecialWords.OperationDiv, "/");
            _dictionary.Add(SpecialWords.BeginExpr, "(");
            _dictionary.Add(SpecialWords.EndExpr, ")");
            _dictionary.Add(SpecialWords.OperationPow, "^");
            _dictionary.Add(SpecialWords.OperationUnm, "~");

            _dictionary.Add(SpecialWords.OperatorAnd, "&&");
            _dictionary.Add(SpecialWords.OperatorNot, "!");
            _dictionary.Add(SpecialWords.OperatorOr, "||");
        }
    }
}
