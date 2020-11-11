using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals
{
    public partial class FormKoch1 : Form
    {
        public FormKoch1()
        {
            InitializeComponent();
        }

        double angle = 0;
        double x = 0;
        double y = 0;

        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);

        private void line(double L)
        {
            double x1 = x + L * Math.Cos(angle);
            double y1 = y + L * Math.Sin(angle);

            Graphics panel = panelDraw.CreateGraphics();
            panel.DrawLine(new Pen(penColor, 1), (float) x, (float) y, (float) x1, (float) y1);

            x = x1;
            y = y1;
        }

        void rotate(double x)
        {
            angle += x * Math.PI / 180;
        }

        private void koch(int n, double length, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                line(length);
            }
            else
            {
                length = length / (2 + Math.Sin(Math.PI / 18) / Math.Sin(Math.PI * 0.47));
                koch(n - 1, length, worker); rotate(-85);
                koch(n - 1, length, worker); rotate(170);
                koch(n - 1, length, worker); rotate(-85);
                koch(n - 1, length, worker);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int n;
            double L = panelDraw.Width;
            x = 0;
            y = 3 * panelDraw.Width / 5;
            n = Convert.ToInt32(numericUpDown1.Text);
            Graphics panel = panelDraw.CreateGraphics();
            panel.Clear(bgColor);

            koch(n, L, backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }

        private void formKoch1_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            Dispose();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
        }
    }
}
