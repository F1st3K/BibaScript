using System;
using System.Collections.Generic;
using SyntaxBibaScript;

namespace InterpreterBibaScript
{
    internal sealed class Parser
    {
        private string _strSeparator;

        public string[] EmptySeparators { get; private set; }

        public Parser(string[] empty)
        {
            //Get useless strings
            EmptySeparators = empty;
            _strSeparator = CodeSeparators.GetInstance().GetValue(SpecialWords.SeparatorString);
        }

        //This method separate use without useless
        public string[] Parse(string programm)
        {
            var commands = new List<string>();
            var temp = Split(programm, EmptySeparators);
            foreach (var element in temp)
            {
                if (element.StartsWith(_strSeparator) && element.EndsWith(_strSeparator))
                    commands.Add(element);
                else
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

        private string[] Split(string value, string[] separators)
        {
            //convert string ""
            var s = new List<string>(separators);
            var list = new List<string>();
            var countS = 0;
            string tempS = string.Empty;
            string temp = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                string str = value[i].ToString();
                if (str == _strSeparator)
                    countS++;
                if (countS % 2 != 0 && countS > 0)
                {
                    tempS += str;
                    continue;
                }
                else if (countS > 0)
                {
                    list.Add(temp);
                    temp = string.Empty;
                    list.Add(tempS + _strSeparator);
                    countS = 0;
                    tempS = string.Empty;
                    continue;
                }
                if (s.Contains(str))
                {
                    list.Add(temp);
                    temp = string.Empty;
                    continue;
                }
                temp += str;
            }
            if (countS != 0)
                throw new Exception("Invalid string separator: " + tempS);
            return list.ToArray();
        }
    }
}
