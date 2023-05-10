using SyntaxBibaScript;
using System;
using System.Collections.Generic;

namespace InterpreterBibaScript
{
    internal sealed class Memory
    {
        private static Memory _instance;

        public static Memory GetInstance()
        {
            if (_instance == null)
                _instance = new Memory();
            return _instance;
        }

        private string _true;
        private string _false;

        public Dictionary<string, int> Integers { get; }

        public Dictionary<string, float> Floats { get; }

        public Dictionary<string, string> Strings { get; }

        public Dictionary<string, bool> Booleans { get; }

        public Dictionary<string, string[]> Functions { get; }

        public Memory()
        {
            Integers = new Dictionary<string, int>();
            Floats = new Dictionary<string, float>();
            Strings = new Dictionary<string, string>();
            Booleans = new Dictionary<string, bool>();
            Functions = new Dictionary<string, string[]>();
            if (CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueFalse, out _false) == false)
                throw new Exception("Non such value false");
            if (CodeTypeWords.GetInstance().TryGetValue(SpecialWords.ValueTrue, out _true) == false)
                throw new Exception("Non such value true");
        }

        public List<string> GetAllNames()
        {
            var list = new List<string>();
            foreach (var item in Integers)
                list.Add(item.Key);
            foreach (var item in Floats)
                list.Add(item.Key);
            foreach (var item in Strings)
                list.Add(item.Key);
            foreach (var item in Booleans)
                list.Add(item.Key);
            foreach (var item in Functions)
                list.Add(item.Key);
            return list;
        }

        public void DeclareVariable(Types type, string name)
        {
            if (GetAllNames().Contains(name))
                throw new Exception("Variable: " + name + " does exist");
            switch (type)
            {
                case Types.Integer:
                    Integers.Add(name, 0);
                    break;
                case Types.String:
                    Strings.Add(name, string.Empty);
                    break;
                case Types.Float:
                    Floats.Add(name, 0);
                    break;
                case Types.Boolean:
                    Booleans.Add(name, false);
                    break;
                default:
                    throw new Exception("Invalid type");
            }
        }

        public void SetVariable(string name, string value)
        {
            if (Integers.ContainsKey(name))
            {
                if (int.TryParse(value, out var num))
                    Integers[name] = num;
                else throw new Exception("Invalid integer value: " + value);
                return;
            }
            if (Strings.ContainsKey(name))
            {
                CodeTypeWords.GetInstance().TryGetValue(SpecialWords.SeparatorString, out var regix);
                if (value.StartsWith(regix) && value.EndsWith(regix))
                    Strings[name] = value.Substring(1, value.Length - 1);
                else throw new Exception("Invalid string value: " + value);
                return;
            }
            if (Floats.ContainsKey(name))
            {
                if (float.TryParse(value, out var num))
                    Floats[name] = num;
                else throw new Exception("Invalid float value: " + value);
                return;
            }
            if (Booleans.ContainsKey(name))
            {
                if (value == _true)
                    Booleans[name] = true;
                else if (value == _false)
                    Booleans[name] = false;
                else throw new Exception("Invalid Bolean value: " + value);
                return;
            }
            throw new Exception("Variable: "+name+" does not exist");
        }

        public string GetVariable(string name)
        {
            if (Integers.TryGetValue(name, out var valueInt))
                return valueInt.ToString();
            if (Strings.TryGetValue(name, out var valueStr))
                return "\""+valueStr +"\"";
            if (Floats.TryGetValue(name, out var valueFloat))
                return valueFloat.ToString();
            if (Booleans.TryGetValue(name, out var valueBool))
                return valueBool ? _true : _false;
            throw new Exception("No such variable: " + name);
        }

        public Types GetVariableType(string name)
        {
            if (Integers.ContainsKey(name))
                return Types.Integer;
            if (Strings.ContainsKey(name))
                return Types.String;
            if (Floats.ContainsKey(name))
                return Types.Float;
            if (Booleans.ContainsKey(name))
                return Types.Boolean;
            throw new Exception("No such variable: " + name);
        }

        public void RemoveWithout(string[] values)
        {
            var list = new List<string>(values);
            var integer = new string[Integers.Keys.Count];
            Integers.Keys.CopyTo(integer, 0);
            foreach (var item in integer)
                if (list.Contains(item) == false)
                    Integers.Remove(item);
            var floats = new string[Floats.Keys.Count];
            Floats.Keys.CopyTo(floats, 0);
            foreach (var item in floats)
                if (list.Contains(item) == false)
                    Floats.Remove(item);
            var strings = new string[Strings.Keys.Count];
            Strings.Keys.CopyTo(strings, 0);
            foreach (var item in strings)
                if (list.Contains(item) == false)
                    Strings.Remove(item);
            var booleans = new string[Booleans.Keys.Count];
            Booleans.Keys.CopyTo(booleans, 0);
            foreach (var item in booleans)
                if (list.Contains(item) == false)
                    Booleans.Remove(item);
            var functions = new string[Functions.Keys.Count];
            Functions.Keys.CopyTo(functions, 0);
            foreach (var item in functions)
                if (list.Contains(item) == false)
                    Functions.Remove(item);
        }
    }
}
