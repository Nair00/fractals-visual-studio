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
    public partial class FormSphinx : Form
    {

        public FormSphinx()
        {
            InitializeComponent();
        }

        double x = 0;
        double y = 0;
        double angle = 0;
        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);
        Color fillColor = Color.FromArgb(100, Color.Gray);

        PointF[] corners = new PointF[5];
        int count = 0;

        private void rotate(double x)
        {
            angle += x;
        }

        private void translate(double len, double a)
        {
            rotate(a);
            x += len * Math.Cos(angle);
            y += len * Math.Sin(angle);
        }

        private void line(double len)
        {
            corners[count++] = new PointF((float) x, (float) y);

            double x1 = x + len * Math.Cos(angle);
            double y1 = y + len * Math.Sin(angle);

            Graphics panel = panelDraw.CreateGraphics();

            panel.DrawLine(new Pen(penColor, 1), (float) x, (float) y, (float) x1, (float) y1);
            x = x1;
            y = y1;
        }

        private void sphinx(int n, double l, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                pyramid(l);
            }
            else
            {
                l = l / 2;
                sphinxFlip(n - 1, l, worker); translate(l * 3, 0);
                sphinxFlip(n - 1, l, worker); translate(l * 3, 0); translate(l * 4, -2 * Math.PI / 3);
                translate(l * 2, -Math.PI / 3);
                rotate(-Math.PI / 3);
                sphinx(n - 1, l, worker); translate(l * 4, 0);
                rotate(4 * Math.PI / 3);
            }
        }

        private void sphinxFlip(int n, double l, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                pyramidFlip(l);
            }
            else
            {
                l = l / 2;
                sphinx(n - 1, l, worker); translate(l * 3, 0);
                sphinx(n - 1, l, worker); translate(l * 3, 0); translate(l, -2 * Math.PI / 3);
                sphinxFlip(n - 1, l, worker);
                translate(l * 5, -Math.PI / 3); translate(l, -Math.PI / 3);
                rotate(4 * Math.PI / 3);
            }
        }

        private void pyramid(double length)
        {
            count = 0;

            line(length * 3); rotate(-2 * Math.PI / 3);
            line(length); rotate(-Math.PI / 3);
            line(length); rotate(Math.PI / 3);
            line(length); rotate(-2 * Math.PI / 3);
            line(length * 2); rotate(4 * Math.PI / 3);

            if (checkBox1.Checked == true)
            {
                Graphics panel = panelDraw.CreateGraphics();
                panel.FillPolygon(new SolidBrush(fillColor), corners);
            }

        }

        private void pyramidFlip(double length)
        {
            count = 0;

            line(length * 3); rotate(-2 * Math.PI / 3);
            line(length * 2); rotate(-2 * Math.PI / 3);
            line(length); rotate(Math.PI / 3);
            line(length); rotate(-Math.PI / 3);
            line(length); rotate(4 * Math.PI / 3);

            if (checkBox1.Checked == true)
            {
                Graphics panel = panelDraw.CreateGraphics();
                panel.FillPolygon(new SolidBrush(fillColor), corners);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            buttonStop.Enabled = true;
            checkBox1.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int n = 0;

            double length = panelDraw.Width / 3;
            x = 0;
            y = 8 * panelDraw.Height / 10;

            n = Convert.ToInt32(numericUpDown1.Text);

            Graphics panel = panelDraw.CreateGraphics();
            panel.Clear(bgColor);
            angle = 0;

            sphinx(n, length, backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }
        private void formSphinx_Resize(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
            labelInfo.MaximumSize = new Size(panelInfo.Width, panelInfo.Height);
            labelTitle.MaximumSize = new Size(panelTitle.Width, panelTitle.Height);
            if (panelDraw.Height == panelDraw.Width) return;
            panelDraw.Width = panelDraw.Height;
        }

        private void formSphinx_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            Dispose();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            checkBox1.Enabled = true;
        }
    }
}
