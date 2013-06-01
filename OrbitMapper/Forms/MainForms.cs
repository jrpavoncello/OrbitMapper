using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.IO;
using System.Xml;
using System.Drawing.Drawing2D;
using OrbitMapper.Tessellations;
using OrbitMapper.Shapes;

namespace OrbitMapper
{
    /// <summary>
    ///  This is the main control form for the program's execution. Everything should report back to this form in some way.
    /// </summary>
    public partial class MainForms : Form
    {
        
        // Used to determine whether a new version is available and used in the About.cs and Version.cs form.
        private const string OMVersion = "1.1.5";

        // newShape is used as a member field to hold the state of the NewShapeForm when a user chooses a new shape to create.
        // shapes holds all of the instances of shapes that users creates, and tessellations holds all of the instances of tessellations.
        // lastTab is used during tab switching to determine whether appropriate action is needed.
        private NewShapeForm newShape;
        private List<Shape> shapes = new List<Shape>();
        private List<Tessellation> tessellations = new List<Tessellation>();
        private Shape lastTab;
        
        /// <summary>
        /// This constructor initializes the layout and components, adds the event handlers to their appropriate dispatchers,
        /// and begins the check for the appropriate version
        /// </summary>
        public MainForms()
        {
            InitializeComponent();
            EventSource.tessellate += new Tessellate(updateFields);
            EventSource.finishedTessellate += new FinishedDrawTess(updateBounces);
            EventSource.finishedShape += new FinishedDrawShape(OnPostShapeCollisions);
            EventSource.tabRemove += new RemoveTab(removeThisTab);

            // To address a bug with the table layout in Windows Vista where the text field and labels in the layout builds incorrectly.
            // I check if they are placed incorrectly, if so, then I place them in the intended positions.
            TableLayoutPanelCellPosition box = tableLayoutPanel2.GetCellPosition(angleBox);
            if(box.Column != 1 || box.Row != 1){
                tableLayoutPanel2.Controls.Remove(label3);
                tableLayoutPanel2.Controls.Remove(bouncesBox);
                tableLayoutPanel2.Controls.Remove(angleBox);
                tableLayoutPanel2.Controls.Remove(label2);
                tableLayoutPanel2.Controls.Add(label3, 2, 0);
                tableLayoutPanel2.Controls.Add(bouncesBox, 2, 1);
                tableLayoutPanel2.Controls.Add(label2, 1, 0);
                tableLayoutPanel2.Controls.Add(angleBox, 1, 1);

            }
            this.backgroundWorker1.RunWorkerAsync();
        }

