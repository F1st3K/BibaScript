using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    static class ColorRichBox
    {
        private static string _oldWords;

        public static void SetColor(this RichTextBox box, string words, Color color)
        {
            var r = "^.[$()|*+?{\\";
            if (r.Contains(words))
                words = "\\" + words;
            MatchCollection allIp = Regex.Matches(box.Text, $@"\b{words}\s");
            foreach (Match ip in allIp)
            {
                box.SelectionStart = ip.Index;
                box.SelectionLength = ip.Length;
                box.SelectionColor = color;
            }
        }

        public static void SetColorAbs(this RichTextBox box, string words, Color color)
        {
            var r = "^.[$()|*+?{\\";
            if (r.Contains(words))
                words = "\\" + words;
            MatchCollection allIp = Regex.Matches(box.Text, $@"{words}");
            foreach (Match ip in allIp)
            {
                box.SelectionStart = ip.Index;
                box.SelectionLength = ip.Length;
                box.SelectionColor = color;
            }
        }
    }
}
