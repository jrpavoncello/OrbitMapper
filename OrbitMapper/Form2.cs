using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace OrbitMapper
{
    public partial class Form2 : Form
    {
        private int shape = -1;
        private bool cancelled = true;

        public Form2()
        {
            InitializeComponent();
        }

        public int getShape(){
            return shape;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            EventSource.output("Panel1 was selected.");
            resetPanels();
            shape = 0;
            this.panel1.BackColor = SystemColors.GradientActiveCaption;
            label1.BackColor = SystemColors.GradientActiveCaption;
        }

        private void resetPanels()
        {
            panel1.BackColor = SystemColors.Control;
            label1.BackColor = SystemColors.Control;
            panel2.BackColor = SystemColors.Control;
            label2.BackColor = SystemColors.Control;
            panel3.BackColor = SystemColors.Control;
            label3.BackColor = SystemColors.Control;
            panel4.BackColor = SystemColors.Control;
            label4.BackColor = SystemColors.Control;
            panel5.BackColor = SystemColors.Control;
            label5.BackColor = SystemColors.Control;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            string temp = null;
            switch (shape)
            {
                case 0:
                    temp = "Equilateral ";
                    break;
                case 1:
                    temp = "Isosceles ";
                    break;
                case 2:
                    temp = "60-120 Rhombus ";
                    break;
                case 3:
                    temp = "60-90-120 Kite ";
                    break;
                case 4:
                    temp = "120 Hexagon ";
                    break;
                default:
                    temp = "No ";
                    break;
            }
            EventSource.output(temp + "shape was selected.");
            if (shape == -1 || cancelled == true)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.Yes;
                cancelled = false;
            }
            EventSource.output("Form3 closed.");
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            EventSource.output("Panels reset.");
            shape = -1;
            resetPanels();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            EventSource.output("Panel2 was selected.");
            resetPanels();
            shape = 1;
            this.panel2.BackColor = SystemColors.GradientActiveCaption;
            label2.BackColor = SystemColors.GradientActiveCaption;
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            EventSource.output("Panel3 was selected.");
            resetPanels();
            shape = 2;
            this.panel3.BackColor = SystemColors.GradientActiveCaption;
            label3.BackColor = SystemColors.GradientActiveCaption;
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            EventSource.output("Panel4 was selected.");
            resetPanels();
            shape = 3;
            this.panel4.BackColor = SystemColors.GradientActiveCaption;
            label4.BackColor = SystemColors.GradientActiveCaption;
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            EventSource.output("Panel5 was selected.");
            resetPanels();
            shape = 4;
            this.panel5.BackColor = SystemColors.GradientActiveCaption;
            label5.BackColor = SystemColors.GradientActiveCaption;
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            panel1_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {
            panel2_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void panel3_DoubleClick(object sender, EventArgs e)
        {
            panel3_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void panel4_DoubleClick(object sender, EventArgs e)
        {
            panel4_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void panel5_DoubleClick(object sender, EventArgs e)
        {
            panel5_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void Form2_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point p = tableLayoutPanel1.AutoScrollPosition;
            this.tableLayoutPanel1.AutoScrollPosition = new Point(tableLayoutPanel1.AutoScrollPosition.X, Math.Abs(tableLayoutPanel1.AutoScrollPosition.Y) - (e.Delta / 10));
        }
    }
}
