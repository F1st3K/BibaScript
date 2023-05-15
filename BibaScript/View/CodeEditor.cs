using InterpreterBibaScript;
using SyntaxBibaScript;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class CodeEditor : Form
    {
        private string _nameCurentFile;
        private Task _currentTask;

        public CodeEditor()
        {
            InitializeComponent();
            KeyPreview = true;
            KeyDown += CodeEditor_KeyDown;
            MouseWheel += CodeEditor_MouseWheel;
        }

        private void CodeEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                SendKeys.Send("=");
            else SendKeys.Send("-");
        }

        private void CodeEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Add)
                richTextBox.Font = new Font(richTextBox.Font.FontFamily, richTextBox.Font.Size + 1);
            else if (e.Control && e.KeyCode == Keys.Add)
                richTextBox.Font = new Font(richTextBox.Font.FontFamily, richTextBox.Font.Size - 1);

            if (e.Control || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
                ColorText();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _nameCurentFile = null;
            richTextBox.Text = string.Empty;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var OFD = new OpenFileDialog();
            OFD.Filter = "Скрипты BS|*.bs";
            if (OFD.ShowDialog() != DialogResult.OK)
                return;
            _nameCurentFile = OFD.FileName;
            richTextBox.Text = File.ReadAllText(_nameCurentFile);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_nameCurentFile == null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
                return;
            }
            File.WriteAllText(_nameCurentFile, richTextBox.Text);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var SFD = new SaveFileDialog();
            SFD.Filter = "Скрипты BS|*.bs";
            if (_nameCurentFile != null)
                SFD.InitialDirectory = Directory.GetParent(_nameCurentFile).FullName;
            SFD.FileName = _nameCurentFile == null ? "Script.bs" : _nameCurentFile.Replace(SFD.InitialDirectory + "\\", string.Empty);
            if (SFD.ShowDialog() != DialogResult.OK)
                return;
            File.WriteAllText(SFD.FileName, richTextBox.Text);
            _nameCurentFile = SFD.FileName;
        }

        private void runProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
            string programm;
            if (_nameCurentFile == null)
                programm = richTextBox.Text;
            else programm = File.ReadAllText(_nameCurentFile);

            if (_currentTask == null || _currentTask.IsCompleted)
            {
                _currentTask = Task.Run(() => { 
                    BSExecutor.GetInstance().Run(programm);
                    HideConsoleWindow(); 
                });
            }
            
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;

        public static void HideConsoleWindow()
        {
            Console.ReadKey();
            Console.Clear();
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(64, 64, 64);
            richTextBox.ForeColor = Color.AntiqueWhite;
            richTextBox.BackColor = Color.DimGray;
            menuStrip.ForeColor = Color.White;
            menuStrip.BackColor = Color.FromArgb(64, 64, 64);
            foreach (ToolStripMenuItem item in menuStrip.Items)
                foreach (ToolStripMenuItem item1 in item.DropDownItems)
                {
                    item1.ForeColor = Color.White;
                    foreach (ToolStripMenuItem item2 in item1.DropDownItems)
                        item2.ForeColor = Color.White;
                }
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new DarkColorTable());
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(200, 200, 200);
            richTextBox.ForeColor = Color.Black;
            richTextBox.BackColor = Color.WhiteSmoke;
            menuStrip.ForeColor = Color.Black;
            menuStrip.BackColor = Color.FromArgb(200, 200, 200);
            foreach (ToolStripMenuItem item in menuStrip.Items)
                foreach (ToolStripMenuItem item1 in item.DropDownItems)
                {
                    item1.ForeColor = Color.Black;
                    foreach (ToolStripMenuItem item2 in item1.DropDownItems)
                        item2.ForeColor = Color.Black;
                }
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new LightColorTable());
        }

        private void CodeEditor_Load(object sender, EventArgs e)
        {
            darkToolStripMenuItem_Click(sender, e);
        }

        private void ColorText()
        {
            var color = richTextBox.ForeColor;
            richTextBox.SetColor(CodeConstructions.GetInstance().Values, Color.FromArgb(200, 100, 200));
            richTextBox.SetColor(CodeTypes.GetInstance().Values, Color.FromArgb(100, 100, 200));
            richTextBox.SetColor(CodeTypeWords.GetInstance().Values, Color.FromArgb(100, 200, 100));
            //string word = string.Empty;
            //int countS = 0;
            //foreach (var ch in richTextBox.Text)
            //{
            //    if (ch == CodeTypeWords.GetInstance().GetValue(SpecialWords.SeparatorString)[0])
            //        countS++;
            //    if (countS % 2 != 0)
            //        word += ch;
            //}
            //richTextBox.SetColor(word, Color.FromArgb(200, 200, 100));

            richTextBox.SelectionStart += richTextBox.SelectionLength;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = color;
        }
    }
}
