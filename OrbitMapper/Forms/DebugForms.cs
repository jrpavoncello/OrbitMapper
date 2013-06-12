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
        /// <summary>
        /// Set this to true when you want the debug menu to be visible
        /// </summary>
        public static bool logRealtime = false;

        /// <summary>
        /// TODO
        /// </summary>
        public DebugForms()
        {
            InitializeComponent();
            // Subscribes to the textChanged event
            EventSource.textChanged += new TextChanged(updateText);
            this.textBox1.Text = EventSource.getText();
        }

        /// <summary>
        /// Only update the text if logRealtime is true, otherwise it will be updated only when the program crashes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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
