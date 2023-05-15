using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    static class ColorRichBox
    {
        private static string _oldWords;

        public static void SetColor(this RichTextBox box, string[] words, Color color)
        {
            string text;
            if (_oldWords != null)
                text = box.Text.Replace(_oldWords, string.Empty);
            else text = box.Text;
            int index = 0;
            for (int i = 0; i < words.Length; i++)
            {
                var temp = text.IndexOf(words[i], index, StringComparison.Ordinal);
                if (temp == -1)
                    continue;

                box.SelectionStart = index;
                box.SelectionLength = words[i].Length;
                box.SelectionColor = color;
                index = temp;
                text = text.Substring(temp, words[i].Length);
            }
            _oldWords += text;
        }
    }
}
