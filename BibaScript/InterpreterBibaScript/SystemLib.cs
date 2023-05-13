using System.Collections.Generic;
using System;

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
                SyntaxBibaScript.Types.Boolean, 
                new Parameter(SyntaxBibaScript.Types.String, "a"),
                new Parameter(SyntaxBibaScript.Types.String, "b")));
            Functions.Add("More", new SystemFunction(More,
                SyntaxBibaScript.Types.Boolean,
                new Parameter(SyntaxBibaScript.Types.String, "a"),
                new Parameter(SyntaxBibaScript.Types.String, "b")));
            Functions.Add("Less", new SystemFunction(Less,
                SyntaxBibaScript.Types.Boolean,
                new Parameter(SyntaxBibaScript.Types.String, "a"),
                new Parameter(SyntaxBibaScript.Types.String, "b")));
            Functions.Add("MoreEqual", new SystemFunction(MoreEqual,
                SyntaxBibaScript.Types.Boolean,
                new Parameter(SyntaxBibaScript.Types.String, "a"),
                new Parameter(SyntaxBibaScript.Types.String, "b")));
            Functions.Add("LessEqual", new SystemFunction(LessEqual,
                SyntaxBibaScript.Types.Boolean,
                new Parameter(SyntaxBibaScript.Types.String, "a"),
                new Parameter(SyntaxBibaScript.Types.String, "b")));
            Functions.Add("UnEqual", new SystemFunction(ReadLine,
                SyntaxBibaScript.Types.Boolean,
                new Parameter(SyntaxBibaScript.Types.String, "a"),
                new Parameter(SyntaxBibaScript.Types.String, "b")));
            Functions.Add("ReadLine", new SystemFunction(ReadLine,
                SyntaxBibaScript.Types.String));
            Procedures.Add("Write", new SystemProcedure(Write,
                new Parameter(SyntaxBibaScript.Types.String, "value")));
        }

        private string Write(string[] arg)
        {
            throw new NotImplementedException();
        }

        private string ReadLine(string[] arg)
        {
            throw new NotImplementedException();
        }

        private string LessEqual(string[] arg)
        {
            throw new NotImplementedException();
        }

        private string MoreEqual(string[] arg)
        {
            throw new NotImplementedException();
        }

        private string Less(string[] arg)
        {
            throw new NotImplementedException();
        }

        private string More(string[] arg)
        {
            throw new NotImplementedException();
        }

        private string Equal(params string[] arg)
        {
            if (arg.Length != 2)
                throw new Exception("Equals(2): Invalid count parameters");
            return "";
        }

    }
}
