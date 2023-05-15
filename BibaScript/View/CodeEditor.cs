using InterpreterBibaScript;
using System;
using System.IO;
using System.Windows.Forms;

namespace View
{
    public partial class CodeEditor : Form
    {
        private string _nameCurentFile;

        public CodeEditor()
        {
            InitializeComponent();
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

            BSExecutor.GetInstance().Run(programm);
        }
    }
}
