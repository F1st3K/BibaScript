using System;
using System.Windows.Forms;

namespace View
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        public void MidleLoad(Action func, int time)
        {
            var timer = new Timer();
            timer.Interval = time;
            timer.Tick += (a, ev) => { func.Invoke(); timer.Stop(); Close(); };
            timer.Start();
        }

        public void StartLoad(int time)
        {
            var form = new CodeEditor();
            var timer = new Timer();
            timer.Interval = time;
            timer.Tick += (a, ev) => { form.Show(); Hide(); timer.Stop(); };
            form.FormClosed += (a, ev) => { Show(); MidleLoad(() => { Application.Exit(); }, 1000); };
            timer.Start();
        }
    }
}
