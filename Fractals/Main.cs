using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new FormLevyC());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new FormKoch1());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openChildForm(new FormKoch2());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openChildForm(new FormSierpinski());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openChildForm(new FormZOrder());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openChildForm(new FormGolden());
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            labelInfo.Visible = false;
        }

        private void MainPanel_SizeChanged(object sender, EventArgs e)
        {
            labelInfo.MaximumSize = new Size(panelMain.Width, panelMain.Height);
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (activeForm == null) return;
            activeForm.Close();
        }
    }
}
