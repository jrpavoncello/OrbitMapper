using OrbitMapper.Shapes;
using OrbitMapper.Tessellations;
using OrbitMapper.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OrbitMapper.Forms
{
    /// <summary>
    /// This class has the utiliz
    /// </summary>
    public partial class Export : Form
    {

        private List<Shape> shapes = new List<Shape>();
        private List<Tessellation> tessellations = null;
        private List<int> lastWidths = new List<int>();
        private List<int> lastHeights = new List<int>();
        private List<Bitmap> previews = new List<Bitmap>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Export"/> class.
        /// </summary>
        public Export(List<Shape> shapes, List<Tessellation> tessellations, int tabNum)
        {
            InitializeComponent();

            this.shapes = shapes;
            this.tessellations = tessellations;

            // Set the title on this Export Window
            this.Text = "Export Shape";
            // These filters dictate what the user can select as a target
            this.saveFileDialog1.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|TIFF (*.tiff)|*.tiff";
            // Set the title on the save dialog
            this.saveFileDialog1.Title = "Export Shape";

            // Run through each shape fed to it by the ctor and set properties
            int i = 0;
            foreach(Shape shape in shapes){
                // Only used to help user identify which shape they want to select
                string text = shape.Text + " - Tab #" + (i + 1);
                // Every shape needs an appropriate lastWidth added so that when you are switching between shapes and settings in this menu, you don't lose your progress
                this.lastWidths.Add(shape.Width);
                this.lastHeights.Add(shape.Height);
                // Add the text to the combobox for user selection
                this.comboBox1.Items.Add(text);
                // If the current iteration is the tab number, we will default it to be the selected tab
                if (i == tabNum)
                {
                    this.comboBox1.SelectedIndex = i;
                }
                // Create a bitmap preview ahead of time for each shape
                previews.Add(ImageUtilities.createBitmapPreview(shape));
                i++;
            }
            // If there were actually shapes...
            if (i != 0)
            {
                Shape selectedShape = shapes.ElementAt<Shape>(this.comboBox1.SelectedIndex);
                WidthBox.Text = "" + selectedShape.Width;
                HeightBox.Text = "" + selectedShape.Height;
                this.lastWidths[this.comboBox1.SelectedIndex] = selectedShape.Width;
                this.lastHeights[this.comboBox1.SelectedIndex] = selectedShape.Height;
            }
            // Draw the preview and resize if necessary (in the paint method)
            pictureBox1.Invalidate();
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (this.shapes.Count == 0 || this.tessellations.Count == 0)
            {
                MessageBox.Show("There is no shape to save!");
                return;
            }

            // Get the save location
            this.saveFileDialog1.ShowDialog();
            
            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                Shape selectedShape = shapes.ElementAt<Shape>(this.comboBox1.SelectedIndex);
                Tessellation selectedTess = tessellations.ElementAt<Tessellation>(this.comboBox1.SelectedIndex);

                // If FilterIndex is 1 BMP, 2 JPEG, 3 PNG, 4 TIFF. All must be converted to image format (BMP) in order to convert to other formats.
                switch(saveFileDialog1.FilterIndex)
                {
                    case 1: // BMP
                        if(this.SaveShapeChk.Checked){
                            using (var bmp = new Bitmap(selectedShape.Width, selectedShape.Height))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(this.lastWidths[this.comboBox1.SelectedIndex], this.lastHeights[this.comboBox1.SelectedIndex]));
                                scaledBMP.Save(saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Shape"));
                            }
                        }
                        if (this.SaveTessChk.Checked)
                        {
                            using (var bmp = ImageUtilities.createTessellation(selectedTess))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(bmp.Width, bmp.Height));
                                scaledBMP.Save(saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Tess"));
                            }
                        }
                        break;
                    case 2: // JPG
                        if (this.SaveShapeChk.Checked)
                        {
                            using (var bmp = new Bitmap(selectedShape.Width, selectedShape.Height))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(this.lastWidths[this.comboBox1.SelectedIndex], this.lastHeights[this.comboBox1.SelectedIndex]));
                                ImageUtilities.SaveJpeg(scaledBMP, saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Shape"), long.MaxValue);
                            }
                        }
                        if (this.SaveTessChk.Checked)
                        {
                            using (var bmp = ImageUtilities.createTessellation(selectedTess))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(bmp.Width, bmp.Height));
                                ImageUtilities.SaveJpeg(scaledBMP, saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Tess"), long.MaxValue);
                            }
                        }
                        break;
                    case 3: // PNG
                        if (this.SaveShapeChk.Checked)
                        {
                            using (var bmp = new Bitmap(selectedShape.Width, selectedShape.Height))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(this.lastWidths[this.comboBox1.SelectedIndex], this.lastHeights[this.comboBox1.SelectedIndex]));
                                ImageUtilities.SavePng(scaledBMP, saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Shape"), long.MaxValue);
                            }
                        }
                        if (this.SaveTessChk.Checked)
                        {
                            using (var bmp = ImageUtilities.createTessellation(selectedTess))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(bmp.Width, bmp.Height));
                                ImageUtilities.SavePng(scaledBMP, saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Tess"), long.MaxValue);
                            }
                        }
                        break;
                    case 4: // TIFF
                        if (this.SaveShapeChk.Checked)
                        {
                        using (var bmp = new Bitmap(selectedShape.Width, selectedShape.Height))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(this.lastWidths[this.comboBox1.SelectedIndex], this.lastHeights[this.comboBox1.SelectedIndex]));
                                ImageUtilities.SaveTiff(scaledBMP, saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Shape"), long.MaxValue);
                            }
                        }
                        if (this.SaveTessChk.Checked)
                        {
                            using (var bmp = new Bitmap(selectedShape.Width, selectedShape.Height))
                            {
                                selectedShape.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                                Bitmap scaledBMP = ImageUtilities.ResizeImage(bmp, new Size(this.lastWidths[this.comboBox1.SelectedIndex], this.lastHeights[this.comboBox1.SelectedIndex]));
                                ImageUtilities.SaveTiff(scaledBMP, saveFileDialog1.FileName.Insert(saveFileDialog1.FileName.Length - 4, "_Tess"), long.MaxValue);
                            }
                        }
                        break;
                    default:
                        break;
                }

            }
            saveFileDialog1.FileName = "";
            this.Close();
        }

        /// <summary>
        /// Determines the ratio.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DetermineRatio(object sender, EventArgs e)
        {
            // If there is an error parsing or the sender is not a text box (should never happen under my usage), just return.
            TextBox textBox = null;
            int newSize = 0;
            try
            {
                textBox = (TextBox)sender;
                newSize = int.Parse(textBox.Text);
            }
            catch (Exception ex)
            {
                return;
            }
            // If the Lock Ratio checkbox is checked, run through resize logic
            if (this.LockChk.Checked)
            {
                double ratio = 0;
                // If the WidthBox was just stepped out of
                if (textBox.Name == "WidthBox")
                {   
                    // If last width has been set before...
                    if (this.lastWidths[this.comboBox1.SelectedIndex] != 0)
                    {
                        // Find the ratio of change between the last width and the current
                        ratio = ((double)newSize) / ((double)this.lastWidths[this.comboBox1.SelectedIndex]);
                        // Multiply the last height by that amount
                        this.lastHeights[this.comboBox1.SelectedIndex] = (int)(this.lastHeights[this.comboBox1.SelectedIndex] * ratio);
                        // Set the last width to the new size now
                        this.lastWidths[this.comboBox1.SelectedIndex] = newSize;
                        // Update the height boxes text
                        HeightBox.Text = "" + this.lastHeights[this.comboBox1.SelectedIndex];
                    }
                    // Set the last width for the first time
                    else
                        this.lastWidths[this.comboBox1.SelectedIndex] = newSize;
                }
                else
                {
                    // Do the exact same for the height box, but switch last height and last width around
                    // If the HeightBox was just stepped out of
                    if (textBox.Name == "HeightBox")
                    {
                        if (this.lastHeights[this.comboBox1.SelectedIndex] != 0)
                        {
                            ratio = ((double)newSize) / ((double)this.lastHeights[this.comboBox1.SelectedIndex]);
                            this.lastWidths[this.comboBox1.SelectedIndex] = (int)(this.lastWidths[this.comboBox1.SelectedIndex] * ratio);
                            this.lastHeights[this.comboBox1.SelectedIndex] = newSize;
                            WidthBox.Text = "" + this.lastWidths[this.comboBox1.SelectedIndex];
                        }
                        else
                            this.lastHeights[this.comboBox1.SelectedIndex] = newSize;
                    }
                }
            }
            // Otherwise, we will just go ahead and set the lastwidth or lastheight for the current sender of this event accordingly
            else
            {
                if (textBox.Name == "WidthBox")
                {
                    this.lastWidths[this.comboBox1.SelectedIndex] = newSize;
                }
                else
                {
                    if (textBox.Name == "HeightBox")
                    {
                        this.lastHeights[this.comboBox1.SelectedIndex] = newSize;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            WidthBox.Text = "" + lastWidths[this.comboBox1.SelectedIndex];
            HeightBox.Text = "" + lastHeights[this.comboBox1.SelectedIndex];
            pictureBox1.Invalidate();
        }

        /// <summary>
        /// Handles the Paint event of the pictureBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (this.comboBox1.SelectedIndex != -1 && shapes.Count > this.comboBox1.SelectedIndex && previews.Count > this.comboBox1.SelectedIndex)
            {
                // Get the current shape
                Shape shape = shapes.ElementAt<Shape>(this.comboBox1.SelectedIndex);
                // Set its image
                pictureBox1.Image = previews[this.comboBox1.SelectedIndex];
                // If the window needs resizing to make room or decrease space for the preview...
                if (pictureBox1.Width != shape.Width || pictureBox1.Height != shape.Height)
                {
                    // Calculate the padding around the picture box's width and height
                    int xDif = this.Width - pictureBox1.Width;
                    int yDif = this.Height - pictureBox1.Height;

                    // Add that to the shapes width and height
                    int xAdd = shape.Width + xDif;
                    int yAdd = shape.Height + yDif;

                    // Make sure the new values are not less than the min width and height for the whole form, if so, just make them the minimum size
                    if (xAdd < this.MinimumSize.Width)
                        xAdd = this.MinimumSize.Width;
                    if (yAdd < this.MinimumSize.Height)
                        yAdd = this.MinimumSize.Height;

                    // Resize the window
                    this.Width = xAdd;
                    this.Height = yAdd;
                }
            }
            base.Update(); //Have the base update after all the resize logic has been decided.
        }

        /// <summary>
        /// When closing, set all of the fields to null in order to clear the references.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_FormClosed(object sender, FormClosedEventArgs e)
        {
            shapes = null;
            tessellations = null;
            lastWidths = null;
            lastHeights = null;
            previews = null;
        }
    }
}
