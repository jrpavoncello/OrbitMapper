using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OrbitMapper
{
    /// <summary>
    /// Displays information about the program. Hangs from the main menu toolbar
    /// </summary>
    partial class About : Form
    {
        /// <summary>
        /// </summary>
        /// <param name="version">Used to fill in the version label in the about form.</param>
        public About(string version)
        {
            InitializeComponent();
            this.Text = "About";
            this.labelProductName.Text = "Orbit Mapper";
            this.labelVersion.Text = "Version: " + version;
            this.labelCopyright.Text = "© 2013";
            this.labelCompanyName.Text = "Joshua Pavoncello";
        }
    }
}
