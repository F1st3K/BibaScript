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

        public static void SetInstance(Memory value)
        {
            _instance = value;
        }

        public Dictionary<string, int> Integers { get; }

        public Dictionary<string, float> Floats { get; }

        public Dictionary<string, string> Strings { get; }

        public Dictionary<string, bool> Booleans { get; }

        public Memory()
        {
            Integers = new Dictionary<string, int>();
            Floats = new Dictionary<string, float>();
            Strings = new Dictionary<string, string>();
            Booleans = new Dictionary<string, bool>();
        }
    }
}
