using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbitMapper
{
    public partial class Form3 : Form
    {

        public Form3()
        {
            InitializeComponent();
            EventSource.textChanged += new TextChanged(updateText);
            this.textBox1.Text = EventSource.getText();
        }

        public void updateText(object source, Events e){
                this.textBox1.Text = EventSource.getText();
        }
    }
}
