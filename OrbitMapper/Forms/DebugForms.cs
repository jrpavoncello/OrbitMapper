using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OrbitMapper
{
    /// <summary>
    /// This pops up on a crash or if a developer wants to debug. 
    /// If a developer needs to see debug information, set the static bool logRealtime to true.
    /// </summary>
    public partial class DebugForms : Form
    {
        public static bool logRealtime = false;

        public DebugForms()
        {
            InitializeComponent();
            /// Subscribes to the textChanged event
            EventSource.textChanged += new TextChanged(updateText);
            this.textBox1.Text = EventSource.getText();
        }

        public void updateText(object source, Events e){
            if(logRealtime)
                this.textBox1.Text = EventSource.getText();
        }

        private void textBox1_VisibleChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = EventSource.getText();
        }
    }
}
