using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Collections;
using System.IO;

namespace TestCreatorTool__TCT_
{
    public partial class Form1 : Form
    {
        List<fullTest> allTest = new List<fullTest>();
        public Form1()
        {
            InitializeComponent();
            //read all tests from file
        }

        private void конфигураторТестовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fullTestConfigurator test = new fullTestConfigurator();
            test.ShowDialog();
            if (test.save == true)
            {
                allTest.Add(test.fullTestRes);
                dataGridView1.Rows.Add(test.fullTestRes.Name, test.fullTestRes.comment, test.fullTestRes.countTests());
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedCells.Count > 0)
            {
                int index = dataGridView1.CurrentRow.Index;
                dataGridView1.Rows.RemoveAt(index);
                allTest.RemoveAt(index);
            }
            
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int index = dataGridView1.CurrentRow.Index;

                fullTestConfigurator test = new fullTestConfigurator(allTest[index]);
                test.ShowDialog();
                if (test.save == true)
                {
                    dataGridView1.Rows.RemoveAt(index);
                    allTest.RemoveAt(index);
                    allTest.Add(test.fullTestRes);
                    dataGridView1.Rows.Add(test.fullTestRes.Name, test.fullTestRes.comment, test.fullTestRes.countTests());
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = JsonSerializer.Serialize(allTest, typeof(List<fullTest>));
            File.WriteAllText("test.dat", json);

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = File.ReadAllText("test.dat");
            allTest = JsonSerializer.Deserialize<List<fullTest>>(json);
            dataGridView1.Rows.Clear();
            foreach (fullTest test in allTest)
                dataGridView1.Rows.Add(test.Name, test.comment, test.countTests());

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int index = dataGridView1.CurrentRow.Index;

                serverTestGUI gui = new serverTestGUI(allTest[index]);
                gui.ShowDialog();
            }



        }
    }
}
