using SyntaxBibaScript;
using System;
using System.Collections.Generic;
using Calc = Calculator.Calculator;
using LogicCalc = LogicCalculator.LogicCalculator;

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

        public readonly static Dictionary<string, string> Logics = new Dictionary<string, string>()
        {
            { CodeOperators.GetInstance().GetValue(SpecialWords.OperatorAnd), " And "},
            { CodeOperators.GetInstance().GetValue(SpecialWords.OperatorOr), " Or "},
            { CodeOperators.GetInstance().GetValue(SpecialWords.OperatorNot), " Not "},
            { CodeOperators.GetInstance().GetValue(SpecialWords.BeginExpr), " ( " },
            { CodeOperators.GetInstance().GetValue(SpecialWords.EndExpr), " ) " },
        };

        public readonly static string SeparatorStr = CodeTypeWords.GetInstance().GetValue(SpecialWords.SeparatorString);
        public readonly static string ValueTrue = CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
        public readonly static string ValueFalse = CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);

        //Run calculate
        public static string Calculate(Types type, string[] expr)
        {
            expr = Prepocess(expr);
            if (expr.Length <= 0)
                throw new Exception("Value is not find");
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
                    throw new Exception("Invalid type: " + type.ToString());
            }
        }

        private static string[] Prepocess(string[] expr)
        {
            var list = new List<string>();
            var beginParam = CodeSeparators.GetInstance().GetValue(SpecialWords.BeginParameters);
            var endParam = CodeSeparators.GetInstance().GetValue(SpecialWords.EndParameters);
            var paramSeparator = CodeSeparators.GetInstance().GetValue(SpecialWords.SeparatorParameters);
            for (int i = 0; i < expr.Length; i++)
                try
                {
                    if (Memory.GetInstance().GetAllNames().Contains(expr[i]))
                        if (Memory.GetInstance().Functions.TryGetValue(expr[i], out var func))
                        {
                            var j = i;
                            var values = Code.GetParameters(i + 1, func.Parameters, expr, beginParam, endParam, paramSeparator, out i);
                            Memory.GetInstance().RunFunction(expr[j], out var result, values);
                            list.Add(result);
                            i--;
                        }
                        else list.Add(Memory.GetInstance().GetVariable(expr[i]).ToString());
                    else if (SystemLib.GetInctance().Functions.TryGetValue(expr[i], out var func))
                    {
                        var values = Code.GetParameters(i + 1, func.Parameters, expr, beginParam, endParam, paramSeparator, out i);
                        list.Add(func.Run(values));
                        i--;
                    }
                    else list.Add(expr[i]);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Preprocessor: " + expr[i] + ": " + ex.Message);
                    }
            return list.ToArray();
        }

        //Calculate boolean type
        private static string CalculateBool(string[] expr)
        {
            string expression = string.Empty;
            var calculator = new LogicCalc();

            foreach (var element in expr)
            {
                if (Logics.TryGetValue(element, out var el))
                    expression += el;
                else if (element == Operations[4] || element == Operations[5])
                    expression += element;
                else if (int.TryParse(element, out var intNum))
                {
                    var i = intNum.ToString();
                    if (i == "0" || i == "1")
                        expression += i == "0" ? "false" : "true";
                    else throw new Exception("No such conversion: " + element);
                }
                else if (float.TryParse(element, out var fNum))
                {
                    var f = fNum.ToString();
                    if (f == "0" || f == "1")
                        expression += f == "0" ? "false" : "true";
                    else throw new Exception("No such conversion: " + element);
                }
                else if (element == SeparatorStr + ValueTrue + SeparatorStr)
                    expression += "true";
                else if (element == SeparatorStr + ValueFalse + SeparatorStr)
                    expression += "false";
                else if (element == ValueTrue)
                    expression += "true";
                else if (element == ValueFalse)
                    expression += "false";
                else throw new Exception("Invalid value: " + element);
            }
            calculator.Calculate(expression);
            return calculator.Result ? ValueTrue : ValueFalse;
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
                else if (int.TryParse(element, out var intNum))
                    expression += intNum.ToString();
                else if (float.TryParse(element.Replace('.', ','), out var fNum))
                    expression += fNum.ToString();
                else if (element.StartsWith(SeparatorStr) && element.EndsWith(SeparatorStr))
                    expression += Convert.ToSingle(element.Substring(1, element.Length - 2));
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
                else if (int.TryParse(element, out var intNum))
                    expression += intNum.ToString();
                else if (float.TryParse(element, out var fNum))
                    expression += fNum.ToString();
                else if (element.StartsWith(SeparatorStr) && element.EndsWith(SeparatorStr))
                    expression += Convert.ToInt32(element.Substring(1, element.Length - 2));
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
