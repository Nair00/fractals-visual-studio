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
    public partial class FormGolden : Form
    {
        static double phi = (1 + (Math.Sqrt(5))) / 2;
        static double r = Math.Pow(1 / phi, 1 / phi);
        double A = Math.Acos((1 + r * r - r * r * r * r) / (2 * r));
        double B = Math.Acos((1 + r * r * r * r - r * r) / (2 * r * r));
        double angle = 0;

        double x = 0;
        double y = 0;
        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);

        public FormGolden()
        {
            InitializeComponent();
        }

        private void line(double x1, double y1)
        {
            Graphics panel = panelDraw.CreateGraphics();
            panel.DrawLine(new Pen(penColor, 1), (float) x, (float) y, (float) x1, (float) y1);
            x = x1;
            y = y1;
        }

        private void rotate(double x)
        {
            angle += x;
        }

        private void goldenA(int n, double L, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                line(x + L * Math.Cos(angle), y + L * Math.Sin(angle));
            }
            else
            {
                rotate(-A);
                goldenA(n - 1, L * r, worker);
                rotate(A + B);
                goldenB(n - 1, L * r * r, worker);
                rotate(-B);
            }
        }

        private void goldenB(int n, double L, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                line(x + L * Math.Cos(angle), y + L * Math.Sin(angle));
            }
            else
            {
                rotate(B);
                goldenA(n - 1, L * r * r, worker);
                rotate(-A - B);
                goldenB(n - 1, L * r, worker);
                rotate(A);
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

            //Starting parameters
            double length = panelDraw.Width * 0.7;
            x = panelDraw.Width * 0.2;
            y = panelDraw.Height / 2;

            n = Convert.ToInt32(numericUpDown1.Text); //Save given iterations number

            //Reset screen
            Graphics panel = panelDraw.CreateGraphics();
            panel.Clear(bgColor);
            angle = 0;

            //Call Recursive function
            goldenA(n, length, backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }

        private void formGolden_Resize(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
            labelInfo.MaximumSize = new Size(panelInfo.Width, panelInfo.Height);
            labelTitle.MaximumSize = new Size(panelTitle.Width, panelTitle.Height);
            if (panelDraw.Height == panelDraw.Width) return;
            panelDraw.Width = panelDraw.Height;
        }

        private void formGolden_FormClosing(object sender, FormClosingEventArgs e)
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
