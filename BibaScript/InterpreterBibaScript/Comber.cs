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
            "+", "-", "/", "*", "^", ")", "(", "~"
        };

        public readonly static Dictionary<string, string> Logics = new Dictionary<string, string>()
        {
            { "&&", Operations[0]},
            { "||", Operations[3]},
            { "!", Operations[7]},
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
            if (countS != 0)
                throw new Exception("Invalid string separator: " + temp);
            //Run calculate
            switch (type)
            {
                case Types.Integer:
                    return CalculateInt(list.ToArray());
                case Types.String:
                    return s+CalculateStr(list.ToArray())+s;
                case Types.Float:
                    return CalculateFloat(list.ToArray());
                case Types.Boolean:
                    return CalculateBool(list.ToArray());
                default:
                    break;
            }
            throw new Exception("Invalid type: " + type.ToString());
        }

        //Calculate boolean type
        private static string CalculateBool(string[] expr)
        {
            string expression = string.Empty;
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueTrue, out var vt);
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueFalse, out var vf);
            var calculator = new Calc();

            foreach (var element in expr)
            {
                if (Logics.TryGetValue(element, out var el))
                    expression += el;
                else if (Memory.GetInstance().GetAllNames().Contains(element))
                    try
                    {
                        switch (Memory.GetInstance().GetVariableType(element))
                        {
                            case Types.Integer:
                                var i = Memory.GetInstance().GetVariable(element).ToString();
                                if (i == "0" || i == "1")
                                    expression += i == "0" ? "~1" : "1";
                                else throw new Exception("No such conversion: " + element);
                                break;
                            case Types.String:
                                var s = Memory.GetInstance().GetVariable(element);
                                s = Convert.ToSingle(s.Substring(1, s.Length - 2)).ToString();
                                if (s == vf || s == vt)
                                    expression += s == "false" ?"~1" : "1";
                                else throw new Exception("No such conversion: " + element);
                                break;
                            case Types.Float:
                                var f = ((int)Convert.ToSingle(Memory.GetInstance().GetVariable(element))).ToString();
                                if (f == "0" || f == "1")
                                    expression += f == "0" ? "~1" : "1";
                                else throw new Exception("No such conversion: " + element);
                                break;
                            case Types.Boolean:
                                var b = Memory.GetInstance().GetVariable(element);
                                if (b == vf || b == vt)
                                    expression += b == "false" ? "~1" : "1";
                                else throw new Exception("No such conversion: " + element);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Invalid type conversion: " + element + ": " + ex.Message);
                    }
                else if (int.TryParse(element, out var intNum))
                {
                    var i = intNum.ToString();
                    if (i == "0" || i == "1")
                        expression += i == "0" ? "~1" : "1";
                    else throw new Exception("No such conversion: " + element);
                }
                else if (float.TryParse(element, out var fNum))
                {
                    var f = fNum.ToString();
                    if (f == "0" || f == "1")
                        expression += f == "0" ? "~1" : "1";
                    else throw new Exception("No such conversion: " + element);
                }
                else if (element == vt)
                    expression += "1";
                else if (element == vf)
                    expression += "~1";
                else throw new Exception("Invalid value: " + element);
            }
            calculator.Calculate(expression);
            return calculator.Result <= 0 ? vf : vt;
        }

        //Calculate float type
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
                    try
                    {
                        switch (Memory.GetInstance().GetVariableType(element))
                        {
                            case Types.Integer:
                                expression += Memory.GetInstance().GetVariable(element).ToString();
                                break;
                            case Types.String:
                                var s = Memory.GetInstance().GetVariable(element);
                                expression += Convert.ToSingle(s.Substring(1, s.Length - 2)).ToString();
                                break;
                            case Types.Float:
                                expression += Memory.GetInstance().GetVariable(element).ToString();
                                break;
                            case Types.Boolean:
                                CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueTrue, out var v);
                                expression += Memory.GetInstance().GetVariable(element) == v ? 1 : 0;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Invalid type conversion: " + element + ": " + ex.Message);
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

        //Calculate string type
        private static string CalculateStr(string[] expr)
        {
            var expression = new List<string>();
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueTrue, out var vt);
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueFalse, out var vf);
            CodeTypeWords.GetInstance().TryGetValue(SpecialWords.SeparatorString, out var s);

            foreach (var element in expr)
            {
                if (element == Operations[0])
                    expression.Add(element);
                else if (Memory.GetInstance().GetAllNames().Contains(element))
                    try
                    {
                        switch (Memory.GetInstance().GetVariableType(element))
                        {
                            case Types.Integer:
                                expression.Add(Memory.GetInstance().GetVariable(element));
                                break;
                            case Types.String:
                                var v = Memory.GetInstance().GetVariable(element);
                                expression.Add(v.Substring(1, v.Length - 2));
                                break;
                            case Types.Float:
                                expression.Add(Memory.GetInstance().GetVariable(element));
                                break;
                            case Types.Boolean:
                                expression.Add(Memory.GetInstance().GetVariable(element));
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Invalid type conversion: " + element + ": " + ex.Message);
                    }
                else if (int.TryParse(element, out var intNum))
                    expression.Add(intNum.ToString());
                else if (float.TryParse(element, out var fNum))
                    expression.Add(fNum.ToString());
                else if (element == vt)
                    expression.Add(vt);
                else if (element == vf)
                    expression.Add(vf);
                else if (element.StartsWith(s) && element.EndsWith(s))
                    expression.Add(element.Substring(1, element.Length - 2));
                else throw new Exception("Invalid value: " + element);
            }
            var result = string.Empty;
            var isOperand = false;
            foreach (var op in expression)
            {
                if (isOperand == false)
                    result += op;
                isOperand = !isOperand;
            }
            if (isOperand == false)
                throw new Exception("Invalid operand: +:" + result);
            return result;
        }

        //Calculate integer type
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
                    try
                    {
                        switch (Memory.GetInstance().GetVariableType(element))
                        {
                            case Types.Integer:
                                expression += Memory.GetInstance().GetVariable(element);
                                break;
                            case Types.String:
                                var s = Memory.GetInstance().GetVariable(element);
                                expression += Convert.ToInt32(s.Substring(1, s.Length - 2)).ToString();
                                break;
                            case Types.Float:
                                expression += Memory.GetInstance().GetVariable(element).ToString();
                                break;
                            case Types.Boolean:
                                expression += Memory.GetInstance().GetVariable(element) == vt ? 1 : 0;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Invalid type conversion: " + element + ": " + ex.Message);
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
            return ((int)calculator.Result).ToString();
        }
    }
}
