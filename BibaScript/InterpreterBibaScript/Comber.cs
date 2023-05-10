using SyntaxBibaScript;
using System;
using System.Collections.Generic;
using Calc = Calculator.Calculator;

namespace InterpreterBibaScript
{
    internal static class Comber
    {
        public readonly static List<string> Operations = new List<string>()
        {
            "+", "-", "/", "*", "^", ")", "("
        };

        public static string Calculate(Types type, string[] expr)
        {
            //convert string ""
            var list = new List<string>();
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.SeparatorString, out var s);
            var countS = 0;
            string temp = string.Empty;
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == s)
                    countS++;
                if (countS % 2 != 0 && countS > 0)
                {
                    temp += expr[i];
                    continue;
                }
                else if (countS > 0)
                {
                    list.Add(temp.Replace(s, string.Empty));
                    countS = 0;
                    temp = string.Empty;
                    continue;
                }
                list.Add(expr[i]);
            }
            //Run calculate
            switch (type)
            {
                case Types.Integer:
                    return CalculateInt(list.ToArray());
                case Types.String:
                    return CalculateStr(list.ToArray());
                case Types.Float:
                    return CalculateFloat(list.ToArray());
                case Types.Boolean:
                    return CalculateBool(list.ToArray());
                default:
                    break;
            }
            throw new Exception("Invalid type");
        }

        private static string CalculateBool(string[] expr)
        {
            throw new NotImplementedException();
        }

        private static string CalculateFloat(string[] expr)
        {
            string expression = string.Empty;
            var calculator = new Calc();
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueTrue, out var vt);
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueFalse, out var vf);

            foreach (var element in expr)
            {
                if (Operations.Contains(element))
                    expression += element;
                else if (Memory.GetInstance().GetAllNames().Contains(element))
                    switch (Memory.GetInstance().GetVariableType(element))
                    {
                        case Types.Integer:
                            expression += Convert.ToSingle(Memory.GetInstance().GetVariable(element)).ToString();
                            break;
                        case Types.String:
                            var s = Memory.GetInstance().GetVariable(element);
                            expression += Convert.ToSingle(s.Substring(1, s.Length - 1)).ToString();
                            break;
                        case Types.Float:
                            expression += Memory.GetInstance().GetVariable(element);
                            break;
                        case Types.Boolean:
                            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueTrue, out var v);
                            expression += Memory.GetInstance().GetVariable(element) == v ? 1 : 0;
                            break;
                        default:
                            break;
                    }
                else if (int.TryParse(element, out var intNum))
                    expression += intNum.ToString();
                else if (float.TryParse(element, out var fNum))
                    expression += fNum.ToString();
                else if (element == vt)
                    expression += 1;
                else if (element == vf)
                    expression += 0;
                else throw new Exception("Invalid value: " + element);
            }
            calculator.Calculate(expression);
            return Convert.ToSingle(calculator.Result).ToString();
        }

        private static string CalculateStr(string[] expr)
        {
            throw new NotImplementedException();
        }

        private static string CalculateInt(string[] expr)
        {
            string expression = string.Empty;
            var calculator = new Calc();
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueTrue, out var vt);
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueFalse, out var vf);

            foreach (var element in expr)
            {
                if (Operations.Contains(element))
                    expression += element;
                else if (Memory.GetInstance().GetAllNames().Contains(element))
                    switch (Memory.GetInstance().GetVariableType(element))
                    {
                        case Types.Integer:
                            expression += Memory.GetInstance().GetVariable(element);
                            break;
                        case Types.String:
                            var s = Memory.GetInstance().GetVariable(element);
                            expression += Convert.ToInt32(s.Substring(1, s.Length - 1)).ToString();
                            break;
                        case Types.Float:
                            expression += Convert.ToInt32(Memory.GetInstance().GetVariable(element)).ToString();
                            break;
                        case Types.Boolean:
                            expression += Memory.GetInstance().GetVariable(element) == vt ? 1 : 0;
                            break;
                        default:
                            break;
                    }
                else if (int.TryParse(element, out var intNum))
                    expression += intNum.ToString();
                else if (float.TryParse(element, out var fNum))
                    expression += fNum.ToString();
                else if (element == vt)
                    expression += 1;
                else if (element == vf)
                    expression += 0;
                else throw new Exception("Invalid value: " + element);
            }
            calculator.Calculate(expression);
            return Convert.ToInt32(calculator.Result).ToString();
        }
    }
}
