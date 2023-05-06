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
        }

        public string[] GetAllNames()
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
            return list.ToArray();
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
