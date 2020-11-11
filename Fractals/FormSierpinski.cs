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
    public partial class FormSierpinski : Form
    {

        public FormSierpinski()
        {
            InitializeComponent();
        }

        double angle = 0;
        double x = 0;
        double y = 0;

        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);
        Color fillColor = Color.FromArgb(100, Color.Gray);

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

        private void carpet(int n, double length, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                square(length);
            }
            else
            {
                length = length / 3;
                carpet(n - 1, length, worker); x += length;
                carpet(n - 1, length, worker); x += length;
                carpet(n - 1, length, worker); x -= 2 * length; y += length;
                carpet(n - 1, length, worker); x += 2 * length;
                carpet(n - 1, length, worker); x -= 2 * length; y += length;
                carpet(n - 1, length, worker); x += length;
                carpet(n - 1, length, worker); x += length;
                carpet(n - 1, length, worker); x -= 2 * length; y -= 2 * length;
            }
        }

        private void square(double length)
        {
            PointF[] corners = new PointF[4];
            for (int i = 0; i < 4; i++)
            {
                corners[i] = new PointF((float) x, (float) y);
                line(length); rotate(Math.PI / 2);
            }

            if (checkBox1.Checked == true)
            {
                Graphics panel = panelDraw.CreateGraphics();
                panel.FillPolygon(new SolidBrush(fillColor), corners);
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

            n = Convert.ToInt32(numericUpDown1.Value);

            Graphics fractal = panelDraw.CreateGraphics();
            fractal.Clear(bgColor);
            angle = 0;
            x = 0;
            y = 0;
            double length = panelDraw.Width <= panelDraw.Height ? panelDraw.Width - 1 : panelDraw.Height - 1;

            carpet(n, length, backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }

        private void formSierpinski_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            Dispose();
        }
        private void formSierpinski_Resize(object sender, EventArgs e)
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
    }
}
