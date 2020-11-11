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
    public partial class FormPinwheel : Form
    {
        double angA = Math.Acos(1 / Math.Sqrt(5));
        double angB = Math.Acos(2 / Math.Sqrt(5));
        double angle = 0;
        double x = 0;
        double y = 0;

        Color penColor = Color.White;
        Color bgColor = Color.FromArgb(43, 40, 76);
        Color fillColor = Color.FromArgb(100, Color.Gray);

        PointF[] corners = new PointF[3];
        int count = 0;

        public FormPinwheel()
        {
            InitializeComponent();
        }

        private void rotate(double x)
        {
            angle += x;
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

        private void translate(double len, double a)
        {
            rotate(a);
            x += len * Math.Cos(angle);
            y += len * Math.Sin(angle);
        }

        private void pinwheel(int n, double length, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                triangle(length);
            }
            else
            {
                double xx = x;
                double yy = y;
                length = length / Math.Sqrt(5);
                translate(length, -angB);
                pinwheelFlip(n - 1, length, worker);
                translate(length, 0);
                rotate(-Math.PI / 2);
                pinwheelFlip(n - 1, length, worker);
                translate(length * 2, 0);
                rotate(Math.PI / 2);
                pinwheelFlip(n - 1, length, worker);
                rotate(Math.PI);
                pinwheel(n - 1, length, worker);
                x = xx;
                y = yy;
                rotate(-Math.PI + angB);
            }
        }

        private void pinwheelFlip(int n, double length, BackgroundWorker worker)
        {
            if (worker.CancellationPending) return;
            if (n == 0)
            {
                triangleFlip(length);
            }
            else
            {
                double xx = x;
                double yy = y;
                length = length / Math.Sqrt(5);
                translate(length, -Math.PI / 2 - angA);
                rotate(Math.PI);
                pinwheel(n - 1, length, worker);
                translate(length, -Math.PI);
                rotate(-Math.PI / 2);
                pinwheel(n - 1, length, worker);
                translate(length * 2, Math.PI);
                rotate(Math.PI / 2);
                pinwheel(n - 1, length, worker);
                rotate(-Math.PI);
                pinwheelFlip(n - 1, length, worker);
                x = xx;
                y = yy;
                rotate(Math.PI - angB);
            }
        }
        private void triangle(double length)
        {
            count = 0;

            line(length);
            rotate(-(Math.PI / 2 + angB));
            line(length * Math.Sqrt(5));
            rotate(-(Math.PI - angB));
            line(length * 2);
            rotate(-(Math.PI / 2));

            if (checkBox1.Checked == true)
            {
                Graphics panel = panelDraw.CreateGraphics();
                panel.FillPolygon(new SolidBrush(fillColor), corners);
            }
        }

        private void triangleFlip(double length)
        {
            count = 0;

            rotate(Math.PI);
            line(length);
            rotate(Math.PI / 2 + angB);
            line(length * Math.Sqrt(5));
            rotate(Math.PI - angB);
            line(length * 2);
            rotate(-(Math.PI / 2));

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

            x = panelDraw.Width / 10 * 3;
            y = panelDraw.Height - 1;
            double length = panelDraw.Height / 2;

            n = Convert.ToInt32(numericUpDown1.Text);

            Graphics panel = panelDraw.CreateGraphics();
            panel.Clear(bgColor);
            angle = 0;

            pinwheel(n, length, backgroundWorker1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

        }

        private void panel2_SizeChanged(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
        }
        private void formPinwheel_Resize(object sender, EventArgs e)
        {
            buttonStart.Width = panelButtons.Width / 2;
            labelInfo.MaximumSize = new Size(panelInfo.Width, panelInfo.Height);
            labelTitle.MaximumSize = new Size(panelTitle.Width, panelTitle.Height);
            if (panelDraw.Height == panelDraw.Width) return;
            panelDraw.Width = panelDraw.Height;
        }


        private void formPinwheel_FormClosing(object sender, FormClosingEventArgs e)
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
