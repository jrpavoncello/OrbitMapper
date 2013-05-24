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
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.IO;
using System.Xml;

namespace OrbitMapper
{
    public partial class MainForms : Form
    {
        
        private const string OMVersion = "1.0.1";

        private NewShapeForm newShape;
        private List<Shape> shapes = new List<Shape>();
        private List<Tessellation> tessellations = new List<Tessellation>();
        private Shape lastTab;
        private Point Position { get; set; }
        
        public MainForms()
        {
            InitializeComponent();
            EventSource.tessellate += new Tessellate(updateFields);
            EventSource.finishedTessellate += new FinishedDrawTess(updateBounces);
            EventSource.finishedShape += new FinishedDrawShape(OnPostShapeCollisions);
            EventSource.tabRemove += new RemoveTab(removeThisTab);

            //To address a bug with the table layout in Windows Vista where the text field and labels in the layout builds incorrectly.
            //I check if they are placed incorrectly, if so, then I place them in the intended positions.
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

        private Control[] instantiateShapes(int shape)
        {
            if (tabControl1.TabPages.Contains(tabPage1))
            {
                tabControl1.TabPages.Remove(tabPage1);
            }
            Shape tempShape = null;
            Tessellation tempTess = null;
            switch (shape)
            {
                case 0:
                    if (shapes.Count() != 0)
                        lastTab = (Shape)tabControl1.SelectedTab;
                    tempShape = new Equilateral();
                    shapes.Add(tempShape);
                    EventSource.output("Equilateral Triangle tab created.");
                    tempTess = new EquilateralTess();
                    tempTess.Name = "Equilateral" + (tempShape.getShapeCount() - 1);
                    tempTess.Dock = DockStyle.Fill;
                    tessellations.Add(tempTess);
                    tabControl1.TabPages.Add(shapes.ElementAt<Shape>(shapes.Count - 1));
                    tabControl1.Controls.Find(tempShape.Name, true)[0].ContextMenu = tempShape.cm;
                    break;
                case 1:
                    if (shapes.Count() != 0)
                        lastTab = (Shape)tabControl1.SelectedTab;
                    tempShape = new IsosTri90();
                    shapes.Add(tempShape);
                    EventSource.output("90 Isosceles Triangle tab created.");
                    tempTess = new IsosTri90Tess();
                    tempTess.Name = "IsosTri90" + (tempShape.getShapeCount() - 1);
                    tempTess.Dock = DockStyle.Fill;
                    tessellations.Add(tempTess);
                    tabControl1.TabPages.Add(shapes.ElementAt<Shape>(shapes.Count - 1));
                    tabControl1.Controls.Find(tempShape.Name, true)[0].ContextMenu = tempShape.cm;
                    break;
                case 2:
                    if (shapes.Count() != 0)
                        lastTab = (Shape)tabControl1.SelectedTab;
                    tempShape = new IsosTri120();
                    shapes.Add(tempShape);
                    EventSource.output("120 Isosceles Triangle tab created.");
                    tempTess = new IsosTri120Tess();
                    tempTess.Name = "IsosTri120" + (tempShape.getShapeCount() - 1);
                    tempTess.Dock = DockStyle.Fill;
                    tessellations.Add(tempTess);
                    tabControl1.TabPages.Add(shapes.ElementAt<Shape>(shapes.Count - 1));
                    tabControl1.Controls.Find(tempShape.Name, true)[0].ContextMenu = tempShape.cm;
                    break;
                case 3:
                    if (shapes.Count() != 0)
                        lastTab = (Shape)tabControl1.SelectedTab;
                    tempShape = new Tri3060();
                    shapes.Add(tempShape);
                    EventSource.output("30-60-90 Triangle tab created.");
                    tempTess = new Tri3060Tess();
                    tempTess.Name = "Tri3060" + (tempShape.getShapeCount() - 1);
                    tempTess.Dock = DockStyle.Fill;
                    tessellations.Add(tempTess);
                    tabControl1.TabPages.Add(shapes.ElementAt<Shape>(shapes.Count - 1));
                    tabControl1.Controls.Find(tempShape.Name, true)[0].ContextMenu = tempShape.cm;
                    break;
                case 4:
                    if (shapes.Count() != 0)
                        lastTab = (Shape)tabControl1.SelectedTab;
                    tempShape = new Hexagon();
                    shapes.Add(tempShape);
                    EventSource.output("120 Hexagon tab created.");
                    tempTess = new HexagonTess();
                    tempTess.Name = "Hexagon" + (tempShape.getShapeCount() - 1);
                    tempTess.Dock = DockStyle.Fill;
                    tessellations.Add(tempTess);
                    tabControl1.TabPages.Add(shapes.ElementAt<Shape>(shapes.Count - 1));
                    tabControl1.Controls.Find(tempShape.Name, true)[0].ContextMenu = tempShape.cm;
                    break;
                case 5:
                    if (shapes.Count() != 0)
                        lastTab = (Shape)tabControl1.SelectedTab;
                    tempShape = new Rhombus();
                    shapes.Add(tempShape);
                    EventSource.output("60-120 Rhombus tab created.");
                    tempTess = new RhombusTess();
                    tempTess.Name = "Rhombus" + (tempShape.getShapeCount() - 1);
                    tempTess.Dock = DockStyle.Fill;
                    tessellations.Add(tempTess);
                    tabControl1.TabPages.Add(shapes.ElementAt<Shape>(shapes.Count - 1));
                    tabControl1.Controls.Find(tempShape.Name, true)[0].ContextMenu = tempShape.cm;
                    break;
                case 6:
                    if (shapes.Count() != 0)
                        lastTab = (Shape)tabControl1.SelectedTab;
                    tempShape = new Kite();
                    shapes.Add(tempShape);
                    EventSource.output("60-90-120 Kite tab created.");
                    tempTess = new KiteTess();
                    tempTess.Name = "Kite" + (tempShape.getShapeCount() - 1);
                    tempTess.Dock = DockStyle.Fill;
                    tessellations.Add(tempTess);
                    tabControl1.TabPages.Add(shapes.ElementAt<Shape>(shapes.Count - 1));
                    tabControl1.Controls.Find(tempShape.Name, true)[0].ContextMenu = tempShape.cm;
                    break;
                default:
                    break;
            }
            Control[] controls = new Control[2];
            controls[0] = tempShape;
            controls[1] = tempTess;

            return controls;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(newShape == null)
                newShape = new NewShapeForm();
            newShape.ShowDialog();
            if (newShape.DialogResult != DialogResult.Cancel && newShape.getShape() != -1)
            {
                Control[] controls = instantiateShapes(newShape.getShape());
                EventSource.output("New tab number: " + shapes.Count + " was added.");
            }
        }

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

        private void removeThisTab(object source, Events e)
        {
            if(!source.Equals(tabPage1)){
                string name = (string)source;
                Shape shapeTemp = (Shape)tabControl1.Controls.Find(name, true)[0];
                Tessellation tessTemp = null;
                for (int i = 0; i < tessellations.Count; i++)
                {
                    if (tessellations.ElementAt<Tessellation>(i).Name.Equals(name))
                    {
                        tessTemp = tessellations.ElementAt<Tessellation>(i);
                        break;
                    }
                }
                if (tessTemp == null)
                {
                    EventSource.output("The tessellation could not be found with name: " + name);
                    return;
                }
                splitContainer1.Panel2.Controls.RemoveByKey(name);
                tabControl1.Controls.RemoveByKey(name);
                if(tabControl1.TabCount == 0){
                    tabControl1.Controls.Add(tabPage1);
                }
                tessellations.Remove(tessTemp);
                tessTemp.decTabCount();
                shapes.Remove(shapeTemp);
                shapeTemp.decTabCount();
            }
        }

        private void OnPostShapeCollisions(object source, Events e)
        {
            Shape selectedShape = (Shape)tabControl1.SelectedTab;
            if(selectedShape.undefCollision){
                angleBox.Text = "Undefined";
                bouncesBox.Text = "Undefined";
                pointBox.Text = "Undefined";
            }
        }

        private void updateBounces(object source, Events e)
        {
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            if(!tempShape.undefCollision)
                bouncesBox.Text = "" + tempShape.getBounces();
        }

        private void updateFields(object source, Events e)
        {
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            Tessellation tessTemp = null;
            for (int i = 0; i < tessellations.Count; i++)
            {
                if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tempShape.Name))
                {
                    tessTemp = tessellations.ElementAt<Tessellation>(i);
                    break;
                }
            }
            if (tessTemp == null)
            {
                EventSource.output("The tessellation could not be found with name: " + tempShape.Name);
                return;
            }
            if (tessTemp.populateByTess)
            {
                double startingPoint = ((Tessellation)source).getStartingPoint();
                double startingAngle = ((Tessellation)source).getStartingAngle();
                double distance = ((Tessellation)source).getDistance();
                double tessHeight = ((Tessellation)source).getShapeHeight();

                tempShape.setFromTessellation(true);
                tempShape.setStartPoint(startingPoint);
                tempShape.setStartAngle(startingAngle);
                tempShape.setDistance(distance);
                tempShape.setTessShapeHeight(tessHeight);

                angleBox.Text = "" + startingAngle;
                bouncesBox.Text = "" + 0;
                pointBox.Text = "" + startingPoint;
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
                tempShape.Invalidate();
            }
            if (!tessTemp.baseIsGood)
            {
                tempShape.undefCollision = true;
                EventSource.finishedDrawShape();
            }
        }

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
                        Shape tempShape = (Shape)this.tabControl1.SelectedTab;
                        Tessellation tessTemp = null;
                        for (int i = 0; i < tessellations.Count; i++)
                        {
                            if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tempShape.Name))
                            {
                                tessTemp = tessellations.ElementAt<Tessellation>(i);
                                break;
                            }
                        }
                        if (tessTemp == null)
                        {
                            EventSource.output("The tessellation could not be found with name: " + tempShape.Name);
                            return true;
                        }
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

                        tempShape.setFromTessellation(false);
                        tempShape.Invalidate();
                    }
                }
                catch (System.ArgumentNullException ane)
                {
                    EventSource.output("Message: " + ane.Message + " Source: " + ane.Source);
                }
                catch (System.FormatException fe)
                {
                    EventSource.output("Message: " + fe.Message + " Source: " + fe.Source);
                }
                catch (System.OverflowException oe)
                {
                    EventSource.output("Message: " + oe.Message + " Source: " + oe.Source);
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Starting Point
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            try
            {
                Tessellation tessTemp = null;
                for (int i = 0; i < tessellations.Count; i++)
                {
                    if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tempShape.Name))
                    {
                        tessTemp = tessellations.ElementAt<Tessellation>(i);
                        break;
                    }
                }
                if (tessTemp == null)
                {
                    EventSource.output("The tessellation could not be found with name: " + tempShape.Name);
                    return;
                }
                tessTemp.populateByTess = false;
                double temp = double.Parse(pointBox.Text);
                tempShape.setStartPoint(temp);

            }
            catch(System.ArgumentNullException ane){
                EventSource.output("Message: " + ane.Message + " Source: " + ane.Source);
                tempShape.setStartPoint(0);
            }
            catch (System.FormatException fe)
            {
                EventSource.output("Message: " + fe.Message + " Source: " + fe.Source);
                tempShape.setStartPoint(0);
            }
            catch (System.OverflowException oe)
            {
                EventSource.output("Message: " + oe.Message + " Source: " + oe.Source);
                tempShape.setStartPoint(0);
            }
        }

        //Starting Angle
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            try
            {
                Tessellation tessTemp = null;
                for (int i = 0; i < tessellations.Count; i++)
                {
                    if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tempShape.Name))
                    {
                        tessTemp = tessellations.ElementAt<Tessellation>(i);
                        break;
                    }
                }
                if (tessTemp == null)
                {
                    EventSource.output("The tessellation could not be found with name: " + tempShape.Name);
                    return;
                }
                tessTemp.populateByTess = false;
                double temp = double.Parse(angleBox.Text);
                tempShape.setStartAngle(temp);
            }
            catch (System.ArgumentNullException ane)
            {
                EventSource.output("Message: " + ane.Message + " Source: " + ane.Source);
                tempShape.setStartAngle(0);
            }
            catch (System.FormatException fe)
            {
                EventSource.output("Message: " + fe.Message + " Source: " + fe.Source);
                tempShape.setStartAngle(0);
            }
            catch (System.OverflowException oe)
            {
                EventSource.output("Message: " + oe.Message + " Source: " + oe.Source);
                tempShape.setStartAngle(0);
            }
        }

        //Bounces
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            try
            {
                Tessellation tessTemp = null;
                for (int i = 0; i < tessellations.Count; i++)
                {
                    if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tempShape.Name))
                    {
                        tessTemp = tessellations.ElementAt<Tessellation>(i);
                        break;
                    }
                }
                if (tessTemp == null)
                {
                    EventSource.output("The tessellation could not be found with name: " + tempShape.Name);
                    return;
                }
                tessTemp.populateByTess = false;
                int temp = int.Parse(bouncesBox.Text);
                tempShape.setBounces(temp);
            }
            catch (System.ArgumentNullException ane)
            {
                EventSource.output("Message: " + ane.Message + " Source: " + ane.Source);
                tempShape.setBounces(0);
            }
            catch (System.FormatException fe)
            {
                EventSource.output("Message: " + fe.Message + " Source: " + fe.Source);
                tempShape.setBounces(0);
            }
            catch (System.OverflowException oe)
            {
                EventSource.output("Message: " + oe.Message + " Source: " + oe.Source);
                tempShape.setBounces(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            Shape tempShape = (Shape)tabControl1.SelectedTab;
            Tessellation tessTemp = null;
            for (int i = 0; i < tessellations.Count; i++)
            {
                if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tempShape.Name))
                {
                    tessTemp = tessellations.ElementAt<Tessellation>(i);
                    break;
                }
            }
            if (tessTemp == null)
            {
                EventSource.output("The tessellation could not be found with name: " + tempShape.Name);
                return;
            }
            tessTemp.populateByTess = false;
            for (int i = 0; i < tessellations.Count; i++)
            {
                if (tessellations.ElementAt<Tessellation>(i).Visible)
                {
                    if (i == 0)
                    {
                        Tessellation.lastWidth = tessTemp.Width;
                        this.Width -= tessTemp.Width + splitContainer1.SplitterWidth;
                        splitContainer1.Panel2Collapsed = true;
                        tessellations.ElementAt<Tessellation>(i).Visible = false;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        splitContainer1.Panel2Collapsed = false;
                        this.Width += Tessellation.lastWidth + splitContainer1.SplitterWidth;
                        tessellations.ElementAt<Tessellation>(i).Visible = true;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (shapes.Count() == 0)
                    return;
                Shape tempShape = (Shape)this.tabControl1.SelectedTab;
                Tessellation tessTemp = null;
                for (int i = 0; i < tessellations.Count; i++)
                {
                    if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tempShape.Name))
                    {
                        tessTemp = tessellations.ElementAt<Tessellation>(i);
                        break;
                    }
                }
                if (tessTemp == null)
                {
                    EventSource.output("The tessellation could not be found with name: " + tempShape.Name);
                    return;
                }
                tessTemp.populateByTess = false;

                int tempBounces = int.Parse(bouncesBox.Text);
                tempShape.setBounces(tempBounces);

                double tempPoint = double.Parse(pointBox.Text);
                tempShape.setStartPoint(tempPoint);
                if(tempPoint > 0 && tempPoint < 1)
                    trackBar2.Value = (int)(tempPoint*100);

                double tempAngle = double.Parse(angleBox.Text);
                tempShape.setStartAngle(tempAngle);
                if(tempAngle > 0 && tempAngle < 180)
                    trackBar4.Value = (int)(180 - tempAngle);
                tempShape.setFromTessellation(false);
                tempShape.Invalidate();
            }
            catch (System.ArgumentNullException ane)
            {
                EventSource.output("Message: " + ane.Message + " Source: " + ane.Source);
            }
            catch (System.FormatException fe)
            {
                EventSource.output("Message: " + fe.Message + " Source: " + fe.Source);
            }
            catch (System.OverflowException oe)
            {
                EventSource.output("Message: " + oe.Message + " Source: " + oe.Source);
            }
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            tessellations.ElementAt<Tessellation>(((Shape)tabControl1.SelectedTab).getTabNum()).populateByTess = false;
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            tempShape.setStartAngle(180 - trackBar4.Value);
            angleBox.Text = "" + (180 - trackBar4.Value);
            tempShape.setFromTessellation(false);
            tempShape.Invalidate();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (shapes.Count() == 0)
                return;
            tessellations.ElementAt<Tessellation>(((Shape)tabControl1.SelectedTab).getTabNum()).populateByTess = false;
            Shape tempShape = (Shape)this.tabControl1.SelectedTab;
            tempShape.setStartPoint(trackBar2.Value / 100d);
            pointBox.Text = "" + (trackBar2.Value / 100d);
            tempShape.setFromTessellation(false);
            tempShape.Invalidate();
        }

        private void tabControl1_Selected_1(object sender, TabControlEventArgs e)
        {
            if (!tabControl1.Controls.Contains(tabPage1))
            {
                Shape tab = (Shape)e.TabPage;
                if (tab != null)
                {
                    EventSource.output(tab.Name + " tab was selected");
                    angleBox.Text = "" + tab.getStartAngle();
                    bouncesBox.Text = "" + tab.getBounces();
                    pointBox.Text = "" + tab.getStartPoint();
                    if (tab.getStartAngle() > 0 && tab.getStartAngle() < 180)
                        trackBar4.Value = (int)tab.getStartAngle();
                    if (tab.getStartPoint() > 0 && tab.getStartPoint() < 1)
                        trackBar2.Value = (int)(tab.getStartPoint() * 100);

                    if(this.shapes.Count > 1 && !((Shape)tabControl1.SelectedTab).Equals(lastTab)){
                        splitContainer1.Panel2.Controls.RemoveByKey(lastTab.Name);
                        Tessellation tessTemp = null;
                        for (int i = 0; i < tessellations.Count; i++)
                        {
                            if (tessellations.ElementAt<Tessellation>(i).Name.Equals(tab.Name))
                            {
                                tessTemp = tessellations.ElementAt<Tessellation>(i);
                                break;
                            }
                        }
                        if (tessTemp == null)
                        {
                            EventSource.output("The tessellation could not be found with name: " + tab.Name);
                            return;
                        }
                        splitContainer1.Panel2.Controls.Add(tessTemp);
                        lastTab = (Shape)tabControl1.SelectedTab;
                    }
                }
            }
        }

        private void tabControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            if(!tabControl1.Controls.Contains(tabPage1)){
                if (this.shapes.Count == 1)
                {
                    Shape temp = (Shape)e.Control;
                    Tessellation tessTemp = null;
                    for (int i = 0; i < tessellations.Count; i++)
                    {
                        if (tessellations.ElementAt<Tessellation>(i).Name.Equals(temp.Name))
                        {
                            tessTemp = tessellations.ElementAt<Tessellation>(i);
                            break;
                        }
                    }
                    if (tessTemp == null)
                    {
                        EventSource.output("The tessellation could not be found with name: " + temp.Name);
                        return;
                    }
                    splitContainer1.Panel2.Controls.Add(tessTemp);
                    splitContainer1.Panel2Collapsed = false;
                    splitContainer1.SplitterDistance = this.Width / 2;
                    lastTab = (Shape)tabControl1.SelectedTab;
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.pragmaticparadigm.com/OrbitMapperGetLatest.php");
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream responseStream = webResponse.GetResponseStream();
                StreamReader response = new StreamReader(responseStream);
                string tempVersion = response.ReadToEnd();
                if (OMVersion != tempVersion)
                {
                    EventSource.output("New version detected!");
                    EventSource.output("Current Version: " + OMVersion);
                    EventSource.output("Latest Version: " + tempVersion);
                    new VersionBox1().ShowDialog();
                }
                else
                {
                    EventSource.output("No new version detected!");
                    EventSource.output("Current Version: " + OMVersion);
                    EventSource.output("Latest Version: " + tempVersion);
                }
                webResponse.Close();
                responseStream.Close();
                response.Close();
            }
            catch (WebException ex) { /* Ignore the exception, failure communication with server */ }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try{
               SaveFileDialog saveFileDialog = new SaveFileDialog();
               saveFileDialog.Filter = "Orbit Mapper Data (*.omd)|*.omd";
               saveFileDialog.Title = "Save Orbit Mapper";
               saveFileDialog.ShowDialog();

               // If the file name is not an empty string open it for saving.
               if (saveFileDialog.FileName != "")
               {
                  // Saves the .omd via a FileStream created by the OpenFile method.
                  System.IO.FileStream fs =
                     (System.IO.FileStream)saveFileDialog.OpenFile();

                  switch (saveFileDialog.FilterIndex)
                  {
                     case 1 :
                          string shapeData = @"<?xml version=""1.0""?>" + System.Environment.NewLine + "<shapes>" + System.Environment.NewLine;
                        for (int i = 0; i < shapes.Count; i++)
                        {
                            if(tabControl1.Controls.Contains(shapes.ElementAt<Shape>(i))){
                                shapeData += @"<shape type=""" + shapes.ElementAt<Shape>(i).Text + @""" name=""" + shapes.ElementAt<Shape>(i).Name + @""">";
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
                     break;

                     default:
                     break;
                  }
                  fs.Close();
               }
            }
            catch (Exception saveE)
            {
                EventSource.output(saveE.StackTrace);
                EventSource.output(saveE.Source);
                MessageBox.Show(this, "Problem saving file.");
            }
        }

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
                    XmlDocument xmlDoc= new XmlDocument(); //* create an xml document object.
                    xmlDoc.Load(fs);
                    XmlNodeList shapes = xmlDoc.GetElementsByTagName("shape");
                    
                    for(int i = 0; i < shapes.Count; i++){
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
                            tess.populateByTess = bool.Parse(populateByTess.InnerText);

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
                            if (i == 0)
                                first = shape;
                            EventSource.output("New tab number: " + this.shapes.Count + " was added.");
                        }
                    }
                }
                if(first != null)
                    tabControl1.SelectTab(first);
            }
            catch (Exception openE)
            {
                EventSource.output(openE.StackTrace);
                EventSource.output(openE.Source);
                MessageBox.Show(this, "Problem opening file.");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About(OMVersion).ShowDialog();
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            int size = button1.Height / 8;
            int offsetX = button1.Width / 2;
            int offsetY = (button1.Height / 2) - (size / 2);
            int vertices = 2;
            Point[] go = new Point[vertices];
            Graphics g = e.Graphics;
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

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            int width = button2.Width / 2;
            int height = width * 2;
            int offsetX = (button2.Width / 2) - (width / 2) + 1;
            int offsetY = (button2.Height / 2) - (height / 2);
            int vertices = 3;
            Point[] go = new Point[vertices];
            go[0] = new Point(offsetX, offsetY);
            go[1] = new Point(offsetX, offsetY + height);
            go[2] = new Point(offsetX + width, offsetY + (height / 2));

            Graphics g = e.Graphics;
            g.FillPolygon(new SolidBrush(Color.Teal), go);
        }

        private void splitContainer1_Paint(object sender, PaintEventArgs e)
        {
            int offsetX = splitContainer1.SplitterDistance;
            Graphics g = e.Graphics;
            Pen pen = new Pen(System.Drawing.Brushes.DimGray, 4);
            g.DrawLine(pen, new Point(offsetX, 0), new Point(offsetX, splitContainer1.Height));
        }
    }
}