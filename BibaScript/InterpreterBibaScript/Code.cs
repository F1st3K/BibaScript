using SyntaxBibaScript;
using System;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal static class Code
    {
        public static string[] FindBlock(int i, string[] mass, string begin, string end, out int endParam)
        {
            var expr = new List<string>();
            int count = 0;
            do
            {
                if (mass[i] == begin)
                    count++;
                else if (mass[i] == end)
                    count--;
                expr.Add(mass[i]);
                i++;
            } while (i < mass.Length && count != 0);
            expr.RemoveAt(0);
            expr.RemoveAt(expr.Count - 1);
            endParam = i;
            return expr.ToArray();
        }

        public static string[] FindInstruction(int i, string[] mass, string end, out int endParam)
        {
            var expr = new List<string>();
            while (i < mass.Length)
            {
                expr.Add(mass[i]);
                if (mass[i] == end)
                {
                    i++;
                    break;
                }
                i++;
            }
            endParam = i;
            return expr.ToArray();
        }

        public static Types ConvertWordTypeToTypes(SpecialWords typeWord)
        {
            Types type;
            switch (typeWord)
            {
                case SpecialWords.TypeInteger: type = Types.Integer; break;
                case SpecialWords.TypeString: type = Types.String; break;
                case SpecialWords.TypeFloat: type = Types.Float; break;
                case SpecialWords.TypeBoolean: type = Types.Boolean; break;
                default:
                    throw new Exception("Invalid type: " + typeWord.ToString());
            }
            return type;
        }

        public static string SubStringMass(string[] mass)
        {
            string str = string.Empty;
            foreach (var item in mass)
                str += item + " ";
            return str;
        }

        public static string SubStringMass(string[] mass, int maxIndex)
        {
            var list = new List<string>(mass);
            if (list.Count > maxIndex)
                list.RemoveRange(maxIndex, list.Count - maxIndex);
            return SubStringMass(list.ToArray());
        }

        public static string[] GetParameters(int i, Parameter[] parameters, string[] command, string begin, string end, string separator, out int endIndex)
        {
            var values = new List<string>();
            var blockParam = Code.FindBlock(i, command, begin, end, out endIndex);
            int k = 0;
            for (int countParam = 0; countParam < parameters.Length; countParam++)
            {
                if (k >= blockParam.Length)
                    throw new Exception("Less Parameters to Expect: " + command[0] + begin + parameters.Length + end);
                var commands = new List<string>(Code.FindInstruction(k, blockParam, separator, out k));
                if (commands[commands.Count - 1] == separator)
                    commands.RemoveAt(commands.Count - 1);
                values.Add(Comber.Calculate(parameters[countParam].Type, commands.ToArray()));
            }
            if (k < blockParam.Length)
                throw new Exception("More Parameters to Expect: " + command[0] + begin + parameters.Length + end);
            return values.ToArray();
        }

        public static string TryRemoveStringSeparators(string str)
        {
            var output = string.Empty;
            for (int i = 0; i < str.Length; i++)
                if ((i == 0 || i == str.Length - 1) &&
                    str[i].ToString() == CodeSeparators.GetInstance().GetValue(SpecialWords.SeparatorString))
                    continue;
                else output += str[i];
            return output;
        }
    }
}
