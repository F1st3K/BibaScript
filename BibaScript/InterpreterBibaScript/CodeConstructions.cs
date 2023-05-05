using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class CodeConstructions : Dictionary<SpecialWords, string>
    {
        private static CodeConstructions _instance;

        public static CodeConstructions GetInstance()
        {
            if (_instance == null)
                _instance = new CodeConstructions();
            return _instance;
        }

        public CodeConstructions()
        {
            Add(SpecialWords.DeclarationFunction, "func");
            Add(SpecialWords.ConstructionIf, "if");
            Add(SpecialWords.ConstructionElse, "else");
            Add(SpecialWords.ConstructionWhile, "while");
        }
    }
}
