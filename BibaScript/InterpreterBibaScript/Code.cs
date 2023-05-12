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
    }
}