        /// <summary>
        /// Centralized method to handle the instantiation of all Shapes and Tessellations.
        /// </summary>
        /// <param name="shape">Used to determine which shape to instantiate.</param>
        /// <param name="ratio">Optional parameter that defaults to 0. (Only used for rectangle)</param>
        /// <returns>Returns a Control array of length 2, the first element being the instance of the Shape 
        /// and the second being the instance of the Tessellation.</returns>
        private Control[] instantiateShapes(int shape, double ratio = 0d)
        {
            // Checks for the default tabPage that is added as a placeholder, and removes it if a shape is being created.
            if (tabControl1.TabPages.Contains(tabPage1))
            {
                tabControl1.TabPages.Remove(tabPage1);
            }
            Shape tempShape = null;
            Tessellation tempTess = null;
            // Each switch case sets the lastTab field, adds the Shape instance to the shapes List, adds the Tessellation instance to the tessellations List,
            // adds the Shape to the tabControl and sets the ContextMenu of that tab to the context menu field in the Shape base class.
            switch (shape)
            {
                case 0:
                    tempShape = new Equilateral();
                    EventSource.output("Equilateral Triangle tab created.");
                    tempTess = new EquilateralTess();
                    tempTess.Name = "Equilateral" + (tempShape.getShapeCount() - 1);
                    break;
                case 1:
                    tempShape = new IsosTri90();
                    EventSource.output("90 Isosceles Triangle tab created.");
                    tempTess = new IsosTri90Tess();
                    tempTess.Name = "IsosTri90" + (tempShape.getShapeCount() - 1);
                    break;
                case 2:
                    tempShape = new IsosTri120();
                    EventSource.output("120 Isosceles Triangle tab created.");
                    tempTess = new IsosTri120Tess();
                    tempTess.Name = "IsosTri120" + (tempShape.getShapeCount() - 1);
                    break;
                case 3:
                    tempShape = new Tri3060();
                    EventSource.output("30-60-90 Triangle tab created.");
                    tempTess = new Tri3060Tess();
                    tempTess.Name = "Tri3060" + (tempShape.getShapeCount() - 1);
                    break;
                case 4:
                    tempShape = new Hexagon();
                    EventSource.output("120 Hexagon tab created.");
                    tempTess = new HexagonTess();
                    tempTess.Name = "Hexagon" + (tempShape.getShapeCount() - 1);
                    break;
                case 5:
                    tempShape = new Rhombus();
                    EventSource.output("60-120 Rhombus tab created.");
                    tempTess = new RhombusTess();
                    tempTess.Name = "Rhombus" + (tempShape.getShapeCount() - 1);
                    break;
                case 6:
                    tempShape = new Kite();
                    EventSource.output("60-90-120 Kite tab created.");
                    tempTess = new KiteTess();
                    tempTess.Name = "Kite" + (tempShape.getShapeCount() - 1);
                    break;
                case 7:
                    tempShape = new Rect(ratio);
                    EventSource.output("Rectangle tab created.");
                    tempTess = new RectTess(ratio);
                    tempTess.Name = "Rectangle" + (tempShape.getShapeCount() - 1);
                    break;
                default:
                    break;
            }

            // The order for this must remain the same, the Shape and Tessellation instances must be added to the Lists before they are added to the Controls
            // because it triggers an event for the tabControl when a new Control is added, and the lastTab must be configured correctly for it.
            if (shapes.Count() != 0)
                lastTab = (Shape)tabControl1.SelectedTab;
            shapes.Add(tempShape);
            tessellations.Add(tempTess);
            tabControl1.TabPages.Add(tempShape);
            Control[] controls = new Control[2];
            controls[0] = tempShape;
            controls[1] = tempTess;

            return controls;
        }

        /// <summary>
        /// Used to handle the Click event raised by newToolStripMenuItem when a user clicks the New Shape item in the main menu.
        /// </summary>
        /// <param name="sender">The menu item that was selected.</param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(newShape == null)
                newShape = new NewShapeForm();
            newShape.ShowDialog();
            // If the dialog returns DialogResult.Cancel, it means that the user hit the X at the top.
            // If the dialog returns a '-1' it means that no shape was selected and the user hit Create.
            if (newShape.DialogResult != DialogResult.Cancel && newShape.getShape() != -1)
            {
                Control[] controls = instantiateShapes(newShape.getShape(), newShape.getRectSize());
                EventSource.output("New tab number: " + shapes.Count + " was added.");
            }
        }

