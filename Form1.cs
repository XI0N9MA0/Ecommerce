using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.BunifuCircleProgress.Transitions;

namespace ecommercetestttt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Panha p = new Panha();
            p.Pid = 0;
            p.PidStr = "Panha";

            MessageBox.Show("name:"+p.PidStr);
        }
    }
}
