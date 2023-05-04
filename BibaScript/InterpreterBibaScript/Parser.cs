using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterBibaScript
{
    internal sealed class Parser
    {
        private Dictionary<Separator, string> CodeSeparators;

        public string[] EmptySeparators { get; private set; }

        public Parser()
        {
            EmptySeparators = new string[] { " ", "\n", "\t", "\r" };
            CodeSeparators = new Dictionary<Separator, string>();
            CodeSeparators.Add(Separator.BeginCode, "{");
            CodeSeparators.Add(Separator.EndCode, "}");
            CodeSeparators.Add(Separator.EndLine, ";");
        }

        public string[] Parse(string programm)
        {
            var commands = new List<string>();
            var temp = programm.Split(EmptySeparators, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var element in temp)
            {
                bool isSeparator = false;
                foreach (var sp in CodeSeparators.Values)
                    if (element.EndsWith(sp) && element != sp)
                    {
                        isSeparator = true;
                        element.Replace(sp, string.Empty);
                        commands.Add(element);
                        commands.Add(sp);
                        break;
                    }
                if (isSeparator == false)
                {
                    commands.Add(element);
                }
            }
            return commands.ToArray();
        }

        internal enum Separator
        {
            BeginCode,
            EndCode,
            EndLine,
        }
    }
}
