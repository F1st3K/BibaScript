using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class Parser
    {
        public string[] EmptySeparators { get; private set; }

        public Parser()
        {
            EmptySeparators = new string[] { " ", "\n", "\t", "\r" };
        }

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

        private string[] Select(string value, string selection)
        {
            return value.Replace(selection, " " + selection + " ").Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
