using InterpreterBibaScript;
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
                _currentTask = Task.Run(() => { BSExecutor.GetInstance().Run(programm); HideConsoleWindow(); });
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
    }
}
