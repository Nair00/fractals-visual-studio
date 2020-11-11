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
    public partial class FormKoch2 : Form
    {

        public FormKoch2()
        {
            InitializeComponent();
        }

        double angle = 0;
        double x = 50;
        double y = 50;
        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);

        private void line(double L)
        {
            double x1 = x;
            double y1 = y;

            x = x + L * Math.Cos(angle);
            y = y + L * Math.Sin(angle);

            Graphics back = panelDraw.CreateGraphics();
            back.DrawLine(new Pen(penColor, 1), (float) x, (float) y, (float) x1, (float) y1);
        }

        void rotate(double r)
        {
            angle += r;
        }

        private void desen(int n, double L, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                line(L);
            }
            else
            {
                desen(n - 1, L / 3, worker); rotate(Math.PI / 3);
                desen(n - 1, L / 3, worker); rotate(-2 * Math.PI / 3);
                desen(n - 1, L / 3, worker); rotate(Math.PI / 3);
                desen(n - 1, L / 3, worker);
            }
        }

        private void centru(int n, double L, BackgroundWorker worker)
        {
            desen(n, L, worker); rotate(2 * Math.PI / 3);
            desen(n, L, worker); rotate(2 * Math.PI / 3);
            desen(n, L, worker); rotate(2 * Math.PI / 3);
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
            y = panelDraw.Width / 10;

            n = Convert.ToInt32(numericUpDown1.Text);

            Graphics back = panelDraw.CreateGraphics();
            back.Clear(bgColor);

            centru(n, L , backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }

        private void formKoch2_Resize(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
            labelInfo.MaximumSize = new Size(panelInfo.Width, panelInfo.Height);
            labelTitle.MaximumSize = new Size(panelTitle.Width, panelTitle.Height);
            if (panelDraw.Height == panelDraw.Width) return;
            panelDraw.Width = panelDraw.Height;
        }

        private void formKoch2_FormClosing(object sender, FormClosingEventArgs e)
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
