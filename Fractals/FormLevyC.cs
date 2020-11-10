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
    public partial class FormLevyC : Form
    {
        double x = 0;
        double y = 0;
        double angle = 0;
        double length = 0;
        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);

        public FormLevyC()
        {
            InitializeComponent();
            panelDraw.Height = this.Height;
            panelDraw.Width = panelDraw.Height;
        }

        private void rotate(double x)
        {
            angle += x;
        }

        private void line(double length)
        {
            double x1 = x + length * Math.Cos(angle);
            double y1 = y + length * Math.Sin(angle);

            Graphics panel = panelDraw.CreateGraphics();
            panel.DrawLine(new Pen(penColor, 1), (float) x, (float) y, (float) x1, (float) y1);

            x = x1;
            y = y1;
        }

        private void levy(int n, double L, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                line(L);
            }
            else
            {
                rotate(Math.PI / 2);
                levy(n - 1, L / 2, worker);
                rotate(-Math.PI / 2);
                levy(n - 1, L / 2, worker);
                levy(n - 1, L / 2, worker);
                rotate(-Math.PI / 2);
                levy(n - 1, L / 2, worker);
                rotate(Math.PI / 2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Graphics panel = panelDraw.CreateGraphics();
            panel.Clear(bgColor);
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            int n = 0;
            n = Convert.ToInt32(numericUpDown1.Text);

            x = panelDraw.Width / 4;
            y = panelDraw.Height / 3;
            angle = 0;
            length = panelDraw.Width / 2;
            levy(n, length, backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }

        private void formLevyC_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            Dispose();
        }

        private void formLevyC_Resize(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
            labelInfo.MaximumSize = new Size(panelInfo.Width, panelInfo.Height);
            labelTitle.MaximumSize = new Size(panelTitle.Width, panelTitle.Height);
            if (panelDraw.Height == panelDraw.Width) return;
            panelDraw.Width = panelDraw.Height;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
        }

        //private System.ComponentModel.IContainer components = null;

    }
}
