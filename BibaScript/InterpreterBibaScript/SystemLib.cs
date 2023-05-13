using System.Collections.Generic;
using System;
using SyntaxBibaScript;

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

        public SystemLib()
        {
            Functions = new Dictionary<string, SystemFunction>();
            Procedures = new Dictionary<string, SystemProcedure>();
            Functions.Add("Equal", new SystemFunction(Equal,
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
            Procedures.Add("WriteLine", new SystemProcedure(Write,
                new Parameter(Types.String, "value")));
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
            var s = CodeSeparators.GetInstance().GetValue(SpecialWords.SeparatorString);
            return s + Console.ReadLine() + s;
        }

        private string LessEqual(string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Code.TryRemoveStringSeparators(arg[1]);
            if (Convert.ToSingle(a) <= Convert.ToSingle(b))
                return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
            return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);
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
            if (arg.Length != 2)
                throw new Exception("Equals(2): Invalid count parameters");
            var a = Code.TryRemoveStringSeparators(arg[0]);
            var b = Code.TryRemoveStringSeparators(arg[1]);
            if (Convert.ToSingle(a) < Convert.ToSingle(b))
                return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueTrue);
            return CodeTypeWords.GetInstance().GetValue(SpecialWords.ValueFalse);
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

    }
}
