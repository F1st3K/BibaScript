using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterBibaScript
{
    internal sealed class CodeTypeWords : Dictionary<SpecialWords, string>
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
            Add(SpecialWords.ValueVoid, "void");
            Add(SpecialWords.FunctionReturn, "return");
            Add(SpecialWords.SeparatorString, "\"");
            Add(SpecialWords.EndFloat, "f");
            Add(SpecialWords.ValueTrue, "true");
            Add(SpecialWords.ValueFalse, "false");
        }
    }
}
