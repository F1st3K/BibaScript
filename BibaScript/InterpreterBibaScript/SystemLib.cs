using System.Collections.Generic;
using System;
using SyntaxBibaScript;
using Calc = Calculator.Calculator;

namespace InterpreterBibaScript
{
    internal sealed class SystemLib
    {
        private static SystemLib _instance;

        public static SystemLib GetInctance()
        {
            if (_instance == null)
                _instance = new SystemLib();
            return _instance;
        }

        public readonly Dictionary<string, SystemFunction> Functions;

        public readonly Dictionary<string, SystemProcedure> Procedures;

        public readonly string _s = CodeSeparators.GetInstance().GetValue(SpecialWords.SeparatorString);

        public SystemLib()
        {
            Functions = new Dictionary<string, SystemFunction>();
            Procedures = new Dictionary<string, SystemProcedure>();
            Functions.Add("Equal", new SystemFunction(Equal,
                Types.Boolean, 
                new Parameter(Types.String, "a"),
                new Parameter(Types.String, "b")));
            Functions.Add("Unequal", new SystemFunction(Unequal,
                Types.Boolean,
                new Parameter(Types.String, "a"),
                new Parameter(Types.String, "b")));
            Functions.Add("More", new SystemFunction(More,
                Types.Boolean,
                new Parameter(Types.String, "a"),
                new Parameter(Types.String, "b")));
            Functions.Add("Less", new SystemFunction(Less,
                Types.Boolean,
                new Parameter(Types.String, "a"),
                new Parameter(Types.String, "b")));
            Functions.Add("MoreEqual", new SystemFunction(MoreEqual,
                Types.Boolean,
                new Parameter(Types.String, "a"),
                new Parameter(Types.String, "b")));
            Functions.Add("LessEqual", new SystemFunction(LessEqual,
                Types.Boolean,
                new Parameter(Types.String, "a"),
                new Parameter(Types.String, "b")));
            Functions.Add("UnEqual", new SystemFunction(ReadLine,
                Types.Boolean,
                new Parameter(Types.String, "a"),
                new Parameter(Types.String, "b")));
            Functions.Add("ReadLine", new SystemFunction(ReadLine,
                Types.String));
            Procedures.Add("Write", new SystemProcedure(Write,
                new Parameter(Types.String, "value")));
            Procedures.Add("WriteLine", new SystemProcedure(WriteLine,
                new Parameter(Types.String, "value")));
            Functions.Add("Math.Calculate", new SystemFunction(Calculate,
                Types.Float,
                new Parameter(Types.String, "expression")));
            Functions.Add("Math.Min", new SystemFunction(Min,
                Types.Float,
                new Parameter(Types.Float, "a"),
                new Parameter(Types.Float, "b")));
            Functions.Add("Math.Max", new SystemFunction(Max,
                Types.Float,
                new Parameter(Types.Float, "a"),
                new Parameter(Types.Float, "b")));
            Functions.Add("Math.Sqrt", new SystemFunction(Sqrt,
                Types.Float,
                new Parameter(Types.Float, "value")));
            Functions.Add("Math.Abs", new SystemFunction(Abs,
                Types.Float,
                new Parameter(Types.Float, "value")));
            Functions.Add("String.Replace", new SystemFunction(Replace,
                Types.String,
                new Parameter(Types.String, "word"),
                new Parameter(Types.String, "oldValue"),
                new Parameter(Types.String, "newValue")));
            Functions.Add("String.Substring", new SystemFunction(Substring,
                Types.String,
                new Parameter(Types.String, "word"),
                new Parameter(Types.Integer, "indexStart"),
                new Parameter(Types.Integer, "lenght")));
            Functions.Add("String.Contains", new SystemFunction(Contains,
                Types.Boolean,
                new Parameter(Types.String, "word"),
                new Parameter(Types.String, "value")));
        }

        private string Contains(string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Code.TryRemoveStringSeparators(arg[1]);
            return a.Contains(b)
                ? CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue)
                : CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);
        }

        private string Substring(string[] arg)
        {
            if (arg.Length != 3)
                throw new Exception("Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Convert.ToInt32(Code.TryRemoveStringSeparators(arg[1]));
            var c = Convert.ToInt32(Code.TryRemoveStringSeparators(arg[2]));
            return _s + a.Substring(b, c) + _s;
        }

        private string Replace(string[] arg)
        {
            if (arg.Length != 3)
                throw new Exception("Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Code.TryRemoveStringSeparators(arg[1]);
            var c = Code.TryRemoveStringSeparators(arg[2]);
            return _s + a.Replace(b, c) + _s;
        }

        private string Abs(string[] arg)
        {
            if (arg.Length != 1)
                throw new Exception("Invalid count parameters");
            var a = Convert.ToSingle(Code.TryRemoveStringSeparators(arg[0]));
            return Math.Abs(a).ToString();
        }

        private string Sqrt(string[] arg)
        {
            if (arg.Length != 1)
                throw new Exception("Invalid count parameters");
            var a = Convert.ToSingle(Code.TryRemoveStringSeparators(arg[0]));
            return Math.Sqrt(a).ToString();
        }

        private string Max(string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Invalid count parameters");
            var a = Convert.ToSingle(Code.TryRemoveStringSeparators(arg[0]));
            var b = Convert.ToSingle(Code.TryRemoveStringSeparators(arg[1]));
            return Math.Max(a, b).ToString();
        }

        private string Min(string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Invalid count parameters");
            var a = Convert.ToSingle(Code.TryRemoveStringSeparators(arg[0]));
            var b = Convert.ToSingle(Code.TryRemoveStringSeparators(arg[1]));
            return Math.Min(a, b).ToString();
        }

        private string Calculate(string[] arg)
        {
            if (arg.Length != 1)
                throw new Exception("Invalid count parameters");
            var calculator = new Calc();
            calculator.Calculate(Code.TryRemoveStringSeparators(arg[0]));
            return calculator.Result.ToString();
        }

        private string WriteLine(string[] arg)
        {
            if (arg.Length != 1)
                throw new Exception("Invalid count parameters");
            Console.WriteLine(Code.TryRemoveStringSeparators(arg[0]));
            return string.Empty;
        }

        private string Write(string[] arg)
        {
            if (arg.Length != 1)
                throw new Exception("Invalid count parameters");
            Console.Write(Code.TryRemoveStringSeparators(arg[0]));
            return string.Empty;
        }

        private string ReadLine(string[] arg)
        {
            if (arg.Length != 0)
                throw new Exception("Invalid count parameters");
            return _s + Console.ReadLine() + _s;
        }

        private string LessEqual(string[] arg)
        {
            return More(arg) == CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue)
                ? CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse)
                : CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
        }

        private string MoreEqual(string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Code.TryRemoveStringSeparators(arg[1]);
            if (Convert.ToSingle(a) >= Convert.ToSingle(b))
                return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
            return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);
        }

        private string Less(string[] arg)
        {
            return MoreEqual(arg) == CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue)
                ? CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse)
                : CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
        }

        private string More(string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Code.TryRemoveStringSeparators(arg[1]);
            if (Convert.ToSingle(a) > Convert.ToSingle(b))
                return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
            return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);
        }

        private string Equal(params string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Code.TryRemoveStringSeparators(arg[1]);
            if (a == b)
                return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
            return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);
        }

        private string Unequal(params string[] arg)
        {
            return Equal(arg) == CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue)
                ? CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse)
                : CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
        }

    }
}
