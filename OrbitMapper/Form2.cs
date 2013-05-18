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

        private void resetPanels()
        {
            EquiPanel.BackColor = SystemColors.Control;
            EquiLabel.BackColor = SystemColors.Control;
            IsosTri90Panel.BackColor = SystemColors.Control;
            IsosTri90Label.BackColor = SystemColors.Control;
            IsosTri120Panel.BackColor = SystemColors.Control;
            IsosTri120Label.BackColor = SystemColors.Control;
            Rhom60Panel.BackColor = SystemColors.Control;
            Rhom60Label.BackColor = SystemColors.Control;
            Tri3060Panel.BackColor = SystemColors.Control;
            Tri3060Label.BackColor = SystemColors.Control;
            Kite6090120Panel.BackColor = SystemColors.Control;
            Kite6090120Label.BackColor = SystemColors.Control;
            HexagonPanel.BackColor = SystemColors.Control;
            HexagonLabel.BackColor = SystemColors.Control;
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

        private void button1_Click(object sender, EventArgs e)
        {
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

        private void EquiTri_Click(object sender, EventArgs e)
        {
            EventSource.output("EquiTri was selected.");
            resetPanels();
            shape = 0;
            this.EquiPanel.BackColor = SystemColors.WindowFrame;
            this.EquiLabel.BackColor = SystemColors.WindowFrame;
        }

        private void IsosTri90_Click(object sender, EventArgs e)
        {
            EventSource.output("IsosTri90 was selected.");
            resetPanels();
            shape = 1;
            this.IsosTri90Panel.BackColor = SystemColors.WindowFrame;
            this.IsosTri90Label.BackColor = SystemColors.WindowFrame;
        }

        private void IsosTri120_Click(object sender, EventArgs e)
        {
            EventSource.output("IsosTri120 was selected.");
            resetPanels();
            shape = 2;
            this.IsosTri120Panel.BackColor = SystemColors.WindowFrame;
            this.IsosTri120Label.BackColor = SystemColors.WindowFrame;
        }

        private void Tri3060_Click(object sender, EventArgs e)
        {
            EventSource.output("Tri3060 was selected.");
            resetPanels();
            shape = 3;
            this.Tri3060Panel.BackColor = SystemColors.WindowFrame;
            this.Tri3060Label.BackColor = SystemColors.WindowFrame;
        }

        private void Hexagon_Click(object sender, EventArgs e)
        {
            EventSource.output("Hexagon was selected.");
            resetPanels();
            shape = 4;
            this.HexagonPanel.BackColor = SystemColors.WindowFrame;
            this.HexagonLabel.BackColor = SystemColors.WindowFrame;
        }

        private void Rhom60_Click(object sender, EventArgs e)
        {
            EventSource.output("Rhom60 was selected.");
            resetPanels();
            shape = 5;
            this.Rhom60Panel.BackColor = SystemColors.WindowFrame;
            this.Rhom60Label.BackColor = SystemColors.WindowFrame;
        }

        private void Kite6090120_Click(object sender, EventArgs e)
        {
            EventSource.output("Kite6090120 was selected.");
            resetPanels();
            shape = 6;
            this.Kite6090120Panel.BackColor = SystemColors.WindowFrame;
            this.Kite6090120Label.BackColor = SystemColors.WindowFrame;
        }

        private void EquiTri_DoubleClick(object sender, EventArgs e)
        {
            EquiTri_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void IsosTri90_DoubleClick(object sender, EventArgs e)
        {
            IsosTri90_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void IsosTri120_DoubleClick(object sender, EventArgs e)
        {
            IsosTri120_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void Tri3060_DoubleClick(object sender, EventArgs e)
        {
            Tri3060_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void Hexagon_DoubleClick(object sender, EventArgs e)
        {
            Hexagon_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void Kite6090120_DoubleClick(object sender, EventArgs e)
        {
            Kite6090120_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }

        private void Rhom60_DoubleClick(object sender, EventArgs e)
        {
            Rhom60_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }
    }
}