        /// <summary>
        /// Used to handle the MouseUp event raised by tabControl1 when a used selects a tab. Specifically a right click. 
        /// </summary>
        /// <param name="sender">The tab that was selected.</param>
        /// <param name="e">Event information about the mouse click.</param>
        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            // check if the right mouse button was pressed
            if (e.Button == MouseButtons.Right && !tabControl1.Controls.Contains(tabPage1))
            {
                // iterate through all the tab pages
                for (int i = 0; i < tabControl1.TabCount; i++)
                {
                    // get their rectangle area and check if it contains the mouse cursor
                    Rectangle r = tabControl1.GetTabRect(i);
                    if (r.Contains(e.Location))
                    {
                        Shape temp = (Shape)tabControl1.TabPages[i];
                        Point point = e.Location;
                        point.X += 10;
                        point.Y += 40;
                        temp.ContextMenu.Show(this, point, LeftRightAlignment.Right);
                    }
                }
            }
        }

        /// <summary>
        /// This is the handler for a tabRemove event.
        /// </summary>
        /// <param name="source">The name of the tab that was selected via the right click after Remove was selected from the menu.</param>
        /// <param name="e"></param>
        private void removeThisTab(object source, Events e)
        {
            // If the source was from the default tab, ignore.
            if(!source.Equals(tabPage1)){
                string name = (string)source;
                // Find the instance of the Shape that matches the name (a unique key).
                Shape shapeTemp = (Shape)tabControl1.Controls.Find(name, true)[0];
                Tessellation tessTemp = findTessellation(shapeTemp.Name);
                // Remove the Tessellation from the splitContainer's panel2.
                splitContainer1.Panel2.Controls.RemoveByKey(name);
                // Remove the Shape from the tabControl.
                tabControl1.Controls.RemoveByKey(name);
                // If the tabControl is now empty, add the default placeholder tab back and collapse panel2 which housed the tessellation.
                if(tabControl1.TabCount == 0){
                    tabControl1.Controls.Add(tabPage1);
                    splitContainer1.Panel2Collapsed = true;
                }
                // Remove both from their respective Lists and decrease the tab count for the Shape that is used in some situations to get the .
                tessellations.Remove(tessTemp);
                shapes.Remove(shapeTemp);
            }
        }

        /// <summary>
        /// Used to handle the FinishedDrawShape event when all of the logic + events have been performed after the collision simuilation has been performed.
        /// This is important so that the simulation can try to run before it fills in the textboxes with Undefined.
        /// </summary>
        /// <param name="source">Empty string.</param>
        /// <param name="e">Generic event.</param>
        private void OnPostShapeCollisions(object source, Events e)
        {
            // Per suggestion by Dr. Umble, if there was an issue calculating the collisions due to the tessellation's base lying directly over a vertex 
            // or if a corner has been hit, just filled in the textboxes with Undefined to convey this to the user.
            Shape selectedShape = (Shape)tabControl1.SelectedTab;
            if(selectedShape.undefCollision){
                angleBox.Text = "Undefined";
                bouncesBox.Text = "Undefined";
                pointBox.Text = "Undefined";
            }
        }

        /// <summary>
        /// Used to handle the FinishedDrawTessellation event when a Shape collision has been finished using the Tessellation.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void updateBounces(object source, Events e)
        {
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            if(!tempShape.undefCollision)
                bouncesBox.Text = "" + tempShape.getBounces();
        }

        /// <summary>
        /// Used to handle the Tessellate event when a Shape collision has been simulated and the resulting data is ready to be inserted into the 
        /// textboxes and used to set the scroll bars.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void updateFields(object source, Events e)
        {
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            Tessellation tessTemp = findTessellation(tempShape.Name);
            // If that tessellation reports that the tessellation should be used for the next collision simulation
            if (tessTemp.populateByTess)
            {
                double startingPoint = ((Tessellation)source).getStartingPoint();
                double startingAngle = ((Tessellation)source).getStartingAngle();
                double distance = ((Tessellation)source).getDistance();
                double tessHeight = ((Tessellation)source).getShapeHeight();

                // Set the Shape (tab) to report the same as well as set the proper fields to run the new simulation.
                tempShape.setFromTessellation(true);
                tempShape.setStartPoint(startingPoint);
                tempShape.setStartAngle(startingAngle);
                tempShape.setDistance(distance);
                tempShape.setTessShapeHeight(tessHeight);

                // Set the text boxes to reflect those changes
                angleBox.Text = "" + startingAngle;
                bouncesBox.Text = "" + 0;
                pointBox.Text = "" + startingPoint;
                // Set the track bars to reflect those changes
                int val = 0;
                if (tempShape.getStartAngle() > 0 && tempShape.getStartAngle() < 180)
                {
                    val = 180 - (int)tempShape.getStartAngle();
                    if (val > trackBar4.Minimum && val < trackBar4.Maximum)
                    {
                        trackBar4.Value = val;
                    }
                }
                if (tempShape.getStartPoint() > 0 && tempShape.getStartPoint() < 1){
                    val = (int)(tempShape.getStartPoint() * 100);
                    if (val > trackBar2.Minimum && val < trackBar2.Maximum)
                    {
                        trackBar2.Value = val;
                    }
                }
                // Causes redraw of shape (runs the simulation)
                tempShape.Invalidate();
            }
            // If the tessellation reports back that the baseClick was on a vertex, baseIsGood will be false
            if (!tessTemp.baseIsGood)
            {
                // If so, set the shape to report that there was an undefined collision.
                tempShape.undefCollision = true;
                // Trigger the FinishedDrawShape event to check fill the boxes in with Undefined.
                EventSource.finishedDrawShape();
            }
        }

        /// <summary>
        /// This is used only to process if the Enter (Return) key is hit as a shortcut to run the simulation.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                try
                {
                    if (shapes.Count() == 0)
                    {
                        return true;
                    }
                    else
                    {
                        // Get the current tab, set the fields, update the track bars, and invalidate the shape to run the simulation.
                        Shape tempShape = (Shape)this.tabControl1.SelectedTab;
                        Tessellation tessTemp = findTessellation(tempShape.Name);
                        tessTemp.populateByTess = false;

                        int tempBounces = int.Parse(bouncesBox.Text);
                        tempShape.setBounces(tempBounces);

                        double tempPoint = double.Parse(pointBox.Text);
                        tempShape.setStartPoint(tempPoint);
                        int val = (int)(tempPoint * 100);
                        if (val > trackBar2.Minimum && val < trackBar2.Maximum)
                            trackBar2.Value = val;

                        double tempAngle = double.Parse(angleBox.Text);
                        tempShape.setStartAngle(tempAngle);
                        val = (int)(180 - tempAngle);
                        if (val > trackBar4.Minimum && val < trackBar4.Maximum)
                            trackBar4.Value = val;

                        // tempShape.setFromTessellation(false) is important so that the current shape knows not to used the current tessellation data.
                        tempShape.setFromTessellation(false);
                        tempShape.Invalidate();
                    }
                }
                catch (System.ArgumentNullException ex){ /*Do nothing */ }
                catch (System.FormatException ex) { /*Do nothing */ }
                catch (System.OverflowException ex) { /*Do nothing */ }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Process if the Close item is selected from the main menu. (Not if the X button is hit in the window header)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This handles when the Starting Point textbox has new text entered into it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            try
            {
                Tessellation tessTemp = findTessellation(tempShape.Name);
                // If text is enter into this text box, then set the current tessellation that is active to not report that the collision simulation should be drawn from it.
                tessTemp.populateByTess = false;
                double temp = double.Parse(pointBox.Text);
                tempShape.setStartPoint(temp);

            }
            catch (System.ArgumentNullException ex) { /*Do nothing */ }
            catch (System.FormatException ex) { /*Do nothing */ }
            catch (System.OverflowException ex) { /*Do nothing */ }
        }

        /// <summary>
        /// This handles when the Angle textbox has new text entered into it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            try
            {
                Tessellation tessTemp = findTessellation(tempShape.Name);
                // If text is enter into this text box, then set the current tessellation that is active to not report that the collision simulation should be drawn from it.
                tessTemp.populateByTess = false;
                double temp = double.Parse(angleBox.Text);
                tempShape.setStartAngle(temp);
            }
            catch (System.ArgumentNullException ex) { /*Do nothing */ }
            catch (System.FormatException ex) { /*Do nothing */ }
            catch (System.OverflowException ex) { /*Do nothing */ }
        }

        /// <summary>
        /// This handles when the Bounces textbox has new text entered into it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            try
            {
                Tessellation tessTemp = findTessellation(tempShape.Name);
                // If text is enter into this text box, then set the current tessellation that is active to not report that the collision simulation should be drawn from it.
                tessTemp.populateByTess = false;
                int temp = int.Parse(bouncesBox.Text);
                tempShape.setBounces(temp);
            }
            catch (System.ArgumentNullException ex) { /*Do nothing */ }
            catch (System.FormatException ex) { /*Do nothing */ }
            catch (System.OverflowException ex) { /*Do nothing */ }
        }

        /// <summary>
        /// When the tall button in the middle is pressed, the behavior for it is to collapse or expand the tessellation user control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            Tessellation tessTemp = findTessellation(tempShape.Name);
            for (int i = 0; i < tessellations.Count; i++)
            {
                if (tessellations.ElementAt<Tessellation>(i).Visible)
                {
                    // If there are no tessellations, then there is nothing to collapse.
                    if (i == 0)
                    {
                        // The lastWidth field is used specifically for this purpose, it is a static member field that used to remember the last configured
                        // width of a tessellation for ALL tessellations to use when it is expanded again
                        Tessellation.lastWidth = tessTemp.Width;
                        this.Width -= tessTemp.Width + splitContainer1.SplitterWidth;
                        splitContainer1.Panel2Collapsed = true;
                    }
                }
                else
                {
                    // If there are no tessellations, then there is nothing to expand.
                    if (i == 0)
                    {
                        this.Width += Tessellation.lastWidth + splitContainer1.SplitterWidth;
                        splitContainer1.Panel2Collapsed = false;
                    }
                }
            }
        }

        /// <summary>
        /// Used whenever typically whenever you need to find the currently selected tab's corresponding tessellation.
        /// </summary>
        /// <param name="name">The Shape that </param>
        /// <returns></returns>
        private Tessellation findTessellation(String name)
        {
            for (int i = 0; i < tessellations.Count; i++)
            {
                if (tessellations.ElementAt<Tessellation>(i).Name.Equals(name))
                {
                    return tessellations.ElementAt<Tessellation>(i);
                }
            }
            EventSource.output("The tessellation could not be found with name: " + name);
            return null;
        }

        /// <summary>
        /// When the Go button is pressed, run the simulation from the input in the text boxes (which were already set as the fields for the currently selected shape).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (shapes.Count() == 0)
                    return;
                Shape tempShape = (Shape)this.tabControl1.SelectedTab;
                Tessellation tessTemp = findTessellation(tempShape.Name);
                // If this go button on the main form is pressed, then set the current tessellation that is active to not report that the collision simulation should be drawn from it.
                tessTemp.populateByTess = false;

                int tempBounces = int.Parse(bouncesBox.Text);
                tempShape.setBounces(tempBounces);

                double tempPoint = double.Parse(pointBox.Text);
                tempShape.setStartPoint(tempPoint);
                if(tempPoint > 0 && tempPoint < 1)
                    trackBar2.Value = (int)(tempPoint*100d);

                double tempAngle = double.Parse(angleBox.Text);
                tempShape.setStartAngle(tempAngle);
                if(tempAngle > 0 && tempAngle < 180d)
                    trackBar4.Value = (int)(180d - tempAngle);
                tempShape.setFromTessellation(false);
                tempShape.Invalidate();
            }
            catch (System.ArgumentNullException ex) { /*Do nothing */ }
            catch (System.FormatException ex) { /*Do nothing */ }
            catch (System.OverflowException ex) { /*Do nothing */ }
        }

        /// <summary>
        /// This handles when the track bar for the Angle is dragged around. Update the current shapes Angle field and invalidate it to restart the simulation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            findTessellation(((Shape)tabControl1.SelectedTab).Name).populateByTess = false;
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            // the track bar returns values from (int) 1-179, this converts it into a format that is fiendly to the Shapes StartAngle field.
            tempShape.setStartAngle(180d - trackBar4.Value);
            angleBox.Text = "" + (180d - trackBar4.Value);
            // When these track bars are moved, the simulation should not be run from data from the Tessellation
            tempShape.setFromTessellation(false);
            tempShape.Invalidate();
        }


        /// <summary>
        /// This handles when the track bar for the Starting Position is dragged around. Update the current shapes StartingPosition field and invalidate it to restart the simulation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            findTessellation(((Shape)tabControl1.SelectedTab).Name).populateByTess = false;
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            // The track bar returns values from (int) 1-99, this converts it into a format that is friend to the Shape's StartPoint field.
            tempShape.setStartPoint(trackBar2.Value / 100d);
            pointBox.Text = "" + (trackBar2.Value / 100d);
            // When these track bars are moved, the simulation should not be run from data from the Tessellation
            tempShape.setFromTessellation(false);
            tempShape.Invalidate();
        }

        /// <summary>
        /// This handles when a new tab is selected. It grabs all the data pertinent to the currently selected tab from the corresponding instances
        /// of Shape and Tessellations from their respective Lists, updates the textboxes/track bars and reruns the simulation (invalidates them).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_Selected_1(object sender, TabControlEventArgs e)
        {
            // If the default tab is not there and there is more than one Shape current initialized, perform a selected tab change.
            if (!tabControl1.Controls.Contains(tabPage1) && this.shapes.Count > 1)
            {
                Shape tab = (Shape)e.TabPage;
                EventSource.output(tab.Name + " tab was selected");
                angleBox.Text = "" + tab.getStartAngle();
                bouncesBox.Text = "" + tab.getBounces();
                pointBox.Text = "" + tab.getStartPoint();
                if (tab.getStartAngle() > 0 && tab.getStartAngle() < 180)
                    trackBar4.Value = (int)tab.getStartAngle();
                if (tab.getStartPoint() > 0 && tab.getStartPoint() < 1)
                    trackBar2.Value = (int)(tab.getStartPoint() * 100d);

                splitContainer1.Panel2.Controls.RemoveByKey(lastTab.Name);
                Tessellation tessTemp = findTessellation(tab.Name);
                splitContainer1.Panel2.Controls.Add(tessTemp);
                lastTab = (Shape)tabControl1.SelectedTab;
            }
        }

        /// <summary>
        /// When a new Control (Shape) is added to the form, and it is the only Shape currently instantiated, set up the selectTab to add the Tessellation
        /// to the split container correctly when more than one tab is created (when this adding behavior is handled by tabControl1_Selected_1),
        /// and do the first time insert into the split container. As well, set the splitter distance as it needs to be configured the first time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            if(!tabControl1.Controls.Contains(tabPage1) && this.shapes.Count == 1){
                Shape temp = (Shape)e.Control;
                Tessellation tessTemp = findTessellation(temp.Name);
                splitContainer1.Panel2.Controls.Add(tessTemp);
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.SplitterDistance = this.Width / 2;
                lastTab = (Shape)tabControl1.SelectedTab;
            }
        }

        /// <summary>
        /// This background worker is the Async thread that runs from MainForms constructor. It runs a PHP script on my server to check the latest records
        /// of release version in the MySQL DB. If the returned version is not the same in any way, it will prompt the user to download the latest version.
        /// If I (Josh Pavoncello) am no longer supporting this project (and that address is no longer valid), it will just swallow the resulting
        /// exception and carry on, so as to not pop up at the start of the program every time in the future.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.pragmaticparadigm.com/OrbitMapperGetLatest.php");
            HttpWebResponse webResponse = null;
            Stream responseStream = null;
            StreamReader response = null;
            try
            {
                // This goes to the address using a POST method, starts the script which checks the latest addition to the versions table in the DB
                // and returns that version string in a bit stream.
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                responseStream = webResponse.GetResponseStream();
                response = new StreamReader(responseStream);
                string tempVersion = response.ReadToEnd();
                if (OMVersion != tempVersion)
                {
                    EventSource.output("New version detected!");
                    EventSource.output("Current Version: " + OMVersion);
                    EventSource.output("Latest Version: " + tempVersion);
                    // Report to the user that a new version is available.
                    new Version().ShowDialog();
                }
                else
                {
                    EventSource.output("No new version detected!");
                    EventSource.output("Current Version: " + OMVersion);
                    EventSource.output("Latest Version: " + tempVersion);
                }
            }
            catch (WebException ex) { /* Ignore the exception, failure communication with server */ }
            // Run each time, if the streams/connection are not null, make sure they are ended.
            finally
            {
                if(webResponse != null)
                    webResponse.Close();
                if(responseStream != null)
                    responseStream.Close();
                if(response != null)
                    response.Close();
            }
        }


        /// <summary>
        /// If the Save button is pressed from the main menu, saved the current shape data/tessellation data to a user specified file.
        /// The data is stored in an XML format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.Controls.Contains(tabPage1))
            {
                MessageBox.Show(this, "There is no data to save!", "Save Error");
                return;
            }
            try
            {
               SaveFileDialog saveFileDialog = new SaveFileDialog();
               // Sets the filter for the file dialog to only show files that end in .omd and display this on the bottom of the dialog.
               saveFileDialog.Filter = "Orbit Mapper Data (*.omd)|*.omd";
               saveFileDialog.Title = "Save Orbit Mapper";
               saveFileDialog.ShowDialog();

               // If the file name is not an empty string open it for saving.
               if (saveFileDialog.FileName != "")
               {
                  // Saves the .omd via a FileStream created by the OpenFile method.
                  System.IO.FileStream fs =
                     (System.IO.FileStream)saveFileDialog.OpenFile();

                  if(saveFileDialog.FilterIndex == 1){
                        string shapeData = @"<?xml version=""1.0""?>" + System.Environment.NewLine + "<shapes>" + System.Environment.NewLine;
                        for (int i = 0; i < shapes.Count; i++)
                        {
                            // Aggregate all of the data necessary to reproduce the current state of OrbitMapper in a new instance.
                            // This means StartPosition, StartAngle, Bounces, baseClick (X,Y), endClick (X,Y), the ratio field used for rectangles only, 
                            // the text of the field WHICH IS VERY IMPORTANT as it is used to determine what the shape is to instantiate later, and the name of the field
                            if(tabControl1.Controls.Contains(shapes.ElementAt<Shape>(i))){
                                shapeData += @"<shape type=""" + shapes.ElementAt<Shape>(i).Text + @""">";
                                shapeData += shapes.ElementAt<Shape>(i).getShapeData() + System.Environment.NewLine;
                                shapeData += tessellations.ElementAt<Tessellation>(i).getTessData() + System.Environment.NewLine;
                                shapeData += "</shape>";
                            }
                        }
                        shapeData += "</shapes>";
                        if(shapes.Count != 0){
                            byte[] shapeBytes = System.Text.Encoding.UTF8.GetBytes(shapeData);
                            fs.Write(shapeBytes, 0, shapeBytes.Length);
                        }
                  }
                  fs.Close();
               }
            }
            catch (Exception ex)
            {
                EventSource.output(ex.StackTrace);
                MessageBox.Show(this, "Problem saving file.", "Save Error");
            }
        }

        /// <summary>
        /// This handles when the Open item is selected in the main menu. It houses all the logic to parse .omd XML files and begin the object
        /// instantiation. If there is any exception thrown during the file open, it will simply pop up a dialog and say that there was a problem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try{
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Orbit Mapper Data (*.omd)|*.omd";
                openFileDialog.Title = "Open Orbit Mapper";
                openFileDialog.ShowDialog();

                TabPage first = null;
                // If the file name is not an empty string open it for saving.
                if (openFileDialog.FileName != "" && openFileDialog.CheckFileExists && openFileDialog.CheckPathExists)
                {
                    // Saves the .omd via a FileStream created by the OpenFile method.
                    System.IO.Stream fs = openFileDialog.OpenFile();
                    XmlDocument xmlDoc = new XmlDocument(); // create an xml document object.
                    xmlDoc.Load(fs); // Use it to load the IOStream from the OpenFile dialog
                    XmlNodeList shapes = xmlDoc.GetElementsByTagName("shape"); // Aggregate all of the individual saved Shape to iterate through
                    
                    for(int i = 0; i < shapes.Count; i++){
                        // XmlNodeList provides an nice way to index through children where the first index points to the individual highest level element you would like to search through
                        // and the second can be used like a Dictionary where it will search all children under that tag for any elements with the specified tag name
                        XmlElement point = shapes[i]["startpoint"];
                        XmlElement angle = shapes[i]["startangle"];
                        XmlElement bounces = shapes[i]["bounces"];
                        XmlElement baseClickX = shapes[i]["baseclick_x"];
                        XmlElement baseClickY = shapes[i]["baseclick_y"];
                        XmlElement endClickX = shapes[i]["endclick_x"];
                        XmlElement endClickY = shapes[i]["endclick_y"];
                        XmlElement populateByTess = shapes[i]["populateByTess"];
                        XmlNode attr = null;
                        if((attr = shapes[i].Attributes.GetNamedItem("type")) != null){

                            Control[] controls = null;
                            string type = attr.Value;
                            if (tabControl1.TabPages.Contains(tabPage1))
                            {
                                tabControl1.TabPages.Remove(tabPage1);
                            }
                            Shape shape = null;
                            Tessellation tess = null;
                            if(type == "Equilateral"){
                                controls = instantiateShapes(0);
                            }
                            else if (type == "IsosTri90")
                            {
                                controls = instantiateShapes(1);
                            }
                            else if (type == "IsosTri120")
                            {
                                controls = instantiateShapes(2);
                            }
                            else if (type == "Tri3060")
                            {
                                controls = instantiateShapes(3);
                            }
                            else if (type == "Hexagon")
                            {
                                controls = instantiateShapes(4);
                            }
                            else if (type == "Rhombus")
                            {
                                controls = instantiateShapes(5);
                            }
                            else if (type == "Kite")
                            {
                                controls = instantiateShapes(6);
                            }
                            else if (type == "Rectangle")
                            {
                                // Remember that Rectangle shapes need the extra property for the ratio saved so that they can be instantiated again.
                                XmlElement ratio = shapes[i]["ratio"];
                                controls = instantiateShapes(7, double.Parse(ratio.InnerText));
                            }
                            shape = (Shape)controls[0];
                            tess = (Tessellation)controls[1];
                            double startingPoint = double.Parse(point.InnerText);
                            double startingAngle = double.Parse(angle.InnerText);
                            int startingBounces = int.Parse(bounces.InnerText);
                            shape.setStartPoint(startingPoint);
                            shape.setStartAngle(startingAngle);
                            shape.setBounces(startingBounces);
                            tabControl1.SelectTab(shape);
                            tess.setBasePos(new Point(int.Parse(baseClickX.InnerText), tess.Height - int.Parse(baseClickY.InnerText)));
                            tess.setEndPos(new Point(int.Parse(endClickX.InnerText), tess.Height - int.Parse(endClickY.InnerText)));

                            angleBox.Text = "" + startingAngle;
                            bouncesBox.Text = "" + startingBounces;
                            pointBox.Text = "" + startingPoint;
                            int val = 0;
                            if (shape.getStartAngle() > 0 && shape.getStartAngle() < 180)
                            {
                                val = 180 - (int)shape.getStartAngle();
                                if (val > trackBar4.Minimum && val < trackBar4.Maximum)
                                {
                                    trackBar4.Value = val;
                                }
                            }
                            if (shape.getStartPoint() > 0 && shape.getStartPoint() < 1)
                            {
                                val = (int)(shape.getStartPoint() * 100);
                                if (val > trackBar2.Minimum && val < trackBar2.Maximum)
                                {
                                    trackBar2.Value = val;
                                }
                            }
                            // Used to set the tab index to the first one opened
                            if (i == 0)
                                first = shape;
                            EventSource.output("New tab number: " + this.shapes.Count + " was added.");
                        }
                    }
                }
                if(first != null)
                    tabControl1.SelectTab(first);
            }
            catch (Exception ex)
            {
                EventSource.output(ex.StackTrace);
                MessageBox.Show(this, "Problem opening file.", "Open Error");
            }
        }

        /// <summary>
        /// Only used when user clicks the About button on the main menu, open the dialog and persist user closes before continue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About(OMVersion).ShowDialog();
        }

        /// <summary>
        /// This draws the custom stripes down the middle of the button that is pressed to show and hide the Tessellation map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Paint(object sender, PaintEventArgs e)
        {
            int size = button1.Height / 8; // Set the vertical size of the stripes
            int offsetX = button1.Width / 2; // Find the center horizontally
            int offsetY = (button1.Height / 2) - (size / 2); // Find the center vertically and set the topmost edge of the stripes
            int vertices = 2;
            Point[] go = new Point[vertices];
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            go[0] = new Point(offsetX - 2, offsetY);
            go[1] = new Point(offsetX - 2, offsetY + size);
            g.DrawLine(System.Drawing.Pens.DimGray, go[0], go[1]);

            go[0] = new Point(offsetX, offsetY);
            go[1] = new Point(offsetX, offsetY + size);
            g.DrawLine(System.Drawing.Pens.DimGray, go[0], go[1]);

            go[0] = new Point(offsetX + 2, offsetY);
            go[1] = new Point(offsetX + 2, offsetY + size);
            g.DrawLine(System.Drawing.Pens.DimGray, go[0], go[1]);
        }

        /// <summary>
        /// This draws the custom triangle over the "Go" button, or the button pressed to initiate the simulation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Paint(object sender, PaintEventArgs e)
        {
            // Find the center
            int width = button2.Width / 2;
            int height = width * 2;
            // Find the top left corner of the triangle to start it centered + 1
            int offsetX = (button2.Width / 2) - (width / 2) + 1;
            int offsetY = (button2.Height / 2) - (height / 2);
            int vertices = 3;
            Point[] go = new Point[vertices];
            go[0] = new Point(offsetX, offsetY);
            go[1] = new Point(offsetX, offsetY + height);
            go[2] = new Point(offsetX + width, offsetY + (height / 2));

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPolygon(new SolidBrush(Color.Teal), go);
        }

        /// <summary>
        /// This draws the custom stripe over the split containers vertical splitter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainer1_Paint(object sender, PaintEventArgs e)
        {
            int offsetX = splitContainer1.SplitterDistance;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(System.Drawing.Brushes.DimGray, 4);
            g.DrawLine(pen, new Point(offsetX, 0), new Point(offsetX, splitContainer1.Height));
        }
    }
}