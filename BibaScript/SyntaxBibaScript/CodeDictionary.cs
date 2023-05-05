using System.Collections.Generic;

namespace SyntaxBibaScript
{
    public abstract class CodeDictionary
    {
        private protected Dictionary<SpecialWords, string> _dictionary;

        public CodeDictionary()
        {
            _dictionary = new Dictionary<SpecialWords, string>();
        }

        public string[] Values
        {
            get
            {
                string[] v = new string[_dictionary.Count];
                _dictionary.Values.CopyTo(v,0);
                return v;
            }
            set { }
        }

        public SpecialWords[] Keys
        {
            get
            {
                SpecialWords[] k = new SpecialWords[_dictionary.Count];
                _dictionary.Keys.CopyTo(k, 0);
                return k;
            }
            set { }
        }

        public bool ContainsKey(SpecialWords key) => _dictionary.ContainsKey(key);

        public bool ContainsValue(string value) => _dictionary.ContainsValue(value);

        public int Count => _dictionary.Count;
    }
}
