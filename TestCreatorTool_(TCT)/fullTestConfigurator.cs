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
    public partial class fullTestConfigurator : Form
    {
        public fullTest fullTestRes = new fullTest();
        public bool save = false;

        public fullTestConfigurator(fullTest fullTest = null)
        {
            InitializeComponent();
            fullTestRes.testList = new List<test>();
            if (fullTest != null)
            {
 
                textBox1.Text = fullTest.Name;
                richTextBox1.Text = fullTest.comment;
                foreach (test i in fullTest.testList)
                    listBox1.Items.Add(i);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            testConfigurator test = new testConfigurator(null);
            test.ShowDialog();
            if (test.save)
                listBox1.Items.Add(test._test);

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                testConfigurator test = new testConfigurator((test)listBox1.SelectedItem); 
                test.ShowDialog();
                if (test.save)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);
                    listBox1.Items.Add(test._test);
                }

            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            fullTestRes.Name = textBox1.Text;
            fullTestRes.comment = richTextBox1.Text;

            foreach (test i in listBox1.Items)
                fullTestRes.testList.Add(i);
            save = true;
            Close();
        }
    }
}
