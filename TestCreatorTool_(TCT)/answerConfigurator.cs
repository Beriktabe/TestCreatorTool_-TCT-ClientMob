using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestCreatorTool__TCT_
{
    public partial class answerConfigurator : Form
    {
        public answer ans = new answer();
        public bool save = false;
        public answerConfigurator(answer ans)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ans.data = richTextBox1.Text;
            save = true;
            Close();
        }
    }
}
