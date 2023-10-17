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
    public partial class testConfigurator : Form
    {
        public test _test;
        public bool save = false;
        public testConfigurator(test _test)
        {
            InitializeComponent();


            toolTip1.SetToolTip(comboBox1, "Вид вопроса");
            toolTip1.SetToolTip(richTextBox1, "Вопрос");
            toolTip1.SetToolTip(listBox1, "Варианты ответов");
            toolTip1.SetToolTip(richTextBox3, "Ответы. Через запятую.");
            if (_test == null)
            {
                this._test = new test();
                this._test.type = testTypes.one;
                this._test.answers = new Dictionary<int, answer>();
                this._test.rightAnswer = new List<int>();
            }
            else
            {
                this._test = _test;
                richTextBox1.Text = _test.question;
                foreach (KeyValuePair<int, answer> i in _test.answers)
                {

                    listBox1.Items.Add(i.Value);
                }

                string temp = "";
                foreach (int i in _test.rightAnswer)
                    temp += i.ToString() + ",";

                richTextBox3.Text = temp.Remove(temp.Length-1,1);
            }


            comboBox1.SelectedIndex = (int)this._test.type;

        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            answerConfigurator conf = new answerConfigurator(null);
            conf.ShowDialog();
            if (conf.save == true)
            {
                conf.ans.order = listBox1.Items.Count+1;
                listBox1.Items.Add(conf.ans);
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                answerConfigurator conf = new answerConfigurator((answer)listBox1.SelectedItem);
                conf.ans.order = listBox1.SelectedIndex + 1;
                conf.ShowDialog();
                if (conf.save == true)
                {
                    listBox1.Items.Remove(listBox1.SelectedItem);

                    listBox1.Items.Add(conf.ans);
                }
            }

        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
                sortListBox();
            }
        }

        public void sortListBox()
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                answer temp = (answer)listBox1.Items[i];
                temp.order = i + 1;

                listBox1.Items.Remove(listBox1.Items[i]);
                listBox1.Items.Add(temp);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _test.answers.Clear();
            _test.rightAnswer.Clear();


            _test.question = richTextBox1.Text;
            _test.type = (testTypes)comboBox1.SelectedIndex; ///ХУЕТА!!!!!!!"!!!
            string[] split = richTextBox3.Text.Split(',');
            foreach (answer i in listBox1.Items)
            {
                _test.answers.Add(i.order, i);
            }

            foreach (string i in split)
            {
                _test.rightAnswer.Add(int.Parse(i));
            }
            save = true;
            //вернуть тест
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";

            string preview = _test.question + Environment.NewLine;
            foreach (KeyValuePair<int, answer> i in _test.answers)
            {
                preview += i.Key + ": " + i.Value.data + Environment.NewLine;
            }

            preview += "Правильный ответ: ";

            foreach (int i in _test.rightAnswer)
            {
                preview += i + ",";
            }
            preview = preview.Remove(preview.Length - 1, 1);

            richTextBox2.Text = preview;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        /*
        private void testConfigurator_FormClosing(object sender, FormClosingEventArgs e)
        {
            save = false;
        }*/
    }
}
