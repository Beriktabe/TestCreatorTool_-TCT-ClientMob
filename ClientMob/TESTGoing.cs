using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UDP;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ClientMob
{
    public partial class TESTGoing : Form
    {

        private string ip;
        private UDPSocket client = new UDPSocket();
        private UDPSocket server = new UDPSocket();
        private fullTest test;

        private string nickname;
        private int Answered = 0;
        private int AnsweredRight = 0;
        private int AnsweredRightPercent = 0;
        private string state = "-";
        private int currentIndexTest = 0;

        public TESTGoing(string nickname, string ip)
        {
            InitializeComponent();
            this.nickname = nickname;
            this.ip = ip;

            client.Client(ip, 27000);
            server.Server("0.0.0.0", 27001);

            client.Send("GET|" + nickname);
            server.onReceive += Server_onReceive;

            
        }

        private void Server_onReceive(object sender, UDPSocket.dataReceiveEventArgs e)
        {
            if (e.data.Split('|')[0].Contains("TEST"))
            {
                test = JsonSerializer.Deserialize<fullTest>(e.data.Split('|')[1]);

                this.Invoke((Action)delegate
                {
                    this.Text = test.Name;
                });


                this.richTextBox1.Invoke((Action)delegate
                {
                    this.Text = test.testList[currentIndexTest].question+Environment.NewLine;
                    updQuestion();
                });

            }
            else if(e.data.Split('|')[0].Contains("STOP"))
            {
                MessageBox.Show("ТЕСТ ОКОНЧЕН");
                Environment.Exit(0);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Answered += 1;
            switch (test.testList[currentIndexTest-1].type)
            {
                case testTypes.one:
                    if (textBox1.Text == test.testList[currentIndexTest-1].rightAnswer[0].ToString())
                        AnsweredRight += 1;
                    break;

                case testTypes.several:
                    List<string> lst = new List<string>(textBox1.Text.Split(','));
                    List<int> rightAnswerList = new List<int>(test.testList[currentIndexTest - 1].rightAnswer);

                    if (lst.Count != rightAnswerList.Count)
                        break;

                    for(int i = 0; i < test.testList[currentIndexTest - 1].rightAnswer.Count; i++)
                    {
                        
                        if (lst.Contains(test.testList[currentIndexTest - 1].rightAnswer[i].ToString()))
                        {
                            rightAnswerList.Remove(int.Parse(lst[i]));
                        }

                    }
                    if(rightAnswerList.Count == 0)
                        AnsweredRight += 1;
                    //if(counter == test.testList[currentIndexTest - 1].rightAnswer.Count)
                    //    AnsweredRight += 1;
                    break;

                case testTypes.order:
                    string ans = "";
                    for (int i = 0; i < test.testList[currentIndexTest - 1].rightAnswer.Count; i++)
                    {
                        ans += test.testList[currentIndexTest - 1].rightAnswer[i].ToString() + ",";
                    }
                    if (ans.Substring(0, ans.Length - 1) == textBox1.Text)
                        AnsweredRight += 1;
                    break;
            }
            if (AnsweredRight == 0)
                AnsweredRightPercent = 0;
            else
                AnsweredRightPercent = (int)(((float)AnsweredRight / (float)Answered)*100);



            client.Send("STATUS|"+nickname+"|" + Answered + "|" + AnsweredRight + "|" + AnsweredRightPercent + "|" + state);

            if (currentIndexTest-1 == test.countTests() - 1)
            {
                state = "+";
                client.Send("STATUS|" + nickname + "|" + Answered + "|" + AnsweredRight + "|" + AnsweredRightPercent + "|" + state);
                MessageBox.Show("ТЕСТ ОКОНЧЕН" + Environment.NewLine + "Процент правильных ответов: " + AnsweredRightPercent);
                Environment.Exit(0);
            }
            else
                updQuestion();

        }

        private void updQuestion()
        {
            richTextBox1.Text = "";

            string preview = test.testList[currentIndexTest].question + Environment.NewLine;
            foreach (KeyValuePair<int, answer> i in test.testList[currentIndexTest].answers)
            {
                preview += i.Key + ": " + i.Value.data + Environment.NewLine;
            }
            preview += Environment.NewLine;
            switch (test.testList[currentIndexTest].type)
            {
                case testTypes.one:
                    preview += "Выберите один ответ";
                    break;

                case testTypes.several:
                    preview += "Выберите один или больше вариантов ответа";
                    break;

                case testTypes.order:
                    preview += "Выберите правильный порядок";
                    break;
            }

            richTextBox1.Text = preview;
            currentIndexTest++;
        }
    }
}
