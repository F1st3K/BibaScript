﻿using SyntaxBibaScript;
using System;
using System.Collections.Generic;
using Calc = Calculator.Calculator;

namespace InterpreterBibaScript
{
    internal static class Comber
    {
        public readonly static List<string> Operations = new List<string>()
        {
            CodeOperators.GetInstance().GetValue(SpecialWords.OperationAdd),
            CodeOperators.GetInstance().GetValue(SpecialWords.OperationSub),
            CodeOperators.GetInstance().GetValue(SpecialWords.OperationMul),
            CodeOperators.GetInstance().GetValue(SpecialWords.OperationDiv),
            CodeOperators.GetInstance().GetValue(SpecialWords.BeginExpr),
            CodeOperators.GetInstance().GetValue(SpecialWords.EndExpr),
            CodeOperators.GetInstance().GetValue(SpecialWords.OperationPow),
            CodeOperators.GetInstance().GetValue(SpecialWords.OperationUnm),
        };

        public readonly static List<string> BoolExpressions = new List<string>()
        {
            CodeOperators.GetInstance().GetValue(SpecialWords.ExprEqual),
            CodeOperators.GetInstance().GetValue(SpecialWords.ExprUnequal),
            CodeOperators.GetInstance().GetValue(SpecialWords.ExprMore),
            CodeOperators.GetInstance().GetValue(SpecialWords.ExprLess),
            CodeOperators.GetInstance().GetValue(SpecialWords.ExprMoreEqual),
            CodeOperators.GetInstance().GetValue(SpecialWords.ExprLessEqual),
        };

        public readonly static Dictionary<string, string> Logics = new Dictionary<string, string>()
        {
            { "&&", Operations[0]},
            { "||", Operations[2]},
            { "!", Operations[7]},
        };

        public readonly static string SeparatorStr = CodeTypeWords.GetInstance().GetValue(SpecialWords.SeparatorString);
        public readonly static string ValueTrue = CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
        public readonly static string ValueFalse = CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);

        //Run calculate
        public static string Calculate(Types type, string[] expr)
        {
            switch (type)
            {
                case Types.Integer:
                    return CalculateInt(expr);
                case Types.String:
                    return SeparatorStr+CalculateStr(expr)+SeparatorStr;
                case Types.Float:
                    return CalculateFloat(expr);
                case Types.Boolean:
                    return CalculateBool(expr);
                default:
                    break;
            }
            throw new Exception("Invalid type: " + type.ToString());
        }

        //Calculate equals expression
        private static string CalculateEqualsExpression(Types type, string expr, string[] a, string[] b)
        {
            bool result;
            try
            {
                var d = BoolExpressions.IndexOf(expr);
                switch (d)
                {
                    case 0: result = Calculate(type, a) == Calculate(type, b); break;
                    case 1: result = Calculate(type, a) != Calculate(type, b); break;
                    case 2: result = Convert.ToSingle(Calculate(type, a)) > Convert.ToSingle(Calculate(type, b)); break;
                    case 3: result = Convert.ToSingle(Calculate(type, a)) < Convert.ToSingle(Calculate(type, b)); break;
                    case 4: result = Convert.ToSingle(Calculate(type, a)) >= Convert.ToSingle(Calculate(type, b)); break;
                    case 5: result = Convert.ToSingle(Calculate(type, a)) <= Convert.ToSingle(Calculate(type, b)); break;
                    default: throw new Exception("Invalid expression: " + expr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid expression: " + a + expr + b + ": " + ex.Message);
            }
            return result ? ValueTrue : ValueFalse;
        }

        //Calculate boolean type
        private static string CalculateBool(string[] expr)
        {
            string expression = string.Empty;
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
                                if (s == ValueFalse || s == ValueTrue)
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
                                if (b == ValueFalse || b == ValueTrue)
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
                else if (element == SeparatorStr+ValueTrue+SeparatorStr)
                    expression += "1";
                else if (element == SeparatorStr+ValueFalse+SeparatorStr)
                    expression += "~1";
                else if (element == ValueTrue)
                    expression += "1";
                else if (element == ValueFalse)
                    expression += "~1";
                else throw new Exception("Invalid value: " + element);
            }
            calculator.Calculate(expression);
            return calculator.Result <= 0 ? ValueFalse : ValueTrue;
        }

        //Calculate float type
        private static string CalculateFloat(string[] expr)
        {
            string expression = string.Empty;
            var calculator = new Calc();

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
                else if (element == ValueTrue)
                    expression += 1;
                else if (element == ValueFalse)
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
                else if (element == ValueTrue)
                    expression.Add(ValueTrue);
                else if (element == ValueFalse)
                    expression.Add(ValueFalse);
                else if (element.StartsWith(SeparatorStr) && element.EndsWith(SeparatorStr))
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
                                expression += Memory.GetInstance().GetVariable(element) == ValueTrue ? 1 : 0;
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
                else if (element == ValueTrue)
                    expression += 1;
                else if (element == ValueFalse)
                    expression += 0;
                else throw new Exception("Invalid value: " + element);
            }
            calculator.Calculate(expression);
            return ((int)calculator.Result).ToString();
        }
    }
}
