using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace OrbitMapper
{
    /// <summary>
    /// This form is used to get user data in order to instantiate a new shape. This hangs from the main menu toolbar.
    /// </summary>
    public partial class NewShapeForm : Form
    {
        private int shape = -1;
        private bool cancelled = true;
        private double rectSize = 0;

        public NewShapeForm()
        {
            InitializeComponent();
        }

        public int getShape(){
            return shape;
        }

        public double getRectSize(){ return rectSize; }

        /// <summary>
        /// Used when a new shape box is clicked, it resets all of the labels and panels to the default color I wanted the to be
        /// </summary>
        private void resetPanels()
        {
            EquiPanel.BackColor = SystemColors.ButtonFace;
            EquiLabel.BackColor = SystemColors.ButtonFace;
            IsosTri90Panel.BackColor = SystemColors.ButtonFace;
            IsosTri90Label.BackColor = SystemColors.ButtonFace;
            IsosTri120Panel.BackColor = SystemColors.ButtonFace;
            IsosTri120Label.BackColor = SystemColors.ButtonFace;
            Rhom60Panel.BackColor = SystemColors.ButtonFace;
            Rhom60Label.BackColor = SystemColors.ButtonFace;
            Tri3060Panel.BackColor = SystemColors.ButtonFace;
            Tri3060Label.BackColor = SystemColors.ButtonFace;
            Kite6090120Panel.BackColor = SystemColors.ButtonFace;
            Kite6090120Label.BackColor = SystemColors.ButtonFace;
            HexagonPanel.BackColor = SystemColors.ButtonFace;
            HexagonLabel.BackColor = SystemColors.ButtonFace;
            RectanglePanel.BackColor = SystemColors.ButtonFace;
            RectangleLabel.BackColor = SystemColors.ButtonFace;
        }

        /// <summary>
        /// This is the last method called BEFORE the form closes AFTER the X button is hit or this.close() is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // This switch statement is used only for debug output
            string temp = null;
            switch (shape)
            {
                case 0:
                    temp = "Equilateral ";
                    break;
                case 1:
                    temp = "90 Isosceles Triangle ";
                    break;
                case 2:
                    temp = "120 Isosceles Triangle ";
                    break;
                case 3:
                    temp = "30-60-90 Triangle ";
                    break;
                case 4:
                    temp = "120 Hexagon ";
                    break;
                case 5:
                    temp = "60-120 Rhombus ";
                    break;
                case 6:
                    temp = "60-90-120 Kite ";
                    break;
                case 7:
                    temp = "Rectangle ";
                    break;
                default:
                    temp = "No ";
                    break;
            }
            EventSource.output(temp + "shape was selected.");
            // When the X button was hit, make sure the result reflects that the user did not want a shape created
            if (shape == -1 || cancelled == true)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.Yes;
            }
            EventSource.output("Form3 closed.");
        }

        /// <summary>
        /// This gets reset because even when the dialog is closed, the instance is still in memory, therefore the fields need reset so that
        /// when the instance is shown again, and the last instance set cancelled=true, it does not consistently report that it was cancelled from my logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Shown(object sender, EventArgs e)
        {
            EventSource.output("Panels reset.");
            cancelled = true; 
            shape = -1;
            resetPanels();
        }

        /// <summary>
        /// The Create button. If the user selected a rectangle, attempt to parse it and show a dialog box if it fails or is 0.
        /// If the User selected a shape, set the cancelled field to reflect it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (shape != -1)
                {
                    rectSize = double.Parse(textBox1.Text);
                    if(rectSize == 0)
                    {
                        MessageBox.Show("Cannot create a new rectangle of that type.", "Orbit Mapper",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                    }
                    cancelled = false;
                    this.Close();
                }
            }
            catch (System.ArgumentNullException ex)
            {
                MessageBox.Show("Cannot create a new rectangle of that type.", "Orbit Mapper",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show("Cannot create a new rectangle of that type.", "Orbit Mapper",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (System.OverflowException ex)
            {
                MessageBox.Show("Cannot create a new rectangle of that type.", "Orbit Mapper",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// This allows the user to use the mouse sheel to scroll up and down with the AutoScroll bar feature.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point p = tableLayoutPanel1.AutoScrollPosition;
            // To scroll, you must use the AutoScrollPosition field and instantiate new Points every time you fire the event use the mouse sheel
            // Because we are not horizontally scrolling, set the X coordinate point to remain the same.
            // The Y coordinate Point reports back negative for the scroll bar, however it needs a positive coordinate to set it to something new (so stupid)
            // So just get that and subtract the Delta property (change). This could be different depending on how Windows is configured to handle the mouse scroll (some people set Windows to scroll faster)
            this.tableLayoutPanel1.AutoScrollPosition = new Point(tableLayoutPanel1.AutoScrollPosition.X, Math.Abs(tableLayoutPanel1.AutoScrollPosition.Y) - (e.Delta / 10));
        }

        private void EquiTri_Click(object sender, EventArgs e)
        {
            EventSource.output("EquiTri was selected.");
            resetPanels();
            shape = 0;
            this.EquiPanel.BackColor = SystemColors.MenuHighlight;
            this.EquiLabel.BackColor = SystemColors.MenuHighlight;
        }

        private void IsosTri90_Click(object sender, EventArgs e)
        {
            EventSource.output("IsosTri90 was selected.");
            resetPanels();
            shape = 1;
            this.IsosTri90Panel.BackColor = SystemColors.MenuHighlight;
            this.IsosTri90Label.BackColor = SystemColors.MenuHighlight;
        }

        private void IsosTri120_Click(object sender, EventArgs e)
        {
            EventSource.output("IsosTri120 was selected.");
            resetPanels();
            shape = 2;
            this.IsosTri120Panel.BackColor = SystemColors.MenuHighlight;
            this.IsosTri120Label.BackColor = SystemColors.MenuHighlight;
        }

        private void Tri3060_Click(object sender, EventArgs e)
        {
            EventSource.output("Tri3060 was selected.");
            resetPanels();
            shape = 3;
            this.Tri3060Panel.BackColor = SystemColors.MenuHighlight;
            this.Tri3060Label.BackColor = SystemColors.MenuHighlight;
        }

        private void Hexagon_Click(object sender, EventArgs e)
        {
            EventSource.output("Hexagon was selected.");
            resetPanels();
            shape = 4;
            this.HexagonPanel.BackColor = SystemColors.MenuHighlight;
            this.HexagonLabel.BackColor = SystemColors.MenuHighlight;
        }

        private void Rhom60_Click(object sender, EventArgs e)
        {
            EventSource.output("Rhom60 was selected.");
            resetPanels();
            shape = 5;
            this.Rhom60Panel.BackColor = SystemColors.MenuHighlight;
            this.Rhom60Label.BackColor = SystemColors.MenuHighlight;
        }

        private void Kite6090120_Click(object sender, EventArgs e)
        {
            EventSource.output("Kite6090120 was selected.");
            resetPanels();
            shape = 6;
            this.Kite6090120Panel.BackColor = SystemColors.MenuHighlight;
            this.Kite6090120Label.BackColor = SystemColors.MenuHighlight;
        }

        private void Rectangle_Click(object sender, EventArgs e)
        {
            EventSource.output("Rectangle was selected.");
            resetPanels();
            shape = 7;
            this.RectanglePanel.BackColor = SystemColors.MenuHighlight;
            this.RectangleLabel.BackColor = SystemColors.MenuHighlight;
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

        private void Rectangle_DoubleClick(object sender, EventArgs e)
        {
            Rectangle_Click(sender, e);
            if (shape != -1)
            {
                cancelled = false;
                this.Close();
            }
        }
    }
}
