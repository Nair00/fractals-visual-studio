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
    public partial class FormZOrder : Form
    {
        public FormZOrder()
        {
            InitializeComponent();
        }

        PointF start = new PointF(0, 0);
        PointF end = new PointF(0, 0);
        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);

        private void Line()
        {
            Graphics panel = panelDraw.CreateGraphics();
            panel.DrawLine(new Pen(penColor, 1), start, end);

            start = end;
        }

        private void Z_order(int n, float x, float y, float L, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                end = new PointF(x - L / 2, y - L / 2); Line();
                end = new PointF(x + L / 2, y - L / 2); Line();
                end = new PointF(x - L / 2, y + L / 2); Line();
                end = new PointF(x + L / 2, y + L / 2); Line();
            }
            else
            {
                Z_order(n - 1, x - L / 2, y - L / 2, L / 2, worker);
                Z_order(n - 1, x + L / 2, y - L / 2, L / 2, worker);
                Z_order(n - 1, x - L / 2, y + L / 2, L / 2, worker);
                Z_order(n - 1, x + L / 2, y + L / 2, L / 2, worker);
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
            int n = 0;

            float x = panelDraw.Width / 2;
            float y = panelDraw.Height / 2;
            float L = panelDraw.Height / 2;

            n = Convert.ToInt32(numericUpDown1.Text);

            Graphics panel = panelDraw.CreateGraphics();
            panel.Clear(bgColor);

            start = new PointF(L / (float) Math.Pow(2, n + 1), L / (float) Math.Pow(2, n + 1));
            Z_order(n, x, y, L, backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }
        private void formZOrder_Resize(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
            labelInfo.MaximumSize = new Size(panelInfo.Width, panelInfo.Height);
            labelTitle.MaximumSize = new Size(panelTitle.Width, panelTitle.Height);
            if (panelDraw.Height == panelDraw.Width) return;
            panelDraw.Width = panelDraw.Height;
        }

        private void formZOrder_FormClosing(object sender, FormClosingEventArgs e)
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
