﻿using System.Collections.Generic;
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
                bool isSeparator = false;
                foreach (var sp in CodeSeparators.GetInstance().Values)
                    if (element.Contains(sp) && element != sp)
                    {
                        isSeparator = true;
                        commands.AddRange(Select(element, sp));
                        break;
                    }
                if (isSeparator == false)
                {
                    commands.Add(element);
                }
            }
            return commands.ToArray();
        }

        //Select main separators
        private string[] Select(string value, string selection)
        {
            return value.Replace(selection, " " + selection + " ").Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
