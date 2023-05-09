using System.Collections.Generic;
using SyntaxBibaScript;

namespace InterpreterBibaScript
{
    internal sealed class Parser
    {
        public string[] EmptySeparators { get; private set; }

        public Parser(string[] empty)
        {
            //Get useless strings
            EmptySeparators = empty;
        }

        //This method separate use without useless
        public string[] Parse(string programm)
        {
            var commands = new List<string>();
            var temp = programm.Split(EmptySeparators, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var element in temp)
            {
                commands.AddRange(Select(element, CodeSeparators.GetInstance().Values));
            }
            return commands.ToArray();
        }

        //Select main separators
        private string[] Select(string value, string[] selection)
        {
            foreach (var item in selection)
                value =  value.Replace(item, " " + item + " ");
            return value.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
