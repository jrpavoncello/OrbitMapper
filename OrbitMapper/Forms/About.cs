using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbitMapper
{
    partial class About : Form
    {
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
